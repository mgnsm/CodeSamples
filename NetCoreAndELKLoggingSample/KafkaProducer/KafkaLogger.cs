using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using AvroTypes;
using System.Diagnostics;

namespace KafkaProducer
{
    public sealed class KafkaLogger : ILogger, IDisposable
    {
        private static readonly EmptyDisposable s_emptyDisposable = new EmptyDisposable();

        private readonly Channel<AvroTypes.v1.LogMessage> _channel = Channel.CreateUnbounded<AvroTypes.v1.LogMessage>(
            new UnboundedChannelOptions() { SingleReader = true, SingleWriter = true });

        private readonly ObjectPool<AvroTypes.v1.LogMessage> _avroMessageObjectPool = ObjectPool.Create(new PooledAvroMessageObjectPolicy());
        private readonly ObjectPool<Message<Null, AvroTypes.v1.LogMessage>> _kafkaMessageObjectPool = ObjectPool.Create(new PooledKafkaMessageObjectPolicy());

        private readonly IProducer<Null, AvroTypes.v1.LogMessage> _kafkaProducer;
        private readonly string _topicName;
        private readonly Task _produceTask;

        public KafkaLogger(IProducer<Null, AvroTypes.v1.LogMessage> kafkaProducer, string topicName)
        {
            _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));

            if (string.IsNullOrEmpty(topicName))
                throw new ArgumentNullException(nameof(topicName));
            _topicName = topicName;

            _produceTask = Task.Run(Produce);
        }

        public IDisposable BeginScope<TState>(TState state) => s_emptyDisposable;

        public void Dispose()
        {
            _channel.Writer.TryComplete();
            _produceTask.Wait();
            _produceTask.Dispose();
            _kafkaProducer.Dispose();
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel < LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel != LogLevel.None)
            {
                AvroTypes.v1.LogMessage logMessage = _avroMessageObjectPool.Get() ?? new AvroTypes.v1.LogMessage();
                logMessage.LogLevel = logLevel.GetLogLevel();
                logMessage.EventId = eventId.GetEventId();
                logMessage.Exception = exception.GetException();
                if (state is string message)
                    logMessage.Message = message;
                logMessage.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                _channel.Writer.TryWrite(logMessage);
            }
        }

        private async Task Produce()
        {
            await foreach (AvroTypes.v1.LogMessage logMessage in _channel.Reader.ReadAllAsync())
            {
                Message<Null, AvroTypes.v1.LogMessage> kafkaMessage = _kafkaMessageObjectPool.Get() ?? new Message<Null, AvroTypes.v1.LogMessage>();
                kafkaMessage.Value = logMessage;
                try
                {
                    _kafkaProducer.Produce(_topicName, kafkaMessage, DeliveryHandler);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Failed to produce Kafka message: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                }
            }

            // Wait for any inflight messages to be delivered.
            _kafkaProducer.Flush();
        }

        private void DeliveryHandler(DeliveryReport<Null, AvroTypes.v1.LogMessage> deliveryReport)
        {
            if (deliveryReport != null)
            {
                _kafkaMessageObjectPool.Return(deliveryReport.Message);
                _avroMessageObjectPool.Return(deliveryReport.Value);
            }
        }
    }
}
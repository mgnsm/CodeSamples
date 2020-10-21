using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaConsumer
{
    class Program
    {
        static readonly ObjectPool<StringBuilder> s_objectPool = ObjectPool.Create(new StringBuilderObjectPolicy());

        static async Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: .. <bootstrap-servers> <schema-registry-url>");
                return;
            }
            
            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task task = Task.Run(() => Consume(bootstrapServers: args[0], schemaRegistryUrl: args[1], cancellationTokenSource.Token));

            Console.WriteLine("Press any key to stop consuming...");
            
            Console.ReadKey();
            cancellationTokenSource.Cancel();

            await task;
        }

        static void Consume(string bootstrapServers, string schemaRegistryUrl, CancellationToken cancellationToken)
        {
            using CachedSchemaRegistryClient schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = schemaRegistryUrl });
            using IConsumer<Null, AvroTypes.v1.LogMessage> consumer =
                new ConsumerBuilder<Null, AvroTypes.v1.LogMessage>(
                    new ConsumerConfig
                    {
                        GroupId = Guid.NewGuid().ToString(),
                        BootstrapServers = bootstrapServers,
                        AutoOffsetReset = AutoOffsetReset.Earliest
                    })
                    .SetValueDeserializer(new AvroDeserializer<AvroTypes.v1.LogMessage>(schemaRegistry).AsSyncOverAsync())
                    .Build();

            consumer.Subscribe("log-messages");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    ConsumeResult<Null, AvroTypes.v1.LogMessage> consumeResult = consumer.Consume(cancellationToken);
                    if (consumeResult?.Message?.Value != null)
                        Console.WriteLine(FormatLogMessage(consumeResult.Message.Value));
                }
                catch (OperationCanceledException)
                {
                    //commit final offsets and leave the group
                    consumer.Close();
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"An error occured: {e.Error.Reason}.");
                }
            }
        }

        static string FormatLogMessage(AvroTypes.v1.LogMessage logMessage)
        {
            const string Null = "<null>";
            StringBuilder stringBuilder = s_objectPool.Get() ?? new StringBuilder();
            DateTime utcTimestamp = DateTimeOffset.FromUnixTimeMilliseconds(logMessage.Timestamp).UtcDateTime;
            stringBuilder.Append($"{utcTimestamp:yyyy-MM-dd HH:mm:ss.fff}: \n");
            stringBuilder.Append($"  Id/Offset:\t0\n");
            stringBuilder.Append($"  {nameof(AvroTypes.v1.LogMessage.LogLevel)}:\t{logMessage.LogLevel}\n");
            stringBuilder.Append($"  {nameof(AvroTypes.v1.LogMessage.Message)}:\t{logMessage.Message ?? Null}\n");

            stringBuilder.Append($"  {nameof(AvroTypes.v1.LogMessage.EventId)}:\t");
            if (logMessage.EventId != null)
            {
                stringBuilder.Append($"\n    {nameof(AvroTypes.v1.EventId.Id)}:\t\t{logMessage.EventId.Id}");
                stringBuilder.Append($"\n    {nameof(AvroTypes.v1.EventId.Name)}:\t{logMessage.EventId.Name ?? Null}");
            }
            else
            {
                stringBuilder.Append(Null);
            }

            stringBuilder.Append($"\n  {nameof(AvroTypes.v1.LogMessage.Exception)}:\t");
            if (logMessage.Exception != null)
            {
                stringBuilder.Append($"\n    {nameof(AvroTypes.v1.Exception.Source)}:\t{logMessage.Exception.Source ?? Null}");
                stringBuilder.Append($"\n    {nameof(AvroTypes.v1.Exception.HelpLink)}:\t{logMessage.Exception.HelpLink ?? Null}");
                stringBuilder.Append($"\n    {nameof(AvroTypes.v1.Exception.HResult)}:\t{logMessage.Exception.HResult}");
                stringBuilder.Append($"\n    {nameof(AvroTypes.v1.Exception.Message)}:\t{logMessage.Exception.Message ?? Null}");
                stringBuilder.Append($"\n    {nameof(AvroTypes.v1.Exception.StackTrace)}:\t{logMessage.Exception.StackTrace?.Trim() ?? Null}");

            }
            else
            {
                stringBuilder.Append(Null);
            }
            stringBuilder.AppendLine();

            string formattedLogMessage = stringBuilder.ToString();
            s_objectPool.Return(stringBuilder);
            return formattedLogMessage;
        }
    }
}
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KafkaProducer
{
    class Program
    {
        static readonly Random s_random = new Random();
        static readonly LogLevel[] s_logLevels = (LogLevel[])Enum.GetValues(typeof(AvroTypes.v1.LogLevel));

        static async Task Main(string[] _)
        {
            if (!TryGetEnvironmentVariable("BOOTSTRAP_SERVERS", out string bootstrapServers)
                || !TryGetEnvironmentVariable("SCHEMA_REGISTRY_URL", out string schemaRegistryUrl))
                return;

            using CachedSchemaRegistryClient schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = schemaRegistryUrl });
            using IProducer<Null, AvroTypes.v1.LogMessage> kafkaProducer =
                new ProducerBuilder<Null, AvroTypes.v1.LogMessage>(new ProducerConfig { BootstrapServers = bootstrapServers })
                    .SetValueSerializer(new AvroSerializer<AvroTypes.v1.LogMessage>(schemaRegistry).AsSyncOverAsync())
                    .Build();
            using KafkaLogger kafkaLogger = new KafkaLogger(kafkaProducer, "log-messages");

            int counter = 0;
            do
            {
                if (++counter % 10 == 0)
                    ThrowAndLogException(kafkaLogger);
                else
                    kafkaLogger.Log(GetRandomLogLevel(), 
                        new EventId(counter, counter % 2 == 0 ? counter.ToString() : default), null, 
                        "some message...");

                //wait at least 0.5 seconds before writing another log message
                await Task.Delay(500);
            }
            while (true);
        }

        static bool TryGetEnvironmentVariable(string variable, out string value)
        {
            value = Environment.GetEnvironmentVariable(variable);
            if (string.IsNullOrEmpty(value))
            {
                Console.WriteLine($"The {variable} environment variable must be set.");
                return false;
            }
            return true;
        }

        static LogLevel GetRandomLogLevel() => s_logLevels[s_random.Next(0, s_logLevels.Length)];

        static void ThrowAndLogException(KafkaLogger kafkaLogger)
        {
            try
            {
                throw new Exception("sample exception message...");
            }
            catch (Exception ex)
            {
                kafkaLogger.LogError(ex, "sample exception thrown...");
            }
        }
    }
}
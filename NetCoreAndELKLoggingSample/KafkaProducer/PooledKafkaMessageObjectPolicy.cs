using Confluent.Kafka;
using Microsoft.Extensions.ObjectPool;

namespace KafkaProducer
{
    internal class PooledKafkaMessageObjectPolicy : PooledObjectPolicy<Message<Null, AvroTypes.v1.LogMessage>>
    {
        public override Message<Null, AvroTypes.v1.LogMessage> Create() => new Message<Null, AvroTypes.v1.LogMessage>();

        public override bool Return(Message<Null, AvroTypes.v1.LogMessage> kafkaMessage)
        {
            if (kafkaMessage == null)
                return false;

            kafkaMessage.Key = default;
            kafkaMessage.Value = default;
            return true;
        }
    }
}
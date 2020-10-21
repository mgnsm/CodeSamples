using Microsoft.Extensions.ObjectPool;

namespace KafkaProducer
{
    internal class PooledAvroMessageObjectPolicy : PooledObjectPolicy<AvroTypes.v1.LogMessage>
    {
        public override AvroTypes.v1.LogMessage Create() => new AvroTypes.v1.LogMessage();

        public override bool Return(AvroTypes.v1.LogMessage avroMessage)
        {
            if (avroMessage == null)
                return false;

            avroMessage.LogLevel = default;
            avroMessage.EventId = default;
            avroMessage.Exception = default;
            avroMessage.Message = default;
            avroMessage.Timestamp = default;
            return true;
        }
    }
}

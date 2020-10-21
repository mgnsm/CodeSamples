using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace KafkaConsumer
{
    internal class StringBuilderObjectPolicy : PooledObjectPolicy<StringBuilder>
    {
        public override StringBuilder Create() => new StringBuilder();

        public override bool Return(StringBuilder stringBuilder)
        {
            if (stringBuilder == null)
                return false;

            stringBuilder.Clear();
            return true;
        }
    }
}
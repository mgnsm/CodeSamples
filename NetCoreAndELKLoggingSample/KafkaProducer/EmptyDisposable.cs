using System;

namespace KafkaProducer
{
    internal sealed class EmptyDisposable : IDisposable
    {
        public void Dispose() { }
    }
}

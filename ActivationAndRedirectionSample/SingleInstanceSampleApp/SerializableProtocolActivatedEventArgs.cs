using System;
using Windows.ApplicationModel.Activation;

namespace SingleInstanceSampleApp
{
    [Serializable]
    public class SerializableProtocolActivatedEventArgs : SerializableActivatedEventArgs, IProtocolActivatedEventArgs
    {
        public Uri Uri { get; set; }
    }
}
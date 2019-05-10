using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace SingleInstanceSampleApp
{
    [Serializable]
    public class SerializableFileActivatedEventArgs : SerializableActivatedEventArgs, IFileActivatedEventArgs
    {
        public string Verb { get; set; }

        public IReadOnlyList<IStorageItem> Files { get; set; }
    }
}
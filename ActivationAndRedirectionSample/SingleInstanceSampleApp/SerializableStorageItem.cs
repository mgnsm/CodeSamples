using System;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace SingleInstanceSampleApp
{
    [Serializable]
    public class SerializableStorageItem : IStorageItem
    {
        public FileAttributes Attributes { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public IAsyncAction RenameAsync(string desiredName) => throw new NotImplementedException();
        public IAsyncAction RenameAsync(string desiredName, NameCollisionOption option) => throw new NotImplementedException();
        public IAsyncAction DeleteAsync() => throw new NotImplementedException();
        public IAsyncAction DeleteAsync(StorageDeleteOption option) => throw new NotImplementedException();
        public IAsyncOperation<BasicProperties> GetBasicPropertiesAsync() => throw new NotImplementedException();
        public bool IsOfType(StorageItemTypes type) => throw new NotImplementedException();
    }
}
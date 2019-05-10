using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace SingleInstanceSampleApp
{
    public class Serializer
    {
        public static SerializableActivatedEventArgs Serialize(IActivatedEventArgs activatedEventArgs)
        {
            SerializableActivatedEventArgs serializableActivatedEventArgs = null;
            switch (activatedEventArgs)
            {
                case IProtocolActivatedEventArgs protocolActivatedEventArgs:
                    serializableActivatedEventArgs = new SerializableProtocolActivatedEventArgs()
                    {
                        Kind = activatedEventArgs.Kind,
                        PreviousExecutionState = activatedEventArgs.PreviousExecutionState,
                        SplashScreen = activatedEventArgs.SplashScreen,
                        Uri = protocolActivatedEventArgs.Uri
                    };
                    break;
                case IFileActivatedEventArgs fileActivatedEventArgs:
                    IList<IStorageItem> serializableFiles =
                        fileActivatedEventArgs.Files?
                        .Select<IStorageItem, IStorageItem>(x => new SerializableStorageItem()
                        {
                             Attributes = x.Attributes,
                             DateCreated = x.DateCreated,
                             Name = x.Name,
                             Path = x.Path
                        })
                        .ToList();

                    serializableActivatedEventArgs = new SerializableFileActivatedEventArgs()
                    {
                        Kind = activatedEventArgs.Kind,
                        PreviousExecutionState = activatedEventArgs.PreviousExecutionState,
                        SplashScreen = activatedEventArgs.SplashScreen,
                        Verb = fileActivatedEventArgs.Verb,
                        Files = serializableFiles != null ? new ReadOnlyCollection<IStorageItem>(serializableFiles) : null
                    };
                    break;
                default:
                    serializableActivatedEventArgs = new SerializableProtocolActivatedEventArgs()
                    {
                        Kind = activatedEventArgs.Kind,
                        PreviousExecutionState = activatedEventArgs.PreviousExecutionState,
                        SplashScreen = activatedEventArgs.SplashScreen
                    };
                    break;
            }
            return serializableActivatedEventArgs;
        }
    }
}
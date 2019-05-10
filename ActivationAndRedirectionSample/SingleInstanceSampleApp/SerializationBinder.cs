using System;
using System.Runtime.Serialization;

namespace SingleInstanceSampleApp
{
    public class CustomBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (typeName == typeof(Windows.ApplicationModel.Activation.ActivationKind).FullName)
                return typeof(Windows.ApplicationModel.Activation.ActivationKind);
            else if (typeName == typeof(Windows.ApplicationModel.Activation.ApplicationExecutionState).FullName)
                return typeof(Windows.ApplicationModel.Activation.ApplicationExecutionState);
            else if (typeName == typeof(Windows.Storage.FileAttributes).FullName)
                return typeof(Windows.Storage.FileAttributes);
            else if (typeName == typeof(Windows.Storage.IStorageItem).FullName)
                return typeof(SerializableStorageItem);

            return Type.GetType(typeName);
        }
    }
}
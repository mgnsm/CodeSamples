using System;
using Windows.ApplicationModel.Activation;

namespace SingleInstanceSampleApp
{
    [Serializable]
    public class SerializableActivatedEventArgs : IActivatedEventArgs
    {
        public ActivationKind Kind { get; set; }

        public ApplicationExecutionState PreviousExecutionState { get; set; }
        
        [NonSerialized]
        private SplashScreen _splashScreen;
        public SplashScreen SplashScreen
        {
            get => _splashScreen;
            set => _splashScreen = value;
        }
    }
}
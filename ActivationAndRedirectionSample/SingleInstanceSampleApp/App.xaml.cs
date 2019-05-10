using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Windows.ApplicationModel.Activation;

namespace SingleInstanceSampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        const string TimeFormat = "HH:mm:ss";

        public ObservableCollection<string> Activations { get; } = new ObservableCollection<string>();

        public void OnProtocolActivated(IActivatedEventArgs args)
        {
            switch (args.Kind)
            {
                case ActivationKind.Protocol:
                    Activations.Add($"Protocol activation triggered at {DateTime.Now.ToString(TimeFormat)}." +
                        (args is IProtocolActivatedEventArgs protocolArgs ? $" URI: {protocolArgs.Uri.ToString()}" : string.Empty));
                    break;
                case ActivationKind.File:
                    Activations.Add($"File activation triggered at {DateTime.Now.ToString(TimeFormat)}." +
                        (args is IFileActivatedEventArgs fileArgs && fileArgs.Files != null && fileArgs.Files.Count > 0 ?
                        $" Files:{Environment.NewLine}{string.Join(Environment.NewLine, fileArgs.Files.Select(x => $"- {x.Path}"))}"
                            : string.Empty));
                    break;
            }
        }
    }
}
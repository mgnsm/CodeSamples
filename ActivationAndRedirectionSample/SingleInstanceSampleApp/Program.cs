using System;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

namespace SingleInstanceSampleApp
{
    class Program
    {
        const string AppUniqueGuid = "9da112cb-a929-4c50-be53-79f31b2135ca";
        const string NamedPipeServerName = ".";
        static readonly int s_connectionTimeout = TimeSpan.FromSeconds(3).Milliseconds;
        static readonly BinaryFormatter s_formatter = new BinaryFormatter() { Binder = new CustomBinder() };
        static App s_application;

        [STAThread]
        static void Main(string[] args)
        {
            IActivatedEventArgs activatedEventArgs = AppInstance.GetActivatedEventArgs();
            using (Mutex mutex = new Mutex(false, AppUniqueGuid))
            {
                if (mutex.WaitOne(0, false))
                {
                    new Thread(CreateNamedPipeServer) { IsBackground = true }
                        .Start();

                    s_application = new App();
                    s_application.InitializeComponent();
                    if (activatedEventArgs != null)
                        s_application.OnProtocolActivated(activatedEventArgs);
                    s_application.Run();
                }
                else if (activatedEventArgs != null)
                {
                    //instance already running
                    using (NamedPipeClientStream namedPipeClientStream
                        = new NamedPipeClientStream(NamedPipeServerName, AppUniqueGuid, PipeDirection.Out))
                    {
                        try
                        {
                            namedPipeClientStream.Connect(s_connectionTimeout);
                            SerializableActivatedEventArgs serializableActivatedEventArgs = Serializer.Serialize(activatedEventArgs);
                            s_formatter.Serialize(namedPipeClientStream, serializableActivatedEventArgs);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        static void CreateNamedPipeServer()
        {
            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(AppUniqueGuid,
                PipeDirection.In, 1, PipeTransmissionMode.Message))
            {
                while (true)
                {
                    pipeServer.WaitForConnection();
                    IActivatedEventArgs activatedEventArgs;
                    do
                    {
                        activatedEventArgs = s_formatter.Deserialize(pipeServer) as IActivatedEventArgs;
                    } while (!pipeServer.IsMessageComplete);

                    if (activatedEventArgs != null)
                        s_application.Dispatcher
                            .BeginInvoke(new Action(() => s_application.OnProtocolActivated(activatedEventArgs)));

                    pipeServer.Disconnect();
                }
            }
        }
    }
}
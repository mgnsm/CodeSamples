using System;
using Windows.ApplicationModel;

namespace MultipleInstancesSampleApp
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            App application = new App();
            application.InitializeComponent();
            application.OnProtocolActivated(AppInstance.GetActivatedEventArgs());
            application.Run();
        }
    }
}
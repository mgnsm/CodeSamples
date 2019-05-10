extern alias Windows;
using System;
using System.Windows;
using Windows::Windows.System;

namespace SingleInstanceSampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("mysampleuri:?sample"));
        }
    }
}
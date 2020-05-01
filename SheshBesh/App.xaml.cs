using System.Windows;

namespace SheshBesh
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Database.Connect();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Database.Close();
        }
    }
}
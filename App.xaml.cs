using FlightSimulatorApp.Model;
using FlightSimulatorApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IServerModel model = new ServerModel(new SimulatorTelnetClient());
            SimulatorViewModel vm = new SimulatorViewModel(model);
            MainWindow window = new MainWindow(vm);
            window.Show();
        }
    }
}

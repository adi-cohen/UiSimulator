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
            SimulatorViewModel SimulatorVm = new SimulatorViewModel(model);
            SteersViewModel SteersVm = new SteersViewModel(model);
            MapViewModel MapVm = new MapViewModel(model);
            DashboardViewModel DashboardVm = new DashboardViewModel(model);
            MainWindow window = new MainWindow(SimulatorVm, SteersVm, MapVm, DashboardVm);
            window.Show();
        }
    }
}

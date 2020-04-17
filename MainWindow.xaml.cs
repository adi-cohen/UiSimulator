using FlightSimulatorApp.Model;
using FlightSimulatorApp.ViewModel;
using FlightSimulatorApp.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly SimulatorViewModel vm;

        public MainWindow(SimulatorViewModel SimulatorVm, SteersViewModel SteersVm, MapViewModel MapVm, DashboardViewModel DashboardVm)
        {
            InitializeComponent();
            this.Dispatcher.UnhandledException += HandleAllExceptions;
            this.vm = SimulatorVm;
            DataContext = vm;
            Steers.DataContext = SteersVm;
            Dashboard.DataContext = DashboardVm;
            Map.DataContext = MapVm;
        }

        private void HandleAllExceptions(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogErros.Text = e.Exception.Message;
            e.Handled = true;
        }

        private void ClearLogs(object sender, RoutedEventArgs e)
        {
            vm.VM_FlightLogs = "";
        }

        private void ResetPortIp()
        {
            vm.VM_Port = int.Parse(ConfigurationManager.AppSettings["default_port"].ToString());
            vm.VM_IP = ConfigurationManager.AppSettings["default_ip"].ToString();
        }

        private void DisConnectOnClick(object sender, RoutedEventArgs e)
        {
            ResetPortIp();
            _ = vm.Disconnect();
        }

        private void ConnectOnClick(object sender, RoutedEventArgs e)
        {
            _ = vm.Connect();
            ResetPortIp();
        }
    }
}

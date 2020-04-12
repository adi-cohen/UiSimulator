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
        SimulatorViewModel vm;
        

        public MainWindow(SimulatorViewModel newVm)
        { 
            InitializeComponent();
            this.Dispatcher.UnhandledException += handleAllExceptions;
            this.vm = newVm;
            DataContext = vm;
            Steers.DataContext = vm;
            Dashboard.DataContext = vm;
            // Map.DataContext = vm;
        }



        private void handleAllExceptions(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogErros.Text = e.Exception.Message;
            e.Handled = true;
        }

        private void connectOnClick(object sender, RoutedEventArgs e)
        {
            vm.connect();
            resetPortIp();
        }
        private void disConnectOnClick(object sender, RoutedEventArgs e)
        {
            resetPortIp();
            vm.disconnect();
        }

        private void clearLogs(object sender, RoutedEventArgs e)
        {
            vm.VM_FlightLogs = "";

        }

        private void resetPortIp()
        {
            vm.VM_Port =  int.Parse(ConfigurationManager.AppSettings["default_port"].ToString());
            vm.VM_IP = ConfigurationManager.AppSettings["default_ip"].ToString();

        }
    }
}

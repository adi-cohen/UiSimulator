using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModel
{

    public class SimulatorViewModel : INotifyPropertyChanged
    {
        private string ip = ConfigurationManager.AppSettings["default_ip"].ToString();
        public string VM_IP
        {
            get { return ip; }
            set
            {
                ip = value;
                NotifyPropertyChanged("VM_IP");
            }
        }
        private int port = int.Parse(ConfigurationManager.AppSettings["default_port"].ToString());

        public int VM_Port
        {
            get { return port; }
            set
            {
                port = value;
                NotifyPropertyChanged("VM_Port");
            }
        }

        private string flightLogs;
        public string VM_FlightLogs
        {
            get
            {
                return model.FlightLogs;
            }
            set
            {
                flightLogs = value;
                model.Setflightlogs(value);
            }
        }

        private readonly IServerModel model;
        public SimulatorViewModel(IServerModel model)
        {
            this.model = model;
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


        public async Task Connect()
        {
            await Task.Run(() => model.Connect(ip, port));
            await Task.Run(() => model.Start());
        }
        public async Task Disconnect()
        {
            await Task.Run(() => model.Disconnect());
            port = int.Parse(ConfigurationManager.AppSettings["default_port"].ToString());
            ip = ConfigurationManager.AppSettings["default_ip"].ToString();
        }

        public Boolean VM_IsClientConnected
        {
            get
            { return model.IsClientConnected; }
        }

        public Boolean VM_IsClientDisConnected
        {
            get
            { return model.IsClientDisConnected; }
        }
    }
}

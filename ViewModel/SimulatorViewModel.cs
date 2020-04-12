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
            set { ip = value;
                NotifyPropertyChanged("VM_IP");
            }
        }
        private int port = int.Parse(ConfigurationManager.AppSettings["default_port"].ToString());

        public int VM_Port
        {
            get { return port; }
            set { 
                port = value;
                NotifyPropertyChanged("VM_Port");

            }
        }

        private double throttle;
        public double VM_Throttle
        {
            get
            { return throttle; }
            set {
                throttle = value;
                model.setThrottle(value);
            }
        }
        private double aileron;
        public double VM_Aileron
        {
            get
            { return aileron; }
            set
            {
                aileron = value;
                model.setAileron(value);
            }
        }
        string flightLogs;
        public string VM_FlightLogs
        {
            get
            {
                return model.FlightLogs;
            }
            set
            {
                flightLogs = value;
                model.setflightlogs(value);
            }
           
        }

        private IServerModel model;
        public SimulatorViewModel (IServerModel model)
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
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void moveJoystick (double rudder, double elevator)
        {
            System.Console.WriteLine("movejoystick!!!!!!!!!!!!!!!!!!!!!!");
            System.Console.WriteLine(Diff);
            model.setRudder(rudder); //X
            model.setElevator(elevator); //Y
        }
        public async Task connect ()
        {
            await Task.Run(() => model.connect(ip, port));
            await Task.Run(() => model.start());
        }
        public async Task disconnect()
        {
            await Task.Run(() => model.disconnect());
            port = int.Parse(ConfigurationManager.AppSettings["default_port"].ToString());
            ip = ConfigurationManager.AppSettings["default_ip"].ToString();
        }

        //properties
        #region Dashboard Properties

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
        public double VM_HeadingDeg
        {
            get
            { return model.HeadingDeg; }
        }
        public string VM_Location
        {
            get
            {
  
                return model.Location;
            }
        }
        
        public double VM_GpsVerticalSpeed
        {
            get
            { return model.HeadingDeg; }
        }
        public double VM_GpsGroundSpeed
        {
            get
            { return model.HeadingDeg; }
        }
        public double VM_AirspeedSpeed
        {
            get
            { return model.AirspeedSpeed; }
        }
        public double VM_GpsAltitude
        {
            get
            { return model.GpsAltitude; }
        }
        public double VM_AltitudeInternalRolDeg
        {
            get
            { return model.AltitudeInternalRolDeg; }
        }
        public double VM_AltitudeInternalPitchDeg
        {
            get
            { return model.AltitudeInternalPitchDeg; }
        }
        public double VM_AltimeterAltitude
        {
            get
            { return model.AltimeterAltitude; }
        }
        #endregion

        #region steering system
        private double Rudder;
       
        public double VM_Rudder
        {
            get
            { return Rudder; }
            set
            { 
                Rudder = value; }
        }

        private double Elevator;
        public double VM_Elevator
        {
            get
            { return Elevator; }
            set
            { Elevator = value; }
        }
        #endregion
        private double Diff;
        public double VM_Diff
        {
            get
            {
                return Diff;
            }
            set
            {
                Diff = value;
                moveJoystick(Rudder, Elevator);
            }
        }
        #region map property
        public double VM_PositionLongitudeDeg
        {
            get
            { return model.PositionLongitudeDeg; }
        }
        public double VM_PositionLatitudeDeg
        {
            get
            { return model.PositionLatitudeDeg; }
        }
        #endregion
    }
}

using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModel
{
    public class SteersViewModel : INotifyPropertyChanged
    {
        private readonly IServerModel model;
        public SteersViewModel(IServerModel model)
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

        private double throttle;
        public double VM_Throttle
        {
            get
            { return throttle; }
            set
            {
                throttle = value;
                model.SetThrottle(value);
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
                model.SetAileron(value);
            }
        }

        public void MoveJoystick(double rudder, double elevator)
        {
            model.SetRudder(rudder); //X
            model.SetElevator(elevator); //Y
        }

        #region steering system
        private double Rudder;
        public double VM_Rudder
        {
            get
            { return Rudder; }
            set
            {
                Rudder = value;
            }
        }

        private double  levator;
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
                MoveJoystick(Rudder, Elevator);
            }
        }
    }
}

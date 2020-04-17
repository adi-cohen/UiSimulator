using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModel
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly IServerModel model;
        public DashboardViewModel(IServerModel model)
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

        //properties
        #region Dashboard Properties


        public double VM_HeadingDeg
        {
            get
            { return model.HeadingDeg; }
        }

        public double VM_GpsVerticalSpeed
        {
            get
            { return model.GpsVerticalSpeed; }
        }
        public double VM_GpsGroundSpeed
        {
            get
            { return model.GpsGroundSpeed; }
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

    }
}

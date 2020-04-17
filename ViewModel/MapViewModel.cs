using FlightSimulatorApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.ViewModel
{
    public class MapViewModel : INotifyPropertyChanged
    {
        private readonly IServerModel model;
        public MapViewModel(IServerModel model)
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

        public string VM_Location
        {

            get
            {
                return model.Location;
            }
        }
        public double VM_PositionLongitudeDeg
        {
            get
            { 
                return model.PositionLongitudeDeg; 
            }
        }
        public double VM_PositionLatitudeDeg
        {
            get
            { 
                return model.PositionLatitudeDeg; 
            }
        }

    }
}

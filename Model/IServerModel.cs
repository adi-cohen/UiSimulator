using System.ComponentModel;

namespace FlightSimulatorApp.Model
{
    public interface IServerModel : INotifyPropertyChanged 
    {
        // connection to the simulator
        void connect(string ip, int port);
        void disconnect();
        void start();

        //dashboard properties
        double HeadingDeg { get; set; }
        double GpsVerticalSpeed { get; set; }
        double GpsGroundSpeed { get; set; }
        double AirspeedSpeed { get; set; }
        double GpsAltitude { get; set; }
        double AltitudeInternalRolDeg { get; set; }
        double AltitudeInternalPitchDeg { get; set; }
        double AltimeterAltitude { get; set; }

        //steering system properties
        double Rudder { get; set; }
        double Throttle { get; set; }
        double Aileron { get; set; }
        double Elevator { get; set; }

        //map properties
        double PositionLongitudeDeg { get; set; }
        double PositionLatitudeDeg { get; set; }

    }
}
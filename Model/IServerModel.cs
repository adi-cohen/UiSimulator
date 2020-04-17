using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    public interface IServerModel : INotifyPropertyChanged
    {
        // connection to the simulator
        void Connect(string ip, int port);
        void Disconnect();
        Task Start();
        Boolean IsClientConnected { get; set; }
        Boolean IsClientDisConnected { get; set; }

        //dashboard properties
        double GpsVerticalSpeed { get; set; }
        double GpsGroundSpeed { get; set; }
        double HeadingDeg { get; set; }

        double AirspeedSpeed { get; set; }
        double GpsAltitude { get; set; }
        double AltitudeInternalRolDeg { get; set; }
        double AltitudeInternalPitchDeg { get; set; }
        double AltimeterAltitude { get; set; }
        string Location { get; set; }
        string FlightLogs { get; set; }
        


        //map properties
        double PositionLongitudeDeg { get; set; }
        double PositionLatitudeDeg { get; set; }

        //comands
        Task SetRudder(double rudder);
        Task SetThrottle(double throttle);
        Task SetAileron(double aileron);
        Task SetElevator(double elevator);
        void Setflightlogs(string value);
    }
}
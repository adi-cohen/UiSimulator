using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    public interface IServerModel : INotifyPropertyChanged
    {
        // connection to the simulator
        void connect(string ip, int port);
        void disconnect();
        Task start();
        Boolean IsClientConnected { get; set; }
        Boolean IsClientDisConnected { get; set; }

        //dashboard properties
        double HeadingDeg { get; set; }
        double GpsVerticalSpeed { get; set; }
        double GpsGroundSpeed { get; set; }
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
        Task setRudder(double rudder);
        Task setThrottle(double throttle);
        Task setAileron(double aileron);
        Task setElevator(double elevator);
        void setflightlogs(string value);
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    public class ServerModel : IServerModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ITelentClient client;
        volatile Boolean stop;

        #region Dashboard Properties

        public double HeadingDeg { get; set; }
        public double GpsVerticalSpeed { get; set; }
        public double GpsGroundSpeed { get; set; }
        public double AirspeedSpeed { get; set; }
        public double GpsAltitude { get; set; }
        public double AltitudeInternalRolDeg { get; set; }
        public double AltitudeInternalPitchDeg { get; set; }
        public double AltimeterAltitude { get; set; }
        #endregion

        #region steering system
        public double Rudder { get; set; }
        public double Throttle { get; set; }
        public double Aileron { get; set; }
        public double Elevator { get; set; }
        #endregion

        #region map property
        public double PositionLongitudeDeg { get; set; }
        public double PositionLatitudeDeg { get; set; }
        #endregion

        public ServerModel(ITelentClient telentClient)
        {
            this.client = telentClient;
            stop = false;
        }
        public void connect(string ip, int port)
        {
            client.connect(ip, port);
        }
        public void disconnect()
        {
            stop = true;
            client.disconnect();
        }
        public void start()
        {
            new Thread(delegate()
            {
                while (!stop)
                {
                    HeadingDeg = WriteAndReadDouble("/instrumentation/heading-indicator/indicated-heading-deg");
                    GpsVerticalSpeed = WriteAndReadDouble("/instrumentation/gps/indicated-vertical-speed");
                    GpsGroundSpeed = WriteAndReadDouble("/instrumentation/gps/indicated-ground-speed-kt");
                    AirspeedSpeed = WriteAndReadDouble("/instrumentation/airspeed-indicator/indicated-speed-kt");
                    GpsAltitude = WriteAndReadDouble("/instrumentation/gps/indicated-altitude-ft");
                    AltitudeInternalRolDeg = WriteAndReadDouble("/instrumentation/attitude-indicator/internal-roll-deg");
                    AltitudeInternalPitchDeg = WriteAndReadDouble("/instrumentation/attitude-indicator/internal-pitch-deg");
                    AltimeterAltitude = WriteAndReadDouble("/instrumentation/altimeter/indicated-altitude-ft");

                    //steering
                    Elevator = WriteAndReadDouble("/controls/flight/elevator");
                    Aileron = WriteAndReadDouble("/controls/flight/elevator");
                    Throttle = WriteAndReadDouble("/controls/engines/current-engine/throttle");
                    Rudder = WriteAndReadDouble("/controls/flight/rudder");
                    
                    // map property
                    PositionLatitudeDeg = WriteAndReadDouble("/position/latitude-deg");
                    PositionLongitudeDeg = WriteAndReadDouble("/position/longitude-deg");

                    Thread.Sleep(250);
                }
            }).Start();
        }

        public double WriteAndReadDouble(string path)
        {
            client.write("get "+path);
            double answer = Double.Parse(client.read());
            Console.WriteLine(path+" " + answer);
            return answer;
            
        }
}
}

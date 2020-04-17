using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    public class ServerModel : IServerModel
    {
        public event PropertyChangedEventHandler PropertyChanged = null;
        private readonly ITelentClient client;
        volatile Boolean isClientConnected;
        volatile bool isClientDisConnected;

        private static readonly Object obj = new Object();


        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #region Dashboard Properties
        private double headingDeg;
        public double HeadingDeg
        {
            get
            { return headingDeg; }
            set
            {
                headingDeg = value;
                NotifyPropertyChanged("HeadingDeg");
            }
        }


        private double gpsVerticalSpeed;
        public double GpsVerticalSpeed
        {
            get
            { return gpsVerticalSpeed; }
            set
            {
                gpsVerticalSpeed = value;
                NotifyPropertyChanged("GpsVerticalSpeed");
            }
        }


        public bool IsClientConnected
        {
            get { return isClientConnected; }
            set
            {
                isClientConnected = value;
                NotifyPropertyChanged("IsClientConnected");
            }
        }

        public bool IsClientDisConnected
        {
            get { return isClientDisConnected; }
            set
            {
                isClientDisConnected = value;
                NotifyPropertyChanged("IsClientDisConnected");
            }
        }


        private string flightLogs;
        public string FlightLogs
        {
            get
            {
                return flightLogs;
            }
            set
            {
                flightLogs = value;
                NotifyPropertyChanged("FlightLogs");
            }
        }

        private double gpsGroundSpeed;
        public double GpsGroundSpeed
        {
            get
            { return gpsGroundSpeed; }
            set
            {
                gpsGroundSpeed = value;
                NotifyPropertyChanged("GpsGroundSpeed");
            }
        }
        private double airspeedSpeed;
        public double AirspeedSpeed
        {
            get
            { return airspeedSpeed; }
            set
            {
                airspeedSpeed = value;
                NotifyPropertyChanged("AirspeedSpeed");
            }
        }
        private double gpsAltitude;
        public double GpsAltitude
        {
            get
            { return gpsAltitude; }
            set
            {
                gpsAltitude = value;
                NotifyPropertyChanged("GpsAltitude");
            }
        }
        private double altitudeInternalRolDeg;
        public double AltitudeInternalRolDeg
        {
            get
            { return altitudeInternalRolDeg; }
            set
            {
                altitudeInternalRolDeg = value;
                NotifyPropertyChanged("AltitudeInternalRolDeg");
            }
        }
        private double altitudeInternalPitchDeg;
        public double AltitudeInternalPitchDeg
        {
            get
            { return altitudeInternalPitchDeg; }
            set
            {
                altitudeInternalPitchDeg = value;
                NotifyPropertyChanged("AltitudeInternalPitchDeg");
            }
        }
        private double altimeterAltitude;
        public double AltimeterAltitude
        {
            get
            { return altimeterAltitude; }
            set
            {
                altimeterAltitude = value;
                NotifyPropertyChanged("AltimeterAltitude");
            }
        }
        #endregion



        #region map property

        private double positionLongitudeDeg;
        public double PositionLongitudeDeg
        {
            get
            { return positionLongitudeDeg; }
            set
            {
                positionLongitudeDeg = value;
                NotifyPropertyChanged("PositionLongitudeDeg");
            }
        }
        private string location;
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                NotifyPropertyChanged("Location");
            }
        }
        private double positionLatitudeDeg;
        public double PositionLatitudeDeg
        {
            get
            { return positionLatitudeDeg; }
            set
            {
                positionLatitudeDeg = value;
                NotifyPropertyChanged("PositionLatitudeDeg");
            }
        }


        #endregion

        public ServerModel(ITelentClient telentClient)
        {
            this.client = telentClient;
            isClientConnected = false;
            isClientDisConnected = true;
            AirspeedSpeed = AltimeterAltitude = AltitudeInternalRolDeg = AltitudeInternalPitchDeg = HeadingDeg = GpsGroundSpeed = GpsVerticalSpeed = 0;
            PositionLatitudeDeg = 32.002644;
            PositionLongitudeDeg = 34.888781;
            Location = "32.00264, 34.888781";
            FlightLogs = "";
        }
        public void Connect(string ip, int port)
        {
            try
            {
                client.Connect(ip, port);
                isClientConnected = true;

                IsClientConnected = true;
                IsClientDisConnected = false;
                FlightLogs = "connected";
            }
            catch (System.Net.Sockets.SocketException)
            {
                FlightLogs = "Unable to connect to server, please try again";
            }
        }
        public void Disconnect()
        {
            isClientConnected = false;
            IsClientConnected = false;
            IsClientDisConnected = true;
            FlightLogs = "Disconnection from the server succeeded";
            client.Disconnect();

        }
        public async Task Start()
        {
            while (isClientConnected)
            {
                lock (obj)
                {
                    HeadingDeg = ReadFromSimulator("/instrumentation/heading-indicator/indicated-heading-deg", headingDeg, "headingDeg");
                    GpsVerticalSpeed = ReadFromSimulator("/instrumentation/gps/indicated-vertical-speed", gpsVerticalSpeed, "gpsVerticalSpeed");
                    GpsGroundSpeed = ReadFromSimulator("/instrumentation/gps/indicated-ground-speed-kt", gpsGroundSpeed, "gpsGroundSpeed");
                    AirspeedSpeed = ReadFromSimulator("/instrumentation/airspeed-indicator/indicated-speed-kt", airspeedSpeed, "airspeedSpeed");
                    GpsAltitude = ReadFromSimulator("/instrumentation/gps/indicated-altitude-ft", gpsAltitude, "gpsAltitude");
                    AltitudeInternalRolDeg = ReadFromSimulator("/instrumentation/attitude-indicator/internal-roll-deg", altitudeInternalRolDeg, "altitudeInternalRolDeg");
                    AltitudeInternalPitchDeg = ReadFromSimulator("/instrumentation/attitude-indicator/internal-pitch-deg", altitudeInternalPitchDeg, "altitudeInternalPitchDeg");
                    AltimeterAltitude = ReadFromSimulator("/instrumentation/altimeter/indicated-altitude-ft", altimeterAltitude, "altimeterAltitude");

                    // map property
                    double tempPositionLatitudeDeg = ReadFromSimulator("/position/latitude-deg", positionLatitudeDeg, "positionLatitudeDeg");
                    if (tempPositionLatitudeDeg < -90)
                    {
                        FlightLogs = "The plane reached the minimum LatitudeDeg";
                        PositionLatitudeDeg = -90;
                    }
                    else if (tempPositionLatitudeDeg > 90)
                    {
                        FlightLogs = "The plane reached the maximum LatitudeDeg";
                        PositionLatitudeDeg = 90;
                    }
                    else
                    {
                        PositionLatitudeDeg = tempPositionLatitudeDeg;
                    }
                    double tempPositionLongitudeDeg = ReadFromSimulator("/position/longitude-deg", positionLongitudeDeg, "positionLongitudeDeg");
                    if (tempPositionLongitudeDeg < -180)
                    {
                        FlightLogs = "The plane reached the minimum LongitudeDeg";
                        PositionLongitudeDeg = -180;
                    }
                    else if (tempPositionLongitudeDeg > 180)
                    {
                        FlightLogs = "The plane reached the maximum LongitudeDeg";
                        PositionLongitudeDeg = 180;
                    }
                    else
                    {
                        PositionLongitudeDeg = tempPositionLongitudeDeg;
                    }

                    Location = positionLatitudeDeg + "," + positionLongitudeDeg;

                }
                await Task.Delay(2000);
            }

        }

        public double ReadFromSimulator(string path, double cuurentValue, string paramName)
        {
            double answer = cuurentValue;
            try
            {
                client.Write("get " + path + "\n");
                string a = client.Read();
                answer = Double.Parse(a);
            }
            catch (System.FormatException)
            {
                answer = cuurentValue;
                FlightLogs = "Server internal error. param: " + paramName + " failed. The previous value is displayed";
            }
            catch (ObjectDisposedException e)
            {
                FlightLogs = e.Message;
            }
            catch (IOException)
            {
                FlightLogs = "Time out exception - failed reading from server: " + paramName;
            }
            catch (Exception e)
            {
                FlightLogs = "General - error" + paramName + " from simulator " + e.Message;
            }
            return answer;
        }

        public void WriteToSimulator(string path, double val, string paramName)
        {
            if (isClientConnected)
            {
                lock (obj)
                {
                    try
                    {
                        client.Write("set " + path + " " + val + "\n");
                        String a = client.Read();
                        if (a.Contains("ERR"))
                        {
                            FlightLogs = "Server internal error. param: " + paramName + " failed. The previous value is displayed";
                        }
                    }
                    catch (System.IO.IOException)
                    {
                        FlightLogs = "Time out exception - failed writing to server: " + paramName;
                    }
                    catch (Exception e)
                    {
                        FlightLogs = "General - error" + paramName + " from simulator " + e.Message;
                    }
                }
            }
        }

        public async Task SetRudder(double rudder)
        {
            await Task.Run(() => WriteToSimulator("/controls/flight/rudder", rudder, "rudder"));
        }

        public async Task SetThrottle(double throttle)
        {
            await Task.Run(() => WriteToSimulator("/controls/engines/current-engine/throttle", throttle, "throttle"));
        }


        public async Task SetAileron(double aileron)
        {
            await Task.Run(() => WriteToSimulator("/controls/flight/aileron", aileron, "aileron"));
        }

        public async Task SetElevator(double elevator)
        {

            await Task.Run(() => WriteToSimulator("/controls/flight/elevator", elevator, "elevator"));
        }

        public void Setflightlogs(string value)
        {
            FlightLogs = value;
        }
    }
}

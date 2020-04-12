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
        public event PropertyChangedEventHandler PropertyChanged = null;
        private ITelentClient client;
        volatile Boolean isClientConnected;
        volatile Boolean isClientDisConnected;
        private readonly Mutex mutex = new Mutex();

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
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
        public double GpsVerticalSpeed {
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
            set { isClientConnected = value;
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
        public double GpsGroundSpeed {
            get
            { return gpsGroundSpeed; }
            set
            {
                gpsGroundSpeed = value;
                NotifyPropertyChanged("GpsGroundSpeed");
            }
        }
        private double airspeedSpeed;
        public double AirspeedSpeed {
            get
            { return airspeedSpeed; }
            set
            {
                airspeedSpeed = value;
                NotifyPropertyChanged("AirspeedSpeed");
            }
        }
        private double gpsAltitude;
        public double GpsAltitude {
            get
            { return gpsAltitude; }
            set
            {
                gpsAltitude = value;
                NotifyPropertyChanged("GpsAltitude");
            }
        }
        private double altitudeInternalRolDeg;
        public double AltitudeInternalRolDeg {
            get
            { return altitudeInternalRolDeg; }
            set
            {
                altitudeInternalRolDeg = value;
                NotifyPropertyChanged("AltitudeInternalRolDeg");
            }
        }
        private double altitudeInternalPitchDeg;
        public double AltitudeInternalPitchDeg {
            get
            { return altitudeInternalPitchDeg; }
            set
            {
                altitudeInternalPitchDeg = value;
                NotifyPropertyChanged("AltitudeInternalPitchDeg");
            }
        }
        private double altimeterAltitude;
        public double AltimeterAltitude {
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
        public void connect(string ip, int port)
        {
            try
            {
                client.connect(ip, port);
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
        public void disconnect()
        {
            
            isClientConnected = false;
            IsClientConnected = false;
            IsClientDisConnected = true;
            FlightLogs = "Disconnection from the server succeeded";
            client.disconnect();
           
        }
        public async Task start()
        {
            /*new Thread(delegate()
            {*/
                while (isClientConnected)
                {
                    mutex.WaitOne();
                    try
                    {
                        HeadingDeg = await Task.Run(()=> ReadFromSimulator("/instrumentation/heading-indicator/indicated-heading-deg", headingDeg, "headingDeg"));
                        GpsVerticalSpeed = await Task.Run(() => ReadFromSimulator("/instrumentation/gps/indicated-vertical-speed", gpsVerticalSpeed, "gpsVerticalSpeed"));
                        GpsGroundSpeed = await Task.Run(() => ReadFromSimulator("/instrumentation/gps/indicated-ground-speed-kt", gpsGroundSpeed, "gpsGroundSpeed"));
                        AirspeedSpeed = await Task.Run(() => ReadFromSimulator("/instrumentation/airspeed-indicator/indicated-speed-kt", airspeedSpeed, "airspeedSpeed"));
                        GpsAltitude = await Task.Run(() => ReadFromSimulator("/instrumentation/gps/indicated-altitude-ft", gpsAltitude, "gpsAltitude"));
                        AltitudeInternalRolDeg = await Task.Run(() => ReadFromSimulator("/instrumentation/attitude-indicator/internal-roll-deg", altitudeInternalRolDeg, "altitudeInternalRolDeg"));
                        AltitudeInternalPitchDeg = await Task.Run(() => ReadFromSimulator("/instrumentation/attitude-indicator/internal-pitch-deg", altitudeInternalPitchDeg, "altitudeInternalPitchDeg"));
                        AltimeterAltitude = await Task.Run(() => ReadFromSimulator("/instrumentation/altimeter/indicated-altitude-ft", altimeterAltitude, "altimeterAltitude"));



                        // map property
                        double tempPositionLatitudeDeg = await Task.Run(() => ReadFromSimulator("/position/latitude-deg", positionLatitudeDeg, "positionLatitudeDeg"));
                            if (tempPositionLatitudeDeg< -90)
                            {
                                PositionLatitudeDeg = -90;
                            }
                            else if (tempPositionLatitudeDeg > 90)
                            {
                                PositionLatitudeDeg = 90;
                            }
                            else
                            {
                                PositionLatitudeDeg = tempPositionLatitudeDeg;
                            }
                        double tempPositionLongitudeDeg = await Task.Run(() => ReadFromSimulator("/position/longitude-deg", positionLongitudeDeg, "positionLongitudeDeg"));
                            if (tempPositionLongitudeDeg < -180)
                            {
                                PositionLongitudeDeg = -180;
                            }
                            else if (tempPositionLongitudeDeg > 180)
                            {
                                PositionLongitudeDeg = 180;
                            }
                            else
                            {
                                PositionLongitudeDeg = tempPositionLongitudeDeg;
                            }
                    Console.WriteLine("lat "+PositionLatitudeDeg);
                    Console.WriteLine("long "+PositionLongitudeDeg);

                    Location = positionLatitudeDeg + "," + positionLongitudeDeg;

                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                await Task.Delay(2000);                    
                }
           /* }).Start();*/
        }

        public  double ReadFromSimulator(string path, double cuurentValue, string paramName)
        {
            String a = "";
            Double answer = cuurentValue;
            try
            {
                client.write("get " + path + "\n");
            }
            catch (Exception)
            {
                
                FlightLogs = "TIME OUT - can not read "+paramName+" from simulator " ;
            }
            try
            {
                 a = client.read();
            } catch (Exception)
            {
                FlightLogs = "TIME OUT - can not read " + paramName + " from simulator ";
            }
            try
            {
                answer = Double.Parse(a);
            }
            catch (System.FormatException )
            {
                answer = cuurentValue;
                FlightLogs = "Error - can not get " + paramName + " value. The previous value is displayed";
            }
        
            return answer;
        }

        public void WriteToSimulator(string path, double val, string paramName)
        {
            if (isClientConnected)

            {
                mutex.WaitOne();
                
                try
                {
                    client.write("set " + path + " " + val + "\n");
                    String a = client.read();
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                }
                catch (System.IO.IOException )
                {
                    FlightLogs = "TIME OUT- can not write" + paramName+ "to simulator";
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
               
            }
            
        }

        public async Task setRudder(double rudder)
        {
            await Task.Run(() => WriteToSimulator("/controls/flight/rudder", rudder,"rudder"));
        }

        public async Task setThrottle(double throttle)
        {
            await Task.Run(() => WriteToSimulator("/controls/engines/current-engine/throttle", throttle, "throttle"));
        }


        public async Task setAileron(double aileron)
        {
            await Task.Run(() => WriteToSimulator("/controls/flight/aileron", aileron, "aileron"));
        }

        public async Task setElevator(double elevator)
        {

            await Task.Run(() => WriteToSimulator("/controls/flight/elevator", elevator, "elevator"));
        }

        public void setflightlogs(string value)
        {
            FlightLogs = value;
        }
    }
}

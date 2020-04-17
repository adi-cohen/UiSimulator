using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulatorApp.Model
{
    class SimulatorTelnetClient : ITelentClient
    {
        public TcpClient Client { get; set; }
        public NetworkStream Stream { get; set; }


        void ITelentClient.Connect(string ip, int port)
        {
            //System.Net.Sockets.SocketException
            Client = new TcpClient(ip, port)
            {
                ReceiveTimeout = 10000,
                SendTimeout = 10000
            };
            this.Stream = Client.GetStream();

        }

        void ITelentClient.Disconnect()
        {
            // Close everything.
            Stream.Close();
            Client.Close();
        }

        string ITelentClient.Read()
        {
            // Buffer to store the response bytes.
            Byte[] data = new Byte[256];

            // Read the first batch of the TcpServer response bytes

            Int32 bytes = Stream.Read(data, 0, data.Length);
            // String to store the response ASCII representation.
            string responseData = Encoding.ASCII.GetString(data, 0, bytes);

            return responseData;
        }

        void ITelentClient.Write(string command)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(command);
            Stream.Write(data, 0, data.Length);
        }
    }
}

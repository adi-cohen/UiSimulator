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
        
    
        void ITelentClient.connect(string ip, int port)
        {
            //System.Net.Sockets.SocketException
            Client = new TcpClient(ip, port);
            Client.ReceiveTimeout = 10000;
            Client.SendTimeout = 10000;
            this.Stream = Client.GetStream();
            
            
        }

        void ITelentClient.disconnect()
        {
            // Close everything.
            Stream.Close();
            Client.Close();
        }

        string ITelentClient.read()
        {
            // Buffer to store the response bytes.
            Byte[] data=new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes
            
                Int32 bytes = Stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
           
            return responseData;
        }

        void ITelentClient.write(string command)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(command);
            Stream.Write(data, 0, data.Length);
        }
    }
}

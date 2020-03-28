using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Model
{
    class SimulatorTelnetClient : ITelentClient
    {
        public TcpClient Client { get; set; }
        public NetworkStream Stream { get; set; }
        
    
        void ITelentClient.connect(string ip, int port)
        {
            Client = new TcpClient(ip, port);
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

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = Stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);
            return responseData;
        }

        void ITelentClient.write(string command)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(command);
            Stream.Write(data, 0, data.Length);
            Console.WriteLine("Sent: {0}", command);
        }
    }
}

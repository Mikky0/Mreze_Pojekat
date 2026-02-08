using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces.Client;

namespace Client.Services
{
    public class ClientConnection : IClientConnection
    {
        public void Connect()
        {
            try
            {
                UdpClient udpClient = new UdpClient();
                udpClient.Connect("127.0.0.1", 9000);

                byte[] data = Encoding.UTF8.GetBytes("CONNECT");
                udpClient.Send(data, data.Length);

                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] responseData = udpClient.Receive(ref endPoint);
                string[] serverInfo = Encoding.UTF8.GetString(responseData).Split(':');

                Program.tcpClient = new TcpClient();
                Program.tcpClient.Connect(IPAddress.Parse(serverInfo[0]), int.Parse(serverInfo[1]));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri povezivanju: {ex.Message}");
                throw;
            }
        }
    }
}

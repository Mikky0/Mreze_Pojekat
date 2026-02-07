using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Common.DTO;

namespace Common.Models
{
    public class ServerData
    {
        public static Dictionary<int, Parking> Parkinzi { get; set; } = new Dictionary<int, Parking>();
        public static Dictionary<int, Zauzece> Zauzeća { get; set; } = new Dictionary<int, Zauzece>();
        public static int NextZahtevId { get; set; } = 1000;
        public static UdpClient UdpServer { get; set; } = new UdpClient();
        public static TcpListener TcpListener { get; set; } = new TcpListener(IPAddress.Any, TcpPort);
        public static List<TcpClient> Clients { get; set; } = new List<TcpClient>();
        public static bool Running { get; set; } = true;
        public static int TcpPort { get; set; } = 9001;

        public ServerData() { }

        public ServerData(Dictionary<int, Parking> parkinzi, Dictionary<int, Zauzece> zauzeća, int nextZahtevId, UdpClient udpServer, TcpListener tcpListener, List<TcpClient> clients, bool running)
        {
            Parkinzi = parkinzi;
            Zauzeća = zauzeća;
            NextZahtevId = nextZahtevId;
            UdpServer = udpServer;
            TcpListener = tcpListener;
            Clients = clients;
            Running = running;
        }

        public static int GetNextAvailablePort()
        {
            int port = TcpPort;
            bool isAvailable = false;

            while (!isAvailable)
            {
                try
                {
                    // Test if port is available
                    TcpListener testListener = new TcpListener(IPAddress.Any, port); port++;
                    testListener.Start();
                    testListener.Stop();
                    isAvailable = true;

                }
                catch (SocketException)
                {
                    port++;
                }
            }

            TcpPort = port + 1;
            return port;
        }
    }
}

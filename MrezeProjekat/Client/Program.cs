using Client.Services;
using Common.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        public static TcpClient tcpClient = new TcpClient();
        public static Dictionary<int, int> aktivnaZauzeca = new Dictionary<int, int>();

        static IClientConnection clientConnection = new ClientConnection();

        public static void Main()
        {
            try
            {
                clientConnection.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.ReadLine();
                return;
            }
        }
    }
}

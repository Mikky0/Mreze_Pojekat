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
        static IParkingOperations parkingOperations = new ParkingOperations();
        static IServerCommunication serverCommunication = new ServerCommunication();

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

        private static void StartMainLoop()
        {
            while (true)
            {
                Console.WriteLine("\nIzaberite opciju:");
                Console.WriteLine("1. Zauzmi parking");
                Console.WriteLine("2. Oslobodi parking");
                Console.WriteLine("3. Izlaz");

                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        parkingOperations.ZauzmiParking();
                        break;
                    case "2":
                        parkingOperations.OslobodiParking();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Neispravan unos, pokušajte ponovo.");
                        continue;
                }

                serverCommunication.ReceiveServerResponse();
            }
        }
    }
}

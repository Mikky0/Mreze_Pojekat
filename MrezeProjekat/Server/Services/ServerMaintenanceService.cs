using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces.Server;
using Common.Models;

namespace Server.Services
{
    public class ServerMaintenanceService : IServerMaintanceService
    {
        static IConnectionHandler connHandlerService = Program.connHandlerService;

        public void Start()
        {
            Console.Write("Unesite broj parkinga: ");
            int brojParkinga;
            while (!int.TryParse(Console.ReadLine(), out brojParkinga) || brojParkinga <= 0)
            {
                Console.Write("Greška: Unesite validan broj parkinga (veći od 0): ");
            }

            for (int i = 1; i <= brojParkinga; i++)
            {
                int ukupnoMesta;
                Console.Write($"Unesite ukupan broj mesta za parking {i}: ");
                while (!int.TryParse(Console.ReadLine(), out ukupnoMesta) || ukupnoMesta <= 0)
                {
                    Console.Write($"Greška: Unesite validan broj mesta za parking {i} (veći od 0): ");
                }

                decimal cenaPoSatu;
                Console.Write($"Unesite cenu po satu za parking {i}: ");
                while (!decimal.TryParse(Console.ReadLine(), out cenaPoSatu) || cenaPoSatu <= 0)
                {
                    Console.Write($"Greška: Unesite validnu cenu po satu za parking {i} (veću od 0): ");
                }

                ServerData.Parkinzi[i] = new Parking
                {
                    UkupnoMesta = ukupnoMesta,
                    ZauzetaMesta = 0,
                    CenaPoSatu = cenaPoSatu
                };
            }

            ServerData.UdpServer = new UdpClient(9000);
            ServerData.TcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 9001);
            ServerData.TcpListener.Start();

            Thread udpThread = new Thread(connHandlerService.HandleUdpClients);
            udpThread.Start();

            Console.WriteLine("\nParking Server je pokrenut...\n");

            Task.Run(() =>
            {
                while (ServerData.Running)
                {
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                    {
                        ServerData.Running = false;
                        break;
                    }
                }
            });


            try
            {
                while (ServerData.Running)
                {
                    connHandlerService.HandleTcpClients();
                }
            }
            catch (SocketException) when (!ServerData.Running)
            {
                // Expected exception during shutdown
            }

            CleanupResources();
        }

        public void Stop()
        {
            Console.WriteLine();
            ServerData.Running = false;

            HashSet<int> prikazaniParkinzi = new HashSet<int>();


            foreach (var parking in ServerData.Parkinzi)
            {
                if (prikazaniParkinzi.Contains(parking.Key))
                    continue;

                prikazaniParkinzi.Add(parking.Key);

                int maxVozila = parking.Value.VozilaPoSatu.Count > 0
                    ? parking.Value.VozilaPoSatu.Values.Max()
                    : 0;

                Console.WriteLine($"Parking {parking.Key}: Najveći broj vozila u jednom satu: {maxVozila}");
            }
        }



        public void CleanupResources()
        {
            var running = ServerData.Running;
            var udpServer = ServerData.UdpServer;
            var tcpListener = ServerData.TcpListener;
            var clients = ServerData.Clients;

            foreach (var client in clients)
            {
                try { client.Close(); } catch { }
            }

            try { tcpListener.Stop(); ServerData.UdpServer.Close(); } catch { }
        }
    }
}

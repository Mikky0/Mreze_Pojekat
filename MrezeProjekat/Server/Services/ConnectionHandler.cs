using Common.Interfaces.Server;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ConnectionHandler : IConnectionHandler
    {
        IServerMessageService messageService = Program.messageService;
        IParkingService parkingService = Program.parkingService;

        public void HandleTcpClients()
        {
            var clients = ServerData.Clients;
            var tcpListener = ServerData.TcpListener;

            List<TcpClient> disconnectedClients = new List<TcpClient>();
            List<Socket> checkSockets = clients.Select(c => c.Client).ToList();
            checkSockets.Add(tcpListener.Server);

            Socket.Select(checkSockets, null, null, 1000);

            foreach (Socket socket in checkSockets)
            {
                if (socket == tcpListener.Server)
                {
                    TcpClient newClient = tcpListener.AcceptTcpClient();

                    if (newClient.Connected)
                    {
                        clients.Add(newClient);
                        messageService.SendParkingStatus(newClient);
                    }

                }
                else
                {
                    TcpClient client = clients.First(c => c.Client == socket);
                    try
                    {
                        messageService.ProcessClientMessage(client);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("[TCP]: Klijent je prekinuo vezu sa serverom.");
                        disconnectedClients.Add(client);
                    }
                }
            }

            foreach (TcpClient client in disconnectedClients)
            {
                clients.Remove(client);
                client.Close();
            }
        }

        public void HandleUdpClients()
        {
            var running = ServerData.Running;
            var udpServer = ServerData.UdpServer;
            var tcpListener = ServerData.TcpListener;

            try
            {
                while (running)
                {
                    int newPort = ServerData.GetNextAvailablePort();
                    IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, newPort);
                    byte[] receivedData = udpServer.Receive(ref clientEndPoint);
                    string message = Encoding.UTF8.GetString(receivedData);

                    if (message == "CONNECT")
                    {
                        string response = $"127.0.0.1:{((IPEndPoint)tcpListener.LocalEndpoint).Port}";
                        byte[] responseData = Encoding.UTF8.GetBytes(response);
                        udpServer.Send(responseData, responseData.Length, clientEndPoint);
                        Console.WriteLine($"[UDP]: Povezan je novi klijent i poslata je TCP Socket ({clientEndPoint.Address}:{clientEndPoint.Port})");
                    }
                }
            }
            catch (SocketException) when (!running) { }
            catch { }
        }
    }
}

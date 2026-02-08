using Common.DTO;
using Common.Interfaces.Server;
using Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ServerMessageService : IServerMessageService
    {
        static IParkingService parkingService = Program.parkingService;

        public void ProcessClientMessage(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            if (stream.DataAvailable)
            {
                byte[] buffer = new byte[4096];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonString = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                try
                {
                    if (jsonString.StartsWith("Oslobađam:"))
                    {
                        parkingService.ProcessOslobadjanje(jsonString, client);
                    }
                    else
                    {
                        var zauzece = JsonConvert.DeserializeObject<Zauzece>(jsonString);


                        if (zauzece != null)
                            parkingService.ProcessZauzece(zauzece, client);
                        else
                            throw new Exception("No data provided");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Greška: {ex.Message}");
                }
            }
        }

        public void SendMessage(TcpClient client, string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.GetStream().Write(data, 0, data.Length);
        }

        public void SendParkingStatus(TcpClient client)
        {
            var status = ServerData.Parkinzi.Select(p => $"Parking {p.Key}: {p.Value.ZauzetaMesta}/{p.Value.UkupnoMesta}").ToList();
            string message = string.Join("\n", status);
            byte[] data = Encoding.UTF8.GetBytes(message);

            client.GetStream().Write(data, 0, data.Length);
        }
    }
}

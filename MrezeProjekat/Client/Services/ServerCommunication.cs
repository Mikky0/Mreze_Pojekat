using Common.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class ServerCommunication : IServerCommunication
    {
        public void ReceiveInitialStatus()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = Program.tcpClient.GetStream().Read(buffer, 0, buffer.Length);
                string status = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Status parkinga:");
                Console.WriteLine(status);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri prijemu početnog statusa: {ex.Message}");
                throw;
            }
        }

        public void ReceiveServerResponse()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = Program.tcpClient.GetStream().Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Server odgovara: {response}");

                if (response.Contains("Zahtev"))
                {
                    string[] parts = response.Split(':');
                    int zahtevId = int.Parse(parts[0].Split(' ')[1]);
                    int parkingBroj = int.Parse(parts[1].Trim().Split(' ')[1]);
                    Program.aktivnaZauzeca[zahtevId] = parkingBroj;
                    Console.WriteLine($"Zapamtite broj zahteva: {zahtevId} za parking {parkingBroj}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri prijemu odgovora sa servera: {ex.Message}");
            }
        }
    }
}

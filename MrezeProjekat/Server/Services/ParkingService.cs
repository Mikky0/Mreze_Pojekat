using Common.DTO;
using Common.Interfaces.Server;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ParkingService : IParkingService
    {
        IServerMessageService messageService => Program.messageService;


        public void ProcessOslobadjanje(string message, TcpClient client)
        {
            int zahtevId = int.Parse(message.Split(':')[1].Trim());
            if (ServerData.Zauzeća.ContainsKey(zahtevId))
            {
                var zauzece = ServerData.Zauzeća[zahtevId];
                var parking = ServerData.Parkinzi[zauzece.BrojParkinga];
                parking.ZauzetaMesta -= zauzece.BrojMesta;

                TimeSpan trajanje = DateTime.Now - DateTime.Parse(zauzece.VremenskiTrenutak);
                int sati = (int)Math.Ceiling(trajanje.TotalMinutes);
                decimal racun = sati * parking.CenaPoSatu * zauzece.BrojMesta;

                messageService.SendMessage(client, $"Račun: {racun:C}");
                ServerData.Zauzeća.Remove(zahtevId);

                Console.WriteLine($"[Parking Servis]: Parking {zauzece.BrojParkinga}: {parking.ZauzetaMesta}/{parking.UkupnoMesta}");
                Console.WriteLine($"[Parking Servis]: Račun iznosi {racun:C} RSD");
            }
            else
                messageService.SendMessage(client, "Nevalidan id za oslobadjanje");
        }



        public void ProcessZauzece(Zauzece zauzece, TcpClient client)
        {
            if (!ServerData.Parkinzi.ContainsKey(zauzece.BrojParkinga))
            {
                messageService.SendMessage(client, "Nevalidan broj parkinga");
                return;
            }

            var parking = ServerData.Parkinzi[zauzece.BrojParkinga];
            int dostupnaMesta = parking.UkupnoMesta - parking.ZauzetaMesta;
            int mestaZaZauzimanje = Math.Min(zauzece.BrojMesta, dostupnaMesta);

            if (mestaZaZauzimanje > 0)
            {
                zauzece.ZahtevId = ++ServerData.NextZahtevId;
                zauzece.BrojMesta = mestaZaZauzimanje;
                ServerData.Zauzeća[zauzece.ZahtevId] = zauzece;
                parking.ZauzetaMesta += mestaZaZauzimanje;

                int sat = DateTime.Parse(zauzece.VremenskiTrenutak).Hour;
                if (!parking.VozilaPoSatu.ContainsKey(sat))
                    parking.VozilaPoSatu[sat] = 0;
                parking.VozilaPoSatu[sat] += mestaZaZauzimanje;

                messageService.SendMessage(client, $"Zahtev {zauzece.ZahtevId}: zauzeto {mestaZaZauzimanje} mesta");
                Console.WriteLine($"[Parking Servis]: ID = {zauzece.ZahtevId}, Zauzeto {mestaZaZauzimanje} mesta");
            }
            else
            {
                messageService.SendMessage(client, "Nema dostupnih mesta");
            }
        }
    }
}

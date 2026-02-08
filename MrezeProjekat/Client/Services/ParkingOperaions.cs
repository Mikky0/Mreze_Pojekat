using Common.DTO;
using Common.Interfaces.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class ParkingOperations : IParkingOperations
    {
        public void ZauzmiParking()
        {
            try
            {
                Zauzece zauzece = new Zauzece();

                Console.Write("Unesite broj parkinga: ");
                zauzece.BrojParkinga = int.Parse(Console.ReadLine() ?? "");

                Console.Write("Unesite broj mesta za zauzimanje: ");
                zauzece.BrojMesta = int.Parse(Console.ReadLine() ?? "");

                zauzece.VremenskiTrenutak = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (zauzece.BrojMesta == 1)
                {
                    Console.Write("Unesite proizvođača: ");
                    zauzece.Proizvodjac = Console.ReadLine() ?? "";

                    Console.Write("Unesite model: ");
                    zauzece.Model = Console.ReadLine() ?? "";

                    Console.Write("Unesite boju: ");
                    zauzece.Boja = Console.ReadLine() ?? "";

                    Console.Write("Unesite registarski broj: ");
                    zauzece.RegistarskiBroj = Console.ReadLine() ?? "";
                }

                string jsonString = JsonConvert.SerializeObject(zauzece);
                byte[] data = Encoding.UTF8.GetBytes(jsonString);
                Program.tcpClient.GetStream().Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri zauzimanju parkinga: {ex.Message}");
            }
        }

        public void OslobodiParking()
        {
            try
            {
                if (Program.aktivnaZauzeca.Count == 0)
                {
                    Console.WriteLine("Nema aktivnih zauzeća.");
                    return;
                }

                Console.WriteLine("\nOdaberite broj parkinga za oslobađanje:");
                int index = 1;
                Dictionary<int, int> izborMap = new Dictionary<int, int>();

                foreach (var entry in Program.aktivnaZauzeca)
                {
                    Console.WriteLine($"{index}. Broj zauzetih mesta: {entry.Value} (ID: {entry.Key})");
                    izborMap[index] = entry.Key;
                    index++;
                }

                Console.Write("Izbor: ");
                if (int.TryParse(Console.ReadLine(), out int izbor) && izborMap.ContainsKey(izbor))
                {
                    int zahtevId = izborMap[izbor];
                    string message = $"Oslobađam: {zahtevId}";
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    Program.tcpClient.GetStream().Write(data, 0, data.Length);
                    Program.aktivnaZauzeca.Remove(zahtevId);
                    Console.WriteLine("Parking uspešno oslobođen.");
                }
                else
                {
                    Console.WriteLine("Neispravan izbor.");
                    OslobodiParking();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri oslobađanju parkinga: {ex.Message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class Zauzece
    {
        public int BrojParkinga { get; set; } = 0;
        public int BrojMesta { get; set; } = 0;
        public string VremenskiTrenutak { get; set; } = string.Empty;
        public string Proizvodjac { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Boja { get; set; } = string.Empty;
        public string RegistarskiBroj { get; set; } = string.Empty;
        public int ZahtevId { get; set; } = 0;

        public Zauzece() { }

        public Zauzece(int brojParkinga, int brojMesta, string vremenskiTrenutak, string proizvodjac, string model, string boja, string registarskiBroj, int zahtevId)
        {
            BrojParkinga = brojParkinga;
            BrojMesta = brojMesta;
            VremenskiTrenutak = vremenskiTrenutak;
            Proizvodjac = proizvodjac;
            Model = model;
            Boja = boja;
            RegistarskiBroj = registarskiBroj;
            ZahtevId = zahtevId;
        }
    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Parking
    {
        public int UkupnoMesta { get; set; }
        public int ZauzetaMesta { get; set; }
        public decimal CenaPoSatu { get; set; }
        public Dictionary<int, int> VozilaPoSatu { get; set; } = new Dictionary<int, int>();


        public Parking() { }

        public Parking(int ukupnoMesta, int zauzetaMesta, decimal cenaPoSatu, Dictionary<int, int> vozilaPoSatu)
        {
            UkupnoMesta = ukupnoMesta;
            ZauzetaMesta = zauzetaMesta;
            CenaPoSatu = cenaPoSatu;
            VozilaPoSatu = vozilaPoSatu;
        }
    }
}

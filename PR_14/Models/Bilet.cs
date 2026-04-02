using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_14.Models
{
    public class Bilet
    {
        public int Id { get; set; }
        public int SeansId { get; set; }
        public int PolzovatelId { get; set; }
        public int Mesto { get; set; }
        public DateTime DataPokupki { get; set; }
        public string FilmNazvanie { get; set; }
        public int ZalNomer { get; set; }
        public DateTime DataSeansa { get; set; }
        public TimeSpan Vremya { get; set; }
        public decimal Cena { get; set; }
    }
}

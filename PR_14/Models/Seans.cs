using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_14.Models
{
    public class Seans
    {
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int ZalId { get; set; }
        public DateTime DataSeansa { get; set; }
        public TimeSpan Vremya { get; set; }
        public decimal Cena { get; set; }
        public string FilmNazvanie { get; set; }
        public int ZalNomer { get; set; }
    }
}

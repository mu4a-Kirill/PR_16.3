using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR_14.Models
{
    public class Polzovatel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Parol { get; set; }
        public string Imya { get; set; }
        public string Familiya { get; set; }
        public int Vozrast { get; set; }
        public string Email { get; set; }
    }
}

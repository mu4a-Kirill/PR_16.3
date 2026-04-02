using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace PR_14.Models
    {
        public class Film
        {
            public int Id { get; set; }
            public string Nazvanie { get; set; }
            public string Opisanie { get; set; }
            public decimal Reyting { get; set; }
            public int VozrastnoyReyting { get; set; }
            public DateTime DataNachala { get; set; }
            public string Oblozhka { get; set; }
        }
    }
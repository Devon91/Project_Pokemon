using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Pokemon_Models
{
    public class PokedexEntry
    {
        public int PokedexNumber { get; set; }
        public string Name { get; set; }
        public bool Caught { get; set; }
        public bool Encountered { get; set; }
    }
}

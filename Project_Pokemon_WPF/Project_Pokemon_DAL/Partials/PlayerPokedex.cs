using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Pokemon_DAL
{
    public partial class PlayerPokedex
    {
        public override bool Equals(object obj)
        {
            return obj is PlayerPokedex pokedex &&
                   PokedexId == pokedex.PokedexId &&
                   PlayerId == pokedex.PlayerId;
        }

        public override int GetHashCode()
        {
            var hashCode = 1524720837;
            hashCode = hashCode * -1521134295 + PokedexId.GetHashCode();
            hashCode = hashCode * -1521134295 + PlayerId.GetHashCode();
            return hashCode;
        }
    }
}

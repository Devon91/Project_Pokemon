using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Pokemon_DAL
{
    public partial class PokemonAttack
    {
        public override bool Equals(object obj)
        {
            return obj is PokemonAttack attack &&
                   PokedexId == attack.PokedexId &&
                   AttackId == attack.AttackId;
        }

        public override int GetHashCode()
        {
            var hashCode = 1257390416;
            hashCode = hashCode * -1521134295 + PokedexId.GetHashCode();
            hashCode = hashCode * -1521134295 + AttackId.GetHashCode();
            return hashCode;
        }
    }
}

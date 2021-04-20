using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Pokemon_DAL
{
    public partial class OwnedPokemonAttack
    {
        public override bool Equals(object obj)
        {
            return obj is OwnedPokemonAttack attack &&
                   AttackId == attack.AttackId;
        }

        public override int GetHashCode()
        {
            var hashCode = -319405917;
            hashCode = hashCode * -1521134295 + AttackId.GetHashCode();
            return hashCode;
        }
    }
}

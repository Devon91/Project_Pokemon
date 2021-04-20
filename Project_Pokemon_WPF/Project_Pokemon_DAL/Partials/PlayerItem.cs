using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Pokemon_DAL
{
    public partial class PlayerItem
    {
        public override bool Equals(object obj)
        {
            return obj is PlayerItem item &&
                   ItemId == item.ItemId;
        }

        public override int GetHashCode()
        {
            return -2113648141 + ItemId.GetHashCode();
        }
    }
}

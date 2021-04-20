using Project_Pokemon_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Pokemon_Models
{
    public static class PlayerInformation
    {
        public static int PlayerId { get; set; }

        public static void ReducePlayerPokedollar(Player player, int substractAmount)
        {
            // speler zijn pokedollars verminderen
            int money = player.Pokedollar - substractAmount;
            player.Pokedollar = money;
            DatabaseOperations.UpdatePlayer(player);
        }

        public static void IncreasePlayerPokedollar(Player player, int addAmount)
        {
            // speler zijn pokedollars verhogen
            int money = player.Pokedollar + addAmount;
            player.Pokedollar = money;
            DatabaseOperations.UpdatePlayer(player);
        }

    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project_Pokemon_DAL;
using Project_Pokemon_Models;

namespace Pokemon_UnitTests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void IncreasePlayerPokedollar_SellItemPriceIs100_PlayerPokedollarIncreasedBySellPrice()
        {
            //Arrange
            Player p = DatabaseOperations.GetPlayer(2);
            p.Pokedollar = 9900;

            PlayerInformation.IncreasePlayerPokedollar(p, 100);

            Assert.AreEqual(p.Pokedollar, 10000);
        }

        [TestMethod]
        public void ReducePlayerPokedollar_PlayerPokecoinGreaterThanBuyPrice_PlayerPokedollarReducedByBuyPrice()
        {
            Player p = DatabaseOperations.GetPlayer(2);
            p.Pokedollar = 10000;

            PlayerInformation.ReducePlayerPokedollar(p, 100);

            Assert.AreEqual(p.Pokedollar, 9900);
        }

        [TestMethod]
        public void Name_ValueIsNotEmpty_VoornaamIsEqualToValue()
        {
            Player p = DatabaseOperations.GetPlayer(2);
            p.Name = "Bram";

            Assert.IsTrue("Bram" == p.Name);

        }

        [TestMethod]
        public void PlayerId_ValueIsLessThanZero_PlayerIdIsEqualToZero()
        {
            Player p = DatabaseOperations.GetPlayer(2);
            PlayerInformation.PlayerId = -12;

            Assert.IsFalse(PlayerInformation.PlayerId == 0);

        }
    }
}

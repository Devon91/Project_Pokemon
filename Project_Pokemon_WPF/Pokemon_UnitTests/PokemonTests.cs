using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project_Pokemon_DAL;

namespace Pokemon_UnitTests
{
    [TestClass]
    public class PokemonTests
    {
        [TestMethod]
        public void CheckHp_CurrentHpIsLowerThan0_HpIs0AndReturnsIsTrueAndFaintedValueIsFainted()
        {
            //Arrange
            Pokemon pokemon = new Pokemon();
            pokemon.CurrentHp = -4;
            pokemon.CalculatedMaxHP = 34;
            bool dead;
            //Act
            dead = pokemon.CheckHp();

            //Assert
            Assert.AreEqual(0 , pokemon.CurrentHp);
            Assert.AreEqual("Fainted", pokemon.Fainted);
            Assert.IsTrue(dead);
        }

        [TestMethod]
        public void CheckHp_CurrentHpIsHigherThanCalculatedMaxHp_CurrentHpEqualsCalculatedMaxHp()
        {
            //Arrange
            Pokemon pokemon = new Pokemon();
            pokemon.CurrentHp = 59;
            pokemon.CalculatedMaxHP = 48;
            bool dead;

            //Act
            dead = pokemon.CheckHp();

            //Assert
            Assert.AreEqual(pokemon.CalculatedMaxHP, pokemon.CurrentHp);
            Assert.AreEqual(pokemon.Fainted, "Alive");
            Assert.IsTrue(!dead);
        }
    }
}

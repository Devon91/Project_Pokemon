using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Pokemon_DAL
{
    public partial class Pokemon
    {


        public int CalculatedMaxHP { get; set; }
        public double CalculatedAttack { get; set; }
        public double CalculatedSpecialAttack { get; set; }
        public double CalculatedDefense { get; set; }
        public double CalculatedSpecialDefense { get; set; }
        public double CalculatedSpeed { get; set; }
        public string Fainted { get; set; }

  

        //methodes

        //algemene berekening
        private int CalculateStat(int baseStat, int variedStat)
        {
            return ((((baseStat + variedStat) * 2) * Level) / 100) + 5;
        }

        //set stats
        public void SetStats()
        {
            //HP (calulateStat + Pokemon.Level + 5)
            SetMaxHp();
            //Attack
            CalculatedAttack = CalculateStat(Pokedex.BaseAttack, Attack);
            //Special Attack
            CalculatedSpecialAttack = CalculateStat(Pokedex.BaseSpecialAttack, SpecialAttack);
            //Defense
            CalculatedDefense = CalculateStat(Pokedex.BaseDefense, Defense);
            //Special Defense
            CalculatedSpecialDefense = CalculateStat(Pokedex.BaseSpecialDefense,SpecialDefense);
            //Speed
            CalculatedSpeed = CalculateStat(Pokedex.BaseSpeed, Speed);
        }


        public void SetMaxHp()
        {
            //HP (calulateStat + Pokemon.Level + 5)
            CalculatedMaxHP = CalculateStat(Pokedex.BaseHp, Hp) + Level + 5;
        }



        public bool CheckHp()
        {
            bool dead = false;
            Fainted = "Alive";

            if (CurrentHp <= 0)
            {
                CurrentHp = 0;
                dead = true;
                Fainted = "Fainted";
            }
            else if(CurrentHp > CalculatedMaxHP)
            {
                CurrentHp = CalculatedMaxHP;       
            }

            return dead;

        }
        



    }
}

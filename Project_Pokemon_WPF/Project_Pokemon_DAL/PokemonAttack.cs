//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project_Pokemon_DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class PokemonAttack
    {
        public int Id { get; set; }
        public int RequiredLevel { get; set; }
        public int PokedexId { get; set; }
        public int AttackId { get; set; }
    
        public virtual Attack Attack { get; set; }
        public virtual Pokedex Pokedex { get; set; }
    }
}

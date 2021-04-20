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
    
    public partial class Pokemon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pokemon()
        {
            this.OwnedPokemonAttacks = new HashSet<OwnedPokemonAttack>();
            this.PlayerPokemons = new HashSet<PlayerPokemon>();
            this.TrainerPokemons = new HashSet<TrainerPokemon>();
        }
    
        public int Id { get; set; }
        public int Level { get; set; }
        public int Hp { get; set; }
        public int CurrentHp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int Speed { get; set; }
        public int PokedexId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OwnedPokemonAttack> OwnedPokemonAttacks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlayerPokemon> PlayerPokemons { get; set; }
        public virtual Pokedex Pokedex { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrainerPokemon> TrainerPokemons { get; set; }
    }
}

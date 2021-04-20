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
    
    public partial class Attack
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Attack()
        {
            this.OwnedPokemonAttacks = new HashSet<OwnedPokemonAttack>();
            this.PokemonAttacks = new HashSet<PokemonAttack>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int BaseDamage { get; set; }
        public int Accuracy { get; set; }
        public int Pp { get; set; }
        public int TypeId { get; set; }
    
        public virtual Type Type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OwnedPokemonAttack> OwnedPokemonAttacks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PokemonAttack> PokemonAttacks { get; set; }
    }
}

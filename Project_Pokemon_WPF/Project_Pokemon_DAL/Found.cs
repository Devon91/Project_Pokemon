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
    
    public partial class Found
    {
        public int Id { get; set; }
        public int MaxLevel { get; set; }
        public int MinLevel { get; set; }
        public decimal EncounterRate { get; set; }
        public int PokedexId { get; set; }
        public int AreaId { get; set; }
    
        public virtual Area Area { get; set; }
        public virtual Pokedex Pokedex { get; set; }
    }
}

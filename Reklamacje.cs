//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieRental
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class Reklamacje
    {
        public Nullable<int> IDFilmu { get; set; }
        public Nullable<int> IDklienta { get; set; }
        public string Opis { get; set; }
        public string Rozwiązanie { get; set; }
    
        public virtual Filmy Filmy { get; set; }
        public virtual Klienci Klienci { get; set; }
    }
}

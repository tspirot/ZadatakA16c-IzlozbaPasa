//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZadatakA16c
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rezultat
    {
        public int IzlozbaID { get; set; }
        public int KategorijaID { get; set; }
        public int PasID { get; set; }
        public string Rezultat1 { get; set; }
        public string Napomena { get; set; }
    
        public virtual Izlozba Izlozba { get; set; }
        public virtual Kategorija Kategorija { get; set; }
        public virtual Pas Pas { get; set; }
    }
}

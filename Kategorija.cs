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
    
    public partial class Kategorija
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategorija()
        {
            this.Rezultat = new HashSet<Rezultat>();
        }
    
        public int KategorijaID { get; set; }
        public string Naziv { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rezultat> Rezultat { get; set; }
    }
}

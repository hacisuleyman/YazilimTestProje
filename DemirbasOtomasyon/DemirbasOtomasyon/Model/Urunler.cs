//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemirbasOtomasyon.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Urunler
    {
        public Urunler()
        {
            this.Zimmetler = new HashSet<Zimmetler>();
        }
    
        public int urunID { get; set; }
        public string urunAd { get; set; }
        public Nullable<int> stokMiktari { get; set; }
        public Nullable<decimal> fiyat { get; set; }
        public Nullable<System.DateTime> satınAlınmaTarihi { get; set; }
        public Nullable<bool> stokDurum { get; set; }
    
        public virtual ICollection<Zimmetler> Zimmetler { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KayseriEnt.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Birimler
    {
        public int Id { get; set; }
        public Nullable<int> BirimKodu { get; set; }
        public string BirimAdi { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<int> Salon { get; set; }
        public Nullable<int> Mutfak { get; set; }
        public Nullable<int> Kasiyer { get; set; }
        public Nullable<int> Ekstra { get; set; }
        public Nullable<int> SalonId { get; set; }
        public Nullable<int> MutfakId { get; set; }
        public Nullable<int> KasiyerId { get; set; }
        public Nullable<int> EkstraId { get; set; }
        public string CihazID { get; set; }
        public string CihazUstBirimId { get; set; }
    }
}

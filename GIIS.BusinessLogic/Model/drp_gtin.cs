//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GIIS.BusinessLogic.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class drp_gtin
    {
        public string gtin { get; set; }
        public string gtin_description { get; set; }
        public int gtin_type { get; set; }
        public string base_uom { get; set; }
        public string pack_uom_1 { get; set; }
        public Nullable<double> base_uom_per_pack_1 { get; set; }
        public string pack_uom_2 { get; set; }
        public Nullable<double> base_uom_per_pack_2 { get; set; }
        public Nullable<double> litres_storage_per_base_uom { get; set; }
        public int planning_lead_time_days { get; set; }
        public string gtin_parent { get; set; }
        public Nullable<double> base_uom_child_per_base_uom_parent { get; set; }
        public Nullable<int> gtin_lowcode { get; set; }
    }
}

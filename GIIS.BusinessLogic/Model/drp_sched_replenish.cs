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
    
    public partial class drp_sched_replenish
    {
        public string gtin { get; set; }
        public string gln { get; set; }
        public string doc_id { get; set; }
        public Nullable<System.DateTime> sched_replenish_date { get; set; }
        public Nullable<double> sched_replenish_qty { get; set; }
        public string from_gln { get; set; }
    }
}

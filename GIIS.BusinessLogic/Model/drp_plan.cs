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
    
    public partial class drp_plan
    {
        public string gtin { get; set; }
        public string gln { get; set; }
        public Nullable<System.DateTime> plan_date { get; set; }
        public Nullable<double> estimated_on_hand { get; set; }
        public Nullable<double> planned_demand { get; set; }
        public Nullable<double> sched_demand { get; set; }
        public Nullable<double> sched_replenish { get; set; }
        public Nullable<double> sched_on_hand { get; set; }
        public Nullable<System.DateTime> planned_order_release_date { get; set; }
        public Nullable<double> planned_order_receipt { get; set; }
        public Nullable<double> planned_on_hand { get; set; }
        public Nullable<System.DateTime> regen_time { get; set; }
        public decimal drp_id { get; set; }
        public Nullable<int> period { get; set; }
        public Nullable<int> gtin_lowcode { get; set; }
        public Nullable<int> gln_lowcode { get; set; }
    }
}
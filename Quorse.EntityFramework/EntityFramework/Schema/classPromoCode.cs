//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Quorse.EntityFramework.EntityFramework.Schema
{
    using System;
    using System.Collections.Generic;
    
    public partial class classPromoCode
    {
        public int id { get; set; }
        public string promoCode { get; set; }
        public Nullable<decimal> promoCut { get; set; }
        public Nullable<int> promoType { get; set; }
        public Nullable<int> classId { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public Nullable<System.DateTime> created { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> usageLimit { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TotalModel.Models
{
    using System;
    
    public partial class MaterialIssuePendingPlannedOrder
    {
        public int MaterialIssueTypeID { get; set; }
        public int PlannedOrderID { get; set; }
        public string PlannedOrderReference { get; set; }
        public string PlannedOrderCode { get; set; }
        public System.DateTime PlannedOrderEntryDate { get; set; }
        public int WorkshiftID { get; set; }
        public string WorkshiftCode { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
    }
}
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
    
    public partial class ProductionOrderPendingPlannedOrderDetail
    {
        public int PlannedOrderID { get; set; }
        public int PlannedOrderDetailID { get; set; }
        public string PlannedOrderReference { get; set; }
        public string PlannedOrderCode { get; set; }
        public System.DateTime PlannedOrderEntryDate { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public int CommodityTypeID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public int MoldID { get; set; }
        public string MoldCode { get; set; }
        public Nullable<decimal> QuantityRemains { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsSelected { get; set; }
    }
}

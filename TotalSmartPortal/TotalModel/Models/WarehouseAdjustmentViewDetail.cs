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
    
    public partial class WarehouseAdjustmentViewDetail
    {
        public int WarehouseAdjustmentDetailID { get; set; }
        public int WarehouseAdjustmentID { get; set; }
        public Nullable<int> GoodsReceiptID { get; set; }
        public Nullable<int> GoodsReceiptDetailID { get; set; }
        public string GoodsReceiptReference { get; set; }
        public Nullable<System.DateTime> GoodsReceiptEntryDate { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public Nullable<decimal> QuantityAvailable { get; set; }
        public decimal Quantity { get; set; }
        public string Remarks { get; set; }
    }
}

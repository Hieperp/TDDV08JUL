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
    
    public partial class GoodsReceiptDetailAvailable
    {
        public int GoodsReceiptID { get; set; }
        public int GoodsReceiptDetailID { get; set; }
        public string GoodsReceiptReference { get; set; }
        public System.DateTime GoodsReceiptEntryDate { get; set; }
        public int BatchID { get; set; }
        public System.DateTime BatchEntryDate { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseCode { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public string Remarks { get; set; }
        public string LineReferences { get; set; }
        public bool Approved { get; set; }
        public bool Issuable { get; set; }
        public bool IsSelected { get; set; }
        public Nullable<decimal> QuantityAvailables { get; set; }
        public string GoodsReceiptCode { get; set; }
        public int CommodityTypeID { get; set; }
        public string CodePartA { get; set; }
        public string CodePartB { get; set; }
        public string CodePartC { get; set; }
        public string CodePartD { get; set; }
        public string CodePartE { get; set; }
        public string CodePartF { get; set; }
    }
}

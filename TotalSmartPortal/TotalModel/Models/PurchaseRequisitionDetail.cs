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
    using System.Collections.Generic;
    
    public partial class PurchaseRequisitionDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PurchaseRequisitionDetail()
        {
            this.GoodsReceiptDetails = new HashSet<GoodsReceiptDetail>();
        }
    
        public int PurchaseRequisitionDetailID { get; set; }
        public int PurchaseRequisitionID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public int LocationID { get; set; }
        public int CustomerID { get; set; }
        public int CommodityID { get; set; }
        public int CommodityTypeID { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityReceipt { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> VoidTypeID { get; set; }
        public bool Approved { get; set; }
        public bool InActive { get; set; }
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActivePartialDate { get; set; }
    
        public virtual Commodity Commodity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoodsReceiptDetail> GoodsReceiptDetails { get; set; }
        public virtual PurchaseRequisition PurchaseRequisition { get; set; }
        public virtual VoidType VoidType { get; set; }
    }
}

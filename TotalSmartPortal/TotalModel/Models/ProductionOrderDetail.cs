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
    
    public partial class ProductionOrderDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductionOrderDetail()
        {
            this.MaterialIssueDetails = new HashSet<MaterialIssueDetail>();
        }
    
        public int ProductionOrderDetailID { get; set; }
        public int ProductionOrderID { get; set; }
        public int PlannedOrderDetailID { get; set; }
        public int PlannedOrderID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public int LocationID { get; set; }
        public int WorkshiftID { get; set; }
        public int CustomerID { get; set; }
        public int CommodityID { get; set; }
        public int CommodityTypeID { get; set; }
        public int CommodityMaterialID { get; set; }
        public int MoldID { get; set; }
        public int ProductionLineID { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> VoidTypeID { get; set; }
        public bool Approved { get; set; }
        public bool InActive { get; set; }
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActivePartialDate { get; set; }
    
        public virtual Commodity Commodity { get; set; }
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialIssueDetail> MaterialIssueDetails { get; set; }
        public virtual Mold Mold { get; set; }
        public virtual ProductionLine ProductionLine { get; set; }
        public virtual VoidType VoidType { get; set; }
        public virtual Workshift Workshift { get; set; }
        public virtual ProductionOrder ProductionOrder { get; set; }
        public virtual PlannedOrderDetail PlannedOrderDetail { get; set; }
        public virtual PlannedOrder PlannedOrder { get; set; }
    }
}

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalBase.Enums;
using TotalDTO.Helpers;
using TotalDTO.Commons;
using TotalDTO.Helpers.Interfaces;

namespace TotalDTO.Inventories
{
    public class MaterialIssuePrimitiveDTO : QuantityDTO<MaterialIssueDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.MaterialIssue; } }

        public int GetID() { return this.MaterialIssueID; }
        public void SetID(int id) { this.MaterialIssueID = id; }

        public int MaterialIssueID { get; set; }

        public virtual int WorkshiftID { get; set; }

        public virtual Nullable<int> WarehouseID { get; set; }

        public int MaterialIssueTypeID { get { return (int)GlobalEnums.MaterialIssueTypeID.ProductionOrder; } }
        //public int MaterialIssueTypeID { get; set; }

        public Nullable<int> ProductionOrderID { get; set; }
        public string ProductionOrderReference { get; set; }
        public string ProductionOrderReferences { get; set; }
        public string ProductionOrderCode { get; set; }
        public string ProductionOrderCodes { get; set; }
        [Display(Name = "Phiếu đặt hàng")]
        public string ProductionOrderReferenceNote { get { return this.ProductionOrderID != null ? this.ProductionOrderReference : (this.ProductionOrderReferences != "" ? this.ProductionOrderReferences : "Giao hàng tổng hợp của nhiều ĐH"); } }
        [Display(Name = "Số đơn hàng")]
        public string ProductionOrderCodeNote { get { return this.ProductionOrderID != null ? this.ProductionOrderCode : (this.ProductionOrderCodes != "" ? this.ProductionOrderCodes : ""); } }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> ProductionOrderEntryDate { get; set; }

        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }


        public override void PerformPresaveRule()
        {
            this.Approved = true; this.ApprovedDate = this.EntryDate; //At MaterialIssue, Approve right after save. Surely, It can be UnApprove later for editing

            base.PerformPresaveRule();

            string purchaseRequisitionReferences = ""; string purchaseRequisitionCodes = "";
            this.DtoDetails().ToList().ForEach(e => { e.WorkshiftID = this.WorkshiftID; e.WarehouseID = this.WarehouseID; if (this.MaterialIssueTypeID == (int)GlobalEnums.MaterialIssueTypeID.ProductionOrder && purchaseRequisitionReferences.IndexOf(e.ProductionOrderReference) < 0) purchaseRequisitionReferences = purchaseRequisitionReferences + (purchaseRequisitionReferences != "" ? ", " : "") + e.ProductionOrderReference; if (this.MaterialIssueTypeID == (int)GlobalEnums.MaterialIssueTypeID.ProductionOrder && e.ProductionOrderCode != null && purchaseRequisitionCodes.IndexOf(e.ProductionOrderCode) < 0) purchaseRequisitionCodes = purchaseRequisitionCodes + (purchaseRequisitionCodes != "" ? ", " : "") + e.ProductionOrderCode; });
            this.ProductionOrderReferences = purchaseRequisitionReferences; this.ProductionOrderCodes = purchaseRequisitionCodes != "" ? purchaseRequisitionCodes : null; if (this.MaterialIssueTypeID == (int)GlobalEnums.MaterialIssueTypeID.ProductionOrder) this.Code = this.ProductionOrderCodes;
        }
    }


    public class MaterialIssueDTO : MaterialIssuePrimitiveDTO, IBaseDetailEntity<MaterialIssueDetailDTO>
    {
        public MaterialIssueDTO()
        {
            this.MaterialIssueViewDetails = new List<MaterialIssueDetailDTO>();
        }

        public override int WorkshiftID { get { return (1); } }
        //public override int WorkshiftID { get { return (this.Workshift != null ? this.Workshift.WorkshiftID : 0); } }
        //[Display(Name = "Khách hàng")]
        //[UIHint("Commons/WorkshiftBase")]
        //public WorkshiftBaseDTO Workshift { get; set; }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }

        public List<MaterialIssueDetailDTO> MaterialIssueViewDetails { get; set; }
        public List<MaterialIssueDetailDTO> ViewDetails { get { return this.MaterialIssueViewDetails; } set { this.MaterialIssueViewDetails = value; } }

        public ICollection<MaterialIssueDetailDTO> GetDetails() { return this.MaterialIssueViewDetails; }

        protected override IEnumerable<MaterialIssueDetailDTO> DtoDetails() { return this.MaterialIssueViewDetails; }
    }
}


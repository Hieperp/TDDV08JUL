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

        public int MaterialIssueTypeID { get { return (int)GlobalEnums.MaterialIssueTypeID.PlannedOrder; } }
        //public int MaterialIssueTypeID { get; set; }

        public Nullable<int> PlannedOrderID { get; set; }
        public string PlannedOrderReference { get; set; }
        public string PlannedOrderReferences { get; set; }
        public string PlannedOrderCode { get; set; }
        public string PlannedOrderCodes { get; set; }
        [Display(Name = "Phiếu đặt hàng")]
        public string PlannedOrderReferenceNote { get { return this.PlannedOrderID != null ? this.PlannedOrderReference : (this.PlannedOrderReferences != "" ? this.PlannedOrderReferences : "Giao hàng tổng hợp của nhiều ĐH"); } }
        [Display(Name = "Số đơn hàng")]
        public string PlannedOrderCodeNote { get { return this.PlannedOrderID != null ? this.PlannedOrderCode : (this.PlannedOrderCodes != "" ? this.PlannedOrderCodes : ""); } }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> PlannedOrderEntryDate { get; set; }

        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }

        public virtual int StorekeeperID { get; set; }

        public override void PerformPresaveRule()
        {
            this.Approved = true; this.ApprovedDate = this.EntryDate; //At MaterialIssue, Approve right after save. Surely, It can be UnApprove later for editing

            base.PerformPresaveRule();

            string purchaseRequisitionReferences = ""; string purchaseRequisitionCodes = "";
            this.DtoDetails().ToList().ForEach(e => { e.WorkshiftID = this.WorkshiftID; e.WarehouseID = this.WarehouseID; if (this.MaterialIssueTypeID == (int)GlobalEnums.MaterialIssueTypeID.PlannedOrder && purchaseRequisitionReferences.IndexOf(e.PlannedOrderReference) < 0) purchaseRequisitionReferences = purchaseRequisitionReferences + (purchaseRequisitionReferences != "" ? ", " : "") + e.PlannedOrderReference; if (this.MaterialIssueTypeID == (int)GlobalEnums.MaterialIssueTypeID.PlannedOrder && e.PlannedOrderCode != null && purchaseRequisitionCodes.IndexOf(e.PlannedOrderCode) < 0) purchaseRequisitionCodes = purchaseRequisitionCodes + (purchaseRequisitionCodes != "" ? ", " : "") + e.PlannedOrderCode; });
            this.PlannedOrderReferences = purchaseRequisitionReferences; this.PlannedOrderCodes = purchaseRequisitionCodes != "" ? purchaseRequisitionCodes : null; if (this.MaterialIssueTypeID == (int)GlobalEnums.MaterialIssueTypeID.PlannedOrder) this.Code = this.PlannedOrderCodes;
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

        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Storekeeper { get; set; }


        public List<MaterialIssueDetailDTO> MaterialIssueViewDetails { get; set; }
        public List<MaterialIssueDetailDTO> ViewDetails { get { return this.MaterialIssueViewDetails; } set { this.MaterialIssueViewDetails = value; } }

        public ICollection<MaterialIssueDetailDTO> GetDetails() { return this.MaterialIssueViewDetails; }

        protected override IEnumerable<MaterialIssueDetailDTO> DtoDetails() { return this.MaterialIssueViewDetails; }
    }
}


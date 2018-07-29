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

        public int MaterialIssueTypeID { get { return (int)GlobalEnums.MaterialIssueTypeID.PlannedOrder; } }
        //public int MaterialIssueTypeID { get; set; }

        public virtual int CustomerID { get; set; }
        public virtual int CommodityID { get; set; }
        [Display(Name = "Mặt hàng")]
        [UIHint("StringReadonly")]
        public string CommodityCode { get; set; }
        [Display(Name = "Mã hàng")]
        [UIHint("StringReadonly")]
        public string CommodityName { get; set; }

        public int PlannedOrderID { get; set; }
        public string PlannedOrderReference { get; set; }
        public string PlannedOrderCode { get; set; }
        public DateTime PlannedOrderEntryDate { get; set; }

        public int PlannedOrderDetailID { get; set; }

        public int ProductionOrderID { get; set; }
        public int ProductionOrderDetailID { get; set; }

        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }

        public virtual int WorkshiftID { get; set; }
        
        public virtual int ProductionLineID { get; set; }
        [Display(Name = "Line")]
        public string ProductionLineCode { get; set; }

        public virtual int MoldID { get; set; }
        [Display(Name = "Khuôn")]
        public string MoldCode { get; set; }


        public virtual Nullable<int> WarehouseID { get; set; }

        public virtual int StorekeeperID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.MaterialIssueTypeID = this.MaterialIssueTypeID; e.PlannedOrderID = this.PlannedOrderID; e.PlannedOrderDetailID = this.PlannedOrderDetailID; e.ProductionOrderID = this.ProductionOrderID; e.ProductionOrderDetailID = this.ProductionOrderDetailID; e.CustomerID = this.CustomerID; e.WorkshiftID = this.WorkshiftID; e.ProductionLineID = this.ProductionLineID; e.MoldID = this.MoldID; e.WarehouseID = this.WarehouseID; });
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


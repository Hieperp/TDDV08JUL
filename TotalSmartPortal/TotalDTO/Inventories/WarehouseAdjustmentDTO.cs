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
    public class WarehouseAdjustmentPrimitiveDTO : QuantityDTO<WarehouseAdjustmentDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.WarehouseAdjustment; } }

        public int GetID() { return this.WarehouseAdjustmentID; }
        public void SetID(int id) { this.WarehouseAdjustmentID = id; }

        public int WarehouseAdjustmentID { get; set; }

        public int WarehouseAdjustmentTypeID { get { return (int)GlobalEnums.WarehouseAdjustmentTypeID.OtherIssues; } }
        //public int MaterialIssueTypeID { get; set; }


        public virtual Nullable<int> WarehouseID { get; set; }
        public virtual Nullable<int> WarehouseReceiptID { get; set; }


        [Display(Name = "Số đơn hàng")]
        public string AdjustmentJobs { get; set; }

        public virtual int StorekeeperID { get; set; }


        public bool WarehouseReceiptEnabled { get { return this.WarehouseAdjustmentTypeID == (int)GlobalEnums.WarehouseAdjustmentTypeID.OtherIssues; } }
        public bool HasPositiveLine { get { return this.DtoDetails().Where(w => w.Quantity > 0).Count() > 0; } }


        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.WarehouseAdjustmentTypeID = this.WarehouseAdjustmentTypeID; e.WarehouseID = (int)this.WarehouseID; e.WarehouseReceiptID = this.WarehouseReceiptID; });
        }

    }

    public class WarehouseAdjustmentDTO : WarehouseAdjustmentPrimitiveDTO, IBaseDetailEntity<WarehouseAdjustmentDetailDTO>
    {
        public WarehouseAdjustmentDTO()
        {
            this.WarehouseAdjustmentViewDetails = new List<WarehouseAdjustmentDetailDTO>();
        }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }

        public override Nullable<int> WarehouseReceiptID { get { return (this.WarehouseReceipt != null ? this.WarehouseReceipt.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO WarehouseReceipt { get; set; }

        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Storekeeper { get; set; }


        public List<WarehouseAdjustmentDetailDTO> WarehouseAdjustmentViewDetails { get; set; }
        public List<WarehouseAdjustmentDetailDTO> ViewDetails { get { return this.WarehouseAdjustmentViewDetails; } set { this.WarehouseAdjustmentViewDetails = value; } }

        public ICollection<WarehouseAdjustmentDetailDTO> GetDetails() { return this.WarehouseAdjustmentViewDetails; }

        protected override IEnumerable<WarehouseAdjustmentDetailDTO> DtoDetails() { return this.WarehouseAdjustmentViewDetails; }
    }

}

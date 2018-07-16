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
    public class GoodsReceiptPrimitiveDTO : QuantityDTO<GoodsReceiptDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public virtual GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.GoodsReceipt; } }

        public int GetID() { return this.GoodsReceiptID; }
        public void SetID(int id) { this.GoodsReceiptID = id; }

        public int GoodsReceiptID { get; set; }

        public virtual int CustomerID { get; set; }

        public virtual Nullable<int> WarehouseID { get; set; }

        public int GoodsReceiptTypeID { get; set; }

        public Nullable<int> PurchaseRequisitionID { get; set; }
        public string PurchaseRequisitionReference { get; set; }
        public string PurchaseRequisitionReferences { get; set; }
        public string PurchaseRequisitionCode { get; set; }
        public string PurchaseRequisitionCodes { get; set; }
        [Display(Name = "Phiếu đặt hàng")]
        public string PurchaseRequisitionReferenceNote { get { return this.PurchaseRequisitionID != null ? this.PurchaseRequisitionReference : (this.PurchaseRequisitionReferences != "" ? this.PurchaseRequisitionReferences : "Giao hàng tổng hợp của nhiều ĐH"); } }
        [Display(Name = "Số đơn hàng")]
        public string PurchaseRequisitionCodeNote { get { return this.PurchaseRequisitionID != null ? this.PurchaseRequisitionCode : (this.PurchaseRequisitionCodes != "" ? this.PurchaseRequisitionCodes : ""); } }
        [Display(Name = "Ngày đặt hàng")]
        public Nullable<System.DateTime> PurchaseRequisitionEntryDate { get; set; }

        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }

        
        public override void PerformPresaveRule()
        {
            this.Approved = true; this.ApprovedDate = this.EntryDate; //At GoodsReceipt, Approve right after save. Surely, It can be UnApprove later for editing

            base.PerformPresaveRule();

            string salesOrderReferences = ""; string salesOrderCodes = "";
            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice && salesOrderReferences.IndexOf(e.PurchaseRequisitionReference) < 0) salesOrderReferences = salesOrderReferences + (salesOrderReferences != "" ? ", " : "") + e.PurchaseRequisitionReference; if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice && salesOrderCodes.IndexOf(e.PurchaseRequisitionCode) < 0) salesOrderCodes = salesOrderCodes + (salesOrderCodes != "" ? ", " : "") + e.PurchaseRequisitionCode; });
            this.PurchaseRequisitionReferences = salesOrderReferences; this.PurchaseRequisitionCodes = salesOrderCodes != "" ? salesOrderCodes : null; if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice) this.Code = this.PurchaseRequisitionCodes;
        }
    }


    public class GoodsReceiptDTO : GoodsReceiptPrimitiveDTO, IBaseDetailEntity<GoodsReceiptDetailDTO>
    {
        public GoodsReceiptDTO()
        {
            this.GoodsReceiptViewDetails = new List<GoodsReceiptDetailDTO>();
        }

        //public override int CustomerID { get { return (this.Customer != null ? this.Customer.CustomerID : 0); } }
        //[Display(Name = "Khách hàng")]
        //[UIHint("Commons/CustomerBase")]
        //public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }

        public List<GoodsReceiptDetailDTO> GoodsReceiptViewDetails { get; set; }
        public List<GoodsReceiptDetailDTO> ViewDetails { get { return this.GoodsReceiptViewDetails; } set { this.GoodsReceiptViewDetails = value; } }

        public ICollection<GoodsReceiptDetailDTO> GetDetails() { return this.GoodsReceiptViewDetails; }

        protected override IEnumerable<GoodsReceiptDetailDTO> DtoDetails() { return this.GoodsReceiptViewDetails; }
    }
}


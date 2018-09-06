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

        public virtual Nullable<int> CustomerID { get; set; }

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

        public Nullable<int> WarehouseAdjustmentID { get; set; }

        [Display(Name = "Số đơn hàng")]
        [UIHint("Commons/SOCode")]
        public string Code { get; set; }

        [Display(Name = "Mục đích")]
        public string Purposes { get; set; }

        public virtual int StorekeeperID { get; set; }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            string purchaseRequisitionReferences = ""; string purchaseRequisitionCodes = "";
            this.DtoDetails().ToList().ForEach(e => { e.GoodsReceiptTypeID = this.GoodsReceiptTypeID; e.CustomerID = this.CustomerID; e.WarehouseID = this.WarehouseID; e.Code = Code; if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition && purchaseRequisitionReferences.IndexOf(e.PurchaseRequisitionReference) < 0) purchaseRequisitionReferences = purchaseRequisitionReferences + (purchaseRequisitionReferences != "" ? ", " : "") + e.PurchaseRequisitionReference; if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition && e.PurchaseRequisitionCode != null && purchaseRequisitionCodes.IndexOf(e.PurchaseRequisitionCode) < 0) purchaseRequisitionCodes = purchaseRequisitionCodes + (purchaseRequisitionCodes != "" ? ", " : "") + e.PurchaseRequisitionCode; });
            this.PurchaseRequisitionReferences = purchaseRequisitionReferences; this.PurchaseRequisitionCodes = purchaseRequisitionCodes != "" ? purchaseRequisitionCodes : null; if (this.GoodsReceiptTypeID == (int)GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition) this.Code = this.PurchaseRequisitionCodes;
        }
    }


    public class GoodsReceiptDTO : GoodsReceiptPrimitiveDTO, IBaseDetailEntity<GoodsReceiptDetailDTO>, IPriceCategory, IWarehouse
    {
        public GoodsReceiptDTO()
        {
            this.GoodsReceiptViewDetails = new List<GoodsReceiptDetailDTO>();
        }

        public override Nullable<int> CustomerID { get { int? customerID = null; if (this.Customer != null) customerID = this.Customer.CustomerID; return customerID; } }
        [Display(Name = "Khách hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> WarehouseID { get { return (this.Warehouse != null ? this.Warehouse.WarehouseID : null); } }
        [Display(Name = "Kho hàng")]
        [UIHint("AutoCompletes/WarehouseBase")]
        public WarehouseBaseDTO Warehouse { get; set; }

        public override int StorekeeperID { get { return (this.Storekeeper != null ? this.Storekeeper.EmployeeID : 0); } }
        [Display(Name = "Nhân viên kho")]
        [UIHint("AutoCompletes/EmployeeBase")]
        public EmployeeBaseDTO Storekeeper { get; set; }

        public List<GoodsReceiptDetailDTO> GoodsReceiptViewDetails { get; set; }
        public List<GoodsReceiptDetailDTO> ViewDetails { get { return this.GoodsReceiptViewDetails; } set { this.GoodsReceiptViewDetails = value; } }

        public ICollection<GoodsReceiptDetailDTO> GetDetails() { return this.GoodsReceiptViewDetails; }

        protected override IEnumerable<GoodsReceiptDetailDTO> DtoDetails() { return this.GoodsReceiptViewDetails; }



        #region implement ISearchCustomer only

        [Display(Name = "PriceCategoryID")]
        public int PriceCategoryID { get; set; }
        [Display(Name = "PriceCategoryCode")]
        public string PriceCategoryCode { get; set; }

        [Display(Name = "Đơn vị, người nhận hàng")]
        public int ReceiverID { get { return (this.Receiver != null ? this.Receiver.CustomerID : 0); } }
        [Display(Name = "Đơn vị, người nhận hàng")]
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Receiver { get; set; }

        public virtual Nullable<int> TradePromotionID { get; set; }
        [Display(Name = "Addressee")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Addressee")]
        public string Addressee { get; set; }
        #endregion implement ISearchCustomer only
    }
}


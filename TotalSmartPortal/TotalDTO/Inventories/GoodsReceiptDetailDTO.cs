using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalBase.Enums;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class GoodsReceiptDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.GoodsReceiptDetailID; }

        public int GoodsReceiptDetailID { get; set; }
        public int GoodsReceiptID { get; set; }

        public Nullable<int> GoodsReceiptTypeID { get; set; }

        public Nullable<int> PurchaseRequisitionID { get; set; }
        public Nullable<int> PurchaseRequisitionDetailID { get; set; }

        [Display(Name = "Phiếu ĐH")]
        [UIHint("StringReadonly")]
        public string PurchaseRequisitionReference { get; set; }
        [Display(Name = "Số ĐH")]
        [UIHint("StringReadonly")]
        public string PurchaseRequisitionCode { get; set; }
        [Display(Name = "Ngày ĐH")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> PurchaseRequisitionEntryDate { get; set; }


        public Nullable<int> WarehouseAdjustmentID { get; set; }
        public Nullable<int> WarehouseAdjustmentDetailID { get; set; }

        [Display(Name = "Phiếu ĐH")]
        [UIHint("StringReadonly")]
        public string WarehouseAdjustmentReference { get; set; }
        [Display(Name = "Số ĐH")]
        [UIHint("StringReadonly")]
        public string WarehouseAdjustmentCode { get; set; }
        [Display(Name = "Ngày ĐH")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> WarehouseAdjustmentEntryDate { get; set; }
        public Nullable<int> WarehouseAdjustmentTypeID { get; set; }


        public int BatchID { get; set; }
        public System.DateTime BatchEntryDate { get { return DateTime.Now; } set { } }


        public string Code { get; set; }

        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> WarehouseID { get; set; }

        [Display(Name = "Kho")]
        [UIHint("StringReadonly")]
        public string WarehouseCode { get; set; }


        [UIHint("AutoCompletes/CommodityAvailable")]
        public override string CommodityCode { get; set; }

        [Display(Name = "Tồn đơn")]
        [UIHint("QuantityReadonly")]
        public decimal QuantityRemains { get; set; }

        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.PurchaseRequisitionID > 0 && this.Quantity > this.QuantityRemains) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}
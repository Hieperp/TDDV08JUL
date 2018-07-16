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

        public int CustomerID { get; set; }
        public Nullable<int> WarehouseID { get; set; }

        [Display(Name = "Kho")]
        [UIHint("StringReadonly")]
        public string WarehouseCode { get; set; }

        [UIHint("AutoCompletes/CommodityAvailable")]
        public override string CommodityCode { get; set; }

        [Display(Name = "Tồn đơn")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityRemains { get; set; }

        [UIHint("Decimal")]
        public override decimal Quantity { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.PurchaseRequisitionID > 0 && this.Quantity > this.QuantityRemains) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}
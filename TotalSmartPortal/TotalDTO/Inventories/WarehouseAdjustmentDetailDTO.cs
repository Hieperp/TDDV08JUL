using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;
using System.Collections.Generic;
using TotalModel.Helpers;
using TotalBase;

namespace TotalDTO.Inventories
{
    public class WarehouseAdjustmentDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.WarehouseAdjustmentDetailID; }

        public int WarehouseAdjustmentDetailID { get; set; }
        public int WarehouseAdjustmentID { get; set; }

        public int WarehouseAdjustmentTypeID { get; set; }

        public Nullable<int> GoodsReceiptID { get; set; }
        public Nullable<int> GoodsReceiptDetailID { get; set; }

        public string GoodsReceiptReference { get; set; }
        public Nullable<System.DateTime> GoodsReceiptEntryDate { get; set; }

        public int BatchID { get; set; }
        public System.DateTime BatchEntryDate { get; set; }

        public int WarehouseID { get; set; }
        public string WarehouseCode { get; set; }

        public Nullable<int> WarehouseReceiptID { get; set; }

        [Display(Name = "Tồn kho")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityAvailables { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.Quantity > 0 || -this.Quantity <= this.QuantityAvailables) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }





}

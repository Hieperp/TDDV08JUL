using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalModel.Helpers;
using TotalBase.Enums;
using TotalDTO.Helpers;

namespace TotalDTO.Inventories
{
    public class MaterialIssueDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.MaterialIssueDetailID; }

        public int MaterialIssueDetailID { get; set; }
        public int MaterialIssueID { get; set; }

        public int PlannedOrderID { get; set; }
        public int PlannedOrderDetailID { get; set; }

        [Display(Name = "Kế Hoạch")]
        [UIHint("StringReadonly")]
        public string PlannedOrderReference { get; set; }
        [Display(Name = "Mã KH")]
        [UIHint("StringReadonly")]
        public string PlannedOrderCode { get; set; }
        [Display(Name = "Ngày KH")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> PlannedOrderEntryDate { get; set; }

        public int ProductionOrderID { get; set; }
        public int ProductionOrderDetailID { get; set; }

        [Display(Name = "Lệnh SX")]
        [UIHint("StringReadonly")]
        public string ProductionOrderReference { get; set; }
        [Display(Name = "Số LSX")]
        [UIHint("StringReadonly")]
        public string ProductionOrderCode { get; set; }
        [Display(Name = "Ngày SX")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> ProductionOrderEntryDate { get; set; }

        public int WorkshiftID { get; set; }

        public int ProductionLineID { get; set; }
        [Display(Name = "Line")]
        [UIHint("StringReadonly")]
        public string ProductionLineCode { get; set; }


        public int ProductID { get; set; }
        [Display(Name = "Mặt hàng")]
        [UIHint("StringReadonly")]
        public string ProductCode { get; set; }
        [Display(Name = "Mã hàng")]
        [UIHint("StringReadonly")]
        public string ProductName { get; set; }

        public int MaterialIssueTypeID { get; set; }
        public Nullable<int> WarehouseID { get; set; }

        [Display(Name = "Kho")]
        [UIHint("StringReadonly")]
        public string WarehouseCode { get; set; }


        public int GoodsReceiptID { get; set; }
        public int GoodsReceiptDetailID { get; set; }

        [Display(Name = "Kế Hoạch")]
        [UIHint("StringReadonly")]
        public string GoodsReceiptReference { get; set; }
        [Display(Name = "Mã KH")]
        [UIHint("StringReadonly")]
        public string GoodsReceiptCode { get; set; }
        [Display(Name = "Ngày KH")]
        [UIHint("DateTimeReadonly")]
        public Nullable<System.DateTime> GoodsReceiptEntryDate { get; set; }



        [Display(Name = "Tồn đơn")]
        [UIHint("DecimalReadonly")]
        public decimal QuantityRemains { get; set; }

        [UIHint("Decimal")]
        public override decimal Quantity { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext)) { yield return result; }

            if (this.ProductionOrderID > 0 && this.Quantity > this.QuantityRemains) yield return new ValidationResult("Số lượng xuất không được lớn hơn số lượng còn lại [" + this.CommodityName + "]", new[] { "Quantity" });
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Productions
{
    public class ProductionOrderDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.ProductionOrderDetailID; }

        public int ProductionOrderDetailID { get; set; }
        public int ProductionOrderID { get; set; }

        public int CustomerID { get; set; }
        [Display(Name = "Mã khách hàng")]
        [Required(ErrorMessage = "Vui lòng chọn khách hàng")]
        [UIHint("AutoCompletes/CustomerBasic")]
        public virtual string CustomerCode { get; set; }

        [Display(Name = "Tên khách hàng")]
        [UIHint("StringReadonly")]
        public virtual string CustomerName { get; set; }

        public int ProductionLineID { get; set; }
        [Display(Name = "Dây chuyền")]
        [Required(ErrorMessage = "Vui lòng chọn dây chuyền")]
        [UIHint("AutoCompletes/ProductionLineBase")]
        public virtual string ProductionLineCode { get; set; }

        public int MoldID { get; set; }
        [Display(Name = "Mã khuôn")]
        [Required(ErrorMessage = "Vui lòng chọn mã khuôn")]
        [UIHint("AutoCompletes/MoldBase")]
        public virtual string MoldCode { get; set; }


        //[Display(Name = "Mã CK")]
        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }

        public string VoidTypeCode { get; set; }
        [Display(Name = "Lý do")]
        [UIHint("AutoCompletes/VoidTypeBase")]
        public string VoidTypeName { get; set; }
        public Nullable<int> VoidClassID { get; set; }

        [UIHint("Decimal")]
        public override decimal Quantity { get; set; }
    }
}


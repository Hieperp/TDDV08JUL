using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Productions
{
    public class PlannedOrderDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.PlannedOrderDetailID; }

        public int PlannedOrderDetailID { get; set; }
        public int PlannedOrderID { get; set; }

        public int CustomerID { get; set; }
        
        public int MoldID { get; set; }
        [Display(Name = "Mã khuôn")]
        [Required(ErrorMessage = "Vui lòng chọn mã khuôn")]
        [UIHint("AutoCompletes/MoldBase")]
        public virtual string MoldCode { get; set; }

        public int CommodityMaterialID { get; set; }
        [Display(Name = "Mã khuôn")]
        [Required(ErrorMessage = "Vui lòng chọn mã khuôn")]
        [UIHint("AutoCompletes/CommodityMaterialBase")]
        public virtual string CommodityMaterialCode { get; set; }

        //[Display(Name = "Mã CK")]
        [UIHint("AutoCompletes/CommodityBase")]
        public override string CommodityCode { get; set; }

        public string VoidTypeCode { get; set; }
        [Display(Name = "Lý do")]
        [UIHint("AutoCompletes/VoidTypeBase")]
        public string VoidTypeName { get; set; }
        public Nullable<int> VoidClassID { get; set; }

        [UIHint("Quantity")]
        public override decimal Quantity { get; set; }
    }
}
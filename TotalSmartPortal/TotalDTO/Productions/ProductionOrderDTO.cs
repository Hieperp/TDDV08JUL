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

namespace TotalDTO.Productions
{
    public class ProductionOrderPrimitiveDTO : QuantityDTO<ProductionOrderDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.ProductionOrder; } }

        public int GetID() { return this.ProductionOrderID; }
        public void SetID(int id) { this.ProductionOrderID = id; }

        public int ProductionOrderID { get; set; }

        [Display(Name = "Chứng")]
        public string Code { get; set; }
    }

    public class ProductionOrderDTO : ProductionOrderPrimitiveDTO, IBaseDetailEntity<ProductionOrderDetailDTO>
    {
        public ProductionOrderDTO()
        {
            this.ProductionOrderViewDetails = new List<ProductionOrderDetailDTO>();
        }

        public override Nullable<int> VoidTypeID { get { return (this.VoidType != null ? this.VoidType.VoidTypeID : null); } }
        [UIHint("AutoCompletes/VoidType")]
        public VoidTypeBaseDTO VoidType { get; set; }

        public List<ProductionOrderDetailDTO> ProductionOrderViewDetails { get; set; }
        public List<ProductionOrderDetailDTO> ViewDetails { get { return this.ProductionOrderViewDetails; } set { this.ProductionOrderViewDetails = value; } }

        public ICollection<ProductionOrderDetailDTO> GetDetails() { return this.ProductionOrderViewDetails; }

        protected override IEnumerable<ProductionOrderDetailDTO> DtoDetails() { return this.ProductionOrderViewDetails; }

        public override void PrepareVoidDetail(int? detailID)
        {
            this.ViewDetails.RemoveAll(w => w.ProductionOrderDetailID != detailID);
            if (this.ViewDetails.Count() > 0)
                this.VoidType = new VoidTypeBaseDTO() { VoidTypeID = this.ViewDetails[0].VoidTypeID, Code = this.ViewDetails[0].VoidTypeCode, Name = this.ViewDetails[0].VoidTypeName, VoidClassID = this.ViewDetails[0].VoidClassID };
            base.PrepareVoidDetail(detailID);
        }
    }

}

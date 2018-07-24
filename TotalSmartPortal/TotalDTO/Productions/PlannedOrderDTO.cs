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
    public class PlannedOrderPrimitiveDTO : QuantityDTO<PlannedOrderDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PlannedOrder; } }

        public int GetID() { return this.PlannedOrderID; }
        public void SetID(int id) { this.PlannedOrderID = id; }

        public int PlannedOrderID { get; set; }

        [Display(Name = "Chứng")]
        public string Code { get; set; }

        //public virtual int  CustomerID { get; set; }
        public virtual int CustomerID { get { return 68; } }


        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.CustomerID = this.CustomerID; });
        }
    }

    public class PlannedOrderDTO : PlannedOrderPrimitiveDTO, IBaseDetailEntity<PlannedOrderDetailDTO>
    {
        public PlannedOrderDTO()
        {
            this.PlannedOrderViewDetails = new List<PlannedOrderDetailDTO>();
        }

        //public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        //[UIHint("Commons/CustomerBase")]
        //public CustomerBaseDTO Customer { get; set; }

        public override Nullable<int> VoidTypeID { get { return (this.VoidType != null ? this.VoidType.VoidTypeID : null); } }
        [UIHint("AutoCompletes/VoidType")]
        public VoidTypeBaseDTO VoidType { get; set; }

        public List<PlannedOrderDetailDTO> PlannedOrderViewDetails { get; set; }
        public List<PlannedOrderDetailDTO> ViewDetails { get { return this.PlannedOrderViewDetails; } set { this.PlannedOrderViewDetails = value; } }

        public ICollection<PlannedOrderDetailDTO> GetDetails() { return this.PlannedOrderViewDetails; }

        protected override IEnumerable<PlannedOrderDetailDTO> DtoDetails() { return this.PlannedOrderViewDetails; }

        public override void PrepareVoidDetail(int? detailID)
        {
            this.ViewDetails.RemoveAll(w => w.PlannedOrderDetailID != detailID);
            if (this.ViewDetails.Count() > 0)
                this.VoidType = new VoidTypeBaseDTO() { VoidTypeID = this.ViewDetails[0].VoidTypeID, Code = this.ViewDetails[0].VoidTypeCode, Name = this.ViewDetails[0].VoidTypeName, VoidClassID = this.ViewDetails[0].VoidClassID };
            base.PrepareVoidDetail(detailID);
        }
    }

}

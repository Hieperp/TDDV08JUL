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

namespace TotalDTO.Purchases
{
    public class PurchaseRequisitionPrimitiveDTO : QuantityDTO<PurchaseRequisitionDetailDTO>, IPrimitiveEntity, IPrimitiveDTO
    {
        public GlobalEnums.NmvnTaskID NMVNTaskID { get { return GlobalEnums.NmvnTaskID.PurchaseRequisition; } }

        public int GetID() { return this.PurchaseRequisitionID; }
        public void SetID(int id) { this.PurchaseRequisitionID = id; }

        public int PurchaseRequisitionID { get; set; }

        public virtual Nullable<int> CustomerID { get; set; }

        [Display(Name = "Chứng từ khuyến mãi")]
        public string TrackingVouchers { get; set; }
    }

    public class PurchaseRequisitionDTO : PurchaseRequisitionPrimitiveDTO, IBaseDetailEntity<PurchaseRequisitionDetailDTO>
    {
        public PurchaseRequisitionDTO()
        {
            this.PurchaseRequisitionViewDetails = new List<PurchaseRequisitionDetailDTO>();
        }

        public override Nullable<int> CustomerID { get { return (this.Customer != null ? (this.Customer.CustomerID > 0 ? (Nullable<int>)this.Customer.CustomerID : null) : null); } }
        [UIHint("Commons/CustomerBase")]
        public CustomerBaseDTO Customer { get; set; }

        public List<PurchaseRequisitionDetailDTO> PurchaseRequisitionViewDetails { get; set; }
        public List<PurchaseRequisitionDetailDTO> ViewDetails { get { return this.PurchaseRequisitionViewDetails; } set { this.PurchaseRequisitionViewDetails = value; } }

        public ICollection<PurchaseRequisitionDetailDTO> GetDetails() { return this.PurchaseRequisitionViewDetails; }

        protected override IEnumerable<PurchaseRequisitionDetailDTO> DtoDetails() { return this.PurchaseRequisitionViewDetails; }
    }

}

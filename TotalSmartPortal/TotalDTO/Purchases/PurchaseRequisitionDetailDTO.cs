using System;
using System.ComponentModel.DataAnnotations;

using TotalModel;
using TotalDTO.Helpers;

namespace TotalDTO.Purchases
{
    public class PurchaseRequisitionDetailDTO : QuantityDetailDTO, IPrimitiveEntity
    {
        public int GetID() { return this.PurchaseRequisitionDetailID; }

        public int PurchaseRequisitionDetailID { get; set; }
        public int PurchaseRequisitionID { get; set; }

        public int CustomerID { get; set; }
    }
}

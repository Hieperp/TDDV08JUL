using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Purchases;
using TotalCore.Repositories.Purchases;
using TotalCore.Services.Purchases;

namespace TotalService.Purchases
{
    public class PurchaseRequisitionService : GenericWithViewDetailService<PurchaseRequisition, PurchaseRequisitionDetail, PurchaseRequisitionViewDetail, PurchaseRequisitionDTO, PurchaseRequisitionPrimitiveDTO, PurchaseRequisitionDetailDTO>, IPurchaseRequisitionService
    {
        public PurchaseRequisitionService(IPurchaseRequisitionRepository salesOrderRepository)
            : base(salesOrderRepository, "PurchaseRequisitionPostSaveValidate", "PurchaseRequisitionSaveRelative", "PurchaseRequisitionToggleApproved", "PurchaseRequisitionToggleVoid", "PurchaseRequisitionToggleVoidDetail", "GetPurchaseRequisitionViewDetails")
        {
        }

        public override ICollection<PurchaseRequisitionViewDetail> GetViewDetails(int salesOrderID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("PurchaseRequisitionID", salesOrderID) };
            return this.GetViewDetails(parameters);
        }

        public override bool Save(PurchaseRequisitionDTO salesOrderDTO)
        {
            salesOrderDTO.PurchaseRequisitionViewDetails.RemoveAll(x => x.Quantity == 0);
            return base.Save(salesOrderDTO);
        }
    }
}

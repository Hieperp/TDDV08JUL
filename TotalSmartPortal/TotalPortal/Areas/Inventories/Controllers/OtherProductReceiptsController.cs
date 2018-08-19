using System.Web.Mvc;
using TotalCore.Services.Inventories;
using TotalDTO.Inventories;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class OtherProductReceiptsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO, OtherProductReceiptViewModel>
    {
        public OtherProductReceiptsController(IOtherProductReceiptService otherProductReceiptService, IOtherProductReceiptViewModelSelectListBuilder otherProductReceiptViewModelSelectListBuilder)
            : base(otherProductReceiptService, otherProductReceiptViewModelSelectListBuilder)
        {
        }
    }
}
using System.Web.Mvc;
using TotalCore.Services.Inventories;
using TotalDTO.Inventories;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class OtherMaterialReceiptsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO, OtherMaterialReceiptViewModel>
    {
        public OtherMaterialReceiptsController(IOtherMaterialReceiptService otherMaterialReceiptService, IOtherMaterialReceiptViewModelSelectListBuilder otherMaterialReceiptViewModelSelectListBuilder)
            : base(otherMaterialReceiptService, otherMaterialReceiptViewModelSelectListBuilder)
        {
        }
    }
}
using System.Web.Mvc;
using TotalCore.Services.Inventories;
using TotalDTO.Inventories;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class MaterialAdjustmentsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionMTLISS>, WarehouseAdjustmentPrimitiveDTO<WAOptionMTLISS>, WarehouseAdjustmentDetailDTO, MaterialAdjustmentViewModel>
    {
        public MaterialAdjustmentsController(IMaterialAdjustmentService materialAdjustmentService, IMaterialAdjustmentViewModelSelectListBuilder materialAdjustmentViewModelSelectListBuilder)
            : base(materialAdjustmentService, materialAdjustmentViewModelSelectListBuilder)
        {
        }
    }
}
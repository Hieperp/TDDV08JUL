using System.Web.Mvc;
using TotalCore.Services.Inventories;
using TotalDTO.Inventories;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class ProductAdjustmentsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO, ProductAdjustmentViewModel>
    {
        public ProductAdjustmentsController(IProductAdjustmentService productAdjustmentService, IProductAdjustmentViewModelSelectListBuilder productAdjustmentViewModelSelectListBuilder)
            : base(productAdjustmentService, productAdjustmentViewModelSelectListBuilder)
        {
        }
    }
}
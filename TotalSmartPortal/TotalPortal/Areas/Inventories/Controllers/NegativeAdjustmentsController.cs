using System.Web.Mvc;

using TotalCore.Services.Inventories;
using TotalPortal.Areas.Inventories.Builders;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class NegativeAdjustmentsController : WarehouseAdjustmentsController
    {
        public NegativeAdjustmentsController(IWarehouseAdjustmentService warehouseAdjustmentService, IWarehouseAdjustmentViewModelSelectListBuilder warehouseAdjustmentViewModelSelectListBuilder)
            : base(warehouseAdjustmentService, warehouseAdjustmentViewModelSelectListBuilder)
        {
            ViewBag.NegativeOnly = true;
        }

    }
}
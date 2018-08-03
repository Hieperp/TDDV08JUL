using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using TotalBase.Enums;
using TotalDTO.Inventories;
using TotalModel.Models;

using TotalCore.Services.Inventories;

using TotalPortal.Controllers;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class WarehouseAdjustmentsController : GenericViewDetailController<WarehouseAdjustment, WarehouseAdjustmentDetail, WarehouseAdjustmentViewDetail, WarehouseAdjustmentDTO, WarehouseAdjustmentPrimitiveDTO, WarehouseAdjustmentDetailDTO, WarehouseAdjustmentViewModel>
    {
        public WarehouseAdjustmentsController(IWarehouseAdjustmentService productionOrderService, IWarehouseAdjustmentViewModelSelectListBuilder productionOrderViewModelSelectListBuilder)
            : base(productionOrderService, productionOrderViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Parts);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Material);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }

        public virtual ActionResult GetPendingPlannedOrderDetails()
        {
            this.AddRequireJsOptions();
            return View();
        }

    }

}
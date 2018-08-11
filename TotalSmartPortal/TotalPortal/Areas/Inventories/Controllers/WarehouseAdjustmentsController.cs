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
        public WarehouseAdjustmentsController(IWarehouseAdjustmentService warehouseAdjustmentService, IWarehouseAdjustmentViewModelSelectListBuilder warehouseAdjustmentViewModelSelectListBuilder)
            : base(warehouseAdjustmentService, warehouseAdjustmentViewModelSelectListBuilder, true)
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

        public virtual ActionResult GetGoodsReceiptDetailAvailables()
        {
            this.AddRequireJsOptions();
            return View();
        }

    }

}
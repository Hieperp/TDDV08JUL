using System.Net;
using System.Web.Mvc;
using System.Text;

using RequireJsNet;

using TotalBase.Enums;
using TotalModel;
using TotalDTO;
using TotalDTO.Inventories;
using TotalModel.Models;
using TotalPortal.ViewModels.Helpers;

using TotalCore.Services.Inventories;

using TotalPortal.Controllers;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Inventories.Builders;


namespace TotalPortal.Areas.Inventories.Controllers
{
    public class WarehouseAdjustmentsController<TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel> : GenericViewDetailController<WarehouseAdjustment, WarehouseAdjustmentDetail, WarehouseAdjustmentViewDetail, TDto, TPrimitiveDto, TDtoDetail, TViewDetailViewModel>
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO, new()
        where TDtoDetail : class, IPrimitiveEntity
        where TViewDetailViewModel : TDto, IViewDetailViewModel<TDtoDetail>, IA03SimpleViewModel, new()
    {
        public WarehouseAdjustmentsController(IWarehouseAdjustmentService<TDto, TPrimitiveDto, TDtoDetail> warehouseAdjustmentService, IWarehouseAdjustmentViewModelSelectListBuilder<TViewDetailViewModel> warehouseAdjustmentViewModelSelectListBuilder)
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
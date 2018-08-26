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
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Products);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Items);
            commodityTypeIDList.Append(","); commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.Materials);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }

        public virtual ActionResult GetGoodsReceiptDetailAvailables()
        {
            this.AddRequireJsOptions();
            return View();
        }

    }





    public class OtherMaterialReceiptsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionMtlRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlRct>, WarehouseAdjustmentDetailDTO, OtherMaterialReceiptViewModel>
    {
        public OtherMaterialReceiptsController(IOtherMaterialReceiptService otherMaterialReceiptService, IOtherMaterialReceiptViewModelSelectListBuilder otherMaterialReceiptViewModelSelectListBuilder)
            : base(otherMaterialReceiptService, otherMaterialReceiptViewModelSelectListBuilder)
        {
        }
    }

    public class OtherMaterialIssuesController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionMtlIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlIss>, WarehouseAdjustmentDetailDTO, OtherMaterialIssueViewModel>
    {
        public OtherMaterialIssuesController(IOtherMaterialIssueService otherMaterialIssueService, IOtherMaterialIssueViewModelSelectListBuilder otherMaterialIssueViewModelSelectListBuilder)
            : base(otherMaterialIssueService, otherMaterialIssueViewModelSelectListBuilder)
        {
        }
    }

    public class MaterialAdjustmentsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionMtlAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlAdj>, WarehouseAdjustmentDetailDTO, MaterialAdjustmentViewModel>
    {
        public MaterialAdjustmentsController(IMaterialAdjustmentService materialAdjustmentService, IMaterialAdjustmentViewModelSelectListBuilder materialAdjustmentViewModelSelectListBuilder)
            : base(materialAdjustmentService, materialAdjustmentViewModelSelectListBuilder)
        {
        }
    }



    public class OtherItemReceiptsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionItmRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmRct>, WarehouseAdjustmentDetailDTO, OtherItemReceiptViewModel>
    {
        public OtherItemReceiptsController(IOtherItemReceiptService otherItemReceiptService, IOtherItemReceiptViewModelSelectListBuilder otherItemReceiptViewModelSelectListBuilder)
            : base(otherItemReceiptService, otherItemReceiptViewModelSelectListBuilder)
        {
        }
    }

    public class OtherItemIssuesController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionItmIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmIss>, WarehouseAdjustmentDetailDTO, OtherItemIssueViewModel>
    {
        public OtherItemIssuesController(IOtherItemIssueService otherItemIssueService, IOtherItemIssueViewModelSelectListBuilder otherItemIssueViewModelSelectListBuilder)
            : base(otherItemIssueService, otherItemIssueViewModelSelectListBuilder)
        {
        }
    }

    public class ItemAdjustmentsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionItmAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionItmAdj>, WarehouseAdjustmentDetailDTO, ItemAdjustmentViewModel>
    {
        public ItemAdjustmentsController(IItemAdjustmentService itemAdjustmentService, IItemAdjustmentViewModelSelectListBuilder itemAdjustmentViewModelSelectListBuilder)
            : base(itemAdjustmentService, itemAdjustmentViewModelSelectListBuilder)
        {
        }
    }



    public class OtherProductReceiptsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionPrdRct>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdRct>, WarehouseAdjustmentDetailDTO, OtherProductReceiptViewModel>
    {
        public OtherProductReceiptsController(IOtherProductReceiptService otherProductReceiptService, IOtherProductReceiptViewModelSelectListBuilder otherProductReceiptViewModelSelectListBuilder)
            : base(otherProductReceiptService, otherProductReceiptViewModelSelectListBuilder)
        {
        }
    }

    public class OtherProductIssuesController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionPrdIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdIss>, WarehouseAdjustmentDetailDTO, OtherProductIssueViewModel>
    {
        public OtherProductIssuesController(IOtherProductIssueService otherProductIssueService, IOtherProductIssueViewModelSelectListBuilder otherProductIssueViewModelSelectListBuilder)
            : base(otherProductIssueService, otherProductIssueViewModelSelectListBuilder)
        {
        }
    }

    public class ProductAdjustmentsController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionPrdAdj>, WarehouseAdjustmentPrimitiveDTO<WAOptionPrdAdj>, WarehouseAdjustmentDetailDTO, ProductAdjustmentViewModel>
    {
        public ProductAdjustmentsController(IProductAdjustmentService productAdjustmentService, IProductAdjustmentViewModelSelectListBuilder productAdjustmentViewModelSelectListBuilder)
            : base(productAdjustmentService, productAdjustmentViewModelSelectListBuilder)
        {
        }
    }
}
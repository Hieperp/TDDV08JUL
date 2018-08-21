using System.Web.Mvc;
using TotalCore.Services.Inventories;
using TotalDTO.Inventories;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class OtherMaterialIssuesController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionMtlIss>, WarehouseAdjustmentPrimitiveDTO<WAOptionMtlIss>, WarehouseAdjustmentDetailDTO, OtherMaterialIssueViewModel>
    {
        public OtherMaterialIssuesController(IOtherMaterialIssueService otherMaterialIssueService, IOtherMaterialIssueViewModelSelectListBuilder otherMaterialIssueViewModelSelectListBuilder)
            : base(otherMaterialIssueService, otherMaterialIssueViewModelSelectListBuilder)
        {
        }
    }
}
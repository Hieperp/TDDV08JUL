using System.Web.Mvc;
using TotalCore.Services.Inventories;
using TotalDTO.Inventories;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class OtherMaterialIssuesController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionMTLISS>, WarehouseAdjustmentPrimitiveDTO<WAOptionMTLISS>, WarehouseAdjustmentDetailDTO, OtherMaterialIssueViewModel>
    {
        public OtherMaterialIssuesController(IOtherMaterialIssueService otherMaterialIssueService, IOtherMaterialIssueViewModelSelectListBuilder otherMaterialIssueViewModelSelectListBuilder)
            : base(otherMaterialIssueService, otherMaterialIssueViewModelSelectListBuilder)
        {
        }
    }
}
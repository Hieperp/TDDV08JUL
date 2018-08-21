using System.Web.Mvc;
using TotalCore.Services.Inventories;
using TotalDTO.Inventories;
using TotalPortal.Areas.Inventories.Builders;
using TotalPortal.Areas.Inventories.ViewModels;

namespace TotalPortal.Areas.Inventories.Controllers
{
    public class OtherProductIssuesController : WarehouseAdjustmentsController<WarehouseAdjustmentDTO<WAOptionMTLISS>, WarehouseAdjustmentPrimitiveDTO<WAOptionMTLISS>, WarehouseAdjustmentDetailDTO, OtherProductIssueViewModel>
    {
        public OtherProductIssuesController(IOtherProductIssueService otherProductIssueService, IOtherProductIssueViewModelSelectListBuilder otherProductIssueViewModelSelectListBuilder)
            : base(otherProductIssueService, otherProductIssueViewModelSelectListBuilder)
        {
        }
    }
}
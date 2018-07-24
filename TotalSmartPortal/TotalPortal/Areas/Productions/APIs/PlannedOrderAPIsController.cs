using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalModel.Models;
using TotalCore.Repositories.Productions;

using TotalPortal.APIs.Sessions;

namespace TotalPortal.Areas.Productions.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PlannedOrderAPIsController : Controller
    {
        private readonly IPlannedOrderAPIRepository plannedOrderAPIRepository;

        public PlannedOrderAPIsController(IPlannedOrderAPIRepository plannedOrderAPIRepository)
        {
            this.plannedOrderAPIRepository = plannedOrderAPIRepository;
        }


        public JsonResult GetPlannedOrderIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<PlannedOrderIndex> plannedOrderIndexes = this.plannedOrderAPIRepository.GetEntityIndexes<PlannedOrderIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = plannedOrderIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
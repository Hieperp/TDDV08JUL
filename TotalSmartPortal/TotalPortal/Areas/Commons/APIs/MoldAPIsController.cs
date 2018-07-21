using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalCore.Repositories.Commons;
using TotalModel.Models;
using TotalDTO.Commons;
using TotalPortal.APIs.Sessions;

namespace TotalPortal.Areas.Commons.APIs
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class MoldAPIsController : Controller
    {
        private readonly IMoldAPIRepository moldAPIRepository;

        public MoldAPIsController(IMoldAPIRepository moldAPIRepository)
        {
            this.moldAPIRepository = moldAPIRepository;
        }

       
        public JsonResult GetMoldBases(string searchText, int warehouseTaskID)
        {
            var result = this.moldAPIRepository.GetMoldBases(searchText, warehouseTaskID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
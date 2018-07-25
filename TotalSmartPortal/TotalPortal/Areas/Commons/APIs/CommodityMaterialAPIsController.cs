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
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CommodityMaterialAPIsController : Controller
    {
        private readonly ICommodityMaterialAPIRepository commodityMaterialAPIRepository;

        public CommodityMaterialAPIsController(ICommodityMaterialAPIRepository commodityMaterialAPIRepository)
        {
            this.commodityMaterialAPIRepository = commodityMaterialAPIRepository;
        }


        public JsonResult GetCommodityMaterialBases(string searchText, int warehouseTaskID)
        {
            var result = this.commodityMaterialAPIRepository.GetCommodityMaterialBases(searchText, warehouseTaskID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.UI;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Inventories;

using TotalCore.Repositories.Inventories;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.APIs.Sessions;

using Microsoft.AspNet.Identity;

namespace TotalPortal.Areas.Inventories.APIs
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class MaterialIssueAPIsController : Controller
    {
        private readonly IMaterialIssueAPIRepository materialIssueAPIRepository;

        public MaterialIssueAPIsController(IMaterialIssueAPIRepository materialIssueAPIRepository)
        {
            this.materialIssueAPIRepository = materialIssueAPIRepository;
        }


        public JsonResult GetMaterialIssueIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<MaterialIssueIndex> materialIssueIndexes = this.materialIssueAPIRepository.GetEntityIndexes<MaterialIssueIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = materialIssueIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }





        public JsonResult GetWorkshifts([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.materialIssueAPIRepository.GetWorkshifts(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductionOrders([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.materialIssueAPIRepository.GetProductionOrders(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingProductionOrderDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? materialIssueID, int? productionOrderID, int? workshiftID, int warehouseID, string productionOrderDetailIDs, string goodsReceiptDetailIDs)
        {
            var result = this.materialIssueAPIRepository.GetPendingProductionOrderDetails(locationID, materialIssueID, productionOrderID, workshiftID, warehouseID, productionOrderDetailIDs, goodsReceiptDetailIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }



    }
}
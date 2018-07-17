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
    public class GoodsReceiptAPIsController : Controller
    {
        private readonly IGoodsReceiptAPIRepository goodsReceiptAPIRepository;

        public GoodsReceiptAPIsController(IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
        {
            this.goodsReceiptAPIRepository = goodsReceiptAPIRepository;
        }


        public JsonResult GetGoodsReceiptIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<GoodsReceiptIndex> goodsReceiptIndexes = this.goodsReceiptAPIRepository.GetEntityIndexes<GoodsReceiptIndex>(User.Identity.GetUserId(), HomeSession.GetGlobalFromDate(this.HttpContext), HomeSession.GetGlobalToDate(this.HttpContext));

            DataSourceResult response = goodsReceiptIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }





        public JsonResult GetCustomers([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.goodsReceiptAPIRepository.GetCustomers(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPurchaseRequisitions([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID)
        {
            var result = this.goodsReceiptAPIRepository.GetPurchaseRequisitions(locationID);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPendingPurchaseRequisitionDetails([DataSourceRequest] DataSourceRequest dataSourceRequest, int? locationID, int? goodsReceiptID, int? purchaseRequisitionID, int? customerID, string purchaseRequisitionDetailIDs, bool isReadonly)
        {
            var result = this.goodsReceiptAPIRepository.GetPendingPurchaseRequisitionDetails(locationID, goodsReceiptID, purchaseRequisitionID, customerID, purchaseRequisitionDetailIDs, false);
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }



    }
}

using System;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Inventories;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Inventories.ViewModels
{
    public class GoodsReceiptViewModel : GoodsReceiptDTO, IViewDetailViewModel<GoodsReceiptDetailDTO>, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

}

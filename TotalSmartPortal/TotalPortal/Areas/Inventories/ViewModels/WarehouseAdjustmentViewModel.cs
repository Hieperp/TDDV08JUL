using System;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalBase.Enums;
using TotalDTO.Inventories;
using TotalPortal.Builders;
using TotalPortal.ViewModels.Helpers;
using TotalPortal.Areas.Commons.ViewModels.Helpers;
using TotalPortal.Areas.Inventories.Builders;

namespace TotalPortal.Areas.Inventories.ViewModels
{
    public class WarehouseAdjustmentViewModel : WarehouseAdjustmentDTO, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IA03SimpleViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }

    public class OtherMaterialReceiptViewModel : WarehouseAdjustmentViewModel
    { }
    public class OtherMaterialIssueViewModel : WarehouseAdjustmentViewModel
    { }
    public class MaterialAdjustmentViewModel : WarehouseAdjustmentViewModel
    { }

    public class OtherProductReceiptViewModel : WarehouseAdjustmentViewModel
    { }
    public class OtherProductIssueViewModel : WarehouseAdjustmentViewModel
    { }
    public class ProductAdjustmentViewModel : WarehouseAdjustmentViewModel
    { }

}
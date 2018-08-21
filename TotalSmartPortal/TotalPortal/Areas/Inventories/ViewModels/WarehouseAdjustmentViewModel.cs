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
    public interface IWarehouseAdjustmentViewModel : IWarehouseAdjustmentDTO, IA03SimpleViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel
    { }

    public class OtherMaterialReceiptViewModel : WarehouseAdjustmentDTO<WAOptionMTLRCT>, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IA03SimpleViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel, IWarehouseAdjustmentViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }

    public class OtherMaterialIssueViewModel : WarehouseAdjustmentDTO<WAOptionMTLISS>, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IA03SimpleViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel, IWarehouseAdjustmentViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }

    public class MaterialAdjustmentViewModel : OtherMaterialIssueViewModel
    { }

    public class OtherProductReceiptViewModel : OtherMaterialIssueViewModel
    { }
    public class OtherProductIssueViewModel : OtherMaterialIssueViewModel
    { }
    public class ProductAdjustmentViewModel : OtherMaterialIssueViewModel
    { }
}
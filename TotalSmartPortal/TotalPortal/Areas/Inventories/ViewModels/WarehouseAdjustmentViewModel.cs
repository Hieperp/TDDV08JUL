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
    public interface IWarehouseAdjustmentViewModel : IA01SimpleViewModel
    {
        IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }

    public class WarehouseAdjustmentViewModel : WarehouseAdjustmentDTO, IViewDetailViewModel<WarehouseAdjustmentDetailDTO>, IWarehouseAdjustmentViewModel, IPreparedPersonDropDownViewModel, IApproverDropDownViewModel, IA01SimpleViewModel, IWarehouseAdjustmentTypeDropDownViewModel
    {
        public IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }

    public class OtherMaterialIssueViewModel : WarehouseAdjustmentViewModel
    { }
}
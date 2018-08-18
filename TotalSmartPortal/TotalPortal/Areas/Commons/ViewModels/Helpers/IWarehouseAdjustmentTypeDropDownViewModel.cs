using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{

    public interface IWarehouseAdjustmentTypeDropDownViewModel
    {
        [Display(Name = "Phân loại")]
        int WarehouseAdjustmentTypeID { get; set; }
        IEnumerable<SelectListItem> WarehouseAdjustmentTypeSelectList { get; set; }
    }
}
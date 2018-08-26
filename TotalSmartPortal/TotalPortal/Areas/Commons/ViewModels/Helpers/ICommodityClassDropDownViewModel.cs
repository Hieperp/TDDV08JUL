using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICommodityClassDropDownViewModel
    {
        [Display(Name = "Nhóm A/C/G/F")]
        int CommodityClassID { get; set; }
        IEnumerable<SelectListItem> CommodityClassSelectList { get; set; }
    }
}
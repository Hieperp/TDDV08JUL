using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICommodityLineDropDownViewModel
    {
        int CommodityLineID { get; set; }
        IEnumerable<SelectListItem> CommodityLineSelectList { get; set; }
    }
}
﻿using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TotalPortal.Areas.Commons.ViewModels.Helpers
{
    public interface ICommodityClassDropDownViewModel
    {
        int CommodityClassID { get; set; }
        IEnumerable<SelectListItem> CommodityClassSelectList { get; set; }
    }
}
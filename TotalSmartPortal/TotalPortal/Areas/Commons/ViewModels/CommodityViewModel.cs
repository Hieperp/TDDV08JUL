using System.Web.Mvc;
using System.Collections.Generic;

using TotalDTO.Commons;
using TotalPortal.ViewModels.Helpers;

using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Commons.ViewModels
{
    public class CommodityViewModel : CommodityDTO, ISimpleViewModel, ICommodityBrandDropDownViewModel, ICommodityCategoryDropDownViewModel, ICommodityTypeDropDownViewModel
    {
        public IEnumerable<SelectListItem> CommodityBrandSelectList { get; set; }
        public IEnumerable<SelectListItem> CommodityCategorySelectList { get; set; }
        public IEnumerable<SelectListItem> CommodityTypeSelectList { get; set; }
    }
}
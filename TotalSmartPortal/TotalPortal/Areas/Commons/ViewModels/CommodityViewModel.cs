using System.Web.Mvc;
using System.Collections.Generic;

using TotalDTO.Commons;
using TotalPortal.ViewModels.Helpers;

using TotalPortal.Areas.Commons.Builders;
using TotalPortal.Areas.Commons.ViewModels.Helpers;

namespace TotalPortal.Areas.Commons.ViewModels
{
    public interface ICommodityViewModel : ICommodityDTO, ISimpleViewModel, IA0XSimpleViewModel, ICommodityBrandDropDownViewModel, ICommodityCategoryDropDownViewModel, ICommodityTypeDropDownViewModel
    {
        
    }

    public class MaterialViewModel : CommodityDTO<CMDMaterial>, ISimpleViewModel, ICommodityBrandDropDownViewModel, ICommodityCategoryDropDownViewModel, ICommodityTypeDropDownViewModel, ICommodityViewModel
    {
        public IEnumerable<SelectListItem> CommodityBrandSelectList { get; set; }
        public IEnumerable<SelectListItem> CommodityCategorySelectList { get; set; }
        public IEnumerable<SelectListItem> CommodityTypeSelectList { get; set; }
    }

    public class ItemViewModel : CommodityDTO<CMDItem>, ISimpleViewModel, ICommodityBrandDropDownViewModel, ICommodityCategoryDropDownViewModel, ICommodityTypeDropDownViewModel, ICommodityViewModel
    {
        public IEnumerable<SelectListItem> CommodityBrandSelectList { get; set; }
        public IEnumerable<SelectListItem> CommodityCategorySelectList { get; set; }
        public IEnumerable<SelectListItem> CommodityTypeSelectList { get; set; }
    }

    public class ProductViewModel : CommodityDTO<CMDProduct>, ISimpleViewModel, ICommodityBrandDropDownViewModel, ICommodityCategoryDropDownViewModel, ICommodityTypeDropDownViewModel, ICommodityViewModel
    {
        public IEnumerable<SelectListItem> CommodityBrandSelectList { get; set; }
        public IEnumerable<SelectListItem> CommodityCategorySelectList { get; set; }
        public IEnumerable<SelectListItem> CommodityTypeSelectList { get; set; }
    }

}
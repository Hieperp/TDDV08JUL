using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalPortal.Areas.Commons.Builders
{  
    public interface ICommodityClassSelectListBuilder
    {
        IEnumerable<SelectListItem> BuildSelectListItemsForCommodityClasss(IEnumerable<CommodityClass> CommodityClasss);
    }

    public class CommodityClassSelectListBuilder : ICommodityClassSelectListBuilder
    {
        public IEnumerable<SelectListItem> BuildSelectListItemsForCommodityClasss(IEnumerable<CommodityClass> CommodityClasss)
        {
            return CommodityClasss.Select(pt => new SelectListItem { Text = pt.Name, Value = pt.CommodityClassID.ToString() }).ToList();
        }
    }
}
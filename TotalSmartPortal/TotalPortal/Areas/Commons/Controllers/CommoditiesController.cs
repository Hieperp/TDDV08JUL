using System.Net;
using System.Linq;
using System.Web.Mvc;

using TotalBase.Enums;
using TotalModel.Models;

using TotalDTO.Commons;

using TotalCore.Repositories.Commons;
using TotalCore.Services.Commons;

using TotalPortal.Controllers;
using TotalPortal.Areas.Commons.ViewModels;
using TotalPortal.Areas.Commons.Builders;


namespace TotalPortal.Areas.Commons.Controllers
{

    public class CommoditiesController : GenericSimpleController<Commodity, CommodityDTO, CommodityPrimitiveDTO, CommodityViewModel>
    {
        private ICommodityService commodityService;        

        public CommoditiesController(ICommodityService commodityService, ICommoditySelectListBuilder commodityViewModelSelectListBuilder)
            : base(commodityService, commodityViewModelSelectListBuilder)
        {
            this.commodityService = commodityService;            
        }
               

    }
}
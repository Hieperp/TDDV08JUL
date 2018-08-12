using System.Linq;
using System.Collections.Generic;

using TotalModel.Models;
using TotalCore.Repositories.Commons;
namespace TotalDAL.Repositories.Commons
{
    public class CommodityCategoryRepository : ICommodityCategoryRepository
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public CommodityCategoryRepository(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public IList<CommodityCategory> GetAllCommodityCategories()
        {
            return this.totalSmartPortalEntities.CommodityCategories.ToList();
        }

    }
}

using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class CommodityMaterialRepository : GenericRepository<CommodityMaterial>, ICommodityMaterialRepository
    {
        public CommodityMaterialRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities)
        {
        }
    }



    public class CommodityMaterialAPIRepository : GenericAPIRepository, ICommodityMaterialAPIRepository
    {
        public CommodityMaterialAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetCommodityMaterialIndexes")
        {
        }

        public IList<CommodityMaterialBase> GetCommodityMaterialBases(string searchText, int commodityID)
        {
            List<CommodityMaterialBase> commodityMaterialBases = this.TotalSmartPortalEntities.GetCommodityMaterialBases(searchText, commodityID).ToList();

            return commodityMaterialBases;
        }
    }

}
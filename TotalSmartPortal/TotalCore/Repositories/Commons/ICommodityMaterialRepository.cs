using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface ICommodityMaterialRepository : IGenericRepository<CommodityMaterial>
    {
    }

    public interface ICommodityMaterialAPIRepository : IGenericAPIRepository
    {
        IList<CommodityMaterialBase> GetCommodityMaterialBases(string searchText, int commodityID);
    }
}
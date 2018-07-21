using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

using TotalModel.Models;
using TotalCore.Repositories.Commons;

namespace TotalDAL.Repositories.Commons
{
    public class MoldRepository : GenericRepository<Mold>, IMoldRepository
    {
        public MoldRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities)
        {
        }
    }



    public class MoldAPIRepository : GenericAPIRepository, IMoldAPIRepository
    {
        public MoldAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetMoldIndexes")
        {
        }

        public IList<MoldBase> GetMoldBases(string searchText, int commodityID)
        {
            List<MoldBase> moldBases = this.TotalSmartPortalEntities.GetMoldBases(searchText, commodityID).ToList();

            return moldBases;
        }
    }

}
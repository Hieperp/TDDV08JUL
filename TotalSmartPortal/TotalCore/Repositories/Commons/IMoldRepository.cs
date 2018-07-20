using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Commons
{
    public interface IMoldRepository : IGenericRepository<Mold>
    {
    }

    public interface IMoldAPIRepository : IGenericAPIRepository
    {
        IList<MoldBase> GetMoldBases(string searchText, int commodityID);
    }
}
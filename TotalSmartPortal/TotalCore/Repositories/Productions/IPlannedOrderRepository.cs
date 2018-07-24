using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IPlannedOrderRepository : IGenericWithDetailRepository<PlannedOrder, PlannedOrderDetail>
    {
    }

    public interface IPlannedOrderAPIRepository : IGenericAPIRepository
    {
    }
}
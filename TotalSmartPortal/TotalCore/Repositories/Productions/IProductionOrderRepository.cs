using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IProductionOrderRepository : IGenericWithDetailRepository<ProductionOrder, ProductionOrderDetail>
    {
    }

    public interface IProductionOrderAPIRepository : IGenericAPIRepository
    {
    }
}
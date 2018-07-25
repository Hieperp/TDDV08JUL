using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Productions
{
    public interface IProductionOrderRepository : IGenericWithDetailRepository<ProductionOrder, ProductionOrderDetail>
    {
    }

    public interface IProductionOrderAPIRepository : IGenericAPIRepository
    {
        IEnumerable<ProductionOrderPendingCustomer> GetCustomers(int? locationID);
        IEnumerable<ProductionOrderPendingPlannedOrder> GetPlannedOrders(int? locationID);

        IEnumerable<ProductionOrderPendingPlannedOrderDetail> GetPendingPlannedOrderDetails(int? locationID, int? productionOrderID, int? plannedOrderID, int? customerID, string plannedOrderDetailIDs, bool isReadonly);
    }
}
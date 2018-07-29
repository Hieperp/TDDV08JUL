using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IMaterialIssueRepository : IGenericWithDetailRepository<MaterialIssue, MaterialIssueDetail>
    {
    }

    public interface IMaterialIssueAPIRepository : IGenericAPIRepository
    {
        IEnumerable<MaterialIssuePendingPlannedOrder> GetPlannedOrders(int? locationID);

        IEnumerable<MaterialIssuePendingPlannedOrderDetail> GetPendingPlannedOrderDetails(int? locationID, int? materialIssueID, int? plannedOrderDetailID, int? workshiftID, int? moldID, int? warehouseID, string plannedOrderMaterialIDs, bool isReadonly);
    }

}
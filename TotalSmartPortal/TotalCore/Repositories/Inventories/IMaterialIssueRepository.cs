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
        IEnumerable<MaterialIssuePendingWorkshift> GetWorkshifts(int? locationID);
        IEnumerable<MaterialIssuePendingPlannedOrder> GetPlannedOrders(int? locationID);

        IEnumerable<MaterialIssuePendingPlannedOrderDetail> GetPendingPlannedOrderDetails(int? locationID, int? materialIssueID, int? plannedOrderID, int? workshiftID, int warehouseID, string productionOrderDetailIDs, string goodsReceiptDetailIDs, bool isReadonly);
    }

}
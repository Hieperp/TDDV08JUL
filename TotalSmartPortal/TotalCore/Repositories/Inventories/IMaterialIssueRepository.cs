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
        IEnumerable<MaterialIssuePendingProductionOrder> GetProductionOrders(int? locationID);

        IEnumerable<MaterialIssuePendingProductionOrderDetail> GetPendingProductionOrderDetails(int? locationID, int? materialIssueID, int? productionOrderID, int? workshiftID, int warehouseID, string productionOrderDetailIDs, string goodsReceiptDetailIDs, bool isReadonly);
    }

}
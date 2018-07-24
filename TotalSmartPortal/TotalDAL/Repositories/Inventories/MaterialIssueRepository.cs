using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Inventories;

namespace TotalDAL.Repositories.Inventories
{
    public class MaterialIssueRepository : GenericWithDetailRepository<MaterialIssue, MaterialIssueDetail>, IMaterialIssueRepository
    {
        public MaterialIssueRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "MaterialIssueEditable", "MaterialIssueApproved")
        {
        }
    }








    public class MaterialIssueAPIRepository : GenericAPIRepository, IMaterialIssueAPIRepository
    {
        public MaterialIssueAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetMaterialIssueIndexes")
        {
        }

        public IEnumerable<MaterialIssuePendingWorkshift> GetWorkshifts(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<MaterialIssuePendingWorkshift> pendingProductionOrderWorkshifts = base.TotalSmartPortalEntities.GetMaterialIssuePendingWorkshifts(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingProductionOrderWorkshifts;
        }

        public IEnumerable<MaterialIssuePendingPlannedOrder> GetPlannedOrders(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<MaterialIssuePendingPlannedOrder> pendingPlannedOrders = base.TotalSmartPortalEntities.GetMaterialIssuePendingPlannedOrders(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrders;
        }

        public IEnumerable<MaterialIssuePendingPlannedOrderDetail> GetPendingPlannedOrderDetails(int? locationID, int? materialIssueID, int? plannedOrderID, int? workshiftID, int warehouseID, string productionOrderDetailIDs, string goodsReceiptDetailIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<MaterialIssuePendingPlannedOrderDetail> pendingPlannedOrderDetails = base.TotalSmartPortalEntities.GetMaterialIssuePendingPlannedOrderDetails(locationID, materialIssueID, plannedOrderID, workshiftID, warehouseID, productionOrderDetailIDs, goodsReceiptDetailIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderDetails;
        }

    }


}

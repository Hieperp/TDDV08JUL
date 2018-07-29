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

        public IEnumerable<MaterialIssuePendingPlannedOrder> GetPlannedOrders(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<MaterialIssuePendingPlannedOrder> pendingPlannedOrders = base.TotalSmartPortalEntities.GetMaterialIssuePendingPlannedOrders(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrders;
        }

        public IEnumerable<MaterialIssuePendingPlannedOrderDetail> GetPendingPlannedOrderDetails(int? locationID, int? materialIssueID, int? plannedOrderDetailID, int? workshiftID, int? moldID, int? warehouseID, string plannedOrderMaterialIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<MaterialIssuePendingPlannedOrderDetail> pendingPlannedOrderDetails = base.TotalSmartPortalEntities.GetMaterialIssuePendingPlannedOrderDetails(locationID, materialIssueID, plannedOrderDetailID, workshiftID, moldID, warehouseID, plannedOrderMaterialIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderDetails;
        }

    }


}

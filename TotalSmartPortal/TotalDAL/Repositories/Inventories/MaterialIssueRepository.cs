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

        public IEnumerable<MaterialIssuePendingProductionOrder> GetProductionOrders(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<MaterialIssuePendingProductionOrder> pendingProductionOrders = base.TotalSmartPortalEntities.GetMaterialIssuePendingProductionOrders(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingProductionOrders;
        }

        public IEnumerable<MaterialIssuePendingProductionOrderDetail> GetPendingProductionOrderDetails(int? locationID, int? materialIssueID, int? productionOrderID, int? workshiftID, int warehouseID, string productionOrderDetailIDs, string goodsReceiptDetailIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<MaterialIssuePendingProductionOrderDetail> pendingProductionOrderDetails = base.TotalSmartPortalEntities.GetMaterialIssuePendingProductionOrderDetails(locationID, materialIssueID, productionOrderID, workshiftID, warehouseID, productionOrderDetailIDs, goodsReceiptDetailIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingProductionOrderDetails;
        }

    }


}

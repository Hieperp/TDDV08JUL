using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Productions;

namespace TotalDAL.Repositories.Productions
{
    public class ProductionOrderRepository : GenericWithDetailRepository<ProductionOrder, ProductionOrderDetail>, IProductionOrderRepository
    {
        public ProductionOrderRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "ProductionOrderEditable", "ProductionOrderApproved", null, "ProductionOrderVoidable")
        {
        }
    }








    public class ProductionOrderAPIRepository : GenericAPIRepository, IProductionOrderAPIRepository
    {
        public ProductionOrderAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetProductionOrderIndexes")
        {
        }

        public IEnumerable<ProductionOrderPendingCustomer> GetCustomers(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<ProductionOrderPendingCustomer> pendingPlannedOrderCustomers = base.TotalSmartPortalEntities.GetProductionOrderPendingCustomers(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderCustomers;
        }

        public IEnumerable<ProductionOrderPendingPlannedOrder> GetPlannedOrders(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<ProductionOrderPendingPlannedOrder> pendingPlannedOrders = base.TotalSmartPortalEntities.GetProductionOrderPendingPlannedOrders(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrders;
        }

        public IEnumerable<ProductionOrderPendingPlannedOrderDetail> GetPendingPlannedOrderDetails(int? locationID, int? productionOrderID, int? plannedOrderID, int? customerID, string plannedOrderDetailIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<ProductionOrderPendingPlannedOrderDetail> pendingPlannedOrderDetails = base.TotalSmartPortalEntities.GetProductionOrderPendingPlannedOrderDetails(locationID, productionOrderID, plannedOrderID, customerID, plannedOrderDetailIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPlannedOrderDetails;
        }
    }


}
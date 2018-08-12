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
    public class GoodsReceiptRepository : GenericWithDetailRepository<GoodsReceipt, GoodsReceiptDetail>, IGoodsReceiptRepository
    {
        public GoodsReceiptRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GoodsReceiptEditable", "GoodsReceiptApproved")
        {
        }
    }








    public class GoodsReceiptAPIRepository : GenericAPIRepository, IGoodsReceiptAPIRepository
    {
        public GoodsReceiptAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetGoodsReceiptIndexes")
        {
        }

        public IEnumerable<GoodsReceiptPendingCustomer> GetCustomers(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingCustomer> pendingPurchaseRequisitionCustomers = base.TotalSmartPortalEntities.GetGoodsReceiptPendingCustomers(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPurchaseRequisitionCustomers;
        }

        public IEnumerable<GoodsReceiptPendingPurchaseRequisition> GetPurchaseRequisitions(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingPurchaseRequisition> pendingPurchaseRequisitions = base.TotalSmartPortalEntities.GetGoodsReceiptPendingPurchaseRequisitions(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPurchaseRequisitions;
        }

        public IEnumerable<GoodsReceiptPendingPurchaseRequisitionDetail> GetPendingPurchaseRequisitionDetails(int? locationID, int? goodsReceiptID, int? purchaseRequisitionID, int? customerID, string purchaseRequisitionDetailIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptPendingPurchaseRequisitionDetail> pendingPurchaseRequisitionDetails = base.TotalSmartPortalEntities.GetGoodsReceiptPendingPurchaseRequisitionDetails(locationID, goodsReceiptID, purchaseRequisitionID, customerID, purchaseRequisitionDetailIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingPurchaseRequisitionDetails;
        }




        public List<PendingWarehouseAdjustmentDetail> GetPendingWarehouseAdjustmentDetails(int? locationID, int? goodsReceiptID, int? warehouseAdjustmentID, int? warehouseID, string warehouseAdjustmentDetailIDs, bool isReadonly)
        {
            return base.TotalSmartPortalEntities.GetPendingWarehouseAdjustmentDetails(locationID, goodsReceiptID, warehouseAdjustmentID, warehouseID, warehouseAdjustmentDetailIDs, isReadonly).ToList();
        }

        public int? GetGoodsReceiptIDofWarehouseAdjustment(int? warehouseAdjustmentID)
        {
            return base.TotalSmartPortalEntities.GetGoodsReceiptIDofWarehouseAdjustment(warehouseAdjustmentID).FirstOrDefault();
        }



        public IEnumerable<GoodsReceiptDetailAvailable> GetGoodsReceiptDetailAvailables(int? locationID, int? warehouseID, int? commodityID, string commodityIDs, int? batchID, string goodsReceiptDetailIDs, bool onlyApproved, bool onlyIssuable)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<GoodsReceiptDetailAvailable> goodsReceiptDetailAvailables = base.TotalSmartPortalEntities.GetGoodsReceiptDetailAvailables(locationID, warehouseID, commodityID, commodityIDs, batchID, goodsReceiptDetailIDs, onlyApproved, onlyIssuable).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return goodsReceiptDetailAvailables;
        }

    }


}

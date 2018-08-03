using System;
using System.Collections.Generic;

using TotalModel.Models;

namespace TotalCore.Repositories.Inventories
{
    public interface IGoodsReceiptRepository : IGenericWithDetailRepository<GoodsReceipt, GoodsReceiptDetail>
    {
    }

    public interface IGoodsReceiptAPIRepository : IGenericAPIRepository
    {
        IEnumerable<GoodsReceiptPendingCustomer> GetCustomers(int? locationID);
        IEnumerable<GoodsReceiptPendingPurchaseRequisition> GetPurchaseRequisitions(int? locationID);

        IEnumerable<GoodsReceiptPendingPurchaseRequisitionDetail> GetPendingPurchaseRequisitionDetails(int? locationID, int? goodsReceiptID, int? purchaseRequisitionID, int? customerID, string purchaseRequisitionDetailIDs, bool isReadonly);

        IEnumerable<GoodsReceiptDetailAvailable> GetGoodsReceiptDetailAvailables(int? locationID, int? warehouseID, int? commodityID, string commodityIDs, int? batchID, string goodsReceiptDetailIDs, bool onlyApproved, bool onlyIssuable);
    }

}

using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalBase.Enums;
using TotalModel.Models;
using TotalCore.Repositories.Sales;


namespace TotalDAL.Repositories.Sales
{
    public class DeliveryAdviceRepository : GenericWithDetailRepository<DeliveryAdvice, DeliveryAdviceDetail>, IDeliveryAdviceRepository
    {
        //1-Set Balance Date to 23.59.59
        //2-Copy 3 table (D.A/ GoodsIssue)
        //3-Add two Store procedure (Update balance/ WH journal)
        //4-Modify to VB Project, verify report 1280.rpt (-> create new report in SSRS -> publish to server)


        //31/03/18: CHU Y: SalesReturnSaveRelative CO NEN LAM GIONG NHU GoodsIssueSaveRelative? NHAM EXECUTE SPSKUBalanceUpdate TO UPDATE ???

        public DeliveryAdviceRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "DeliveryAdviceEditable", "DeliveryAdviceApproved", null, "DeliveryAdviceVoidable")
        {
            return;

            Helpers.SqlProgrammability.Analysis.Report report = new Helpers.SqlProgrammability.Analysis.Report(totalSmartPortalEntities);
            report.RestoreProcedure();


            return;

            Helpers.SqlProgrammability.Generals.UserReference userReference = new Helpers.SqlProgrammability.Generals.UserReference(totalSmartPortalEntities);
            userReference.RestoreProcedure();


            return;
            Helpers.SqlProgrammability.Generals.AccessControl accessControl = new Helpers.SqlProgrammability.Generals.AccessControl(totalSmartPortalEntities);
            accessControl.RestoreProcedure();

            //return;


            Helpers.SqlProgrammability.Sales.DeliveryAdvice deliveryAdvice = new Helpers.SqlProgrammability.Sales.DeliveryAdvice(totalSmartPortalEntities);
            deliveryAdvice.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Reports.SaleReports saleReports = new Helpers.SqlProgrammability.Reports.SaleReports(totalSmartPortalEntities);
            saleReports.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Commons.Customer customer = new Helpers.SqlProgrammability.Commons.Customer(totalSmartPortalEntities);
            customer.RestoreProcedure();

            

            



            return;

            return;

            Helpers.SqlProgrammability.Accounts.Receipt receipt = new Helpers.SqlProgrammability.Accounts.Receipt(totalSmartPortalEntities);
            receipt.RestoreProcedure();



           


            return;
            return;

            Helpers.SqlProgrammability.Inventories.HandlingUnit handlingUnit = new Helpers.SqlProgrammability.Inventories.HandlingUnit(totalSmartPortalEntities);
            handlingUnit.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Inventories.GoodsIssue goodsIssue = new Helpers.SqlProgrammability.Inventories.GoodsIssue(totalSmartPortalEntities);
            goodsIssue.RestoreProcedure();

            //return;

            Helpers.SqlProgrammability.Commons.CommodityPrice commodityPrice = new Helpers.SqlProgrammability.Commons.CommodityPrice(totalSmartPortalEntities);
            commodityPrice.RestoreProcedure();

            

           

            
            


            //return;

            //return;
            Helpers.SqlProgrammability.Inventories.Inventories inventories = new Helpers.SqlProgrammability.Inventories.Inventories(totalSmartPortalEntities);
            inventories.RestoreProcedure();
            

            //return;

            Helpers.SqlProgrammability.Sales.SalesOrder salesOrder = new Helpers.SqlProgrammability.Sales.SalesOrder(totalSmartPortalEntities);
            salesOrder.RestoreProcedure();


            //return;

            Helpers.SqlProgrammability.Sales.SalesReturn salesReturn = new Helpers.SqlProgrammability.Sales.SalesReturn(totalSmartPortalEntities);
            salesReturn.RestoreProcedure();





            //return;

            Helpers.SqlProgrammability.Commons.Promotion promotion = new Helpers.SqlProgrammability.Commons.Promotion(totalSmartPortalEntities);
            promotion.RestoreProcedure();

            //return;

            //AccountInvoice: NOT CHECK FOR Approved COMMPLETELY, PLS CHECK IT CAREFULLY LATER. (SaveRelative, GetPendingGoodsIssueDetails, ...). ALSO DO THE SAME CHECK FOR ALL OTHER MODULES
            //AccountInvoice: SHOULD SAVE BillingAddress

            Helpers.SqlProgrammability.Accounts.AccountInvoice accountInvoice = new Helpers.SqlProgrammability.Accounts.AccountInvoice(totalSmartPortalEntities);
            accountInvoice.RestoreProcedure();

            

            //return;


            Helpers.SqlProgrammability.Commons.Commons commons = new Helpers.SqlProgrammability.Commons.Commons(totalSmartPortalEntities);
            commons.RestoreProcedure();


            //return;

            Helpers.SqlProgrammability.Commons.Employee employee = new Helpers.SqlProgrammability.Commons.Employee(totalSmartPortalEntities);
            employee.RestoreProcedure();

            
            







            


            //return;

            Helpers.SqlProgrammability.Accounts.CreditNote creditNote = new Helpers.SqlProgrammability.Accounts.CreditNote(totalSmartPortalEntities);
            creditNote.RestoreProcedure();



            


            

            
            //return;



















































            Helpers.SqlProgrammability.Inventories.GoodsDelivery goodsDelivery = new Helpers.SqlProgrammability.Inventories.GoodsDelivery(totalSmartPortalEntities);
            goodsDelivery.RestoreProcedure();









        }
    }








    public class DeliveryAdviceAPIRepository : GenericAPIRepository, IDeliveryAdviceAPIRepository
    {
        public DeliveryAdviceAPIRepository(TotalSmartPortalEntities totalSmartPortalEntities)
            : base(totalSmartPortalEntities, "GetDeliveryAdviceIndexes")
        {
        }

        protected override ObjectParameter[] GetEntityIndexParameters(string aspUserID, DateTime fromDate, DateTime toDate)
        {

            ObjectParameter[] baseParameters = base.GetEntityIndexParameters(aspUserID, fromDate, toDate);
            ObjectParameter[] objectParameters = new ObjectParameter[] { baseParameters[0], baseParameters[1], baseParameters[2], new ObjectParameter("PendingOnly", this.RepositoryBag.ContainsKey("PendingOnly") && this.RepositoryBag["PendingOnly"] != null ? this.RepositoryBag["PendingOnly"] : false) };

            this.RepositoryBag.Remove("PendingOnly");

            return objectParameters;


            
        }

        public IEnumerable<DeliveryAdvicePendingCustomer> GetCustomers(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<DeliveryAdvicePendingCustomer> pendingSalesOrderCustomers = base.TotalSmartPortalEntities.GetDeliveryAdvicePendingCustomers(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingSalesOrderCustomers;
        }

        public IEnumerable<DeliveryAdvicePendingSalesOrder> GetSalesOrders(int? locationID)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<DeliveryAdvicePendingSalesOrder> pendingSalesOrders = base.TotalSmartPortalEntities.GetDeliveryAdvicePendingSalesOrders(locationID).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingSalesOrders;
        }

        public IEnumerable<DeliveryAdvicePendingSalesOrderDetail> GetPendingSalesOrderDetails(int? locationID, int? deliveryAdviceID, int? salesOrderID, int? customerID, int? receiverID, int? priceCategoryID, int? warehouseID, string shippingAddress, string addressee, int? tradePromotionID, decimal? vatPercent, DateTime? entryDate, string salesOrderDetailIDs, bool isReadonly)
        {
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<DeliveryAdvicePendingSalesOrderDetail> pendingSalesOrderDetails = base.TotalSmartPortalEntities.GetDeliveryAdvicePendingSalesOrderDetails(locationID, deliveryAdviceID, salesOrderID, customerID, receiverID, priceCategoryID, warehouseID, shippingAddress, addressee, tradePromotionID, vatPercent, entryDate, salesOrderDetailIDs, isReadonly).ToList();
            this.TotalSmartPortalEntities.Configuration.ProxyCreationEnabled = true;

            return pendingSalesOrderDetails;
        }

    }


}

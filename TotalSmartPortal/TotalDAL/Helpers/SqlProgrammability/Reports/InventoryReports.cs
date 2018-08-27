using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Reports
{
    public class InventoryReports
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public InventoryReports(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            ////this.SearchWarehouseEntries();
        }



        private void VehicleJournal()
        {
            string queryString = " @WarehouseID int, @FromDate DateTime, @ToDate DateTime " + "\r\n"; //Filter by @LocalWarehouseID to make this stored procedure run faster, but it may be removed without any effect the algorithm

            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime, @LocalToDate DateTime " + "\r\n";
            queryString = queryString + "       SET         @LocalFromDate = @FromDate      SET @LocalToDate = @ToDate " + "\r\n";

            queryString = queryString + "       IF         (@WarehouseID <= 0 ) " + "\r\n";
            queryString = queryString + "                   " + this.VehicleJournalBUILD(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "                   " + this.VehicleJournalBUILD(false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("VehicleJournal", queryString);
        }


        private string VehicleJournalBUILD(bool isAllWarehouses)
        {
            string queryString = "" + "\r\n";

            queryString = queryString + "    BEGIN " + "\r\n";

            if (!isAllWarehouses)
            {
                queryString = queryString + "   DECLARE     @LocalWarehouseID int, @LocationID int " + "\r\n";
                queryString = queryString + "   SET         @LocalWarehouseID = @WarehouseID    " + "\r\n";
                queryString = queryString + "   SET         @LocationID = (SELECT LocationID FROM Warehouses WHERE WarehouseID = @LocalWarehouseID) " + "\r\n";
            }

            queryString = queryString + "       SELECT      Commodities.CommodityID, Commodities.Code, Commodities.Name, Commodities.SalesUnit, Commodities.LeadTime, " + "\r\n";
            queryString = queryString + "                   VehicleJournalMaster.GoodsReceiptDetailID, VehicleJournalMaster.EntryDate, VehicleJournalMaster.ChassisCode, VehicleJournalMaster.EngineCode, VehicleJournalMaster.ColorCode, " + "\r\n";
            queryString = queryString + "                   ISNULL(Warehouses.LocationID, 0) AS LocationID, ISNULL(Warehouses.WarehouseCategoryID, 0) AS WarehouseCategoryID, ISNULL(Warehouses.WarehouseID, 0) AS WarehouseID, ISNULL(Warehouses.Name, '') AS WarehouseName, " + "\r\n";
            queryString = queryString + "                   Customers.CustomerTypeID AS SupplierTypeID, Customers.CustomerID AS SupplierID, Customers.OfficialName AS SupplierName, " + "\r\n";

            queryString = queryString + "                   VehicleJournalMaster.QuantityBegin, VehicleJournalMaster.QuantityInputINV, VehicleJournalMaster.QuantityInputRTN, VehicleJournalMaster.QuantityInputTRF, VehicleJournalMaster.QuantityInputADJ, VehicleJournalMaster.QuantityInputINV + VehicleJournalMaster.QuantityInputRTN + VehicleJournalMaster.QuantityInputTRF + VehicleJournalMaster.QuantityInputADJ AS QuantityInput, " + "\r\n";
            queryString = queryString + "                   VehicleJournalMaster.QuantityIssuedINV, VehicleJournalMaster.QuantityIssuedTRF, VehicleJournalMaster.QuantityIssuedADJ, VehicleJournalMaster.QuantityIssuedINV + VehicleJournalMaster.QuantityIssuedTRF + VehicleJournalMaster.QuantityIssuedADJ AS QuantityIssued, " + "\r\n";
            queryString = queryString + "                   VehicleJournalMaster.QuantityBegin + VehicleJournalMaster.QuantityInputINV + VehicleJournalMaster.QuantityInputRTN + VehicleJournalMaster.QuantityInputTRF + VehicleJournalMaster.QuantityInputADJ - VehicleJournalMaster.QuantityIssuedINV - VehicleJournalMaster.QuantityIssuedTRF - VehicleJournalMaster.QuantityIssuedADJ AS QuantityEnd, " + "\r\n";
            queryString = queryString + "                   VehicleJournalMaster.QuantityOnPurchasing, VehicleJournalMaster.QuantityOnReceipt, " + "\r\n";
            queryString = queryString + "                   VehicleJournalMaster.UnitPrice, VehicleJournalMaster.MovementMIN, VehicleJournalMaster.MovementMAX, VehicleJournalMaster.MovementAVG, " + "\r\n";

            queryString = queryString + "                   VWCommodityCategories.CommodityCategoryID, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name1 AS CommodityCategory1, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name2 AS CommodityCategory2, " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories.Name3 AS CommodityCategory3 " + "\r\n";

            queryString = queryString + "       FROM       (" + "\r\n";

            //--BEGIN-INPUT-OUTPUT-END.END
            queryString = queryString + "                   SELECT  GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.SupplierID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.WarehouseID, " + "\r\n";
            queryString = queryString + "                           GoodsReceiptDetailUnionMaster.QuantityBegin, GoodsReceiptDetailUnionMaster.QuantityInputINV, GoodsReceiptDetailUnionMaster.QuantityInputRTN, GoodsReceiptDetailUnionMaster.QuantityInputTRF, GoodsReceiptDetailUnionMaster.QuantityInputADJ, GoodsReceiptDetailUnionMaster.QuantityIssuedINV, GoodsReceiptDetailUnionMaster.QuantityIssuedTRF, GoodsReceiptDetailUnionMaster.QuantityIssuedADJ, 0 AS QuantityOnPurchasing, 0 AS QuantityOnReceipt, GoodsReceiptDetails.UnitPrice, GoodsReceiptDetailUnionMaster.MovementMIN, GoodsReceiptDetailUnionMaster.MovementMAX, GoodsReceiptDetailUnionMaster.MovementAVG " + "\r\n";

            // NOTE 24.APR.2007: VIEC TINH GIA TON KHO (GoodsReceiptDetails.AmountCostCUR + GoodsReceiptDetails.AmountClearanceCUR)/ GoodsReceiptDetails.Quantity AS UPriceCURInventory, (GoodsReceiptDetails.AmountCostUSD + GoodsReceiptDetails.AmountClearanceUSD)/ GoodsReceiptDetails.Quantity AS UPriceNMDInventory
            // SU DUNG CONG THUC TREN CHI TAM THOI MA THOI, CO THE DAN DEN SAI SO (SU DUNG TAM THOI DE IN BAO CAO KHO CO SO LIEU)
            // SAU NAY NEN SUA LAI, SU DUNG PHEP +/ - MA THOI
            // XEM SPWHAmountCostofsalesGet DE TINH LUONG REMAIN NHE

            queryString = queryString + "                   FROM   (" + "\r\n";
            queryString = queryString + "                           SELECT  GoodsReceiptDetailUnion.GoodsReceiptDetailID, " + "\r\n";
            queryString = queryString + "                                   SUM(QuantityBegin) AS QuantityBegin, SUM(QuantityInputINV) AS QuantityInputINV, SUM(QuantityInputRTN) AS QuantityInputRTN, SUM(QuantityInputTRF) AS QuantityInputTRF, SUM(QuantityInputADJ) AS QuantityInputADJ, SUM(QuantityIssuedINV) AS QuantityIssuedINV, SUM(QuantityIssuedTRF) AS QuantityIssuedTRF, SUM(QuantityIssuedADJ) AS QuantityIssuedADJ, " + "\r\n";
            queryString = queryString + "                                   MIN(MovementDate) AS MovementMIN, MAX(MovementDate) AS MovementMAX, SUM((QuantityIssuedINV + QuantityIssuedTRF + QuantityIssuedADJ) * MovementDate) / SUM(QuantityIssuedINV + QuantityIssuedTRF + QuantityIssuedADJ) AS MovementAVG " + "\r\n";
            queryString = queryString + "                           FROM    (" + "\r\n";
            
            
            
            
            
            //BEGINING
            //WHINPUT
            queryString = queryString + "                                   SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, NULL AS MovementDate " + "\r\n";
            queryString = queryString + "                                   FROM        GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                                   WHERE       " + (isAllWarehouses ? "" : " GoodsReceiptDetails.WarehouseID = @LocalWarehouseID AND ") + " GoodsReceiptDetails.EntryDate < @LocalFromDate AND GoodsReceiptDetails.Quantity > GoodsReceiptDetails.QuantityIssued " + "\r\n";

            //queryString = queryString + "                                   UNION ALL " + "\r\n";
            ////UNDO (CAC CAU SQL CHO INVOICE, StockTransferDetails, WHADJUST, WHASSEMBLY LA HOAN TOAN GIONG NHAU. LUU Y T/H DAT BIET: WHADJUST.QUANTITY < 0)
            ////UNDO SalesInvoiceDetails
            //queryString = queryString + "                                   SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, SalesInvoiceDetails.Quantity AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, NULL AS MovementDate " + "\r\n";
            //queryString = queryString + "                                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            //queryString = queryString + "                                               SalesInvoiceDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID ") + " AND GoodsReceiptDetails.GoodsReceiptDetailID = SalesInvoiceDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.EntryDate < @LocalFromDate AND SalesInvoiceDetails.EntryDate >= @LocalFromDate " + "\r\n";

            //queryString = queryString + "                                   UNION ALL " + "\r\n";
            ////UNDO StockTransferDetails
            //queryString = queryString + "                                   SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, StockTransferDetails.Quantity AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, NULL AS MovementDate " + "\r\n";
            //queryString = queryString + "                                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            //queryString = queryString + "                                               StockTransferDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND GoodsReceiptDetails.WarehouseID = @LocalWarehouseID ") + " AND GoodsReceiptDetails.GoodsReceiptDetailID = StockTransferDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.EntryDate < @LocalFromDate AND StockTransferDetails.EntryDate >= @LocalFromDate " + "\r\n";

            queryString = queryString + "                                   UNION ALL " + "\r\n";
            //UNDO WarehouseAdjustmentDetails
            queryString = queryString + "                                   SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, -WarehouseAdjustmentDetails.Quantity AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, NULL AS MovementDate " + "\r\n";
            queryString = queryString + "                                   FROM        GoodsReceiptDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                                               WarehouseAdjustmentDetails ON " + (isAllWarehouses ? "" : " GoodsReceiptDetails.WarehouseID = @LocalWarehouseID AND ") + " GoodsReceiptDetails.GoodsReceiptDetailID = WarehouseAdjustmentDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.EntryDate < @LocalFromDate AND WarehouseAdjustmentDetails.EntryDate >= @LocalFromDate " + "\r\n";








            //INTPUT
            queryString = queryString + "                                   UNION ALL " + "\r\n";
            queryString = queryString + "                                   SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, 0 AS QuantityBegin, " + "\r\n";
            queryString = queryString + "                                               CASE WHEN GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " THEN GoodsReceiptDetails.Quantity ELSE 0 END AS QuantityInputINV, " + "\r\n";
            queryString = queryString + "                                               CASE WHEN GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.GoodsReturn + " THEN GoodsReceiptDetails.Quantity ELSE 0 END AS QuantityInputRTN, " + "\r\n";
            queryString = queryString + "                                               CASE WHEN GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " THEN GoodsReceiptDetails.Quantity ELSE 0 END AS QuantityInputTRF, " + "\r\n";
            queryString = queryString + "                                               CASE WHEN GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.WarehouseAdjustments + " THEN GoodsReceiptDetails.Quantity ELSE 0 END AS QuantityInputADJ, " + "\r\n";
            queryString = queryString + "                                               0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, NULL AS MovementDate " + "\r\n";
            queryString = queryString + "                                   FROM        GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                                   WHERE       " + (isAllWarehouses ? "" : " GoodsReceiptDetails.WarehouseID = @LocalWarehouseID AND ") + " GoodsReceiptDetails.EntryDate >= @LocalFromDate AND GoodsReceiptDetails.EntryDate <= @LocalToDate " + "\r\n";











            ////OUTPUT (CAC CAU SQL CHO INVOICE, StockTransferDetails, WHADJUST, WHASSEMBLY LA HOAN TOAN GIONG NHAU. LUU Y T/H DAT BIET: WHADJUST.QUANTITY < 0)
            //queryString = queryString + "                                   UNION ALL " + "\r\n";
            ////SalesInvoiceDetails + "\r\n";
            //queryString = queryString + "                                   SELECT      SalesInvoiceDetails.GoodsReceiptDetailID, 0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, SalesInvoiceDetails.Quantity AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, 0 AS MovementDate " + "\r\n"; //DATEDIFF(DAY, GoodsReceiptDetails.EntryDate, SalesInvoiceDetails.EntryDate) AS MovementDate
            //queryString = queryString + "                                   FROM        SalesInvoiceDetails " + "\r\n";
            //queryString = queryString + "                                   WHERE       SalesInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND SalesInvoiceDetails.WarehouseID = @LocalWarehouseID ") + " AND SalesInvoiceDetails.EntryDate >= @LocalFromDate AND SalesInvoiceDetails.EntryDate <= @LocalToDate " + "\r\n";

            //queryString = queryString + "                                   UNION ALL " + "\r\n";
            ////StockTransferDetails
            //queryString = queryString + "                                   SELECT      StockTransferDetails.GoodsReceiptDetailID, 0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, StockTransferDetails.Quantity AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, 0 AS MovementDate " + "\r\n"; //DATEDIFF(DAY, GoodsReceiptDetails.EntryDate, StockTransferDetails.EntryDate) AS MovementDate
            //queryString = queryString + "                                   FROM        StockTransferDetails " + "\r\n";
            //queryString = queryString + "                                   WHERE       StockTransferDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND StockTransferDetails.WarehouseID = @LocalWarehouseID ") + " AND StockTransferDetails.EntryDate >= @LocalFromDate AND StockTransferDetails.EntryDate <= @LocalToDate " + "\r\n";

            queryString = queryString + "                                   UNION ALL " + "\r\n";
            //WarehouseAdjustmentDetails
            queryString = queryString + "                                   SELECT      WarehouseAdjustmentDetails.GoodsReceiptDetailID, 0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, -WarehouseAdjustmentDetails.Quantity AS QuantityIssuedADJ, 0 AS MovementDate " + "\r\n"; //DATEDIFF(DAY, GoodsReceiptDetails.EntryDate, WarehouseAdjustmentDetails.EntryDate) AS MovementDate
            queryString = queryString + "                                   FROM        WarehouseAdjustmentDetails " + "\r\n";
            queryString = queryString + "                                   WHERE       " + (isAllWarehouses ? "" : " WarehouseAdjustmentDetails.WarehouseID = @LocalWarehouseID AND ") + " WarehouseAdjustmentDetails.EntryDate >= @LocalFromDate AND WarehouseAdjustmentDetails.EntryDate <= @LocalToDate " + "\r\n";

            queryString = queryString + "                                   ) AS GoodsReceiptDetailUnion " + "\r\n";
            queryString = queryString + "                           GROUP BY GoodsReceiptDetailUnion.GoodsReceiptDetailID " + "\r\n";
            queryString = queryString + "                           ) AS GoodsReceiptDetailUnionMaster INNER JOIN " + "\r\n";
            queryString = queryString + "                           GoodsReceiptDetails ON GoodsReceiptDetailUnionMaster.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            //--BEGIN-INPUT-OUTPUT-END.END
















            //////queryString = queryString + "                   UNION ALL " + "\r\n";
            ////////--ON SHIP.BEGIN
            //////queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, PurchaseOrderDetails.CommodityID, PurchaseOrderDetails.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            //////queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, (PurchaseOrderDetails.Quantity - PurchaseOrderDetails.QuantityInvoice) AS QuantityOnPurchasing, 0 AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            //////queryString = queryString + "                   FROM    PurchaseOrderDetails " + "\r\n";
            //////queryString = queryString + "                   WHERE   PurchaseOrderDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND PurchaseOrderDetails.LocationID = @LocationID ") + " AND PurchaseOrderDetails.EntryDate <= @LocalToDate AND PurchaseOrderDetails.Quantity > PurchaseOrderDetails.QuantityInvoice " + "\r\n";

            //////queryString = queryString + "                   UNION ALL " + "\r\n";

            //////queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, PurchaseInvoiceDetails.CommodityID, PurchaseOrders.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            //////queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, PurchaseInvoiceDetails.Quantity AS QuantityOnPurchasing, 0 AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            //////queryString = queryString + "                   FROM    PurchaseOrders INNER JOIN " + "\r\n";
            //////queryString = queryString + "                           PurchaseInvoiceDetails ON PurchaseInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND PurchaseOrders.LocationID = @LocationID ") + " AND PurchaseOrders.PurchaseOrderID = PurchaseInvoiceDetails.PurchaseOrderID " + "\r\n";
            //////queryString = queryString + "                   WHERE   PurchaseOrders.EntryDate <= @LocalToDate AND PurchaseInvoiceDetails.EntryDate > @LocalToDate  " + "\r\n";
            ////////--ON SHIP.END













            //////queryString = queryString + "                   UNION ALL " + "\r\n";
            ////////--ON INPUT.BEGIN (CAC CAU SQL DUNG CHO EWHInputVoucherTypeID.EInvoice, EWHInputVoucherTypeID.EReturn, EWHInputVoucherTypeID.EWHTransfer, EWHInputVoucherTypeID.EWHAdjust, EWHInputVoucherTypeID.EWHAssemblyMaster, EWHInputVoucherTypeID.EWHAssemblyDetail LA HOAN TOAN GIONG NHAU)
            ////////EWHInputVoucherTypeID.EInvoice
            //////queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, PurchaseInvoiceDetails.CommodityID, PurchaseInvoiceDetails.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            //////queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, 0 AS QuantityOnPurchasing, (PurchaseInvoiceDetails.Quantity - PurchaseInvoiceDetails.QuantityReceipt) AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            //////queryString = queryString + "                   FROM    PurchaseInvoiceDetails " + "\r\n";
            //////queryString = queryString + "                   WHERE   PurchaseInvoiceDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND PurchaseInvoiceDetails.LocationID = @LocationID ") + " AND PurchaseInvoiceDetails.EntryDate <= @LocalToDate AND PurchaseInvoiceDetails.Quantity > PurchaseInvoiceDetails.QuantityReceipt " + "\r\n";

            //////queryString = queryString + "                   UNION ALL " + "\r\n";

            //////queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            //////queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, 0 AS QuantityOnPurchasing, GoodsReceiptDetails.Quantity AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            //////queryString = queryString + "                   FROM    PurchaseInvoices INNER JOIN " + "\r\n";
            //////queryString = queryString + "                           GoodsReceiptDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND PurchaseInvoices.LocationID = @LocationID ") + " AND PurchaseInvoices.PurchaseInvoiceID = GoodsReceiptDetails.VoucherID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + " AND PurchaseInvoices.EntryDate <= @LocalToDate AND GoodsReceiptDetails.EntryDate > @LocalToDate " + "\r\n";

            //////queryString = queryString + "                   UNION ALL " + "\r\n";
            ////////EWHInputVoucherTypeID.EWHTransfer
            //////queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, StockTransferDetails.CommodityID, StockTransferDetails.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            //////queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, 0 AS QuantityOnPurchasing, (StockTransferDetails.Quantity - StockTransferDetails.QuantityReceipt) AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            //////queryString = queryString + "                   FROM    StockTransfers INNER JOIN " + "\r\n";
            //////queryString = queryString + "                           StockTransferDetails ON StockTransfers.StockTransferID = StockTransferDetails.StockTransferID AND StockTransferDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND StockTransfers.WarehouseID = @LocalWarehouseID ") + " AND StockTransferDetails.EntryDate <= @LocalToDate AND StockTransferDetails.Quantity > StockTransferDetails.QuantityReceipt " + "\r\n";

            //////queryString = queryString + "                   UNION ALL " + "\r\n";

            //////queryString = queryString + "                   SELECT  CONVERT(smalldatetime, '" + new DateTime(1990, 1, 1).ToString("dd/MM/yyyy") + "', 103) AS EntryDate, 0 AS GoodsReceiptDetailID, GoodsReceiptDetails.CommodityID, GoodsReceiptDetails.SupplierID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS WarehouseID, " + "\r\n";
            //////queryString = queryString + "                           0 AS QuantityBegin, 0 AS QuantityInputINV, 0 AS QuantityInputRTN, 0 AS QuantityInputTRF, 0 AS QuantityInputADJ, 0 AS QuantityIssuedINV, 0 AS QuantityIssuedTRF, 0 AS QuantityIssuedADJ, 0 AS QuantityOnPurchasing, GoodsReceiptDetails.Quantity AS QuantityOnReceipt, 0 AS UnitPrice, 0 AS MovementMIN, 0 AS MovementMAX, 0 AS MovementAVG " + "\r\n";
            //////queryString = queryString + "                   FROM    StockTransfers INNER JOIN " + "\r\n";
            //////queryString = queryString + "                           GoodsReceiptDetails ON GoodsReceiptDetails.CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + (isAllWarehouses ? "" : " AND StockTransfers.WarehouseID = @LocalWarehouseID ") + " AND StockTransfers.StockTransferID = GoodsReceiptDetails.VoucherID AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + " AND StockTransfers.EntryDate <= @LocalToDate AND GoodsReceiptDetails.EntryDate > @LocalToDate " + "\r\n";
            ////////--ON INPUT.END







            queryString = queryString + "                   ) AS VehicleJournalMaster INNER JOIN " + "\r\n";

            queryString = queryString + "                   Customers ON VehicleJournalMaster.SupplierID = Customers.CustomerID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Commodities ON VehicleJournalMaster.CommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories ON Commodities.CommodityCategoryID = VWCommodityCategories.CommodityCategoryID LEFT JOIN " + "\r\n";

            queryString = queryString + "                   Warehouses ON VehicleJournalMaster.WarehouseID = Warehouses.WarehouseID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            return queryString;

        }

    }
}

using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class MaterialIssue
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public MaterialIssue(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetMaterialIssueIndexes();

            this.GetMaterialIssueViewDetails();

            this.GetMaterialIssuePendingWorkshifts();
            this.GetMaterialIssuePendingPlannedOrders();
            this.GetMaterialIssuePendingPlannedOrderDetails();

            this.MaterialIssueSaveRelative();
            this.MaterialIssuePostSaveValidate();

            this.MaterialIssueApproved();
            this.MaterialIssueEditable();

            this.MaterialIssueToggleApproved();

            this.MaterialIssueInitReference();
        }


        private void GetMaterialIssueIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      MaterialIssues.MaterialIssueID, CAST(MaterialIssues.EntryDate AS DATE) AS EntryDate, MaterialIssues.Reference, Locations.Code AS LocationCode, Workshifts.Name AS WorkshiftName, MaterialIssues.Description, MaterialIssues.TotalQuantity, MaterialIssues.Approved " + "\r\n";
            queryString = queryString + "       FROM        MaterialIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON MaterialIssues.EntryDate >= @FromDate AND MaterialIssues.EntryDate <= @ToDate AND MaterialIssues.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.MaterialIssue + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = MaterialIssues.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON MaterialIssues.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssueIndexes", queryString);
        }


        private void GetMaterialIssueViewDetails()
        {
            string queryString;

            queryString = " @MaterialIssueID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      MaterialIssueDetails.MaterialIssueDetailID, MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.ProductionOrderID, MaterialIssueDetails.ProductionOrderDetailID, ProductionOrders.Reference AS ProductionOrderReference, ProductionOrders.Code AS ProductionOrderCode, ProductionOrders.EntryDate AS ProductionOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   MaterialIssueDetails.PlannedOrderID, MaterialIssueDetails.PlannedOrderDetailID, PlannedOrders.Reference AS PlannedOrderReference, PlannedOrders.Code AS PlannedOrderCode, PlannedOrders.EntryDate AS PlannedOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   ProductionLines.Code AS ProductionLineCode, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, MaterialIssueDetails.CommodityTypeID, Products.CommodityID AS ProductID, Products.Code AS ProductCode, Products.Name AS ProductName, " + "\r\n";
            queryString = queryString + "                   ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued + MaterialIssueDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, MaterialIssueDetails.Quantity, MaterialIssueDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        MaterialIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON MaterialIssueDetails.MaterialIssueID = @MaterialIssueID AND MaterialIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionOrderDetails ON MaterialIssueDetails.ProductionOrderDetailID = ProductionOrderDetails.ProductionOrderDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionOrders ON ProductionOrderDetails.ProductionOrderID = ProductionOrders.ProductionOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PlannedOrderDetails ON MaterialIssueDetails.PlannedOrderDetailID = PlannedOrderDetails.PlannedOrderDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PlannedOrders ON MaterialIssueDetails.PlannedOrderID = PlannedOrders.PlannedOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities Products ON ProductionOrderDetails.CommodityID = Products.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON ProductionOrderDetails.ProductionLineID = ProductionLines.ProductionLineID  " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON MaterialIssueDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "       ORDER BY    MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssueViewDetails", queryString);
        }





        private void GetMaterialIssuePendingPlannedOrders()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.MaterialIssueTypeID.PlannedOrder + " AS MaterialIssueTypeID, PlannedOrders.PlannedOrderID, PlannedOrders.Reference AS PlannedOrderReference, PlannedOrders.Code AS PlannedOrderCode, PlannedOrders.EntryDate AS PlannedOrderEntryDate, PlannedOrderWorkshiftPENDING.WorkshiftID, Workshifts.Code AS WorkshiftCode, PlannedOrders.Description, PlannedOrders.Remarks " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT PlannedOrderID, WorkshiftID FROM ProductionOrderDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0) PlannedOrderWorkshiftPENDING " + "\r\n";// AND ROUND(Quantity - QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") > 0

            queryString = queryString + "                       INNER JOIN PlannedOrders ON PlannedOrderWorkshiftPENDING.PlannedOrderID = PlannedOrders.PlannedOrderID " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON PlannedOrderWorkshiftPENDING.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssuePendingPlannedOrders", queryString);
        }

        private void GetMaterialIssuePendingWorkshifts()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.MaterialIssueTypeID.PlannedOrder + " AS MaterialIssueTypeID, Workshifts.WorkshiftID, Workshifts.Code AS WorkshiftCode " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT WorkshiftID FROM ProductionOrders WHERE ProductionOrderID IN (SELECT ProductionOrderID FROM ProductionOrderDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0)) WorkshiftPENDING " + "\r\n";// AND ROUND(Quantity - QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") > 0
            queryString = queryString + "                       INNER JOIN Workshifts ON WorkshiftPENDING.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssuePendingWorkshifts", queryString);
        }



        private void GetMaterialIssuePendingPlannedOrderDetails()
        {
            string queryString;

            queryString = " @LocationID Int, @MaterialIssueID Int, @PlannedOrderID Int, @WorkshiftID Int, @WarehouseID Int, @ProductionOrderDetailIDs varchar(3999), @GoodsReceiptDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@PlannedOrderID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(false) + "\r\n";

            queryString = queryString + "           " + this.BuildSQLPendingDetails(false, false, false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssuePendingPlannedOrderDetails", queryString);
        }

        private string BuildSQLPendingDetails(bool isPlannedOrderID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@ProductionOrderDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(isPlannedOrderID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(isPlannedOrderID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLPendingDetails(bool isPlannedOrderID, bool isProductionOrderDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@GoodsReceiptDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(isPlannedOrderID, true, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPendingDetails(isPlannedOrderID, false, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLPendingDetails(bool isPlannedOrderID, bool isProductionOrderDetailIDs, bool isGoodsReceiptDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@MaterialIssueID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isPlannedOrderID, isProductionOrderDetailIDs, isGoodsReceiptDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY ProductionOrderDetails.EntryDate, ProductionOrderDetails.ProductionOrderID, ProductionOrderDetails.ProductionOrderDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPlannedOrderID, isProductionOrderDetailIDs, isGoodsReceiptDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY ProductionOrderDetails.EntryDate, ProductionOrderDetails.ProductionOrderID, ProductionOrderDetails.ProductionOrderDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLNew(isPlannedOrderID, isProductionOrderDetailIDs, isGoodsReceiptDetailIDs) + " WHERE ProductionOrderDetails.ProductionOrderDetailID NOT IN (SELECT ProductionOrderDetailID FROM MaterialIssueDetails WHERE MaterialIssueID = @MaterialIssueID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPlannedOrderID, isProductionOrderDetailIDs, isGoodsReceiptDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY ProductionOrderDetails.EntryDate, ProductionOrderDetails.ProductionOrderID, ProductionOrderDetails.ProductionOrderDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isPlannedOrderID, bool isProductionOrderDetailIDs, bool isGoodsReceiptDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      PlannedOrders.PlannedOrderID, PlannedOrderDetails.PlannedOrderDetailID, PlannedOrders.EntryDate AS PlannedOrderEntryDate, PlannedOrders.Reference AS PlannedOrderReference, PlannedOrders.Code AS PlannedOrderCode, PlannedOrderDetails.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   ProductionOrderDetails.ProductionOrderID, ProductionOrderDetails.ProductionOrderDetailID, ProductionOrderDetails.EntryDate AS ProductionOrderEntryDate, ProductionOrderDetails.ProductionLineID, ProductionLines.Code AS ProductionLineCode, ProductionOrderDetails.MoldID, Molds.Code AS MoldCode, ProductionOrderDetails.WorkshiftID, Workshifts.Code AS WorkshiftCode, " + "\r\n";
            queryString = queryString + "                   ProductionOrderDetails.CommodityID AS ProductID, Products.Code AS ProductCode, Products.Name AS ProductName, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceipts.EntryDate AS GoodsReceiptEntryDate, GoodsReceipts.Reference AS GoodsReceiptReference, GoodsReceipts.Code AS GoodsReceiptCode, " + "\r\n";
            queryString = queryString + "                   ROUND(PlannedOrderDetails.Quantity- PlannedOrderDetails.QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") AS PlannedOrderRemains, Workshifts.WorkingHours, Molds.CyclePerHours, Molds.Quantity AS MoldQuantity, CommodityMaterialDetails.BlockUnit, CommodityMaterialDetails.BlockQuantity, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, 0 AS Quantity, CAST(0 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        ProductionOrderDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN PlannedOrderDetails ON " + (isPlannedOrderID ? " ProductionOrderDetails.PlannedOrderID = @PlannedOrderID " : "ProductionOrderDetails.LocationID = @LocationID AND ProductionOrderDetails.WorkshiftID = @WorkshiftID") + " AND ProductionOrderDetails.Approved = 1 AND ProductionOrderDetails.InActive = 0 AND ProductionOrderDetails.InActivePartial = 0 AND ProductionOrderDetails.PlannedOrderDetailID = PlannedOrderDetails.PlannedOrderDetailID AND ROUND(PlannedOrderDetails.Quantity- PlannedOrderDetails.QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (isProductionOrderDetailIDs ? " AND ProductionOrderDetails.ProductionOrderDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@ProductionOrderDetailIDs))" : "") + "\r\n";

            queryString = queryString + "                   INNER JOIN PlannedOrders ON PlannedOrderDetails.PlannedOrderID = PlannedOrders.PlannedOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON ProductionOrderDetails.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Molds ON ProductionOrderDetails.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON ProductionOrderDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities Products ON ProductionOrderDetails.CommodityID = Products.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityMaterialDetails ON ProductionOrderDetails.CommodityMaterialID = CommodityMaterialDetails.CommodityMaterialID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON CommodityMaterialDetails.MaterialID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON ProductionOrderDetails.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";

            queryString = queryString + "                   LEFT JOIN GoodsReceiptDetails ON GoodsReceiptDetails.WarehouseID = @WarehouseID AND CommodityMaterialDetails.MaterialID = GoodsReceiptDetails.CommodityID AND GoodsReceiptDetails.Approved = 1 AND ROUND(GoodsReceiptDetails.Quantity- GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (isGoodsReceiptDetailIDs ? " AND GoodsReceiptDetails.GoodsReceiptDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsReceiptDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsReceipts ON GoodsReceiptDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isPlannedOrderID, bool isProductionOrderDetailIDs, bool isGoodsReceiptDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      PlannedOrders.PlannedOrderID, PlannedOrderDetails.PlannedOrderDetailID, PlannedOrders.EntryDate AS PlannedOrderEntryDate, PlannedOrders.Reference AS PlannedOrderReference, PlannedOrders.Code AS PlannedOrderCode, PlannedOrderDetails.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, " + "\r\n";
            queryString = queryString + "                   ProductionOrderDetails.ProductionOrderID, ProductionOrderDetails.ProductionOrderDetailID, ProductionOrderDetails.EntryDate AS ProductionOrderEntryDate, ProductionOrderDetails.ProductionLineID, ProductionLines.Code AS ProductionLineCode, ProductionOrderDetails.MoldID, Molds.Code AS MoldCode, ProductionOrderDetails.WorkshiftID, Workshifts.Code AS WorkshiftCode, " + "\r\n";
            queryString = queryString + "                   ProductionOrderDetails.CommodityID AS ProductID, Products.Code AS ProductCode, Products.Name AS ProductName, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceipts.EntryDate AS GoodsReceiptEntryDate, GoodsReceipts.Reference AS GoodsReceiptReference, GoodsReceipts.Code AS GoodsReceiptCode, " + "\r\n";
            queryString = queryString + "                   ROUND(PlannedOrderDetails.Quantity- PlannedOrderDetails.QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") AS PlannedOrderRemains, Workshifts.WorkingHours, Molds.CyclePerHours, Molds.Quantity AS MoldQuantity, CommodityMaterialDetails.BlockUnit, CommodityMaterialDetails.BlockQuantity, ROUND(GoodsReceiptDetails.Quantity - GoodsReceiptDetails.QuantityIssued + MaterialIssueDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, 0 AS Quantity, CAST(0 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        MaterialIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionOrderDetails ON MaterialIssueDetails.MaterialIssueID = @MaterialIssueID AND MaterialIssueDetails.ProductionOrderDetailID = ProductionOrderDetails.ProductionOrderDetailID " + (isProductionOrderDetailIDs ? " AND ProductionOrderDetails.ProductionOrderDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@ProductionOrderDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN PlannedOrderDetails ON ProductionOrderDetails.PlannedOrderDetailID = PlannedOrderDetails.PlannedOrderDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PlannedOrders ON PlannedOrderDetails.PlannedOrderID = PlannedOrders.PlannedOrderID " + "\r\n";
            
            queryString = queryString + "                   INNER JOIN ProductionLines ON ProductionOrderDetails.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Molds ON ProductionOrderDetails.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON ProductionOrderDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities Products ON ProductionOrderDetails.CommodityID = Products.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityMaterialDetails ON ProductionOrderDetails.CommodityMaterialID = CommodityMaterialDetails.CommodityMaterialID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON CommodityMaterialDetails.MaterialID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON ProductionOrderDetails.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";

            queryString = queryString + "                   LEFT  JOIN GoodsReceiptDetails ON GoodsReceiptDetails.WarehouseID = @WarehouseID AND CommodityMaterialDetails.MaterialID = GoodsReceiptDetails.CommodityID AND GoodsReceiptDetails.Approved = 1 AND ROUND(GoodsReceiptDetails.Quantity- GoodsReceiptDetails.QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") > 0 " + (isGoodsReceiptDetailIDs ? " AND GoodsReceiptDetails.GoodsReceiptDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@GoodsReceiptDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   LEFT JOIN GoodsReceipts ON GoodsReceiptDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID " + "\r\n";

            return queryString;
        }


        private void MaterialIssueSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       BEGIN  " + "\r\n";
            queryString = queryString + "           DECLARE         @MaterialIssueDetails TABLE (GoodsReceiptDetailID int NOT NULL PRIMARY KEY, MaterialIssueTypeID int NOT NULL, Quantity decimal(18, 2) NOT NULL)" + "\r\n";
            queryString = queryString + "           INSERT INTO     @MaterialIssueDetails (GoodsReceiptDetailID, MaterialIssueTypeID, Quantity) SELECT GoodsReceiptDetailID, MIN(MaterialIssueTypeID) AS MaterialIssueTypeID, SUM(Quantity) AS Quantity FROM MaterialIssueDetails WHERE MaterialIssueID = @EntityID GROUP BY GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "           DECLARE         @MaterialIssueTypeID int, @AffectedROWCOUNT int ";
            queryString = queryString + "           SELECT          @MaterialIssueTypeID = MaterialIssueTypeID FROM @MaterialIssueDetails ";

            #region UPDATE GoodsReceiptDetails
            queryString = queryString + "           UPDATE          GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "           SET             GoodsReceiptDetails.QuantityIssued = ROUND(GoodsReceiptDetails.QuantityIssued + MaterialIssueDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "           FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                           INNER JOIN @MaterialIssueDetails MaterialIssueDetails ON GoodsReceiptDetails.GoodsReceiptDetailID = MaterialIssueDetails.GoodsReceiptDetailID AND GoodsReceiptDetails.Approved = 1 " + "\r\n";

            queryString = queryString + "           IF @@ROWCOUNT <> (SELECT COUNT(*) FROM @MaterialIssueDetails) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   DECLARE     @msg NVARCHAR(300) = N'Phiếu nhập kho đã hủy, chưa duyệt hoặc đã xóa.' ; " + "\r\n";
            queryString = queryString + "                   THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            #endregion


            #region ISSUE ADVICE
            //queryString = queryString + "           IF (@MaterialIssueTypeID > 0) " + "\r\n";
            //queryString = queryString + "               BEGIN  " + "\r\n";
            
            
            //queryString = queryString + "                   IF (@MaterialIssueTypeID = " + (int)GlobalEnums.MaterialIssueTypeID.PlannedOrder + ") " + "\r\n";
            //queryString = queryString + "                       BEGIN  " + "\r\n";
            //queryString = queryString + "                           UPDATE          ProductionOrderDetails " + "\r\n";
            //queryString = queryString + "                           SET             ProductionOrderDetails.QuantityIssued = ROUND(ProductionOrderDetails.QuantityIssued + MaterialIssueDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            //queryString = queryString + "                           FROM            MaterialIssueDetails " + "\r\n";
            //queryString = queryString + "                                           INNER JOIN ProductionOrderDetails ON ((ProductionOrderDetails.Approved = 1 AND ProductionOrderDetails.InActive = 0 AND ProductionOrderDetails.InActivePartial = 0) OR @SaveRelativeOption = -1) AND MaterialIssueDetails.MaterialIssueID = @EntityID AND MaterialIssueDetails.ProductionOrderDetailID = ProductionOrderDetails.ProductionOrderDetailID " + "\r\n";
            //queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            //queryString = queryString + "                       END " + "\r\n";


            //queryString = queryString + "                   IF (@MaterialIssueTypeID = " + (int)GlobalEnums.MaterialIssueTypeID.GoodsIssueTransfer + ") " + "\r\n";
            //queryString = queryString + "                       BEGIN  " + "\r\n";
            //queryString = queryString + "                           UPDATE          GoodsIssueTransferDetails " + "\r\n";
            //queryString = queryString + "                           SET             GoodsIssueTransferDetails.QuantityIssued = ROUND(GoodsIssueTransferDetails.QuantityIssued + MaterialIssueDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), GoodsIssueTransferDetails.LineVolumeReceipt = ROUND(GoodsIssueTransferDetails.LineVolumeReceipt + MaterialIssueDetails.LineVolume * @SaveRelativeOption, " + (int)GlobalEnums.rndVolume + ") " + "\r\n";
            //queryString = queryString + "                           FROM            MaterialIssueDetails " + "\r\n";
            //queryString = queryString + "                                           INNER JOIN GoodsIssueTransferDetails ON MaterialIssueDetails.MaterialIssueID = @EntityID AND GoodsIssueTransferDetails.Approved = 1 AND MaterialIssueDetails.GoodsIssueTransferDetailID = GoodsIssueTransferDetails.GoodsIssueTransferDetailID " + "\r\n";
            //queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            //queryString = queryString + "                       END " + "\r\n";


            //queryString = queryString + "                   IF (@MaterialIssueTypeID = " + (int)GlobalEnums.MaterialIssueTypeID.WarehouseAdjustments + ") " + "\r\n";
            //queryString = queryString + "                       BEGIN  " + "\r\n";
            //queryString = queryString + "                           UPDATE          WarehouseAdjustmentDetails " + "\r\n";
            //queryString = queryString + "                           SET             WarehouseAdjustmentDetails.QuantityIssued = ROUND(WarehouseAdjustmentDetails.QuantityIssued + MaterialIssueDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), WarehouseAdjustmentDetails.LineVolumeReceipt = ROUND(WarehouseAdjustmentDetails.LineVolumeReceipt + MaterialIssueDetails.LineVolume * @SaveRelativeOption, " + (int)GlobalEnums.rndVolume + ") " + "\r\n";
            //queryString = queryString + "                           FROM            MaterialIssueDetails " + "\r\n";
            //queryString = queryString + "                                           INNER JOIN WarehouseAdjustmentDetails ON MaterialIssueDetails.MaterialIssueID = @EntityID AND WarehouseAdjustmentDetails.Quantity > 0 AND MaterialIssueDetails.WarehouseAdjustmentDetailID = WarehouseAdjustmentDetails.WarehouseAdjustmentDetailID " + "\r\n";
            //queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            ////------------------------------------------------------SHOULD UPDATE MaterialIssueID, MaterialIssueDetailID BACK TO WarehouseAdjustmentDetails FOR MaterialIssues OF WarehouseAdjustmentDetails? THE ANSWER: WE CAN DO IT HERE, BUT IT BREAK THE RELATIONSHIP (cyclic redundancy relationship: MaterialIssueDetails => WarehouseAdjustmentDetails => THUS: WE SHOULD NOT MAKE ANOTHER RELATIONSHIP FROM WarehouseAdjustmentDetails => MaterialIssueDetails ) => SO: SHOULD NOT!!!
            //queryString = queryString + "                       END " + "\r\n";

            //queryString = queryString + "                   IF @AffectedROWCOUNT <> (SELECT COUNT(*) FROM MaterialIssueDetails WHERE MaterialIssueID = @EntityID) " + "\r\n";
            //queryString = queryString + "                       BEGIN " + "\r\n";
            //queryString = queryString + "                           DECLARE     @msg NVARCHAR(300) = N'Phiếu giao hàng đã hủy, hoặc chưa duyệt' ; " + "\r\n";
            //queryString = queryString + "                           THROW       61001,  @msg, 1; " + "\r\n";
            //queryString = queryString + "                       END " + "\r\n";
            

            //queryString = queryString + "               END  " + "\r\n";
            #endregion


            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("MaterialIssueSaveRelative", queryString);
        }

        private void MaterialIssuePostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đặt hàng: ' + CAST(GoodsReceipts.EntryDate AS nvarchar) FROM MaterialIssueDetails INNER JOIN GoodsReceipts ON MaterialIssueDetails.MaterialIssueID = @EntityID AND MaterialIssueDetails.GoodsReceiptID = GoodsReceipts.GoodsReceiptID AND MaterialIssueDetails.EntryDate < GoodsReceipts.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng tồn kho: ' + CAST(ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM GoodsReceiptDetails WHERE (ROUND(Quantity - QuantityIssued, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("MaterialIssuePostSaveValidate", queryArray);
        }




        private void MaterialIssueApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = MaterialIssueID FROM MaterialIssues WHERE MaterialIssueID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("MaterialIssueApproved", queryArray);
        }


        private void MaterialIssueEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = MaterialIssueID FROM MaterialIssueDetails WHERE MaterialIssueID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("MaterialIssueEditable", queryArray);
        }

        private void MaterialIssueToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      MaterialIssues  SET Approved = @Approved, ApprovedDate = GetDate() WHERE MaterialIssueID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          MaterialIssueDetails  SET Approved = @Approved WHERE MaterialIssueID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("MaterialIssueToggleApproved", queryString);
        }


        private void MaterialIssueInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("MaterialIssues", "MaterialIssueID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.MaterialIssue));
            this.totalSmartPortalEntities.CreateTrigger("MaterialIssueInitReference", simpleInitReference.CreateQuery());
        }
    }
}


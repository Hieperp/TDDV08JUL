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
            this.GetMaterialIssuePendingProductionOrders();
            this.GetMaterialIssuePendingProductionOrderDetails();

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
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, MaterialIssueDetails.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(ISNULL(CASE WHEN ProductionOrderDetails.Approved = 1 AND ProductionOrderDetails.InActive = 0 AND ProductionOrderDetails.InActivePartial = 0 THEN ProductionOrderDetails.Quantity - ProductionOrderDetails.QuantityReceipt ELSE 0 END, 0) + MaterialIssueDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, MaterialIssueDetails.Quantity, MaterialIssueDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        MaterialIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON MaterialIssueDetails.MaterialIssueID = @MaterialIssueID AND MaterialIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN ProductionOrderDetails ON MaterialIssueDetails.ProductionOrderDetailID = ProductionOrderDetails.ProductionOrderDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN ProductionOrders ON ProductionOrderDetails.ProductionOrderID = ProductionOrders.ProductionOrderID " + "\r\n";

            queryString = queryString + "       ORDER BY    Commodities.CommodityTypeID, MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssueViewDetails", queryString);
        }





        private void GetMaterialIssuePendingProductionOrders()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.MaterialIssueTypeID.ProductionOrder + " AS MaterialIssueTypeID, ProductionOrders.ProductionOrderID, ProductionOrders.Reference AS ProductionOrderReference, ProductionOrders.Code AS ProductionOrderCode, ProductionOrders.EntryDate AS ProductionOrderEntryDate, ProductionOrders.Description, ProductionOrders.Remarks, " + "\r\n";
            queryString = queryString + "                       ProductionOrders.WorkshiftID, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, Workshifts.OfficialName AS WorkshiftOfficialName " + "\r\n";

            queryString = queryString + "       FROM            ProductionOrders " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON ProductionOrders.ProductionOrderID IN (SELECT ProductionOrderID FROM ProductionOrderDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") > 0) AND ProductionOrders.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories WorkshiftEntireTerritories ON Workshifts.TerritoryID = WorkshiftEntireTerritories.TerritoryID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssuePendingProductionOrders", queryString);
        }

        private void GetMaterialIssuePendingWorkshifts()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.MaterialIssueTypeID.ProductionOrder + " AS MaterialIssueTypeID, Workshifts.WorkshiftID, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, Workshifts.OfficialName AS WorkshiftOfficialName, Workshifts.VATCode AS WorkshiftVATCode, Workshifts.AttentionName AS WorkshiftAttentionName, Workshifts.TerritoryID AS WorkshiftTerritoryID, WorkshiftEntireTerritories.EntireName AS WorkshiftEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT WorkshiftID FROM ProductionOrders WHERE ProductionOrderID IN (SELECT ProductionOrderID FROM ProductionOrderDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0  AND ROUND(Quantity - QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") > 0)) WorkshiftPENDING " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON WorkshiftPENDING.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories WorkshiftEntireTerritories ON Workshifts.TerritoryID = WorkshiftEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN WorkshiftCategories ON Workshifts.WorkshiftCategoryID = WorkshiftCategories.WorkshiftCategoryID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssuePendingWorkshifts", queryString);
        }



        private void GetMaterialIssuePendingProductionOrderDetails()
        {
            string queryString;

            SqlProgrammability.Inventories.Inventories inventories = new SqlProgrammability.Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = " @LocationID Int, @MaterialIssueID Int, @ProductionOrderID Int, @WorkshiftID Int, @ProductionOrderDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@ProductionOrderID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLProductionOrder(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLProductionOrder(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMaterialIssuePendingProductionOrderDetails", queryString);
        }

        private string BuildSQLProductionOrder(bool isProductionOrderID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@ProductionOrderDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLProductionOrderProductionOrderDetailIDs(isProductionOrderID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLProductionOrderProductionOrderDetailIDs(isProductionOrderID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLProductionOrderProductionOrderDetailIDs(bool isProductionOrderID, bool isProductionOrderDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@MaterialIssueID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isProductionOrderID, isProductionOrderDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY ProductionOrders.EntryDate, ProductionOrders.ProductionOrderID, ProductionOrderDetails.ProductionOrderDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isProductionOrderID, isProductionOrderDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY ProductionOrders.EntryDate, ProductionOrders.ProductionOrderID, ProductionOrderDetails.ProductionOrderDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLNew(isProductionOrderID, isProductionOrderDetailIDs) + " WHERE ProductionOrderDetails.ProductionOrderDetailID NOT IN (SELECT ProductionOrderDetailID FROM MaterialIssueDetails WHERE MaterialIssueID = @MaterialIssueID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isProductionOrderID, isProductionOrderDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY ProductionOrders.EntryDate, ProductionOrders.ProductionOrderID, ProductionOrderDetails.ProductionOrderDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isProductionOrderID, bool isProductionOrderDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      ProductionOrders.ProductionOrderID, ProductionOrderDetails.ProductionOrderDetailID, ProductionOrders.Reference AS ProductionOrderReference, ProductionOrders.Code AS ProductionOrderCode, ProductionOrders.EntryDate AS ProductionOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(ProductionOrderDetails.Quantity - ProductionOrderDetails.QuantityReceipt, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0 AS Quantity, ProductionOrders.Description, ProductionOrderDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        ProductionOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionOrderDetails ON " + (isProductionOrderID ? " ProductionOrders.ProductionOrderID = @ProductionOrderID " : "ProductionOrders.LocationID = @LocationID AND ProductionOrders.WorkshiftID = @WorkshiftID ") + " AND ProductionOrderDetails.Approved = 1 AND ProductionOrderDetails.InActive = 0 AND ProductionOrderDetails.InActivePartial = 0 AND ROUND(ProductionOrderDetails.Quantity- ProductionOrderDetails.QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") > 0 AND ProductionOrders.ProductionOrderID = ProductionOrderDetails.ProductionOrderID" + (isProductionOrderDetailIDs ? " AND ProductionOrderDetails.ProductionOrderDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@ProductionOrderDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON ProductionOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isProductionOrderID, bool isProductionOrderDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      ProductionOrders.ProductionOrderID, ProductionOrderDetails.ProductionOrderDetailID, ProductionOrders.Reference AS ProductionOrderReference, ProductionOrders.Code AS ProductionOrderCode, ProductionOrders.EntryDate AS ProductionOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(ProductionOrderDetails.Quantity - ProductionOrderDetails.QuantityReceipt + MaterialIssueDetails.Quantity, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0 AS Quantity, ProductionOrders.Description, ProductionOrderDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        ProductionOrderDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN MaterialIssueDetails ON MaterialIssueDetails.MaterialIssueID = @MaterialIssueID AND ProductionOrderDetails.ProductionOrderDetailID = MaterialIssueDetails.ProductionOrderDetailID" + (isProductionOrderDetailIDs ? " AND ProductionOrderDetails.ProductionOrderDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@ProductionOrderDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON ProductionOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionOrders ON ProductionOrderDetails.ProductionOrderID = ProductionOrders.ProductionOrderID " + "\r\n";

            return queryString;
        }


        private void MaterialIssueSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";

            //queryString = queryString + "           IF (@SaveRelativeOption = 1) ";
            //queryString = queryString + "               BEGIN ";
            //queryString = queryString + "                   UPDATE          MaterialIssueDetails " + "\r\n";
            //queryString = queryString + "                   SET             MaterialIssueDetails.Reference = MaterialIssues.Reference " + "\r\n";
            //queryString = queryString + "                   FROM            MaterialIssues INNER JOIN MaterialIssueDetails ON MaterialIssues.MaterialIssueID = @EntityID AND MaterialIssues.MaterialIssueID = MaterialIssueDetails.MaterialIssueID " + "\r\n";
            //queryString = queryString + "               END ";

            queryString = queryString + "           DECLARE @MaterialIssueTypeID int, @AffectedROWCOUNT int ";
            queryString = queryString + "           SELECT  @MaterialIssueTypeID = MaterialIssueTypeID FROM MaterialIssues WHERE MaterialIssueID = @EntityID ";

            queryString = queryString + "           IF (@MaterialIssueTypeID > 0) " + "\r\n";
            queryString = queryString + "               BEGIN  " + "\r\n";

            queryString = queryString + "                   IF (@MaterialIssueTypeID = " + (int)GlobalEnums.MaterialIssueTypeID.ProductionOrder + ") " + "\r\n";
            queryString = queryString + "                       BEGIN  " + "\r\n";
            queryString = queryString + "                           UPDATE          ProductionOrderDetails " + "\r\n";
            queryString = queryString + "                           SET             ProductionOrderDetails.QuantityReceipt = ROUND(ProductionOrderDetails.QuantityReceipt + MaterialIssueDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "                           FROM            MaterialIssueDetails " + "\r\n";
            queryString = queryString + "                                           INNER JOIN ProductionOrderDetails ON ((ProductionOrderDetails.Approved = 1 AND ProductionOrderDetails.InActive = 0 AND ProductionOrderDetails.InActivePartial = 0) OR @SaveRelativeOption = -1) AND MaterialIssueDetails.MaterialIssueID = @EntityID AND MaterialIssueDetails.ProductionOrderDetailID = ProductionOrderDetails.ProductionOrderDetailID " + "\r\n";
            queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            queryString = queryString + "                       END " + "\r\n";


            //queryString = queryString + "                   IF (@MaterialIssueTypeID = " + (int)GlobalEnums.MaterialIssueTypeID.GoodsIssueTransfer + ") " + "\r\n";
            //queryString = queryString + "                       BEGIN  " + "\r\n";
            //queryString = queryString + "                           UPDATE          GoodsIssueTransferDetails " + "\r\n";
            //queryString = queryString + "                           SET             GoodsIssueTransferDetails.QuantityReceipt = ROUND(GoodsIssueTransferDetails.QuantityReceipt + MaterialIssueDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), GoodsIssueTransferDetails.LineVolumeReceipt = ROUND(GoodsIssueTransferDetails.LineVolumeReceipt + MaterialIssueDetails.LineVolume * @SaveRelativeOption, " + (int)GlobalEnums.rndVolume + ") " + "\r\n";
            //queryString = queryString + "                           FROM            MaterialIssueDetails " + "\r\n";
            //queryString = queryString + "                                           INNER JOIN GoodsIssueTransferDetails ON MaterialIssueDetails.MaterialIssueID = @EntityID AND GoodsIssueTransferDetails.Approved = 1 AND MaterialIssueDetails.GoodsIssueTransferDetailID = GoodsIssueTransferDetails.GoodsIssueTransferDetailID " + "\r\n";
            //queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            //queryString = queryString + "                       END " + "\r\n";


            //queryString = queryString + "                   IF (@MaterialIssueTypeID = " + (int)GlobalEnums.MaterialIssueTypeID.WarehouseAdjustments + ") " + "\r\n";
            //queryString = queryString + "                       BEGIN  " + "\r\n";
            //queryString = queryString + "                           UPDATE          WarehouseAdjustmentDetails " + "\r\n";
            //queryString = queryString + "                           SET             WarehouseAdjustmentDetails.QuantityReceipt = ROUND(WarehouseAdjustmentDetails.QuantityReceipt + MaterialIssueDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), WarehouseAdjustmentDetails.LineVolumeReceipt = ROUND(WarehouseAdjustmentDetails.LineVolumeReceipt + MaterialIssueDetails.LineVolume * @SaveRelativeOption, " + (int)GlobalEnums.rndVolume + ") " + "\r\n";
            //queryString = queryString + "                           FROM            MaterialIssueDetails " + "\r\n";
            //queryString = queryString + "                                           INNER JOIN WarehouseAdjustmentDetails ON MaterialIssueDetails.MaterialIssueID = @EntityID AND WarehouseAdjustmentDetails.Quantity > 0 AND MaterialIssueDetails.WarehouseAdjustmentDetailID = WarehouseAdjustmentDetails.WarehouseAdjustmentDetailID " + "\r\n";
            //queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            ////------------------------------------------------------SHOULD UPDATE MaterialIssueID, MaterialIssueDetailID BACK TO WarehouseAdjustmentDetails FOR MaterialIssues OF WarehouseAdjustmentDetails? THE ANSWER: WE CAN DO IT HERE, BUT IT BREAK THE RELATIONSHIP (cyclic redundancy relationship: MaterialIssueDetails => WarehouseAdjustmentDetails => THUS: WE SHOULD NOT MAKE ANOTHER RELATIONSHIP FROM WarehouseAdjustmentDetails => MaterialIssueDetails ) => SO: SHOULD NOT!!!
            //queryString = queryString + "                       END " + "\r\n";

            queryString = queryString + "                   IF @AffectedROWCOUNT <> (SELECT COUNT(*) FROM MaterialIssueDetails WHERE MaterialIssueID = @EntityID) " + "\r\n";
            queryString = queryString + "                       BEGIN " + "\r\n";
            queryString = queryString + "                           DECLARE     @msg NVARCHAR(300) = N'Phiếu giao hàng đã hủy, hoặc chưa duyệt' ; " + "\r\n";
            queryString = queryString + "                           THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                       END " + "\r\n";

            queryString = queryString + "               END  " + "\r\n";

            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("MaterialIssueSaveRelative", queryString);
        }

        private void MaterialIssuePostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đặt hàng: ' + CAST(ProductionOrders.EntryDate AS nvarchar) FROM MaterialIssueDetails INNER JOIN ProductionOrders ON MaterialIssueDetails.MaterialIssueID = @EntityID AND MaterialIssueDetails.ProductionOrderID = ProductionOrders.ProductionOrderID AND MaterialIssueDetails.EntryDate < ProductionOrders.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM ProductionOrderDetails WHERE (ROUND(Quantity - QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

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
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = MaterialIssueID FROM MaterialIssueDetails WHERE MaterialIssueID = @EntityID ";

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


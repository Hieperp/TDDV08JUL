using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Inventories
{
    public class GoodsReceipt
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public GoodsReceipt(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetGoodsReceiptIndexes();

            this.GetGoodsReceiptViewDetails();

            this.GetGoodsReceiptPendingCustomers();
            this.GetGoodsReceiptPendingPurchaseRequisitions();
            this.GetGoodsReceiptPendingPurchaseRequisitionDetails();

            this.GoodsReceiptSaveRelative();
            this.GoodsReceiptPostSaveValidate();

            this.GoodsReceiptApproved();
            this.GoodsReceiptEditable();

            this.GoodsReceiptToggleApproved();

            this.GoodsReceiptInitReference();
        }


        private void GetGoodsReceiptIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsReceipts.GoodsReceiptID, CAST(GoodsReceipts.EntryDate AS DATE) AS EntryDate, GoodsReceipts.Reference, Locations.Code AS LocationCode, Customers.Name AS CustomerName, GoodsReceipts.Description, GoodsReceipts.TotalQuantity, GoodsReceipts.Approved " + "\r\n";
            queryString = queryString + "       FROM        GoodsReceipts " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON GoodsReceipts.EntryDate >= @FromDate AND GoodsReceipts.EntryDate <= @ToDate AND GoodsReceipts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsReceipt + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = GoodsReceipts.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON GoodsReceipts.CustomerID = Customers.CustomerID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptIndexes", queryString);
        }
        

        private void GetGoodsReceiptViewDetails()
        {
            string queryString;

            queryString = " @GoodsReceiptID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.PurchaseRequisitionID, GoodsReceiptDetails.PurchaseRequisitionDetailID, PurchaseRequisitions.Reference AS PurchaseRequisitionReference, PurchaseRequisitions.Code AS PurchaseRequisitionCode, PurchaseRequisitions.EntryDate AS PurchaseRequisitionEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, GoodsReceiptDetails.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(ISNULL(CASE WHEN PurchaseRequisitionDetails.Approved = 1 AND PurchaseRequisitionDetails.InActive = 0 AND PurchaseRequisitionDetails.InActivePartial = 0 THEN PurchaseRequisitionDetails.Quantity - PurchaseRequisitionDetails.QuantityReceipt ELSE 0 END, 0) + GoodsReceiptDetails.Quantity, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, GoodsReceiptDetails.Quantity, GoodsReceiptDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsReceiptDetails.GoodsReceiptID = @GoodsReceiptID AND GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN PurchaseRequisitionDetails ON GoodsReceiptDetails.PurchaseRequisitionDetailID = PurchaseRequisitionDetails.PurchaseRequisitionDetailID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN PurchaseRequisitions ON PurchaseRequisitionDetails.PurchaseRequisitionID = PurchaseRequisitions.PurchaseRequisitionID " + "\r\n";

            queryString = queryString + "       ORDER BY    Commodities.CommodityTypeID, GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptViewDetails", queryString);
        }





        private void GetGoodsReceiptPendingPurchaseRequisitions()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition + " AS GoodsReceiptTypeID, PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitions.Reference AS PurchaseRequisitionReference, PurchaseRequisitions.Code AS PurchaseRequisitionCode, PurchaseRequisitions.EntryDate AS PurchaseRequisitionEntryDate, PurchaseRequisitions.Description, PurchaseRequisitions.Remarks, " + "\r\n";
            queryString = queryString + "                       PurchaseRequisitions.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName " + "\r\n";

            queryString = queryString + "       FROM            PurchaseRequisitions " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON PurchaseRequisitions.PurchaseRequisitionID IN (SELECT PurchaseRequisitionID FROM PurchaseRequisitionDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") > 0) AND PurchaseRequisitions.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingPurchaseRequisitions", queryString);
        }

        private void GetGoodsReceiptPendingCustomers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          " + (int)@GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition + " AS GoodsReceiptTypeID, Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.TerritoryID AS CustomerTerritoryID, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID FROM PurchaseRequisitions WHERE PurchaseRequisitionID IN (SELECT PurchaseRequisitionID FROM PurchaseRequisitionDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0  AND ROUND(Quantity - QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") > 0)) CustomerPENDING " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON CustomerPENDING.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingCustomers", queryString);
        }



        private void GetGoodsReceiptPendingPurchaseRequisitionDetails()
        {
            string queryString;

            SqlProgrammability.Inventories.Inventories inventories = new SqlProgrammability.Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = " @LocationID Int, @GoodsReceiptID Int, @PurchaseRequisitionID Int, @CustomerID Int, @PurchaseRequisitionDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@PurchaseRequisitionID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseRequisition(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseRequisition(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetGoodsReceiptPendingPurchaseRequisitionDetails", queryString);
        }

        private string BuildSQLPurchaseRequisition(bool isPurchaseRequisitionID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@PurchaseRequisitionDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseRequisitionPurchaseRequisitionDetailIDs(isPurchaseRequisitionID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPurchaseRequisitionPurchaseRequisitionDetailIDs(isPurchaseRequisitionID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLPurchaseRequisitionPurchaseRequisitionDetailIDs(bool isPurchaseRequisitionID, bool isPurchaseRequisitionDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@GoodsReceiptID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isPurchaseRequisitionID, isPurchaseRequisitionDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY PurchaseRequisitions.EntryDate, PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitionDetails.PurchaseRequisitionDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPurchaseRequisitionID, isPurchaseRequisitionDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY PurchaseRequisitions.EntryDate, PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitionDetails.PurchaseRequisitionDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLNew(isPurchaseRequisitionID, isPurchaseRequisitionDetailIDs) + " WHERE PurchaseRequisitionDetails.PurchaseRequisitionDetailID NOT IN (SELECT PurchaseRequisitionDetailID FROM GoodsReceiptDetails WHERE GoodsReceiptID = @GoodsReceiptID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPurchaseRequisitionID, isPurchaseRequisitionDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY PurchaseRequisitions.EntryDate, PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitionDetails.PurchaseRequisitionDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isPurchaseRequisitionID, bool isPurchaseRequisitionDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitionDetails.PurchaseRequisitionDetailID, PurchaseRequisitions.Reference AS PurchaseRequisitionReference, PurchaseRequisitions.Code AS PurchaseRequisitionCode, PurchaseRequisitions.EntryDate AS PurchaseRequisitionEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(PurchaseRequisitionDetails.Quantity - PurchaseRequisitionDetails.QuantityReceipt, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0 AS Quantity, PurchaseRequisitions.Description, PurchaseRequisitionDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PurchaseRequisitions " + "\r\n";
            queryString = queryString + "                   INNER JOIN PurchaseRequisitionDetails ON " + (isPurchaseRequisitionID ? " PurchaseRequisitions.PurchaseRequisitionID = @PurchaseRequisitionID " : "PurchaseRequisitions.LocationID = @LocationID AND PurchaseRequisitions.CustomerID = @CustomerID ") + " AND PurchaseRequisitionDetails.Approved = 1 AND PurchaseRequisitionDetails.InActive = 0 AND PurchaseRequisitionDetails.InActivePartial = 0 AND ROUND(PurchaseRequisitionDetails.Quantity- PurchaseRequisitionDetails.QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") > 0 AND PurchaseRequisitions.PurchaseRequisitionID = PurchaseRequisitionDetails.PurchaseRequisitionID" + (isPurchaseRequisitionDetailIDs ? " AND PurchaseRequisitionDetails.PurchaseRequisitionDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PurchaseRequisitionDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PurchaseRequisitionDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isPurchaseRequisitionID, bool isPurchaseRequisitionDetailIDs)
        {
            string queryString = "";
            
            queryString = queryString + "       SELECT      PurchaseRequisitions.PurchaseRequisitionID, PurchaseRequisitionDetails.PurchaseRequisitionDetailID, PurchaseRequisitions.Reference AS PurchaseRequisitionReference, PurchaseRequisitions.Code AS PurchaseRequisitionCode, PurchaseRequisitions.EntryDate AS PurchaseRequisitionEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ROUND(PurchaseRequisitionDetails.Quantity - PurchaseRequisitionDetails.QuantityReceipt + GoodsReceiptDetails.Quantity, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   0 AS Quantity, PurchaseRequisitions.Description, PurchaseRequisitionDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PurchaseRequisitionDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsReceiptDetails ON GoodsReceiptDetails.GoodsReceiptID = @GoodsReceiptID AND PurchaseRequisitionDetails.PurchaseRequisitionDetailID = GoodsReceiptDetails.PurchaseRequisitionDetailID" + (isPurchaseRequisitionDetailIDs ? " AND PurchaseRequisitionDetails.PurchaseRequisitionDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PurchaseRequisitionDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PurchaseRequisitionDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PurchaseRequisitions ON PurchaseRequisitionDetails.PurchaseRequisitionID = PurchaseRequisitions.PurchaseRequisitionID " + "\r\n";

            return queryString;
        }


        private void GoodsReceiptSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "       BEGIN " + "\r\n";

            //queryString = queryString + "           IF (@SaveRelativeOption = 1) ";
            //queryString = queryString + "               BEGIN ";
            //queryString = queryString + "                   UPDATE          GoodsReceiptDetails " + "\r\n";
            //queryString = queryString + "                   SET             GoodsReceiptDetails.Reference = GoodsReceipts.Reference " + "\r\n";
            //queryString = queryString + "                   FROM            GoodsReceipts INNER JOIN GoodsReceiptDetails ON GoodsReceipts.GoodsReceiptID = @EntityID AND GoodsReceipts.GoodsReceiptID = GoodsReceiptDetails.GoodsReceiptID " + "\r\n";
            //queryString = queryString + "               END ";

            queryString = queryString + "           DECLARE @GoodsReceiptTypeID int, @AffectedROWCOUNT int ";
            queryString = queryString + "           SELECT  @GoodsReceiptTypeID = GoodsReceiptTypeID FROM GoodsReceipts WHERE GoodsReceiptID = @EntityID ";

            queryString = queryString + "           IF (@GoodsReceiptTypeID > 0) " + "\r\n";
            queryString = queryString + "               BEGIN  " + "\r\n";

            queryString = queryString + "                   IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseRequisition + ") " + "\r\n";
            queryString = queryString + "                       BEGIN  " + "\r\n";
            queryString = queryString + "                           UPDATE          PurchaseRequisitionDetails " + "\r\n";
            queryString = queryString + "                           SET             PurchaseRequisitionDetails.QuantityReceipt = ROUND(PurchaseRequisitionDetails.QuantityReceipt + GoodsReceiptDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + ") " + "\r\n";
            queryString = queryString + "                           FROM            GoodsReceiptDetails " + "\r\n";
            queryString = queryString + "                                           INNER JOIN PurchaseRequisitionDetails ON ((PurchaseRequisitionDetails.Approved = 1 AND PurchaseRequisitionDetails.InActive = 0 AND PurchaseRequisitionDetails.InActivePartial = 0) OR @SaveRelativeOption = -1) AND GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.PurchaseRequisitionDetailID = PurchaseRequisitionDetails.PurchaseRequisitionDetailID " + "\r\n";
            queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            queryString = queryString + "                       END " + "\r\n";


            //queryString = queryString + "                   IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.GoodsIssueTransfer + ") " + "\r\n";
            //queryString = queryString + "                       BEGIN  " + "\r\n";
            //queryString = queryString + "                           UPDATE          GoodsIssueTransferDetails " + "\r\n";
            //queryString = queryString + "                           SET             GoodsIssueTransferDetails.QuantityReceipt = ROUND(GoodsIssueTransferDetails.QuantityReceipt + GoodsReceiptDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), GoodsIssueTransferDetails.LineVolumeReceipt = ROUND(GoodsIssueTransferDetails.LineVolumeReceipt + GoodsReceiptDetails.LineVolume * @SaveRelativeOption, " + (int)GlobalEnums.rndVolume + ") " + "\r\n";
            //queryString = queryString + "                           FROM            GoodsReceiptDetails " + "\r\n";
            //queryString = queryString + "                                           INNER JOIN GoodsIssueTransferDetails ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsIssueTransferDetails.Approved = 1 AND GoodsReceiptDetails.GoodsIssueTransferDetailID = GoodsIssueTransferDetails.GoodsIssueTransferDetailID " + "\r\n";
            //queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            //queryString = queryString + "                       END " + "\r\n";


            //queryString = queryString + "                   IF (@GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.WarehouseAdjustments + ") " + "\r\n";
            //queryString = queryString + "                       BEGIN  " + "\r\n";
            //queryString = queryString + "                           UPDATE          WarehouseAdjustmentDetails " + "\r\n";
            //queryString = queryString + "                           SET             WarehouseAdjustmentDetails.QuantityReceipt = ROUND(WarehouseAdjustmentDetails.QuantityReceipt + GoodsReceiptDetails.Quantity * @SaveRelativeOption, " + (int)GlobalEnums.rndQuantity + "), WarehouseAdjustmentDetails.LineVolumeReceipt = ROUND(WarehouseAdjustmentDetails.LineVolumeReceipt + GoodsReceiptDetails.LineVolume * @SaveRelativeOption, " + (int)GlobalEnums.rndVolume + ") " + "\r\n";
            //queryString = queryString + "                           FROM            GoodsReceiptDetails " + "\r\n";
            //queryString = queryString + "                                           INNER JOIN WarehouseAdjustmentDetails ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND WarehouseAdjustmentDetails.Quantity > 0 AND GoodsReceiptDetails.WarehouseAdjustmentDetailID = WarehouseAdjustmentDetails.WarehouseAdjustmentDetailID " + "\r\n";
            //queryString = queryString + "                           SET @AffectedROWCOUNT = @@ROWCOUNT " + "\r\n";
            ////------------------------------------------------------SHOULD UPDATE GoodsReceiptID, GoodsReceiptDetailID BACK TO WarehouseAdjustmentDetails FOR GoodsReceipts OF WarehouseAdjustmentDetails? THE ANSWER: WE CAN DO IT HERE, BUT IT BREAK THE RELATIONSHIP (cyclic redundancy relationship: GoodsReceiptDetails => WarehouseAdjustmentDetails => THUS: WE SHOULD NOT MAKE ANOTHER RELATIONSHIP FROM WarehouseAdjustmentDetails => GoodsReceiptDetails ) => SO: SHOULD NOT!!!
            //queryString = queryString + "                       END " + "\r\n";

            queryString = queryString + "                   IF @AffectedROWCOUNT <> (SELECT COUNT(*) FROM GoodsReceiptDetails WHERE GoodsReceiptID = @EntityID) " + "\r\n";
            queryString = queryString + "                       BEGIN " + "\r\n";
            queryString = queryString + "                           DECLARE     @msg NVARCHAR(300) = N'Phiếu giao hàng đã hủy, hoặc chưa duyệt' ; " + "\r\n";
            queryString = queryString + "                           THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "                       END " + "\r\n";

            queryString = queryString + "               END  " + "\r\n";

            queryString = queryString + "       END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GoodsReceiptSaveRelative", queryString);
        }

        private void GoodsReceiptPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đặt hàng: ' + CAST(PurchaseRequisitions.EntryDate AS nvarchar) FROM GoodsReceiptDetails INNER JOIN PurchaseRequisitions ON GoodsReceiptDetails.GoodsReceiptID = @EntityID AND GoodsReceiptDetails.PurchaseRequisitionID = PurchaseRequisitions.PurchaseRequisitionID AND GoodsReceiptDetails.EntryDate < PurchaseRequisitions.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM PurchaseRequisitionDetails WHERE (ROUND(Quantity - QuantityReceipt, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("GoodsReceiptPostSaveValidate", queryArray);
        }




        private void GoodsReceiptApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM GoodsReceipts WHERE GoodsReceiptID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("GoodsReceiptApproved", queryArray);
        }


        private void GoodsReceiptEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = GoodsReceiptID FROM MaterialIssueDetails WHERE GoodsReceiptID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("GoodsReceiptEditable", queryArray);
        }

        private void GoodsReceiptToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      GoodsReceipts  SET Approved = @Approved, ApprovedDate = GetDate() WHERE GoodsReceiptID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          GoodsReceiptDetails  SET Approved = @Approved WHERE GoodsReceiptID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GoodsReceiptToggleApproved", queryString);
        }
        

        private void GoodsReceiptInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("GoodsReceipts", "GoodsReceiptID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.GoodsReceipt));
            this.totalSmartPortalEntities.CreateTrigger("GoodsReceiptInitReference", simpleInitReference.CreateQuery());
        }
    }
}

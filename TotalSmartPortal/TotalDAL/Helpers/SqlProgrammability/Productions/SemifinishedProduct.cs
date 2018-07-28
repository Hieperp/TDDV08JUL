using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class SemifinishedProduct
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public SemifinishedProduct(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetSemifinishedProductIndexes();

            this.GetSemifinishedProductViewDetails();

            this.GetSemifinishedProductPendingCustomers();
            this.GetSemifinishedProductPendingMaterialIssues();
            this.GetSemifinishedProductPendingMaterialIssueDetails();

            this.SemifinishedProductSaveRelative();
            this.SemifinishedProductPostSaveValidate();

            this.SemifinishedProductApproved();
            this.SemifinishedProductEditable();
            this.SemifinishedProductVoidable();

            this.SemifinishedProductToggleApproved();
            this.SemifinishedProductToggleVoid();
            this.SemifinishedProductToggleVoidDetail();

            this.SemifinishedProductInitReference();
        }


        private void GetSemifinishedProductIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SemifinishedProducts.SemifinishedProductID, CAST(SemifinishedProducts.EntryDate AS DATE) AS EntryDate, SemifinishedProducts.Reference, SemifinishedProducts.Code, Locations.Code AS LocationCode, SemifinishedProducts.Description, SemifinishedProducts.Approved, SemifinishedProducts.InActive, SemifinishedProducts.InActivePartial " + "\r\n";
            queryString = queryString + "       FROM        SemifinishedProducts " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON SemifinishedProducts.EntryDate >= @FromDate AND SemifinishedProducts.EntryDate <= @ToDate AND SemifinishedProducts.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.SemifinishedProduct + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = SemifinishedProducts.LocationID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedProductIndexes", queryString);
        }

        #region X


        private void GetSemifinishedProductViewDetails()
        {
            string queryString;
            SqlProgrammability.Inventories.Inventories inventories = new Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = " @SemifinishedProductID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SemifinishedProductDetails.SemifinishedProductDetailID, SemifinishedProductDetails.SemifinishedProductID, SemifinishedProductDetails.MaterialIssueID, SemifinishedProductDetails.MaterialIssueDetailID, MaterialIssues.Reference AS MaterialIssueReference, MaterialIssues.Code AS MaterialIssueCode, MaterialIssues.EntryDate AS MaterialIssueEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, SemifinishedProductDetails.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   SemifinishedProductDetails.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, SemifinishedProductDetails.ProductionLineID, ProductionLines.Code AS ProductionLineCode, SemifinishedProductDetails.MoldID, Molds.Code AS MoldCode, SemifinishedProductDetails.CommodityMaterialID, CommodityMaterials.Code AS CommodityMaterialCode, " + "\r\n";
            queryString = queryString + "                   VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, " + "\r\n";
            queryString = queryString + "                   ROUND(CASE WHEN MaterialIssueDetails.Approved = 1 AND MaterialIssueDetails.InActive = 0 AND MaterialIssueDetails.InActivePartial = 0 THEN MaterialIssueDetails.Quantity - MaterialIssueDetails.QuantitySemifinished ELSE 0 END, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   SemifinishedProductDetails.InActivePartial, SemifinishedProductDetails.InActivePartialDate, SemifinishedProductDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        SemifinishedProductDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON SemifinishedProductDetails.SemifinishedProductID = @SemifinishedProductID AND SemifinishedProductDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN MaterialIssueDetails ON SemifinishedProductDetails.MaterialIssueDetailID = MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN MaterialIssues ON MaterialIssueDetails.MaterialIssueID = MaterialIssues.MaterialIssueID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON SemifinishedProductDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON SemifinishedProductDetails.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Molds ON SemifinishedProductDetails.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityMaterials ON SemifinishedProductDetails.CommodityMaterialID = CommodityMaterials.CommodityMaterialID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON SemifinishedProductDetails.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedProductViewDetails", queryString);
        }

        private void GetSemifinishedProductPendingMaterialIssues()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          MaterialIssues.MaterialIssueID, MaterialIssues.Reference AS MaterialIssueReference, MaterialIssues.Code AS MaterialIssueCode, MaterialIssues.EntryDate AS MaterialIssueEntryDate, MaterialIssues.Description, MaterialIssues.Remarks, " + "\r\n";
            queryString = queryString + "                       MaterialIssues.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName " + "\r\n";

            queryString = queryString + "       FROM            MaterialIssues " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON MaterialIssues.MaterialIssueID IN (SELECT MaterialIssueID FROM MaterialIssueDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") > 0) AND MaterialIssues.CustomerID = Customers.CustomerID " + "\r\n";

            queryString = queryString + "                       SELECT MaterialIssueID FROM MaterialIssueDetails WHERE LocationID = @LocationID AND WorkshiftID = @WorkshiftID AND Approved = 1 AND ROUND(Quantity - QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") > 0 " + "\r\n";

            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";



//            SELECT        MaterialIssueDetails.LocationID, MaterialIssueDetails.WorkshiftID, Workshifts.Code, MaterialIssueDetails.CustomerID, Customers.Code AS Expr1, MaterialIssueDetails.ProductionLineID, 
//                         ProductionLines.Code AS Expr2, MaterialIssueDetails.MoldID, Molds.Code AS Expr3, MaterialIssueDetails.MaterialIssueID, MaterialIssueDetails.ProductionOrderDetailID, MaterialIssueDetails.PlannedOrderID, 
//                         PlannedOrders.Reference, PlannedOrders.Code AS Expr4
//FROM            MaterialIssueDetails INNER JOIN
//                         Customers ON MaterialIssueDetails.CustomerID = Customers.CustomerID INNER JOIN
//                         Workshifts ON MaterialIssueDetails.WorkshiftID = Workshifts.WorkshiftID INNER JOIN
//                         ProductionLines ON MaterialIssueDetails.ProductionLineID = ProductionLines.ProductionLineID INNER JOIN
//                         Molds ON MaterialIssueDetails.MoldID = Molds.MoldID INNER JOIN
//                         PlannedOrders ON MaterialIssueDetails.PlannedOrderID = PlannedOrders.PlannedOrderID


            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedProductPendingMaterialIssues", queryString);
        }

        private void GetSemifinishedProductPendingCustomers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.TerritoryID AS CustomerTerritoryID, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID FROM MaterialIssues WHERE MaterialIssueID IN (SELECT MaterialIssueID FROM MaterialIssueDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0  AND ROUND(Quantity - QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") > 0)) CustomerPENDING " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON CustomerPENDING.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedProductPendingCustomers", queryString);
        }



        private void GetSemifinishedProductPendingMaterialIssueDetails()
        {
            string queryString;

            SqlProgrammability.Inventories.Inventories inventories = new SqlProgrammability.Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = " @LocationID Int, @SemifinishedProductID Int, @MaterialIssueID Int, @CustomerID Int, @MaterialIssueDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@MaterialIssueID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLMaterialIssue(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLMaterialIssue(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetSemifinishedProductPendingMaterialIssueDetails", queryString);
        }

        private string BuildSQLMaterialIssue(bool isMaterialIssueID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@MaterialIssueDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLMaterialIssueMaterialIssueDetailIDs(isMaterialIssueID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLMaterialIssueMaterialIssueDetailIDs(isMaterialIssueID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLMaterialIssueMaterialIssueDetailIDs(bool isMaterialIssueID, bool isMaterialIssueDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SemifinishedProductID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isMaterialIssueID, isMaterialIssueDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY MaterialIssues.EntryDate, MaterialIssues.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isMaterialIssueID, isMaterialIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY MaterialIssues.EntryDate, MaterialIssues.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLNew(isMaterialIssueID, isMaterialIssueDetailIDs) + " WHERE MaterialIssueDetails.MaterialIssueDetailID NOT IN (SELECT MaterialIssueDetailID FROM SemifinishedProductDetails WHERE SemifinishedProductID = @SemifinishedProductID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isMaterialIssueID, isMaterialIssueDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY MaterialIssues.EntryDate, MaterialIssues.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isMaterialIssueID, bool isMaterialIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      MaterialIssues.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID, MaterialIssues.Reference AS MaterialIssueReference, MaterialIssues.Code AS MaterialIssueCode, MaterialIssues.EntryDate AS MaterialIssueEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   MaterialIssueDetails.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, MaterialIssueDetails.MoldID, Molds.Code AS MoldCode, MaterialIssueDetails.CommodityMaterialID, CommodityMaterials.Code AS CommodityMaterialCode, " + "\r\n";

            queryString = queryString + "                   ROUND(MaterialIssueDetails.Quantity - MaterialIssueDetails.QuantitySemifinished, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   MaterialIssues.Description, MaterialIssueDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        MaterialIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN MaterialIssueDetails ON " + (isMaterialIssueID ? " MaterialIssues.MaterialIssueID = @MaterialIssueID " : "MaterialIssues.LocationID = @LocationID AND MaterialIssues.CustomerID = @CustomerID ") + " AND MaterialIssueDetails.Approved = 1 AND MaterialIssueDetails.InActive = 0 AND MaterialIssueDetails.InActivePartial = 0 AND ROUND(MaterialIssueDetails.Quantity- MaterialIssueDetails.QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") > 0 AND MaterialIssues.MaterialIssueID = MaterialIssueDetails.MaterialIssueID" + (isMaterialIssueDetailIDs ? " AND MaterialIssueDetails.MaterialIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@MaterialIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON MaterialIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON MaterialIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Molds ON MaterialIssueDetails.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityMaterials ON MaterialIssueDetails.CommodityMaterialID = CommodityMaterials.CommodityMaterialID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isMaterialIssueID, bool isMaterialIssueDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      MaterialIssues.MaterialIssueID, MaterialIssueDetails.MaterialIssueDetailID, MaterialIssues.Reference AS MaterialIssueReference, MaterialIssues.Code AS MaterialIssueCode, MaterialIssues.EntryDate AS MaterialIssueEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   MaterialIssueDetails.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, MaterialIssueDetails.MoldID, Molds.Code AS MoldCode, MaterialIssueDetails.CommodityMaterialID, CommodityMaterials.Code AS CommodityMaterialCode, " + "\r\n";

            queryString = queryString + "                   ROUND(MaterialIssueDetails.Quantity - MaterialIssueDetails.QuantitySemifinished, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   MaterialIssues.Description, MaterialIssueDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        MaterialIssueDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN SemifinishedProductDetails ON SemifinishedProductDetails.SemifinishedProductID = @SemifinishedProductID AND MaterialIssueDetails.MaterialIssueDetailID = SemifinishedProductDetails.MaterialIssueDetailID" + (isMaterialIssueDetailIDs ? " AND MaterialIssueDetails.MaterialIssueDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@MaterialIssueDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON MaterialIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN MaterialIssues ON MaterialIssueDetails.MaterialIssueID = MaterialIssues.MaterialIssueID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON MaterialIssueDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Molds ON MaterialIssueDetails.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityMaterials ON MaterialIssueDetails.CommodityMaterialID = CommodityMaterials.CommodityMaterialID " + "\r\n";

            return queryString;
        }


        private void SemifinishedProductSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedProductSaveRelative", queryString);
        }

        private void SemifinishedProductPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đặt hàng: ' + CAST(MaterialIssues.EntryDate AS nvarchar) FROM SemifinishedProductDetails INNER JOIN MaterialIssues ON SemifinishedProductDetails.SemifinishedProductID = @EntityID AND SemifinishedProductDetails.MaterialIssueID = MaterialIssues.MaterialIssueID AND SemifinishedProductDetails.EntryDate < MaterialIssues.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM MaterialIssueDetails WHERE (ROUND(Quantity - QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedProductPostSaveValidate", queryArray);
        }




        private void SemifinishedProductApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SemifinishedProductID FROM SemifinishedProducts WHERE SemifinishedProductID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedProductApproved", queryArray);
        }


        private void SemifinishedProductEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SemifinishedProductID FROM SemifinishedProducts WHERE SemifinishedProductID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = SemifinishedProductID FROM SemifinishedProductDetails WHERE SemifinishedProductID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedProductEditable", queryArray);
        }

        private void SemifinishedProductVoidable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = SemifinishedProductID FROM SemifinishedProducts WHERE SemifinishedProductID = @EntityID AND Approved = 0"; //Must approve in order to allow void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = SemifinishedProductID FROM SemifinishedProductDetails WHERE SemifinishedProductID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("SemifinishedProductVoidable", queryArray);
        }


        private void SemifinishedProductToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      SemifinishedProducts  SET Approved = @Approved, ApprovedDate = GetDate() WHERE SemifinishedProductID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          SemifinishedProductDetails  SET Approved = @Approved WHERE SemifinishedProductID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedProductToggleApproved", queryString);
        }

        private void SemifinishedProductToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      SemifinishedProducts  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE SemifinishedProductID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          SemifinishedProductDetails  SET InActive = @InActive WHERE SemifinishedProductID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActive = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedProductToggleVoid", queryString);
        }

        private void SemifinishedProductToggleVoidDetail()
        {
            string queryString = " @EntityID int, @EntityDetailID int, @InActivePartial bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      SemifinishedProductDetails  SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE SemifinishedProductID = @EntityID AND SemifinishedProductDetailID = @EntityDetailID AND InActivePartial = ~@InActivePartial ; " + "\r\n";
            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE         @MaxInActivePartial bit     SET @MaxInActivePartial = (SELECT MAX(CAST(InActivePartial AS int)) FROM SemifinishedProductDetails WHERE SemifinishedProductID = @EntityID) ;" + "\r\n";
            queryString = queryString + "               UPDATE          SemifinishedProducts  SET InActivePartial = @MaxInActivePartial WHERE SemifinishedProductID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActivePartial = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            this.totalSmartPortalEntities.CreateStoredProcedure("SemifinishedProductToggleVoidDetail", queryString);
        }


        private void SemifinishedProductInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("SemifinishedProducts", "SemifinishedProductID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.SemifinishedProduct));
            this.totalSmartPortalEntities.CreateTrigger("SemifinishedProductInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}

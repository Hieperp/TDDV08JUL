using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class ProductionOrder
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public ProductionOrder(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetProductionOrderIndexes();

            this.GetProductionOrderViewDetails();

            this.GetProductionOrderPendingCustomers();
            this.GetProductionOrderPendingPlannedOrders();
            this.GetProductionOrderPendingPlannedOrderDetails();

            this.ProductionOrderSaveRelative();
            this.ProductionOrderPostSaveValidate();

            this.ProductionOrderApproved();
            this.ProductionOrderEditable();
            this.ProductionOrderVoidable();

            this.ProductionOrderToggleApproved();
            this.ProductionOrderToggleVoid();
            this.ProductionOrderToggleVoidDetail();

            this.ProductionOrderInitReference();
        }


        private void GetProductionOrderIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      ProductionOrders.ProductionOrderID, CAST(ProductionOrders.EntryDate AS DATE) AS EntryDate, ProductionOrders.Reference, ProductionOrders.Code, Locations.Code AS LocationCode, ISNULL(VoidTypes.Name, CASE ProductionOrders.InActivePartial WHEN 1 THEN N'Hủy một phần đh' ELSE N'' END) AS VoidTypeName, ProductionOrders.Description, ProductionOrders.Approved, ProductionOrders.InActive, ProductionOrders.InActivePartial " + "\r\n";
            queryString = queryString + "       FROM        ProductionOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON ProductionOrders.EntryDate >= @FromDate AND ProductionOrders.EntryDate <= @ToDate AND ProductionOrders.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.ProductionOrder + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = ProductionOrders.LocationID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON ProductionOrders.VoidTypeID = VoidTypes.VoidTypeID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetProductionOrderIndexes", queryString);
        }

        #region X


        private void GetProductionOrderViewDetails()
        {
            string queryString;
            SqlProgrammability.Inventories.Inventories inventories = new Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = " @ProductionOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      ProductionOrderDetails.ProductionOrderDetailID, ProductionOrderDetails.ProductionOrderID, ProductionOrderDetails.PlannedOrderID, ProductionOrderDetails.PlannedOrderDetailID, PlannedOrders.Reference AS PlannedOrderReference, PlannedOrders.Code AS PlannedOrderCode, PlannedOrders.EntryDate AS PlannedOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, ProductionOrderDetails.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   ProductionOrderDetails.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, ProductionOrderDetails.ProductionLineID, ProductionLines.Code AS ProductionLineCode, ProductionOrderDetails.MoldID, Molds.Code AS MoldCode, ProductionOrderDetails.CommodityMaterialID, CommodityMaterials.Code AS CommodityMaterialCode, " + "\r\n";
            queryString = queryString + "                   VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, " + "\r\n";
            queryString = queryString + "                   ROUND(CASE WHEN PlannedOrderDetails.Approved = 1 AND PlannedOrderDetails.InActive = 0 AND PlannedOrderDetails.InActivePartial = 0 THEN PlannedOrderDetails.Quantity - PlannedOrderDetails.QuantitySemifinished ELSE 0 END, " + (int)GlobalEnums.rndQuantity + ") AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   ProductionOrderDetails.InActivePartial, ProductionOrderDetails.InActivePartialDate, ProductionOrderDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        ProductionOrderDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON ProductionOrderDetails.ProductionOrderID = @ProductionOrderID AND ProductionOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PlannedOrderDetails ON ProductionOrderDetails.PlannedOrderDetailID = PlannedOrderDetails.PlannedOrderDetailID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PlannedOrders ON PlannedOrderDetails.PlannedOrderID = PlannedOrders.PlannedOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON ProductionOrderDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionLines ON ProductionOrderDetails.ProductionLineID = ProductionLines.ProductionLineID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Molds ON ProductionOrderDetails.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityMaterials ON ProductionOrderDetails.CommodityMaterialID = CommodityMaterials.CommodityMaterialID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON ProductionOrderDetails.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetProductionOrderViewDetails", queryString);
        }

        private void GetProductionOrderPendingPlannedOrders()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          PlannedOrders.PlannedOrderID, PlannedOrders.Reference AS PlannedOrderReference, PlannedOrders.Code AS PlannedOrderCode, PlannedOrders.EntryDate AS PlannedOrderEntryDate, PlannedOrders.Description, PlannedOrders.Remarks, " + "\r\n";
            queryString = queryString + "                       PlannedOrders.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName " + "\r\n";

            queryString = queryString + "       FROM            PlannedOrders " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON PlannedOrders.PlannedOrderID IN (SELECT PlannedOrderID FROM PlannedOrderDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0 AND ROUND(Quantity - QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") > 0) AND PlannedOrders.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetProductionOrderPendingPlannedOrders", queryString);
        }

        private void GetProductionOrderPendingCustomers()
        {
            string queryString = " @LocationID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       SELECT          Customers.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, Customers.OfficialName AS CustomerOfficialName, Customers.VATCode AS CustomerVATCode, Customers.AttentionName AS CustomerAttentionName, Customers.TerritoryID AS CustomerTerritoryID, CustomerEntireTerritories.EntireName AS CustomerEntireTerritoryEntireName " + "\r\n";

            queryString = queryString + "       FROM           (SELECT DISTINCT CustomerID FROM PlannedOrders WHERE PlannedOrderID IN (SELECT PlannedOrderID FROM PlannedOrderDetails WHERE LocationID = @LocationID AND Approved = 1 AND InActive = 0 AND InActivePartial = 0  AND ROUND(Quantity - QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") > 0)) CustomerPENDING " + "\r\n";
            queryString = queryString + "                       INNER JOIN Customers ON CustomerPENDING.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                       INNER JOIN EntireTerritories CustomerEntireTerritories ON Customers.TerritoryID = CustomerEntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                       INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetProductionOrderPendingCustomers", queryString);
        }



        private void GetProductionOrderPendingPlannedOrderDetails()
        {
            string queryString;

            SqlProgrammability.Inventories.Inventories inventories = new SqlProgrammability.Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = " @LocationID Int, @ProductionOrderID Int, @PlannedOrderID Int, @CustomerID Int, @PlannedOrderDetailIDs varchar(3999), @IsReadonly bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF  (@PlannedOrderID <> 0) " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPlannedOrder(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPlannedOrder(false) + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetProductionOrderPendingPlannedOrderDetails", queryString);
        }

        private string BuildSQLPlannedOrder(bool isPlannedOrderID)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@PlannedOrderDetailIDs <> '') " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPlannedOrderPlannedOrderDetailIDs(isPlannedOrderID, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.BuildSQLPlannedOrderPlannedOrderDetailIDs(isPlannedOrderID, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLPlannedOrderPlannedOrderDetailIDs(bool isPlannedOrderID, bool isPlannedOrderDetailIDs)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       IF (@ProductionOrderID <= 0) " + "\r\n";
            queryString = queryString + "               BEGIN " + "\r\n";
            queryString = queryString + "                   " + this.BuildSQLNew(isPlannedOrderID, isPlannedOrderDetailIDs) + "\r\n";
            queryString = queryString + "                   ORDER BY PlannedOrders.EntryDate, PlannedOrders.PlannedOrderID, PlannedOrderDetails.PlannedOrderDetailID " + "\r\n";
            queryString = queryString + "               END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";

            queryString = queryString + "               IF (@IsReadonly = 1) " + "\r\n";
            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPlannedOrderID, isPlannedOrderDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY PlannedOrders.EntryDate, PlannedOrders.PlannedOrderID, PlannedOrderDetails.PlannedOrderDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "               ELSE " + "\r\n"; //FULL SELECT FOR EDIT MODE

            queryString = queryString + "                   BEGIN " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLNew(isPlannedOrderID, isPlannedOrderDetailIDs) + " WHERE PlannedOrderDetails.PlannedOrderDetailID NOT IN (SELECT PlannedOrderDetailID FROM ProductionOrderDetails WHERE ProductionOrderID = @ProductionOrderID) " + "\r\n";
            queryString = queryString + "                       UNION ALL " + "\r\n";
            queryString = queryString + "                       " + this.BuildSQLEdit(isPlannedOrderID, isPlannedOrderDetailIDs) + "\r\n";
            queryString = queryString + "                       ORDER BY PlannedOrders.EntryDate, PlannedOrders.PlannedOrderID, PlannedOrderDetails.PlannedOrderDetailID " + "\r\n";
            queryString = queryString + "                   END " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private string BuildSQLNew(bool isPlannedOrderID, bool isPlannedOrderDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      PlannedOrders.PlannedOrderID, PlannedOrderDetails.PlannedOrderDetailID, PlannedOrders.Reference AS PlannedOrderReference, PlannedOrders.Code AS PlannedOrderCode, PlannedOrders.EntryDate AS PlannedOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   PlannedOrderDetails.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, PlannedOrderDetails.MoldID, Molds.Code AS MoldCode, PlannedOrderDetails.CommodityMaterialID, CommodityMaterials.Code AS CommodityMaterialCode, " + "\r\n";

            queryString = queryString + "                   ROUND(PlannedOrderDetails.Quantity - PlannedOrderDetails.QuantitySemifinished, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   PlannedOrders.Description, PlannedOrderDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PlannedOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN PlannedOrderDetails ON " + (isPlannedOrderID ? " PlannedOrders.PlannedOrderID = @PlannedOrderID " : "PlannedOrders.LocationID = @LocationID AND PlannedOrders.CustomerID = @CustomerID ") + " AND PlannedOrderDetails.Approved = 1 AND PlannedOrderDetails.InActive = 0 AND PlannedOrderDetails.InActivePartial = 0 AND ROUND(PlannedOrderDetails.Quantity- PlannedOrderDetails.QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") > 0 AND PlannedOrders.PlannedOrderID = PlannedOrderDetails.PlannedOrderID" + (isPlannedOrderDetailIDs ? " AND PlannedOrderDetails.PlannedOrderDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PlannedOrderDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PlannedOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON PlannedOrderDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Molds ON PlannedOrderDetails.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityMaterials ON PlannedOrderDetails.CommodityMaterialID = CommodityMaterials.CommodityMaterialID " + "\r\n";

            return queryString;
        }

        private string BuildSQLEdit(bool isPlannedOrderID, bool isPlannedOrderDetailIDs)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      PlannedOrders.PlannedOrderID, PlannedOrderDetails.PlannedOrderDetailID, PlannedOrders.Reference AS PlannedOrderReference, PlannedOrders.Code AS PlannedOrderCode, PlannedOrders.EntryDate AS PlannedOrderEntryDate, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   PlannedOrderDetails.CustomerID, Customers.Code AS CustomerCode, Customers.Name AS CustomerName, PlannedOrderDetails.MoldID, Molds.Code AS MoldCode, PlannedOrderDetails.CommodityMaterialID, CommodityMaterials.Code AS CommodityMaterialCode, " + "\r\n";

            queryString = queryString + "                   ROUND(PlannedOrderDetails.Quantity - PlannedOrderDetails.QuantitySemifinished, 0) AS QuantityRemains, " + "\r\n";
            queryString = queryString + "                   PlannedOrders.Description, PlannedOrderDetails.Remarks, CAST(1 AS bit) AS IsSelected " + "\r\n";

            queryString = queryString + "       FROM        PlannedOrderDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN ProductionOrderDetails ON ProductionOrderDetails.ProductionOrderID = @ProductionOrderID AND PlannedOrderDetails.PlannedOrderDetailID = ProductionOrderDetails.PlannedOrderDetailID" + (isPlannedOrderDetailIDs ? " AND PlannedOrderDetails.PlannedOrderDetailID NOT IN (SELECT Id FROM dbo.SplitToIntList (@PlannedOrderDetailIDs))" : "") + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PlannedOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN PlannedOrders ON PlannedOrderDetails.PlannedOrderID = PlannedOrders.PlannedOrderID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON PlannedOrderDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Molds ON PlannedOrderDetails.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityMaterials ON PlannedOrderDetails.CommodityMaterialID = CommodityMaterials.CommodityMaterialID " + "\r\n";

            return queryString;
        }


        private void ProductionOrderSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("ProductionOrderSaveRelative", queryString);
        }

        private void ProductionOrderPostSaveValidate()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày đặt hàng: ' + CAST(PlannedOrders.EntryDate AS nvarchar) FROM ProductionOrderDetails INNER JOIN PlannedOrders ON ProductionOrderDetails.ProductionOrderID = @EntityID AND ProductionOrderDetails.PlannedOrderID = PlannedOrders.PlannedOrderID AND ProductionOrderDetails.EntryDate < PlannedOrders.EntryDate ";
            queryArray[1] = " SELECT TOP 1 @FoundEntity = N'Số lượng xuất vượt quá số lượng đặt hàng: ' + CAST(ROUND(Quantity - QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") AS nvarchar) FROM PlannedOrderDetails WHERE (ROUND(Quantity - QuantitySemifinished, " + (int)GlobalEnums.rndQuantity + ") < 0) ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("ProductionOrderPostSaveValidate", queryArray);
        }




        private void ProductionOrderApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = ProductionOrderID FROM ProductionOrders WHERE ProductionOrderID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("ProductionOrderApproved", queryArray);
        }


        private void ProductionOrderEditable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = ProductionOrderID FROM ProductionOrders WHERE ProductionOrderID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = ProductionOrderID FROM SemifinishedProductDetails WHERE ProductionOrderID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("ProductionOrderEditable", queryArray);
        }

        private void ProductionOrderVoidable()
        {
            string[] queryArray = new string[2];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = ProductionOrderID FROM ProductionOrders WHERE ProductionOrderID = @EntityID AND Approved = 0"; //Must approve in order to allow void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = ProductionOrderID FROM SemifinishedProductDetails WHERE ProductionOrderID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("ProductionOrderVoidable", queryArray);
        }


        private void ProductionOrderToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      ProductionOrders  SET Approved = @Approved, ApprovedDate = GetDate() WHERE ProductionOrderID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          ProductionOrderDetails  SET Approved = @Approved WHERE ProductionOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("ProductionOrderToggleApproved", queryString);
        }

        private void ProductionOrderToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      ProductionOrders  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE ProductionOrderID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          ProductionOrderDetails  SET InActive = @InActive WHERE ProductionOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActive = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("ProductionOrderToggleVoid", queryString);
        }

        private void ProductionOrderToggleVoidDetail()
        {
            string queryString = " @EntityID int, @EntityDetailID int, @InActivePartial bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      ProductionOrderDetails  SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE ProductionOrderID = @EntityID AND ProductionOrderDetailID = @EntityDetailID AND InActivePartial = ~@InActivePartial ; " + "\r\n";
            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE         @MaxInActivePartial bit     SET @MaxInActivePartial = (SELECT MAX(CAST(InActivePartial AS int)) FROM ProductionOrderDetails WHERE ProductionOrderID = @EntityID) ;" + "\r\n";
            queryString = queryString + "               UPDATE          ProductionOrders  SET InActivePartial = @MaxInActivePartial WHERE ProductionOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActivePartial = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            this.totalSmartPortalEntities.CreateStoredProcedure("ProductionOrderToggleVoidDetail", queryString);
        }


        private void ProductionOrderInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("ProductionOrders", "ProductionOrderID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.ProductionOrder));
            this.totalSmartPortalEntities.CreateTrigger("ProductionOrderInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}

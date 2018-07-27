using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Productions
{
    public class PlannedOrder
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public PlannedOrder(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetPlannedOrderIndexes();

            this.GetPlannedOrderViewDetails();
            this.PlannedOrderSaveRelative();
            this.PlannedOrderPostSaveValidate();

            this.PlannedOrderApproved();
            this.PlannedOrderEditable();
            this.PlannedOrderVoidable();

            this.PlannedOrderToggleApproved();
            this.PlannedOrderToggleVoid();
            this.PlannedOrderToggleVoidDetail();

            this.PlannedOrderInitReference();
        }


        private void GetPlannedOrderIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      PlannedOrders.PlannedOrderID, CAST(PlannedOrders.EntryDate AS DATE) AS EntryDate, PlannedOrders.Reference, PlannedOrders.Code, Locations.Code AS LocationCode, ISNULL(VoidTypes.Name, CASE PlannedOrders.InActivePartial WHEN 1 THEN N'Hủy một phần đh' ELSE N'' END) AS VoidTypeName, PlannedOrders.Description, PlannedOrders.Approved, PlannedOrders.InActive, PlannedOrders.InActivePartial " + "\r\n";
            queryString = queryString + "       FROM        PlannedOrders " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON PlannedOrders.EntryDate >= @FromDate AND PlannedOrders.EntryDate <= @ToDate AND PlannedOrders.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.PlannedOrder + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = PlannedOrders.LocationID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON PlannedOrders.VoidTypeID = VoidTypes.VoidTypeID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetPlannedOrderIndexes", queryString);
        }

        #region X


        private void GetPlannedOrderViewDetails()
        {
            string queryString;
            SqlProgrammability.Inventories.Inventories inventories = new Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = " @PlannedOrderID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      PlannedOrderDetails.PlannedOrderDetailID, PlannedOrderDetails.PlannedOrderID, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, PlannedOrderDetails.CommodityTypeID, " + "\r\n";
            queryString = queryString + "                   PlannedOrderDetails.MoldID, Molds.Code AS MoldCode, PlannedOrderDetails.CommodityMaterialID, CommodityMaterials.Code AS CommodityMaterialCode, CommodityMaterials.Name AS CommodityMaterialName, " + "\r\n";
            queryString = queryString + "                   VoidTypes.VoidTypeID, VoidTypes.Code AS VoidTypeCode, VoidTypes.Name AS VoidTypeName, VoidTypes.VoidClassID, " + "\r\n";
            queryString = queryString + "                   PlannedOrderDetails.Quantity, PlannedOrderDetails.InActivePartial, PlannedOrderDetails.InActivePartialDate, PlannedOrderDetails.Remarks " + "\r\n";
            queryString = queryString + "       FROM        PlannedOrderDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON PlannedOrderDetails.PlannedOrderID = @PlannedOrderID AND PlannedOrderDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Molds ON PlannedOrderDetails.MoldID = Molds.MoldID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityMaterials ON PlannedOrderDetails.CommodityMaterialID = CommodityMaterials.CommodityMaterialID " + "\r\n";
            queryString = queryString + "                   LEFT JOIN VoidTypes ON PlannedOrderDetails.VoidTypeID = VoidTypes.VoidTypeID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetPlannedOrderViewDetails", queryString);
        }

        private void PlannedOrderSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("PlannedOrderSaveRelative", queryString);
        }

        private void PlannedOrderPostSaveValidate()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'TEST Date: ' + CAST(EntryDate AS nvarchar) FROM PlannedOrders WHERE PlannedOrderID = @EntityID "; //FOR TEST TO BREAK OUT WHEN SAVE -> CHECK ROLL BACK OF TRANSACTION
            //queryArray[0] = " SELECT TOP 1 @FoundEntity = 'Service Date: ' + CAST(ServiceInvoices.EntryDate AS nvarchar) FROM PlannedOrders INNER JOIN PlannedOrders AS ServiceInvoices ON PlannedOrders.PlannedOrderID = @EntityID AND PlannedOrders.ServiceInvoiceID = ServiceInvoices.PlannedOrderID AND PlannedOrders.EntryDate < ServiceInvoices.EntryDate ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PlannedOrderPostSaveValidate", queryArray);
        }




        private void PlannedOrderApproved()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PlannedOrderID FROM PlannedOrders WHERE PlannedOrderID = @EntityID AND Approved = 1";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PlannedOrderApproved", queryArray);
        }


        private void PlannedOrderEditable()
        {
            string[] queryArray = new string[3];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PlannedOrderID FROM PlannedOrders WHERE PlannedOrderID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            queryArray[1] = " SELECT TOP 1 @FoundEntity = PlannedOrderID FROM ProductionOrderDetails WHERE PlannedOrderID = @EntityID ";
            queryArray[2] = " SELECT TOP 1 @FoundEntity = PlannedOrderID FROM MaterialIssueDetails WHERE PlannedOrderID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PlannedOrderEditable", queryArray);
        }

        private void PlannedOrderVoidable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = PlannedOrderID FROM PlannedOrders WHERE PlannedOrderID = @EntityID AND Approved = 0"; //Must approve in order to allow void

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("PlannedOrderVoidable", queryArray);
        }


        private void PlannedOrderToggleApproved()
        {
            string queryString = " @EntityID int, @Approved bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      PlannedOrders  SET Approved = @Approved, ApprovedDate = GetDate() WHERE PlannedOrderID = @EntityID AND Approved = ~@Approved" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          PlannedOrderDetails  SET Approved = @Approved WHERE PlannedOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "               IF (@Approved = 1) " + "\r\n";
            queryString = queryString + "                   INSERT INTO     PlannedOrderMaterials (PlannedOrderDetailID, PlannedOrderID, EntryDate, LocationID, CustomerID, CommodityID, CommodityTypeID, CommodityMaterialID, MaterialID, MoldID, DeliveryDate, BlockUnit, BlockQuantity, Quantity, QuantityIssued, Remarks, VoidTypeID, Approved, InActive, InActivePartial, InActivePartialDate) " + "\r\n";
            queryString = queryString + "                   SELECT          PlannedOrderDetails.PlannedOrderDetailID, PlannedOrderDetails.PlannedOrderID, PlannedOrderDetails.EntryDate, PlannedOrderDetails.LocationID, PlannedOrderDetails.CustomerID, PlannedOrderDetails.CommodityID, PlannedOrderDetails.CommodityTypeID, PlannedOrderDetails.CommodityMaterialID, CommodityMaterialDetails.MaterialID, PlannedOrderDetails.MoldID, PlannedOrderDetails.DeliveryDate, CommodityMaterialDetails.BlockUnit, CommodityMaterialDetails.BlockQuantity, ROUND(PlannedOrderDetails.Quantity * CommodityMaterialDetails.BlockQuantity / CommodityMaterialDetails.BlockUnit, " + (int)GlobalEnums.rndQuantity + ") AS Quantity, 0 AS QuantityIssued, PlannedOrderDetails.Remarks, PlannedOrderDetails.VoidTypeID, PlannedOrderDetails.Approved, PlannedOrderDetails.InActive, PlannedOrderDetails.InActivePartial, PlannedOrderDetails.InActivePartialDate " + "\r\n";
            queryString = queryString + "                   FROM            PlannedOrderDetails INNER JOIN CommodityMaterialDetails ON PlannedOrderDetails.PlannedOrderID = @EntityID AND PlannedOrderDetails.CommodityMaterialID = CommodityMaterialDetails.CommodityMaterialID " + "\r\n";
            queryString = queryString + "                   ORDER BY        PlannedOrderDetails.PlannedOrderDetailID, CommodityMaterialDetails.CommodityMaterialDetailID ; " + "\r\n";
            
            queryString = queryString + "               ELSE " + "\r\n";
            queryString = queryString + "                   DELETE FROM     PlannedOrderMaterials WHERE PlannedOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@Approved = 0, 'hủy', '')  + ' duyệt' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("PlannedOrderToggleApproved", queryString);
        }

        private void PlannedOrderToggleVoid()
        {
            string queryString = " @EntityID int, @InActive bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      PlannedOrders  SET InActive = @InActive, InActiveDate = GetDate(), VoidTypeID = IIF(@InActive = 1, @VoidTypeID, NULL) WHERE PlannedOrderID = @EntityID AND InActive = ~@InActive" + "\r\n";

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          PlannedOrderMaterials   SET InActive = @InActive WHERE PlannedOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "               UPDATE          PlannedOrderDetails     SET InActive = @InActive WHERE PlannedOrderID = @EntityID ; " + "\r\n";            
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActive = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";


            this.totalSmartPortalEntities.CreateStoredProcedure("PlannedOrderToggleVoid", queryString);
        }

        private void PlannedOrderToggleVoidDetail()
        {
            string queryString = " @EntityID int, @EntityDetailID int, @InActivePartial bit, @VoidTypeID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";

            queryString = queryString + "       UPDATE      PlannedOrderMaterials   SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE PlannedOrderID = @EntityID AND PlannedOrderDetailID = @EntityDetailID AND InActivePartial = ~@InActivePartial ; " + "\r\n";
            queryString = queryString + "       UPDATE      PlannedOrderDetails     SET InActivePartial = @InActivePartial, InActivePartialDate = GetDate(), VoidTypeID = IIF(@InActivePartial = 1, @VoidTypeID, NULL) WHERE PlannedOrderID = @EntityID AND PlannedOrderDetailID = @EntityDetailID AND InActivePartial = ~@InActivePartial ; " + "\r\n";            

            queryString = queryString + "       IF @@ROWCOUNT = 1 " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE         @MaxInActivePartial bit     SET @MaxInActivePartial = (SELECT MAX(CAST(InActivePartial AS int)) FROM PlannedOrderDetails WHERE PlannedOrderID = @EntityID) ;" + "\r\n";
            queryString = queryString + "               UPDATE          PlannedOrders  SET InActivePartial = @MaxInActivePartial WHERE PlannedOrderID = @EntityID ; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               DECLARE     @msg NVARCHAR(300) = N'Dữ liệu không tồn tại hoặc đã ' + iif(@InActivePartial = 0, 'phục hồi lệnh', '')  + ' hủy' ; " + "\r\n";
            queryString = queryString + "               THROW       61001,  @msg, 1; " + "\r\n";
            queryString = queryString + "           END " + "\r\n";
            this.totalSmartPortalEntities.CreateStoredProcedure("PlannedOrderToggleVoidDetail", queryString);
        }


        private void PlannedOrderInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("PlannedOrders", "PlannedOrderID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.PlannedOrder));
            this.totalSmartPortalEntities.CreateTrigger("PlannedOrderInitReference", simpleInitReference.CreateQuery());
        }


        #endregion
    }
}

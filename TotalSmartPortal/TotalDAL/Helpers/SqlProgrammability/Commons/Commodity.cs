using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Commodity
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Commodity(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            this.GetCommodityIndexes();

            this.CommodityEditable();

            this.GetCommodityBases();
            this.GetCommodityMaterialBases();
        }


        private void GetCommodityIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      CommodityID, Code, Name, OfficialName, Remarks " + "\r\n";
            queryString = queryString + "       FROM        Commodities " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetCommodityIndexes", queryString);
        }

        private void CommodityEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = CommodityID FROM Commoditys WHERE @EntityID = 1"; //AT TUE VIET ONLY: Don't allow edit default mold, because it is related to Customers

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = CommodityID FROM Commoditys WHERE CommodityID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = CommodityID FROM GoodsIssueDetails WHERE CommodityID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("CommodityEditable", queryArray);
        }

        private void GetCommodityBases()
        {
            string queryString;

            queryString = " @CommodityTypeIDList varchar(200), @SearchText nvarchar(60) " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      TOP 30 Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CommodityTypeID, Commodities.ListedPrice, Commodities.GrossPrice, 0.0 AS DiscountPercent, 0.0 AS TradeDiscountRate, CommodityCategories.VATPercent " + " \r\n";
            queryString = queryString + "       FROM        Commodities " + "\r\n";
            queryString = queryString + "                   INNER JOIN CommodityCategories ON Commodities.InActive = 0 AND (@SearchText = '' OR Commodities.Code LIKE '%' + @SearchText + '%' OR Commodities.OfficialCode LIKE '%' + @SearchText + '%' OR Commodities.Name LIKE '%' + @SearchText + '%') AND Commodities.CommodityTypeID IN (SELECT Id FROM dbo.SplitToIntList (@CommodityTypeIDList)) AND Commodities.CommodityCategoryID = CommodityCategories.CommodityCategoryID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetCommodityBases", queryString);
        }

        private void GetCommodityMaterialBases()
        {
            string queryString;

            queryString = " @SearchText nvarchar(60), @CommodityID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      TOP 30 CommodityMaterialID, Code AS CommodityMaterialCode, Name AS CommodityMaterialName, Reference AS CommodityMaterialReference " + " \r\n";
            queryString = queryString + "       FROM        CommodityMaterials " + "\r\n";
            queryString = queryString + "       WHERE       InActive = 0 AND (1 = 1 OR CommodityID = @CommodityID) AND (@SearchText = '' OR Code LIKE '%' + @SearchText + '%' OR Reference LIKE '%' + @SearchText + '%') " + "\r\n";
            queryString = queryString + "       ORDER BY    IsDefault " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetCommodityMaterialBases", queryString);
        }

    }
}


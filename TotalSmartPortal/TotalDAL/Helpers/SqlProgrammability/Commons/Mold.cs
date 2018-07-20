using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Mold
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public Mold(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            //this.GetMoldIndexes();

            //this.MoldEditable();

            this.GetMoldBases();
        }


        private void GetMoldIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      MoldID, Code, Name, Title, Birthday, Telephone, Address, Remarks " + "\r\n";
            queryString = queryString + "       FROM        Molds " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMoldIndexes", queryString);
        }

        private void MoldEditable()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = MoldID FROM Molds WHERE @EntityID = 1"; //AT TUE VIET ONLY: Don't allow edit default mold, because it is related to Customers

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = MoldID FROM Molds WHERE MoldID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = MoldID FROM GoodsIssueDetails WHERE MoldID = @EntityID ";

            this.totalSmartPortalEntities.CreateProcedureToCheckExisting("MoldEditable", queryArray);
        }

        private void GetMoldBases()
        {
            string queryString;

            queryString = " @SearchText nvarchar(60), @CommodityID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      TOP 30 MoldID, Code AS MoldCode, Name AS MoldName " + " \r\n";
            queryString = queryString + "       FROM        Molds " + "\r\n";
            queryString = queryString + "       WHERE       InActive = 0 AND MoldID IN (SELECT MoldID FROM CommodityMolds WHERE CommodityID = @CommodityID) AND (@SearchText = '' OR Code LIKE '%' + @SearchText + '%' OR Name LIKE '%' + @SearchText + '%') " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GetMoldBases", queryString);
        }

    }
}


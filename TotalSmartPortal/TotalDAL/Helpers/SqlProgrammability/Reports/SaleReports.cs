﻿using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Reports
{
    public class SaleReports
    {
        private readonly TotalSmartPortalEntities totalSmartPortalEntities;

        public SaleReports(TotalSmartPortalEntities totalSmartPortalEntities)
        {
            this.totalSmartPortalEntities = totalSmartPortalEntities;
        }

        public void RestoreProcedure()
        {
            //this.GoodsIssueJournal();
            ////this.AccountInvoiceJournal();

            this.GoodsIssueBalance();

            this.SalesJournal();
            this.StatementOfAccount();

            //this.StatementOfWarehouses();

            ////this.SearchWarehouseEntries();

            this.SalesReturnJournal();
        }



        private void GoodsIssueJournal()
        {
            string queryString;

            queryString = " @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime         SET @LocalFromDate = @FromDate" + "\r\n";
            queryString = queryString + "       DECLARE     @LocalToDate DateTime           SET @LocalToDate = @ToDate" + "\r\n";

            queryString = queryString + "       SELECT      GoodsIssues.GoodsIssueID, 'Inventories/GoodsIssues' AS TaskAction, GoodsIssues.EntryDate, DATEADD(Month, DateDiff(Month, 0, GoodsIssues.EntryDate), 0) AS EntryMonth, GoodsIssues.Reference, Workshifts.WorkshiftID, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, WorkshiftCategories.Name AS WorkshiftCategoryName, Receivers.WorkshiftID AS ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Employees.Name AS SalespersonName, GoodsIssues.Description, GoodsIssues.DeliveryAdviceReferences, DeliveryAdvices.Reference AS DeliveryAdviceReference, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.GoodsIssueDetailID, GoodsIssueDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.Name AS CommodityName, Commodities.Weight AS UnitWeight, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.Quantity, GoodsIssueDetails.FreeQuantity, GoodsIssueDetails.Quantity + GoodsIssueDetails.FreeQuantity AS LineQuantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.ListedGrossPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.ListedAmount, GoodsIssueDetails.ListedGrossAmount, GoodsIssueDetails.Amount, ROUND(GoodsIssueDetails.ListedAmount - GoodsIssueDetails.Amount, " + (int)GlobalEnums.rndAmount + ") AS DiscountAmount, GoodsIssues.TotalQuantity, GoodsIssues.TotalAmount, GoodsIssues.TradeDiscountAmount, GoodsIssues.TotalTaxableAmount, GoodsIssues.TotalVATAmount, GoodsIssues.SumListedGrossAmount, GoodsIssues.TotalGrossAmount " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssueDetails ON GoodsIssues.EntryDate >= @LocalFromDate AND GoodsIssues.EntryDate <= @LocalToDate AND GoodsIssues.GoodsIssueID = GoodsIssueDetails.GoodsIssueID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON GoodsIssues.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts AS Receivers ON GoodsIssues.ReceiverID = Receivers.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN WorkshiftCategories ON Workshifts.WorkshiftCategoryID = WorkshiftCategories.WorkshiftCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN DeliveryAdvices ON GoodsIssueDetails.DeliveryAdviceID = DeliveryAdvices.DeliveryAdviceID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees ON DeliveryAdvices.SalespersonID = Employees.EmployeeID " + "\r\n";

            queryString = queryString + "       UNION ALL   " + "\r\n";

            queryString = queryString + "       SELECT      SalesReturns.SalesReturnID AS GoodsIssueID, 'Inventories/SalesReturns' AS TaskAction, SalesReturns.EntryDate, DATEADD(Month, DateDiff(Month, 0, SalesReturns.EntryDate), 0) AS EntryMonth, SalesReturns.Reference, Workshifts.WorkshiftID, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, WorkshiftCategories.Name AS WorkshiftCategoryName, Receivers.WorkshiftID AS ReceiverID, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Employees.Name AS SalespersonName, SalesReturns.Description, SalesReturns.GoodsIssueReferences AS DeliveryAdviceReferences, SalesReturns.GoodsIssueReferences AS DeliveryAdviceReference, " + "\r\n";
            queryString = queryString + "                   SalesReturnDetails.SalesReturnDetailID AS GoodsIssueDetailID, SalesReturnDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.Name AS CommodityName, Commodities.Weight AS UnitWeight, " + "\r\n";
            queryString = queryString + "                   -SalesReturnDetails.Quantity, -SalesReturnDetails.FreeQuantity, -SalesReturnDetails.Quantity - SalesReturnDetails.FreeQuantity AS LineQuantity, SalesReturnDetails.ListedPrice, SalesReturnDetails.ListedGrossPrice, SalesReturnDetails.DiscountPercent, SalesReturnDetails.UnitPrice, -SalesReturnDetails.ListedAmount, -SalesReturnDetails.ListedGrossAmount, -SalesReturnDetails.Amount, ROUND(-SalesReturnDetails.ListedAmount + SalesReturnDetails.Amount, " + (int)GlobalEnums.rndAmount + ") AS DiscountAmount, -SalesReturns.TotalQuantity, -SalesReturns.TotalAmount, -SalesReturns.TradeDiscountAmount, -SalesReturns.TotalTaxableAmount, -SalesReturns.TotalVATAmount, -SalesReturns.SumListedGrossAmount, -SalesReturns.TotalGrossAmount " + "\r\n";

            queryString = queryString + "       FROM        SalesReturns " + "\r\n";
            queryString = queryString + "                   INNER JOIN SalesReturnDetails ON SalesReturns.EntryDate >= @LocalFromDate AND SalesReturns.EntryDate <= @LocalToDate AND SalesReturns.SalesReturnID = SalesReturnDetails.SalesReturnID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON SalesReturns.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts AS Receivers ON SalesReturns.ReceiverID = Receivers.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN WorkshiftCategories ON Workshifts.WorkshiftCategoryID = WorkshiftCategories.WorkshiftCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON SalesReturnDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Employees ON SalesReturns.SalespersonID = Employees.EmployeeID " + "\r\n";


            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("GoodsIssueJournal", queryString);
        }


        private void AccountInvoiceJournal()
        {
            string queryString;

            queryString = " @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime         SET @LocalFromDate = @FromDate" + "\r\n";
            queryString = queryString + "       DECLARE     @LocalToDate DateTime           SET @LocalToDate = @ToDate" + "\r\n";

            queryString = queryString + "       SELECT      AccountInvoices.AccountInvoiceID, 'Accounts/AccountInvoices' AS TaskAction, AccountInvoices.EntryDate, AccountInvoices.Reference, AccountInvoices.VATInvoiceNo, AccountInvoices.VATInvoiceDate, Workshifts.WorkshiftID, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, Workshifts.OfficialName AS WorkshiftOfficialName, Workshifts.VATCode, WorkshiftCategories.Name AS WorkshiftCategoryName, Workshifts.BillingAddress, AccountInvoices.Description, " + "\r\n";
            queryString = queryString + "                   AccountInvoiceDetails.AccountInvoiceDetailID, AccountInvoiceDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.OfficialName, Commodities.Weight AS UnitWeight, " + "\r\n";
            queryString = queryString + "                   AccountInvoiceDetails.Quantity, AccountInvoiceDetails.FreeQuantity, AccountInvoiceDetails.ListedPrice, AccountInvoiceDetails.DiscountPercent, AccountInvoiceDetails.UnitPrice, AccountInvoiceDetails.ListedAmount, AccountInvoiceDetails.Amount, ROUND(AccountInvoiceDetails.ListedAmount - AccountInvoiceDetails.Amount, " + (int)GlobalEnums.rndAmount + ") AS DiscountAmount, AccountInvoices.TotalQuantity, AccountInvoices.TotalAmount, AccountInvoices.TradeDiscountAmount, AccountInvoices.TotalTaxableAmount, AccountInvoices.TotalVATAmount, AccountInvoices.TotalGrossAmount " + "\r\n";

            queryString = queryString + "       FROM        AccountInvoices " + "\r\n";
            queryString = queryString + "                   INNER JOIN AccountInvoiceDetails ON AccountInvoices.VATInvoiceDate >= @LocalFromDate AND AccountInvoices.VATInvoiceDate <= @LocalToDate AND AccountInvoices.AccountInvoiceID = AccountInvoiceDetails.AccountInvoiceID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON AccountInvoices.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN WorkshiftCategories ON Workshifts.WorkshiftCategoryID = WorkshiftCategories.WorkshiftCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON AccountInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("AccountInvoiceJournal", queryString);
        }



        private void GoodsIssueBalance()
        {
            string queryString = " (@ToDate DateTime, @WorkshiftCategoryID int, @WorkshiftID int) " + "\r\n";
            queryString = queryString + " RETURNS @GoodsIssueBalances TABLE (NmvnTaskID int NOT NULL, TaskAction varchar(60) NOT NULL, EntryID int NOT NULL, EntryDate datetime NOT NULL, Reference nvarchar(10) NULL, WorkshiftID int NOT NULL, WorkshiftCode nvarchar(50) NOT NULL, WorkshiftName nvarchar(100) NOT NULL, BillingAddress nvarchar(200) NOT NULL, WorkshiftCategoryID int NOT NULL, WorkshiftCategoryName nvarchar(100) NOT NULL, TradePromotionID int NULL, EntryAmount decimal(18, 2) NOT NULL, BalanceAmount decimal(18, 2) NOT NULL) " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalToDate DateTime, @LocalWorkshiftCategoryID int, @LocalWorkshiftID int         SET @LocalToDate = @ToDate          SET @LocalWorkshiftCategoryID = @WorkshiftCategoryID            SET @LocalWorkshiftID = @WorkshiftID " + "\r\n";


            queryString = queryString + "       IF (@LocalWorkshiftID > 0) ";
            queryString = queryString + "           " + this.GoodsIssueBalanceSQLA(true);
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           " + this.GoodsIssueBalanceSQLA(false);


            queryString = queryString + "       IF (@LocalWorkshiftCategoryID > 0 AND @LocalWorkshiftID <= 0) ";
            queryString = queryString + "           " + this.GoodsIssueBalanceSQLB(true);
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           " + this.GoodsIssueBalanceSQLB(false);

            queryString = queryString + "       RETURN " + "\r\n";

            queryString = queryString + "   END " + "\r\n";


            this.totalSmartPortalEntities.CreateUserDefinedFunction("GoodsIssueBalance", queryString);
        }


        private string GoodsIssueBalanceSQLA(bool isWorkshiftID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       INSERT INTO @GoodsIssueBalances (NmvnTaskID, TaskAction, EntryID, EntryDate, Reference, WorkshiftID, WorkshiftCode, WorkshiftName, BillingAddress, WorkshiftCategoryID, WorkshiftCategoryName, TradePromotionID, EntryAmount, BalanceAmount) " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.GoodsIssue + " AS NmvnTaskID, 'Inventories/GoodsIssues' AS TaskAction, GoodsIssues.GoodsIssueID, GoodsIssues.EntryDate, GoodsIssues.Reference, GoodsIssues.WorkshiftID, '' AS WorkshiftCode, '' AS WorkshiftName, '' AS BillingAddress, 0 AS WorkshiftCategoryID, '' AS WorkshiftCategoryName, GoodsIssues.TradePromotionID, GoodsIssues.TotalGrossAmount, GoodsIssueBalances.BalanceAmount " + "\r\n";
            queryString = queryString + "       FROM       (SELECT     GoodsIssueID, SUM(BalanceAmount) AS BalanceAmount " + "\r\n";
            queryString = queryString + "                   FROM        (" + "\r\n";
            queryString = queryString + "                               SELECT      GoodsIssues.GoodsIssueID, ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount - GoodsIssues.TotalCashDiscount - GoodsIssues.TotalOtherDiscount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        GoodsIssues " + "\r\n"; //Raw GoodsIssues Balance
            queryString = queryString + "                               WHERE       " + (isWorkshiftID ? "WorkshiftID = @LocalWorkshiftID AND" : "") + " EntryDate <= @LocalToDate AND ROUND(GoodsIssues.TotalGrossAmount - GoodsIssues.TotalReceiptAmount - GoodsIssues.TotalCashDiscount - GoodsIssues.TotalOtherDiscount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";

            queryString = queryString + "                               UNION ALL " + "\r\n"; //UNDO Receipts

            queryString = queryString + "                               SELECT      GoodsIssues.GoodsIssueID, ReceiptDetails.ReceiptAmount + ReceiptDetails.CashDiscount + ReceiptDetails.OtherDiscount AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        GoodsIssues INNER JOIN ReceiptDetails ON " + (isWorkshiftID ? "GoodsIssues.WorkshiftID = @LocalWorkshiftID AND" : "") + " GoodsIssues.EntryDate <= @LocalToDate AND ReceiptDetails.EntryDate > @LocalToDate AND GoodsIssues.GoodsIssueID = ReceiptDetails.GoodsIssueID" + "\r\n";
            queryString = queryString + "                               ) AS GoodsIssueUnions " + "\r\n";
            queryString = queryString + "                   GROUP BY GoodsIssueID) GoodsIssueBalances " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssues ON GoodsIssueBalances.GoodsIssueID = GoodsIssues.GoodsIssueID " + "\r\n";




            queryString = queryString + "       INSERT INTO @GoodsIssueBalances (NmvnTaskID, TaskAction, EntryID, EntryDate, Reference, WorkshiftID, WorkshiftCode, WorkshiftName, BillingAddress, WorkshiftCategoryID, WorkshiftCategoryName, TradePromotionID, EntryAmount, BalanceAmount) " + "\r\n";

            queryString = queryString + "       SELECT      NmvnTaskID, MAX(TaskAction), EntryID, MAX(EntryDate), MAX(Reference), MAX(WorkshiftID), '' AS WorkshiftCode, '' AS WorkshiftName, '' AS BillingAddress, 0 AS WorkshiftCategoryID, '' AS WorkshiftCategoryName, NULL AS TradePromotionID, MAX(TotalCreditAmount), SUM(BalanceAmount)" + "\r\n";

            queryString = queryString + "       FROM       (SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, 'Accounts/Receipts' AS TaskAction, ReceiptID AS EntryID, Reference, EntryDate, WorkshiftID, TotalDepositAmount AS TotalCreditAmount, -ROUND(TotalDepositAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        Receipts " + "\r\n";
            queryString = queryString + "                               WHERE       " + (isWorkshiftID ? "WorkshiftID = @LocalWorkshiftID AND" : "") + " EntryDate <= @LocalToDate AND ReceiptTypeID = " + GlobalReceiptTypeID.ReceiveMoney + " AND ROUND(TotalDepositAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "                               UNION ALL   " + "\r\n";
            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, 'Accounts/Receipts' AS TaskAction, AdvanceReceipts.ReceiptID AS EntryID, AdvanceReceipts.Reference, AdvanceReceipts.EntryDate, AdvanceReceipts.WorkshiftID, AdvanceReceipts.TotalDepositAmount AS TotalCreditAmount, -ROUND(Receipts.TotalReceiptAmount + Receipts.TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        Receipts AS AdvanceReceipts INNER JOIN Receipts ON " + (isWorkshiftID ? "AdvanceReceipts.WorkshiftID = @LocalWorkshiftID AND" : "") + " AdvanceReceipts.EntryDate <= @LocalToDate AND Receipts.EntryDate > @LocalToDate AND AdvanceReceipts.ReceiptID = Receipts.AdvanceReceiptID " + "\r\n";


            queryString = queryString + "                               UNION ALL   " + "\r\n";

            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.SalesReturn + " AS NmvnTaskID, 'Sales/SalesReturns' AS TaskAction, SalesReturnID AS EntryID, Reference, EntryDate, WorkshiftID, TotalGrossAmount AS TotalCreditAmount, -ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        SalesReturns " + "\r\n";
            queryString = queryString + "                               WHERE       " + (isWorkshiftID ? "WorkshiftID = @LocalWorkshiftID AND" : "") + " EntryDate <= @LocalToDate AND ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "                               UNION ALL   " + "\r\n";
            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.SalesReturn + " AS NmvnTaskID, 'Sales/SalesReturns' AS TaskAction, SalesReturns.SalesReturnID AS EntryID, SalesReturns.Reference, SalesReturns.EntryDate, SalesReturns.WorkshiftID, SalesReturns.TotalGrossAmount AS TotalCreditAmount, -ROUND(Receipts.TotalReceiptAmount + Receipts.TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        SalesReturns INNER JOIN Receipts ON " + (isWorkshiftID ? "SalesReturns.WorkshiftID = @LocalWorkshiftID AND" : "") + " SalesReturns.EntryDate <= @LocalToDate AND Receipts.EntryDate > @LocalToDate AND SalesReturns.SalesReturnID = Receipts.SalesReturnID " + "\r\n";


            queryString = queryString + "                               UNION ALL   " + "\r\n";

            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.CreditNote + " AS NmvnTaskID, 'Accounts/CreditNotes' AS TaskAction, CreditNoteID AS EntryID, Reference, EntryDate, WorkshiftID, TotalGrossAmount AS TotalCreditAmount, -ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        CreditNotes " + "\r\n";
            queryString = queryString + "                               WHERE       " + (isWorkshiftID ? "WorkshiftID = @LocalWorkshiftID AND" : "") + " EntryDate <= @LocalToDate AND ROUND(TotalGrossAmount - TotalReceiptAmount - TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") > 0 " + "\r\n";
            queryString = queryString + "                               UNION ALL   " + "\r\n";
            queryString = queryString + "                               SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, 'Accounts/CreditNotes' AS TaskAction, CreditNotes.CreditNoteID AS EntryID, CreditNotes.Reference, CreditNotes.EntryDate, CreditNotes.WorkshiftID, CreditNotes.TotalGrossAmount AS TotalCreditAmount, -ROUND(Receipts.TotalReceiptAmount + Receipts.TotalFluctuationAmount, " + (int)GlobalEnums.rndAmount + ") AS BalanceAmount " + "\r\n";
            queryString = queryString + "                               FROM        CreditNotes INNER JOIN Receipts ON " + (isWorkshiftID ? "CreditNotes.WorkshiftID = @LocalWorkshiftID AND" : "") + " CreditNotes.EntryDate <= @LocalToDate AND Receipts.EntryDate > @LocalToDate AND CreditNotes.CreditNoteID = Receipts.CreditNoteID " + "\r\n";
            queryString = queryString + "                   ) AS WorkshiftCreditCollections " + "\r\n";
            queryString = queryString + "       GROUP BY    NmvnTaskID, EntryID " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }


        private string GoodsIssueBalanceSQLB(bool isWorkshiftCategoryID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       UPDATE      Balances " + "\r\n";
            queryString = queryString + "       SET         Balances.WorkshiftCode = Workshifts.Code, Balances.WorkshiftName = Workshifts.Name, Balances.BillingAddress = Workshifts.BillingAddress, Balances.WorkshiftCategoryID = Workshifts.WorkshiftCategoryID, Balances.WorkshiftCategoryName = WorkshiftCategories.Name " + "\r\n";
            queryString = queryString + "       FROM        @GoodsIssueBalances Balances " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON " + (isWorkshiftCategoryID ? " Workshifts.WorkshiftCategoryID = @LocalWorkshiftCategoryID AND" : "") + " Balances.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN WorkshiftCategories ON Workshifts.WorkshiftCategoryID = WorkshiftCategories.WorkshiftCategoryID " + "\r\n";

            queryString = queryString + "       " + (isWorkshiftCategoryID ? " DELETE FROM @GoodsIssueBalances WHERE WorkshiftCode = '' " : "") + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }



        private void SalesJournal()
        {
            string queryString;

            queryString = " @FromDate DateTime, @ToDate DateTime, @WorkshiftCategoryID int, @WorkshiftID int " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime, @LocalToDate DateTime, @LocalWorkshiftCategoryID int, @LocalWorkshiftID int         SET @LocalFromDate = @FromDate         SET @LocalToDate = @ToDate          SET @LocalWorkshiftCategoryID = @WorkshiftCategoryID            SET @LocalWorkshiftID = @WorkshiftID " + "\r\n";

            queryString = queryString + "       IF (@LocalWorkshiftID > 0) ";
            queryString = queryString + "           " + this.SalesJournalSQL(false, true);
            queryString = queryString + "       ELSE ";
            queryString = queryString + "           IF (@LocalWorkshiftCategoryID > 0) ";
            queryString = queryString + "               " + this.SalesJournalSQL(true, false);
            queryString = queryString + "           ELSE ";
            queryString = queryString + "               " + this.SalesJournalSQL(false, false);

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SalesJournal", queryString);
        }

        private string SalesJournalSQL(bool isWorkshiftCategoryID, bool isWorkshiftID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";


            queryString = queryString + "       SELECT      NmvnTaskID, TaskAction, WorkshiftID, WorkshiftCode, WorkshiftName, BillingAddress, WorkshiftCategoryID, WorkshiftCategoryName, " + "\r\n";
            queryString = queryString + "                   EntryID, EntryDate, Reference, TradePromotionID, BalanceAmount AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalReturnDiscountAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalOtherDiscount, 0 AS TotalFluctuationAmount, BalanceAmount AS EndingAmount " + "\r\n";
            queryString = queryString + "       FROM        dbo.GoodsIssueBalance(DATEADD(second, -1, @LocalFromDate), @LocalWorkshiftCategoryID, @LocalWorkshiftID) " + "\r\n";

            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.GoodsIssue + " AS NmvnTaskID, 'Inventories/GoodsIssues' AS TaskAction, Workshifts.WorkshiftID, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, Workshifts.BillingAddress, Workshifts.WorkshiftCategoryID, WorkshiftCategories.Name AS WorkshiftCategoryName, " + "\r\n";
            queryString = queryString + "                   GoodsIssues.GoodsIssueID AS EntryID, GoodsIssues.EntryDate, GoodsIssues.Reference, GoodsIssues.TradePromotionID, 0 AS BeginningAmount, GoodsIssues.SumListedGrossAmount, GoodsIssues.TotalGrossAmount, ROUND(GoodsIssues.SumListedGrossAmount - GoodsIssues.TotalGrossAmount, " + (int)GlobalEnums.rndAmount + ") AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalReturnDiscountAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalOtherDiscount, 0 AS TotalFluctuationAmount, GoodsIssues.TotalGrossAmount AS EndingAmount " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON " + (isWorkshiftID ? "GoodsIssues.WorkshiftID = @LocalWorkshiftID AND" : "") + (isWorkshiftCategoryID ? " Workshifts.WorkshiftCategoryID = @LocalWorkshiftCategoryID AND" : "") + " GoodsIssues.EntryDate >= @LocalFromDate AND GoodsIssues.EntryDate <= @LocalToDate AND GoodsIssues.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN WorkshiftCategories ON Workshifts.WorkshiftCategoryID = WorkshiftCategories.WorkshiftCategoryID " + "\r\n";

            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.SalesReturn + " AS NmvnTaskID, 'Sales/SalesReturns' AS TaskAction, Workshifts.WorkshiftID, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, Workshifts.BillingAddress, Workshifts.WorkshiftCategoryID, WorkshiftCategories.Name AS WorkshiftCategoryName, " + "\r\n";
            queryString = queryString + "                   SalesReturns.SalesReturnID AS EntryID, SalesReturns.EntryDate, SalesReturns.Reference, SalesReturns.TradePromotionID, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, SalesReturns.SumListedGrossAmount AS TotalReturnAmount, ROUND(SalesReturns.SumListedGrossAmount - SalesReturns.TotalGrossAmount, " + (int)GlobalEnums.rndAmount + ") AS TotalReturnDiscountAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalOtherDiscount, 0 AS TotalFluctuationAmount, -SalesReturns.TotalGrossAmount AS EndingAmount " + "\r\n";

            queryString = queryString + "       FROM        SalesReturns " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON " + (isWorkshiftID ? "SalesReturns.WorkshiftID = @LocalWorkshiftID AND" : "") + (isWorkshiftCategoryID ? " Workshifts.WorkshiftCategoryID = @LocalWorkshiftCategoryID AND" : "") + " SalesReturns.EntryDate >= @LocalFromDate AND SalesReturns.EntryDate <= @LocalToDate AND SalesReturns.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN WorkshiftCategories ON Workshifts.WorkshiftCategoryID = WorkshiftCategories.WorkshiftCategoryID " + "\r\n";



            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.CreditNote + " AS NmvnTaskID, 'Accounts/CreditNotes' AS TaskAction, Workshifts.WorkshiftID, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, Workshifts.BillingAddress, Workshifts.WorkshiftCategoryID, WorkshiftCategories.Name AS WorkshiftCategoryName, " + "\r\n";
            queryString = queryString + "                   CreditNotes.CreditNoteID AS EntryID, CreditNotes.EntryDate, CreditNotes.Reference, NULL AS TradePromotionID, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalReturnDiscountAmount, CreditNotes.TotalGrossAmount AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalOtherDiscount, 0 AS TotalFluctuationAmount, -CreditNotes.TotalGrossAmount AS EndingAmount " + "\r\n";

            queryString = queryString + "       FROM        CreditNotes " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON " + (isWorkshiftID ? "CreditNotes.WorkshiftID = @LocalWorkshiftID AND" : "") + (isWorkshiftCategoryID ? " Workshifts.WorkshiftCategoryID = @LocalWorkshiftCategoryID AND" : "") + " CreditNotes.EntryDate >= @LocalFromDate AND CreditNotes.EntryDate <= @LocalToDate AND CreditNotes.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN WorkshiftCategories ON Workshifts.WorkshiftCategoryID = WorkshiftCategories.WorkshiftCategoryID " + "\r\n";



            queryString = queryString + "       UNION ALL " + "\r\n";

            queryString = queryString + "       SELECT      " + (int)GlobalEnums.NmvnTaskID.Receipt + " AS NmvnTaskID, 'Accounts/Receipts' AS TaskAction, Workshifts.WorkshiftID, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, Workshifts.BillingAddress, Workshifts.WorkshiftCategoryID, WorkshiftCategories.Name AS WorkshiftCategoryName, " + "\r\n";
            queryString = queryString + "                   Receipts.ReceiptID AS EntryID, Receipts.EntryDate, Receipts.Reference, NULL AS TradePromotionID, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalReturnDiscountAmount, 0 AS TotalCreditAmount, CASE WHEN Receipts.ReceiptTypeID = " + GlobalReceiptTypeID.ReceiveMoney + " AND Receipts.MonetaryAccountID = 3 THEN Receipts.TotalDepositAmount ELSE 0 END AS TotalCashReceiptAmount, CASE WHEN Receipts.ReceiptTypeID = " + GlobalReceiptTypeID.ReceiveMoney + " AND Receipts.MonetaryAccountID <> 3 THEN Receipts.TotalDepositAmount ELSE 0 END TotalBankTransferAmount, Receipts.TotalCashDiscount, Receipts.TotalOtherDiscount, Receipts.TotalFluctuationAmount, (CASE WHEN Receipts.ReceiptTypeID = " + GlobalReceiptTypeID.ReceiveMoney + " THEN -Receipts.TotalDepositAmount ELSE 0 END) - Receipts.TotalCashDiscount - Receipts.TotalOtherDiscount + Receipts.TotalFluctuationAmount AS EndingAmount " + "\r\n";

            queryString = queryString + "       FROM        Receipts " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON " + (isWorkshiftID ? "Receipts.WorkshiftID = @LocalWorkshiftID AND" : "") + (isWorkshiftCategoryID ? " Workshifts.WorkshiftCategoryID = @LocalWorkshiftCategoryID AND" : "") + " Receipts.EntryDate >= @LocalFromDate AND Receipts.EntryDate <= @LocalToDate AND Receipts.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN WorkshiftCategories ON Workshifts.WorkshiftCategoryID = WorkshiftCategories.WorkshiftCategoryID " + "\r\n";


            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }


        public void StatementOfAccount()
        {
            string queryString;

            queryString = " @FromDate DateTime, @ToDate DateTime, @WorkshiftCategoryID int, @WorkshiftID int " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime, @LocalToDate DateTime, @LocalWorkshiftCategoryID int, @LocalWorkshiftID int         SET @LocalFromDate = @FromDate         SET @LocalToDate = @ToDate          SET @LocalWorkshiftCategoryID = @WorkshiftCategoryID            SET @LocalWorkshiftID = @WorkshiftID " + "\r\n";

            queryString = queryString + "       DECLARE         @StatementTable TABLE (NmvnTaskID int NULL, TaskAction nvarchar(50) NULL, WorkshiftID int NOT NULL, WorkshiftCode nvarchar(50) NOT NULL, WorkshiftName nvarchar(100) NOT NULL, BillingAddress nvarchar(200) NOT NULL, WorkshiftCategoryID int NOT NULL, WorkshiftCategoryName nvarchar(100) NOT NULL, EntryID int NULL, EntryDate datetime NULL, GoodsIssueID int NULL, GoodsIssueReference nvarchar(30) NULL, TradePromotionID int NULL, CountGoodsIssueID int, GoodsIssueListedGrossAmount float NOT NULL, GoodsIssueGrossAmount float NOT NULL, GoodsIssueDiscountAmount float NOT NULL, AccountInvoiceID int NULL, AccountInvoiceReference nvarchar(30) NULL, VATInvoiceNo nvarchar(30) NULL, GoodsIssueReferences nvarchar(200) NULL, SerialID int NULL, AccountInvoiceListedGrossAmount float NOT NULL, AccountInvoiceGrossAmount float NOT NULL, AccountInvoiceDiscountAmount float NOT NULL, BeginningAmount float NOT NULL, SumListedGrossAmount float NOT NULL, TotalGrossAmount float NOT NULL, TotalDiscountAmount float NOT NULL, TotalReturnAmount float NOT NULL, TotalReturnDiscountAmount float NOT NULL, TotalCreditAmount float NOT NULL, TotalCashReceiptAmount float NOT NULL, TotalBankTransferAmount float NOT NULL, TotalCashDiscount float NOT NULL, TotalOtherDiscount float NOT NULL, TotalFluctuationAmount float NOT NULL, EndingAmount float NOT NULL, EndingAmountInWords nvarchar(100) NULL) " + "\r\n";
            queryString = queryString + "       DECLARE         @SalesJournalTable TABLE (NmvnTaskID int NULL, TaskAction nvarchar(50) NULL, WorkshiftID int NOT NULL, WorkshiftCode nvarchar(50) NOT NULL, WorkshiftName nvarchar(100) NOT NULL, BillingAddress nvarchar(200) NOT NULL, WorkshiftCategoryID int NOT NULL, WorkshiftCategoryName nvarchar(100) NOT NULL, EntryID int NULL, EntryDate datetime NOT NULL, Reference nvarchar(30) NULL, TradePromotionID int NULL, BeginningAmount float NOT NULL, SumListedGrossAmount float NOT NULL, TotalGrossAmount float NOT NULL, TotalDiscountAmount float NOT NULL, TotalReturnAmount float NOT NULL, TotalReturnDiscountAmount float NOT NULL, TotalCreditAmount float NOT NULL, TotalCashReceiptAmount float NOT NULL, TotalBankTransferAmount float NOT NULL, TotalCashDiscount float NOT NULL, TotalOtherDiscount float NOT NULL, TotalFluctuationAmount float NOT NULL, EndingAmount float NOT NULL) " + "\r\n";

            queryString = queryString + "       IF (@LocalWorkshiftID > 0) " + "\r\n";
            queryString = queryString + "           " + this.StatementOfAccountSQL(false, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           IF (@LocalWorkshiftCategoryID > 0) " + "\r\n";
            queryString = queryString + "               " + this.StatementOfAccountSQL(true, false) + "\r\n";
            queryString = queryString + "           ELSE " + "\r\n";
            queryString = queryString + "               " + this.StatementOfAccountSQL(false, false) + "\r\n";


            queryString = queryString + "       SELECT * FROM  @StatementTable " + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            System.Diagnostics.Debug.WriteLine("--------");
            System.Diagnostics.Debug.WriteLine(queryString);
            this.totalSmartPortalEntities.CreateStoredProcedure("StatementOfAccount", queryString);




            //DECLARE @FromDate DateTime, @ToDate DateTime, @WorkshiftCategoryID int, @WorkshiftID int 

            //SET @FromDate = CONVERT(DATETIME, '2017-05-01 00:00:00', 102)
            //SET @ToDate = CONVERT(DATETIME, '2017-05-31 00:00:00', 102)
            //SET @WorkshiftCategoryID = 0
            //SET @WorkshiftID = 0


            //EXEC StatementOfAccount @FromDate, @ToDate,  0, 0


        }


        private string StatementOfAccountSQL(bool isWorkshiftCategoryID, bool isWorkshiftID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       INSERT INTO     @SalesJournalTable EXEC SalesJournal @LocalFromDate, @LocalToDate, @LocalWorkshiftCategoryID, @LocalWorkshiftID " + "\r\n";


            queryString = queryString + "       INSERT INTO     @StatementTable (NmvnTaskID, TaskAction, WorkshiftID, WorkshiftCode, WorkshiftName, BillingAddress, WorkshiftCategoryID, WorkshiftCategoryName, EntryID, EntryDate, GoodsIssueID, GoodsIssueReference, TradePromotionID, CountGoodsIssueID, GoodsIssueListedGrossAmount, GoodsIssueGrossAmount, GoodsIssueDiscountAmount, AccountInvoiceID, AccountInvoiceReference, VATInvoiceNo, GoodsIssueReferences, SerialID, AccountInvoiceListedGrossAmount, AccountInvoiceGrossAmount, AccountInvoiceDiscountAmount, BeginningAmount, SumListedGrossAmount, TotalGrossAmount, TotalDiscountAmount, TotalReturnAmount, TotalReturnDiscountAmount, TotalCreditAmount, TotalCashReceiptAmount, TotalBankTransferAmount, TotalCashDiscount, TotalOtherDiscount, TotalFluctuationAmount, EndingAmount, EndingAmountInWords) " + "\r\n";
            queryString = queryString + "       SELECT          " + (int)GlobalEnums.NmvnTaskID.AccountInvoice + " AS NmvnTaskID, 'Accounts/AccountInvoices' AS TaskAction, Workshifts.WorkshiftID, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, Workshifts.BillingAddress, Workshifts.WorkshiftCategoryID, WorkshiftCategories.Name AS WorkshiftCategoryName, AccountInvoices.AccountInvoiceID AS EntryID, CAST(AccountInvoices.VATInvoiceDate AS Date) AS EntryDate, AccountInvoices.GoodsIssueFirstID AS GoodsIssueID, NULL AS GoodsIssueReference, NULL AS TradePromotionID, 0 AS CountGoodsIssueID, 0 AS GoodsIssueListedGrossAmount, 0 AS GoodsIssueGrossAmount, 0 AS GoodsIssueDiscountAmount, AccountInvoices.AccountInvoiceID, AccountInvoices.Reference AS AccountInvoiceReference, AccountInvoices.VATInvoiceNo, AccountInvoices.GoodsIssueReferences, 0 AS SerialID, AccountInvoices.SumListedGrossAmount AS AccountInvoiceListedGrossAmount, AccountInvoices.TotalGrossAmount AS AccountInvoiceGrossAmount, ROUND(AccountInvoices.SumListedGrossAmount - AccountInvoices.TotalGrossAmount, " + (int)GlobalEnums.rndAmount + ") AS AccountInvoiceDiscountAmount, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalReturnDiscountAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalOtherDiscount, 0 AS TotalFluctuationAmount, 0 AS EndingAmount, NULL AS EndingAmountInWords " + "\r\n";
            queryString = queryString + "       FROM            AccountInvoices " + "\r\n";
            queryString = queryString + "                       INNER JOIN Workshifts ON " + (isWorkshiftID ? "AccountInvoices.ConsumerID = @LocalWorkshiftID AND" : "") + (isWorkshiftCategoryID ? " Workshifts.WorkshiftCategoryID = @LocalWorkshiftCategoryID AND" : "") + " AccountInvoices.VATInvoiceDate >= @LocalFromDate AND AccountInvoices.VATInvoiceDate <= @LocalToDate AND AccountInvoices.ConsumerID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                       INNER JOIN WorkshiftCategories ON Workshifts.WorkshiftCategoryID = WorkshiftCategories.WorkshiftCategoryID " + "\r\n";

            queryString = queryString + "       UPDATE          StatementTable " + "\r\n"; //THE PURPOSE OF THIS IS: Set SerialID = SomeDistinctValue (Here we use AccountInvoiceID as DistinctValue) FOR THESE AccountInvoices WHICH ARE: THERE ARE MORE THAN 1 AccountInvoice WITH THE SAME GoodsIssueFirstID AT THE SAME DATE (WE USE CONDITION: AccountInvoiceID <> MINAccountInvoiceID TO SET THE SerialID OF THE SECOND, THE THIRDTH, THE FOURTH ... TO AccountInvoiceID, KEEP THE SerialID = 0 FOR ROW WHICH IS AccountInvoiceID = MINAccountInvoiceID)
            queryString = queryString + "       SET             StatementTable.SerialID = StatementTable.AccountInvoiceID " + "\r\n"; //NOTE 1: DEFAULT OF SerialID IS 0, UNLESS THERE ARE MORE THAN 1 AccountInvoice WITH THE SAME GoodsIssueFirstID AT THE SAME DATE
            queryString = queryString + "       FROM            @StatementTable StatementTable " + "\r\n";//NOTE 2: SerialID WILL BE ONLY USED IN REPORT FOR GROUP BY ENTRYDATE
            queryString = queryString + "                       INNER JOIN (SELECT          WorkshiftID, EntryDate, GoodsIssueID, MIN(AccountInvoiceID) AS MINAccountInvoiceID " + "\r\n";
            queryString = queryString + "                                   FROM            @StatementTable StatementTable " + "\r\n"; //NOTE 3: WE SHOULD RUN THIS UPDATE RIGHT AFTER WE INSERT AccountInvoices TO THE StatementTable.
            queryString = queryString + "                                   GROUP BY        WorkshiftID, EntryDate, GoodsIssueID " + "\r\n"; //NOTE 4: VERY IMPORTANT: THIS SHOULD DO WHEN THERE IS NO INSERT TO StatementTable BEFORE. OTHERWISE, WE SHOULD USE THE WHERE CLAUSE TO UPDATE THE AccountInvoice ROWS ONLY
            queryString = queryString + "                                   HAVING          COUNT(*) > 1) AS MoreInvoicePerDatePerIssue ON StatementTable.WorkshiftID = MoreInvoicePerDatePerIssue.WorkshiftID AND StatementTable.EntryDate = MoreInvoicePerDatePerIssue.EntryDate AND StatementTable.GoodsIssueID = MoreInvoicePerDatePerIssue.GoodsIssueID AND StatementTable.AccountInvoiceID <> MoreInvoicePerDatePerIssue.MINAccountInvoiceID " + "\r\n";


            queryString = queryString + "       INSERT INTO     @StatementTable (NmvnTaskID, TaskAction, WorkshiftID, WorkshiftCode, WorkshiftName, BillingAddress, WorkshiftCategoryID, WorkshiftCategoryName, EntryID, EntryDate, GoodsIssueID, GoodsIssueReference, TradePromotionID, CountGoodsIssueID, GoodsIssueListedGrossAmount, GoodsIssueGrossAmount, GoodsIssueDiscountAmount, AccountInvoiceID, AccountInvoiceReference, VATInvoiceNo, GoodsIssueReferences, SerialID, AccountInvoiceListedGrossAmount, AccountInvoiceGrossAmount, AccountInvoiceDiscountAmount, BeginningAmount, SumListedGrossAmount, TotalGrossAmount, TotalDiscountAmount, TotalReturnAmount, TotalReturnDiscountAmount, TotalCreditAmount, TotalCashReceiptAmount, TotalBankTransferAmount, TotalCashDiscount, TotalOtherDiscount, TotalFluctuationAmount, EndingAmount, EndingAmountInWords) " + "\r\n";
            queryString = queryString + "       SELECT          NmvnTaskID, TaskAction, WorkshiftID, WorkshiftCode, WorkshiftName, BillingAddress, WorkshiftCategoryID, WorkshiftCategoryName, EntryID, CAST(EntryDate AS Date) AS EntryDate, EntryID AS GoodsIssueID, Reference, TradePromotionID, 1 AS CountGoodsIssueID, SumListedGrossAmount AS GoodsIssueListedGrossAmount, TotalGrossAmount AS GoodsIssueGrossAmount, TotalDiscountAmount AS GoodsIssueDiscountAmount, NULL AS AccountInvoiceID, NULL AS AccountInvoiceReference, NULL AS VATInvoiceNo, NULL AS GoodsIssueReferences, 0 AS SerialID, 0 AS AccountInvoiceListedGrossAmount, 0 AS AccountInvoiceGrossAmount, 0 AS AccountInvoiceDiscountAmount, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalReturnDiscountAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalOtherDiscount, 0 AS TotalFluctuationAmount, 0 AS EndingAmount, NULL AS EndingAmountInWords " + "\r\n";
            queryString = queryString + "       FROM            @SalesJournalTable " + "\r\n";
            queryString = queryString + "       WHERE           NmvnTaskID = " + (int)GlobalEnums.NmvnTaskID.GoodsIssue + " AND EntryDate >= @LocalFromDate AND EntryDate <= @LocalToDate " + "\r\n";

            queryString = queryString + "       INSERT INTO     @StatementTable (NmvnTaskID, TaskAction, WorkshiftID, WorkshiftCode, WorkshiftName, BillingAddress, WorkshiftCategoryID, WorkshiftCategoryName, EntryID, EntryDate, GoodsIssueID, GoodsIssueReference, TradePromotionID, CountGoodsIssueID, GoodsIssueListedGrossAmount, GoodsIssueGrossAmount, GoodsIssueDiscountAmount, AccountInvoiceID, AccountInvoiceReference, VATInvoiceNo, GoodsIssueReferences, SerialID, AccountInvoiceListedGrossAmount, AccountInvoiceGrossAmount, AccountInvoiceDiscountAmount, BeginningAmount, SumListedGrossAmount, TotalGrossAmount, TotalDiscountAmount, TotalReturnAmount, TotalReturnDiscountAmount, TotalCreditAmount, TotalCashReceiptAmount, TotalBankTransferAmount, TotalCashDiscount, TotalOtherDiscount, TotalFluctuationAmount, EndingAmount, EndingAmountInWords) " + "\r\n";
            queryString = queryString + "       SELECT          DISTINCT NULL AS NmvnTaskID, NULL AS TaskAction, WorkshiftID, WorkshiftCode, WorkshiftName, BillingAddress, WorkshiftCategoryID, WorkshiftCategoryName, NULL AS EntryID, NULL AS EntryDate, NULL AS GoodsIssueID, NULL AS Reference, NULL AS TradePromotionID, 0 AS CountGoodsIssueID, 0 AS GoodsIssueListedGrossAmount, 0 AS GoodsIssueGrossAmount, 0 AS GoodsIssueDiscountAmount, NULL AS AccountInvoiceID, NULL AS AccountInvoiceReference, NULL AS VATInvoiceNo, NULL AS GoodsIssueReferences, NULL AS SerialID, 0 AS AccountInvoiceListedGrossAmount, 0 AS AccountInvoiceGrossAmount, 0 AS AccountInvoiceDiscountAmount, 0 AS BeginningAmount, 0 AS SumListedGrossAmount, 0 AS TotalGrossAmount, 0 AS TotalDiscountAmount, 0 AS TotalReturnAmount, 0 AS TotalReturnDiscountAmount, 0 AS TotalCreditAmount, 0 AS TotalCashReceiptAmount, 0 AS TotalBankTransferAmount, 0 AS TotalCashDiscount, 0 AS TotalOtherDiscount, 0 AS TotalFluctuationAmount, 0 AS EndingAmount, NULL AS EndingAmountInWords " + "\r\n";
            queryString = queryString + "       FROM            @SalesJournalTable " + "\r\n";
            queryString = queryString + "       WHERE           WorkshiftID NOT IN (SELECT WorkshiftID FROM @StatementTable) " + "\r\n";

            queryString = queryString + "       UPDATE          StatementTable " + "\r\n";
            queryString = queryString + "       SET             StatementTable.BeginningAmount = SalesJournalSummary.BeginningAmount, StatementTable.SumListedGrossAmount = SalesJournalSummary.SumListedGrossAmount, StatementTable.TotalGrossAmount = SalesJournalSummary.TotalGrossAmount, StatementTable.TotalDiscountAmount = SalesJournalSummary.TotalDiscountAmount, StatementTable.TotalReturnAmount = SalesJournalSummary.TotalReturnAmount, StatementTable.TotalReturnDiscountAmount = SalesJournalSummary.TotalReturnDiscountAmount, StatementTable.TotalCreditAmount = SalesJournalSummary.TotalCreditAmount, StatementTable.TotalCashReceiptAmount = SalesJournalSummary.TotalCashReceiptAmount, StatementTable.TotalBankTransferAmount = SalesJournalSummary.TotalBankTransferAmount, StatementTable.TotalCashDiscount = SalesJournalSummary.TotalCashDiscount, StatementTable.TotalOtherDiscount = SalesJournalSummary.TotalOtherDiscount, StatementTable.TotalFluctuationAmount = SalesJournalSummary.TotalFluctuationAmount, StatementTable.EndingAmount = SalesJournalSummary.EndingAmount, StatementTable.EndingAmountInWords = dbo.SayVND(SalesJournalSummary.EndingAmount) " + "\r\n";
            queryString = queryString + "       FROM            @StatementTable StatementTable " + "\r\n";
            queryString = queryString + "                       INNER JOIN (SELECT          WorkshiftID, SUM(BeginningAmount) AS BeginningAmount, SUM(SumListedGrossAmount) AS SumListedGrossAmount, SUM(TotalGrossAmount) AS TotalGrossAmount, SUM(TotalDiscountAmount) AS TotalDiscountAmount, SUM(TotalReturnAmount) AS TotalReturnAmount, SUM(TotalReturnDiscountAmount) AS TotalReturnDiscountAmount, SUM(TotalCreditAmount) AS TotalCreditAmount, SUM(TotalCashReceiptAmount) AS TotalCashReceiptAmount, SUM(TotalBankTransferAmount) AS TotalBankTransferAmount, SUM(TotalCashDiscount) AS TotalCashDiscount, SUM(TotalOtherDiscount) AS TotalOtherDiscount, SUM(TotalFluctuationAmount) AS TotalFluctuationAmount, SUM(EndingAmount) AS EndingAmount " + "\r\n";
            queryString = queryString + "                                   FROM            @SalesJournalTable " + "\r\n";
            queryString = queryString + "                                   GROUP BY        WorkshiftID) SalesJournalSummary ON StatementTable.WorkshiftID = SalesJournalSummary.WorkshiftID" + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        private void StatementOfWarehouses()
        {
            string queryString;

            queryString = " @WarehouseGroupIDList varchar(60), @PriceCategoryIDList varchar(60), @ToDate DateTime " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @LocalPriceCategoryIDList varchar(60)   SET @LocalPriceCategoryIDList = @PriceCategoryIDList " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalFromDate DateTime         SET @LocalFromDate = DATEADD(Month, DateDiff(Month, 0, @ToDate) - 12, 0)        IF (@LocalFromDate < CONVERT(DATETIME, '2018-04-01 00:00:00', 102)) SET @LocalFromDate = CONVERT(DATETIME, '2018-04-01 00:00:00', 102) " + "\r\n"; //BACKWARD 12 MONTHS
            queryString = queryString + "       DECLARE     @LocalToDate DateTime           SET @LocalToDate = @ToDate" + "\r\n";

            queryString = queryString + "       DECLARE     @InventoryFromDate DateTime     SET @InventoryFromDate = DATEADD(Month, DateDiff(Month, 0, @ToDate), 0) " + "\r\n"; //START OF MONTHS
            queryString = queryString + "       DECLARE     @DefaultWarehouseID int         SET @DefaultWarehouseID = ISNULL((SELECT TOP 1 WarehouseID FROM Warehouses WHERE WarehouseGroupID = CAST(@WarehouseGroupIDList AS int) AND IsDefault = 1), 0) " + "\r\n";


            queryString = queryString + "       DECLARE         @Commodities    TABLE (CommodityID int NOT NULL PRIMARY KEY, Code nvarchar(50) NOT NULL, CodePartA nvarchar(20) NOT NULL, CodePartB nvarchar(20) NOT NULL, CodePartC nvarchar(20) NOT NULL, Name nvarchar(200) NOT NULL, ListedPrice decimal(18, 2) NOT NULL, GrossPrice decimal(18, 2) NOT NULL, Weight float NULL)" + "\r\n";
            queryString = queryString + "       INSERT INTO     @Commodities    SELECT CommodityID, Code, CodePartA, CodePartB, CodePartC, Name, 0 AS ListedPrice, 0 AS GrossPrice, Weight FROM Commodities " + "\r\n";

            queryString = queryString + "       IF (CHARINDEX(',', @LocalPriceCategoryIDList) = 0) ";
            queryString = queryString + "       " + this.StatementOfWarehousesBUILDSQL("", "");
            queryString = queryString + "       ELSE ";
            queryString = queryString + "       " + this.StatementOfWarehousesBUILDSQL("(SELECT CodePartA, CodePartC, ROUND(AVG(ListedPrice), " + (int)GlobalEnums.rndAmount + ") AS ListedPrice, ROUND(AVG(GrossPrice), " + (int)GlobalEnums.rndAmount + ") AS GrossPrice FROM CommodityPrices WHERE PriceCategoryID IN (SELECT * FROM FNSplitUpIds(@LocalPriceCategoryIDList)) AND CodePartB IS NULL GROUP BY CodePartA, CodePartC) AS ", "(SELECT CodePartA, CodePartC, CodePartB, ROUND(AVG(ListedPrice), " + (int)GlobalEnums.rndAmount + ") AS ListedPrice, ROUND(AVG(GrossPrice), " + (int)GlobalEnums.rndAmount + ") AS GrossPrice FROM CommodityPrices WHERE PriceCategoryID IN (SELECT * FROM FNSplitUpIds(@LocalPriceCategoryIDList)) GROUP BY CodePartA, CodePartC, CodePartB) AS ");


            SqlProgrammability.Inventories.Inventories inventories = new SqlProgrammability.Inventories.Inventories(this.totalSmartPortalEntities);

            queryString = queryString + "       " + inventories.GET_WarehouseJournal_BUILD_SQL(null, "@InventoryFromDate", "@LocalToDate", "", "", "0", "0", (int)GlobalEnums.WarehouseClassID.L1 + "," + (int)GlobalEnums.WarehouseClassID.L5 + "," + (int)GlobalEnums.WarehouseClassID.LD, "@WarehouseGroupIDList") + "\r\n";
            queryString = queryString + "       " + inventories.GET_SPProductionOrderJournalTable("@LocalToDate", "@LocalToDate", (int)GlobalEnums.WarehouseClassID.L1 + "," + (int)GlobalEnums.WarehouseClassID.L5 + "," + (int)GlobalEnums.WarehouseClassID.LD) + "\r\n";
            //TAM THOI: (int)GlobalEnums.WarehouseGroupID.TT + ""  --- CHI LAY HANG THI TRUONGC


            queryString = queryString + "       DECLARE @WarehouseCollection TABLE (CommodityID int NULL, EntryMonth datetime NULL, QuantityGoodsIssue float NULL, QuantityBegin float NULL, QuantityInput float NULL, QuantityOutput float NULL, QuantityOnOutput float NULL, QuantityBalance float NULL, QuantityOnProduction float NULL, QuantityEndSemi float NULL, QuantityEndImprove float NULL, QuantityEndPack float NULL, QuantityEndRefine float NULL, QuantityEndRank float NULL, QuantityEndFinish float NULL) " + "\r\n";

            queryString = queryString + "       INSERT INTO @WarehouseCollection (CommodityID, EntryMonth, QuantityGoodsIssue, QuantityBegin, QuantityInput, QuantityOutput, QuantityOnOutput, QuantityBalance, QuantityOnProduction, QuantityEndSemi, QuantityEndImprove, QuantityEndPack, QuantityEndRefine, QuantityEndRank, QuantityEndFinish) " + "\r\n";

            queryString = queryString + "       SELECT      WarehouseRawData.CommodityID, WarehouseRawData.EntryMonth, SUM(QuantityGoodsIssue) AS QuantityGoodsIssue, SUM(QuantityBegin) AS QuantityBegin, SUM(QuantityInput) AS QuantityInput, SUM(QuantityOutput) AS QuantityOutput, SUM(QuantityOnOutput) AS QuantityOnOutput, SUM(QuantityBalance) AS QuantityBalance, SUM(QuantityOnProduction) AS QuantityOnProduction, SUM(QuantityEndSemi) AS QuantityEndSemi, SUM(QuantityEndImprove) AS QuantityEndImprove, SUM(QuantityEndPack) AS QuantityEndPack, SUM(QuantityEndRefine) AS QuantityEndRefine, SUM(QuantityEndRank) AS QuantityEndRank, SUM(QuantityEndFinish) AS QuantityEndFinish " + "\r\n";
            queryString = queryString + "       FROM        (" + "\r\n";

            queryString = queryString + "                   SELECT      CommodityID, DATEADD(Month, DateDiff(Month, 0, EntryDate), 0) AS EntryMonth, Quantity + FreeQuantity AS QuantityGoodsIssue, 0 AS QuantityBegin, 0 AS QuantityInput, 0 AS QuantityOutput, 0 AS QuantityOnOutput, 0 AS QuantityBalance, 0 AS QuantityOnProduction, 0 AS QuantityEndSemi, 0 AS QuantityEndImprove, 0 AS QuantityEndPack, 0 AS QuantityEndRefine, 0 AS QuantityEndRank, 0 AS QuantityEndFinish " + "\r\n";
            queryString = queryString + "                   FROM        GoodsIssueDetails WHERE WarehouseID = @DefaultWarehouseID AND EntryDate >= @LocalFromDate AND EntryDate <= @LocalToDate " + "\r\n";
            //TAM THOI: WarehouseID = 34  --- CHI LAY HANG THI TRUONGC

            queryString = queryString + "                   UNION ALL   " + "\r\n";

            queryString = queryString + "                   SELECT      CommodityID, DATEADD(Month, DateDiff(Month, 0, @LocalToDate), 0) AS EntryMonth, 0  AS QuantityGoodsIssue, SUM(QuantityBegin) AS QuantityBegin, SUM(QuantityInput) AS QuantityInput, SUM(QuantityOutput) AS QuantityOutput, SUM(QuantityOnAdvice - QuantityOnTransferAdviceOut) AS QuantityOnOutput, SUM(QuantityBegin + QuantityInput - QuantityOutput - QuantityOnAdvice - QuantityOnTransferAdviceOut) AS QuantityBalance, 0 AS QuantityOnProduction, 0 AS QuantityEndSemi, 0 AS QuantityEndImprove, 0 AS QuantityEndPack, 0 AS QuantityEndRefine, 0 AS QuantityEndRank, 0 AS QuantityEndFinish " + "\r\n";
            queryString = queryString + "                   FROM        @SPSKUInventoryJournalTable WHERE WHLocationID = 90 AND (QuantityBegin <> 0 OR QuantityInput <> 0 OR QuantityOutput <> 0 OR QuantityOnAdvice <> 0 OR QuantityOnTransferAdviceOut <> 0) GROUP BY CommodityID " + "\r\n"; ////Phân Xưởng ONLY

            queryString = queryString + "                   UNION ALL   " + "\r\n";

            queryString = queryString + "                   SELECT      CommodityID, DATEADD(Month, DateDiff(Month, 0, @LocalToDate), 0) AS EntryMonth, 0  AS QuantityGoodsIssue, 0 AS QuantityBegin, 0 AS QuantityInput, 0 AS QuantityOutput, 0 AS QuantityOnOutput, 0 AS QuantityBalance, SUM(QuantityEndOrder) AS QuantityOnProduction, SUM(QuantityEndSemi) AS QuantityEndSemi, SUM(QuantityEndImprove) AS QuantityEndImprove, SUM(QuantityEndPack) AS QuantityEndPack, SUM(QuantityEndRefine) AS QuantityEndRefine, SUM(QuantityEndRank) AS QuantityEndRank, SUM(QuantityEndFinish) AS QuantityEndFinish " + "\r\n";
            queryString = queryString + "                   FROM        @SPProductionOrderJournalTable WHERE WarehouseGroupID = CAST(@WarehouseGroupIDList AS int) AND (QuantityEndSemi <> 0 OR QuantityEndImprove <> 0 OR QuantityEndPack <> 0 OR QuantityEndRefine <> 0 OR QuantityEndRank <> 0 OR QuantityEndFinish <> 0 OR QuantityEndOrder <> 0) GROUP BY CommodityID " + "\r\n";
            //TAM THOI: WarehouseGroupID = 4  --- CHI LAY HANG THI TRUONGC
            queryString = queryString + "                   ) WarehouseRawData " + "\r\n";
            queryString = queryString + "       GROUP BY    WarehouseRawData.CommodityID, WarehouseRawData.EntryMonth " + "\r\n";


            queryString = queryString + "       SELECT      Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.Weight, Commodities.ListedPrice, Commodities.GrossPrice, WarehouseCollection.EntryMonth, WarehouseCollection.QuantityGoodsIssue, WarehouseCollection.QuantityBegin, WarehouseCollection.QuantityInput, WarehouseCollection.QuantityOutput, WarehouseCollection.QuantityOnOutput, WarehouseCollection.QuantityBalance, WarehouseCollection.QuantityOnProduction, WarehouseCollection.QuantityEndSemi, WarehouseCollection.QuantityEndImprove, WarehouseCollection.QuantityEndPack, WarehouseCollection.QuantityEndRefine, WarehouseCollection.QuantityEndRank, WarehouseCollection.QuantityEndFinish " + "\r\n";
            queryString = queryString + "       FROM        @WarehouseCollection AS WarehouseCollection" + "\r\n";
            queryString = queryString + "                   INNER JOIN @Commodities AS Commodities ON WarehouseCollection.CommodityID = Commodities.CommodityID " + "\r\n";


            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("StatementOfWarehouses", queryString);
        }

        private string StatementOfWarehousesBUILDSQL(string tableCommodityPrices1, string tableCommodityPrices2)
        {
            string queryString = "" + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";
            queryString = queryString + "               UPDATE          Commodities     SET Commodities.ListedPrice = CommodityPrices.ListedPrice, Commodities.GrossPrice = CommodityPrices.GrossPrice FROM @Commodities Commodities INNER JOIN " + tableCommodityPrices1 + " CommodityPrices ON " + (tableCommodityPrices1 == "" ? "CommodityPrices.PriceCategoryID = CAST(@LocalPriceCategoryIDList AS int) AND " : "") + " Commodities.CodePartA = CommodityPrices.CodePartA AND Commodities.CodePartC = CommodityPrices.CodePartC  " + (tableCommodityPrices1 == "" ? " AND CommodityPrices.CodePartB IS NULL " : "") + "\r\n"; //UPDATE ListedPrice, GrossPrice BY @PriceCategoryID
            queryString = queryString + "               UPDATE          Commodities     SET Commodities.ListedPrice = CommodityPrices.ListedPrice, Commodities.GrossPrice = CommodityPrices.GrossPrice FROM @Commodities Commodities INNER JOIN " + tableCommodityPrices2 + " CommodityPrices ON " + (tableCommodityPrices2 == "" ? "CommodityPrices.PriceCategoryID = CAST(@LocalPriceCategoryIDList AS int) AND " : "") + " Commodities.CodePartA = CommodityPrices.CodePartA AND Commodities.CodePartC = CommodityPrices.CodePartC AND Commodities.CodePartB = CommodityPrices.CodePartB " + "\r\n"; //UPDATE ListedPrice, GrossPrice BY @PriceCategoryID
            queryString = queryString + "           END " + "\r\n";
            return queryString;
        }

        private void SearchWarehouseEntries()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime, @CodePartA nvarchar(20), @CodePartB nvarchar(20) " + "\r\n";
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SET NOCOUNT ON" + "\r\n";

            queryString = queryString + "       DECLARE     @FilterOrganizationalUnitID TABLE (OrganizationalUnitID int NOT NULL) " + "\r\n";
            queryString = queryString + "       INSERT INTO @FilterOrganizationalUnitID SELECT DISTINCT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)TotalBase.Enums.GlobalEnums.NmvnTaskID.GoodsIssue + " AND AccessControls.AccessLevel > 0 " + "\r\n";

            queryString = queryString + "       IF  (@CodePartA <> '') " + "\r\n";
            queryString = queryString + "           " + this.SearchWarehouseEntrySQL(true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.SearchWarehouseEntrySQL(false) + "\r\n";

            queryString = queryString + "       SET NOCOUNT OFF" + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SearchWarehouseEntries", queryString);
        }

        private string SearchWarehouseEntrySQL(bool isCodePartA)
        {
            string queryString = "";
            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF  (@CodePartB <> '') " + "\r\n";
            queryString = queryString + "           " + this.SearchWarehouseEntrySQL(isCodePartA, true) + "\r\n";
            queryString = queryString + "       ELSE " + "\r\n";
            queryString = queryString + "           " + this.SearchWarehouseEntrySQL(isCodePartA, false) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }
        private string SearchWarehouseEntrySQL(bool isCodePartA, bool isCodePartB)
        {
            string queryString = "";

            queryString = queryString + "       SELECT      GoodsIssues.GoodsIssueID, 'Inventories/GoodsIssues' AS TaskAction, GoodsIssues.EntryDate, GoodsIssues.Reference, Warehouses.Code AS WarehouseCode, Workshifts.WorkshiftID, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, GoodsIssues.Description, GoodsIssues.DeliveryAdviceReferences, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.GoodsIssueDetailID, GoodsIssueDetails.CommodityID, Commodities.Code AS CommodityCode, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, Commodities.Name AS CommodityName, " + "\r\n";
            queryString = queryString + "                   GoodsIssueDetails.Quantity, GoodsIssueDetails.FreeQuantity, GoodsIssueDetails.ListedPrice, GoodsIssueDetails.ListedGrossPrice, GoodsIssueDetails.DiscountPercent, GoodsIssueDetails.UnitPrice, GoodsIssueDetails.ListedAmount, GoodsIssueDetails.Amount, ROUND(GoodsIssueDetails.ListedAmount - GoodsIssueDetails.Amount, " + (int)GlobalEnums.rndAmount + ") AS DiscountAmount, GoodsIssues.TotalQuantity, GoodsIssues.TotalAmount, GoodsIssues.TradeDiscountAmount, GoodsIssues.TotalTaxableAmount, GoodsIssues.TotalVATAmount, GoodsIssues.TotalGrossAmount " + "\r\n";

            queryString = queryString + "       FROM        GoodsIssues " + "\r\n";
            queryString = queryString + "                   INNER JOIN GoodsIssueDetails ON GoodsIssues.EntryDate >= @FromDate AND GoodsIssues.EntryDate <= @ToDate " + (isCodePartA || isCodePartB ? "AND GoodsIssueDetails.CommodityID IN (SELECT CommodityID FROM Commodities WHERE " + (isCodePartA ? " CodePartA LIKE '%' + @CodePartA +'%'" : "") + (isCodePartA && isCodePartB ? " AND " : "") + (isCodePartB ? " CodePartB LIKE '%' + @CodePartB +'%'" : "") + ")" : "") + " AND GoodsIssues.OrganizationalUnitID IN (SELECT OrganizationalUnitID FROM @FilterOrganizationalUnitID) AND GoodsIssues.GoodsIssueID = GoodsIssueDetails.GoodsIssueID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON GoodsIssueDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Warehouses ON GoodsIssueDetails.WarehouseID = Warehouses.WarehouseID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON GoodsIssues.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";

            return queryString;
        }

        private void SalesReturnJournal()
        {
            string queryString;

            queryString = " @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesReturns.EntryDate, SalesReturns.Reference, Workshifts.Code AS WorkshiftCode, Workshifts.Name AS WorkshiftName, Receivers.Code AS ReceiverCode, Receivers.Name AS ReceiverName, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, Commodities.CodePartA, Commodities.CodePartB, Commodities.CodePartC, Commodities.CodePartD, SalesReturnDetails.Quantity, SalesReturnDetails.FreeQuantity, SalesReturnDetails.UnitPrice, SalesReturns.Description " + "\r\n";
            queryString = queryString + "       FROM        SalesReturns " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts ON SalesReturns.EntryDate >= @FromDate AND SalesReturns.EntryDate <= @ToDate AND SalesReturns.WorkshiftID = Workshifts.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Workshifts AS Receivers ON SalesReturns.ReceiverID = Receivers.WorkshiftID " + "\r\n";
            queryString = queryString + "                   INNER JOIN SalesReturnDetails ON SalesReturns.SalesReturnID = SalesReturnDetails.SalesReturnID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON SalesReturnDetails.CommodityID = Commodities.CommodityID " + "\r\n";
            queryString = queryString + "    END " + "\r\n";

            this.totalSmartPortalEntities.CreateStoredProcedure("SalesReturnJournal", queryString);
        }
        

    }
}

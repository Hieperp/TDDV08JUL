//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TotalModel.Models
{
    using System;
    
    public partial class PendingCustomerCredit
    {
        public int CreditTypeID { get; set; }
        public string CreditTypeName { get; set; }
        public int ReceiptCreditID { get; set; }
        public string Reference { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string Description { get; set; }
        public decimal TotalCreditAmount { get; set; }
        public decimal TotalReceiptAmount { get; set; }
        public decimal TotalFluctuationAmount { get; set; }
        public Nullable<decimal> TotalCreditAmountPending { get; set; }
    }
}

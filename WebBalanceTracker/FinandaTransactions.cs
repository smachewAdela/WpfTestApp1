//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebBalanceTracker
{
    using System;
    using System.Collections.Generic;
    
    public partial class FinandaTransactions
    {
        public int Id { get; set; }
        public string TransID { get; set; }
        public string uid { get; set; }
        public string InstituteTransID { get; set; }
        public decimal CurrencyDebit { get; set; }
        public string Description { get; set; }
        public int AccountType { get; set; }
        public string TransType { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string TransCurrency { get; set; }
        public string Source { get; set; }
        public decimal Credit { get; set; }
        public decimal ReportedBalance { get; set; }
        public decimal Debit { get; set; }
        public string TransValueDate { get; set; }
        public string ReferenceID { get; set; }
        public string TransDate { get; set; }
        public decimal Amount { get; set; }
        public string category { get; set; }
        public string CatDesc { get; set; }
        public string CatGroupID { get; set; }
        public string CatGroup { get; set; }
        public string AccountNumber { get; set; }
    }
}
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
    
    public partial class FinandaSyncLog
    {
        public int Id { get; set; }
        public System.DateTime SyncStart { get; set; }
        public System.DateTime SyncEnd { get; set; }
        public string LogInfo { get; set; }
        public int NewTransactions { get; set; }
        public bool Success { get; set; }
        public System.DateTime TransactionsFromDate { get; set; }
        public System.DateTime TransactionsToDate { get; set; }
    }
}

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
    
    public partial class AbstractCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int BudgetGroupId { get; set; }
        public bool Active { get; set; }
    }
}

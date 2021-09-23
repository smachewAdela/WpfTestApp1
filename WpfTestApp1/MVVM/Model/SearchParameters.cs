using System;

namespace QBalanceDesktop
{
    public class SearchParameters
    {
        public string TransID { get;  set; }
        public string CategoryCode { get;  set; }
        public int? CategoryGroupId { get;  set; }
        public string CategoryGroupName { get;  set; }
        public int? BudgetCategoryMonthId { get;  set; }
        public DateTime? TranFromDate { get;  set; }
        public DateTime? TranToDate { get;  set; }
        public DateTime? BudgetDate { get;  set; }
        public int? BudgetItemBudgetId { get;  set; }
        public int? BudgetIncomeId { get;  set; }
        public int? BudgetItemGroupId { get;  set; }
        public int? TransactionCheckPointBudgetId { get;  set; }
        public int? BudgetItemLogBudgetItemId { get;  set; }
        public int? BudgetItemId { get; set; }
        public int? TransactionCheckPointId { get; set; }
        public int? BudgetGroupId { get; set; }
        public int? BudgetItemAbstractCategoryId { get; internal set; }
        public int? AbstractAutoTransactionId { get; set; }
    }
}
using System;

namespace QBalanceDesktop
{
    public class SearchParameters
    {
        public string TransID { get; internal set; }
        public string CategoryCode { get; internal set; }
        public int? CategoryGroupId { get; internal set; }
        public string CategoryGroupName { get; internal set; }
        public int? BudgetCategoryMonthId { get; internal set; }
        public DateTime? TranFromDate { get; internal set; }
        public DateTime? TranToDate { get; internal set; }
        public DateTime? BudgetDate { get; internal set; }
        public int? BudgetItemBudgetId { get; internal set; }
        public int? BudgetIncomeId { get; internal set; }
        public int? BudgetItemGroupId { get; internal set; }
        public int? TransactionCheckPointBudgetId { get; internal set; }
    }
}
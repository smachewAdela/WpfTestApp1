using System;

namespace WebBalanceTracker
{
    public class Budget
    {
        private BudgetMonth budgetMonth;
        private BalanceAdmin_Entities context;
        public DateTime Month { get; set; }
        public string Title { get; internal set; }

        public Budget(DateTime month, BalanceAdmin_Entities context)
        {
            this.Month = month;
            Title = $"{Month.ToString("MMMM")} {Month.Year.ToString().Substring(2, 2)}";
        }
        public Budget(BudgetMonth budgetMonth, BalanceAdmin_Entities context)
        {
            this.budgetMonth = budgetMonth;
            this.context = context;
            this.Month = budgetMonth.Month;
            Title = $"{Month.ToString("MMMM")} {Month.Year.ToString().Substring(2, 2)}";
        }
    }
}
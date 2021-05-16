using System.Collections.Generic;
using QBalanceDesktop;

namespace WpfTestApp1.MVVM.Model
{
    public class ReportSourceData
    {
        public List<BudgetGroup> Groups { get;  set; }
        public List<BudgetItem> Categories { get;  set; }
        public List<BudgetIncomeItem> Incomes { get;  set; }
        public List<Budget> Months { get;  set; }
    }
}
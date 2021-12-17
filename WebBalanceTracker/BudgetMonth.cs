namespace WebBalanceTracker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class BudgetMonth
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime Month { get; set; }

        public List<BudgetTransaction> BudgetTransactions
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return context.BudgetTransaction.Where(x => x.BudgetMonthId == this.Id).ToList();
            }
        }

        public List<IncomeTransaction> IncomeTransactions
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return context.IncomeTransaction.Where(x => x.BudgetMonthId == this.Id).ToList();
            }
        }

        public int Totalincomes
        {
            get
            {
                return IncomeTransactions.Sum(x => x.Amount);
            }
        }

        public int LefttoUse
        {
            get
            {
                return TotalBudget - TotalExpenses;
            }
        }

        public int LeftFromIncome
        {
            get
            {
                return Totalincomes - Totalincomes;
            }
        }  
        
        public int TotalExpenses
        {
            get
            {
                return BudgetTransactions.Sum(x => x.Amount);
            }
        }
        
        public int TotalBudget
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return context.AbstractCategory.Where(x => x.Active).Sum(x => x.Amount);
            }
        }
        
    }
}

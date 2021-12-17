using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBalanceTracker
{
    public class GlobalDataProvider
    {
        public List<AbstractCategory> AbstractCategories
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return context.AbstractCategory.ToList();
            }
        }

        public List<AbstractIncome> AbstractIncomes
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return context.AbstractIncome.ToList(); 
            }
        }

        public List<BudgetAutoTransaction> BudgetAutoTransactions
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return context.BudgetAutoTransaction.ToList();
            }
        }

        public List<BudgetGroup> BudgetGroups
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return context.BudgetGroup.ToList();
            }
        }

        public List<BudgetMonth> BudgetMonths
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return context.BudgetMonth.ToList();
            }
        }
    }
}
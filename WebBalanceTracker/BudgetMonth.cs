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

        public List<TransactionCheckPoint> TransactionCheckPoints
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return new List<TransactionCheckPoint>();
            }
        }


        public int Totalincomes
        {
            get
            {
                return IncomeTransactions.Sum(x => x.Amount);
            }
        }

        internal int StatusForGroup(int id)
        {
            return BudgetTransactions.Where(x => x.AbstractCatrgory.BudgetGroupId == id).Sum(x => x.Amount);
        }

        internal int IsGroupOverSpent(int id)
        {
            var groupExpenses = StatusForGroup(id);
            var budgetForGroup = BudgetForGroup(id);
            return (groupExpenses * 100) / (budgetForGroup == 0 ? 100 : budgetForGroup);
        }

        internal int RatioForGroup(int id)
        {
            using (var context = new BalanceAdmin_Entities())
            {
                var groupBudget = BudgetForGroup(id);
                var groupStatus = StatusForGroup(id);
                var res = (groupStatus * 100) / (groupBudget == 0 ? 100 : groupBudget);
                return res;
            }
        }

        internal int BudgetForGroup(int id)
        {
            using (var context = new BalanceAdmin_Entities())
            {
                var resItems = context.AbstractCategory.Where(x => x.Active && x.BudgetGroupId == id).ToList();
                if (resItems.IsEmptyOrNull())
                    return 0;
                return resItems.Sum(x => x.Amount);
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
                return Totalincomes - TotalExpenses;
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

        public int Ratio
        {
            get
            {
                return (TotalExpenses * 100) / (TotalBudget == 0 ? 100 : TotalBudget);
            }
        }
    }
}

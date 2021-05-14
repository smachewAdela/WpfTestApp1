using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBalanceDesktop
{
    [DbEntity("BudgetItemGroup")]
    public class BudgetGroup : BaseDbItem
    {
        [DbField()]
        public string Name { get; set; }
        public List<BudgetItem> BudgetItems { get; set; }

        public int Budget
        {
            set { }
            get { return BudgetItems.Sum(x => x.BudgetAmount); }
        }

        public string BudgetStr
        {
            get { return Budget.ToNumberFormat(); }
        }

        public int Status
        {
            set { }
            get { return BudgetItems.Sum(x => x.StatusAmount); }
        }
        public string StatusStr
        {
            get { return Status.ToNumberFormat(); }
        }
        public int Ratio
        {
            set { }
            get
            {
                // 75/100
                return (Status * 100) / Budget;
            }
        }
    }
}

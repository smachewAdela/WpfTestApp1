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

        public int Budget { get; set; }
        public string BudgetStr
        {
            get { return Budget.ToNumberFormat(); }
        }
        public int Status { get; set; }
        public string StatusStr
        {
            get { return Status.ToNumberFormat(); }
        }

        public override void LoadExtraData()
        {
            base.LoadExtraData();

            var items = GlobalsProviderBL.Db.GetData<BudgetItem>(new SearchParameters { BudgetItemBudgetId = this.Id, BudgetItemGroupId = this.Id });
            Budget = items.Sum(x => x.BudgetAmount);
            Status = items.Sum(x => x.StatusAmount);
        }
    }
}

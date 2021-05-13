using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBalanceDesktop
{
    [DbEntity("BudgetForMonth")]
    public class Budget : BaseDbItem
    {
        [DbField()]
        public DateTime Month { get; set; }

        public string Title
        {
            get { return $"{Month.ToString("MMMM")}-{Month.Year.ToString()}"; }
        }

        public List<BudgetItem> Items { get; set; }

        public List<BudgetIncomeItem> Incomes { get; set; }


        public override void LoadExtraData()
        {
            Items = GlobalsProviderBL.Db.GetData<BudgetItem>(new SearchParameters { BudgetItemBudgetId = this.Id });
            Incomes = GlobalsProviderBL.Db.GetData<BudgetIncomeItem>(new SearchParameters { BudgetItemBudgetId = this.Id });
        }

        public int TotalIncomes
        {
            get { return Incomes.IsNotEmpty() ? Incomes.Sum(x => x.Amount) : 0; }
        }
        public int TotalExpenses
        {
            get { return Items.IsNotEmpty() ? Items.Sum(x => x.StatusAmount) : 0; }
        }

        public int TotalBudget
        {
            get { return Items.IsNotEmpty() ? Items.Sum(x => x.BudgetAmount) : 0; }
        }

        public int OverSpentNumber
        {
            get 
            {
                return Items.IsNotEmpty() ? Items.Where(x => x.BudgetAmount < x.StatusAmount).Count() : 0; ;
            }
        }

        public Dictionary<int, int> GroupOverSpentData
        {
            get
            {
                //var data = new Dictionary<int, int>();
                var data = Items.Select(x => x.GroupId).Distinct().ToDictionary(x => x, x => 0);
                foreach (var item in Items)
                {
                    if(item.BudgetAmount < item.StatusAmount)
                    data[item.GroupId] += 1;
                }
                return data;
            }
        }

        public Dictionary<int, int> CategoryStatusData
        {
            get
            {
                return Items.ToDictionary(x => x.Id, x => x.StatusAmount); 
            }
        }

        public Dictionary<int, int> CategoryOverSpentData
        {
            get
            {
                return Items.ToDictionary(x => x.Id, x => x.BudgetAmount < x.StatusAmount ? 1 : 0);
            }
        }

        public Dictionary<int, int> GroupStatusData
        {
            get
            {
                //var data = new Dictionary<int, int>();
                var data = Items.Select(x => x.GroupId).Distinct().ToDictionary(x => x, x => 0);
                foreach (var item in Items)
                    data[item.GroupId] += item.StatusAmount;
                return data;
            }
        }

        public Dictionary<int, int> GroupBudgetData
        {
            get
            {
                var data = Items.Select(x => x.GroupId).Distinct().ToDictionary(x => x, x => 0);
                foreach (var item in Items)
                    data[item.GroupId] += item.BudgetAmount;
                return data;
            }
        }
    }
}

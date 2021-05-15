using Newtonsoft.Json;
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
        [JsonIgnore]
        public List<BudgetItem> BudgetItems { get; set; }

        [JsonIgnore]
        public int Budget
        {
            set { }
            get { return BudgetItems.Sum(x => x.BudgetAmount); }
        }
        [JsonIgnore]
        public string BudgetStr
        {
            get { return Budget.ToNumberFormat(); }
        }
        [JsonIgnore]
        public int Status
        {
            set { }
            get { return BudgetItems.Sum(x => x.StatusAmount); }
        }
        [JsonIgnore]
        public string StatusStr
        {
            get { return Status.ToNumberFormat(); }
        }
        [JsonIgnore]
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

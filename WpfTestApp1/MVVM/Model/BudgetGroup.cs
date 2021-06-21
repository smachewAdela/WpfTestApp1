﻿using Newtonsoft.Json;
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

        public string NameForBudget
        {
            get
            {
                if (BudgetItems.IsEmptyOrNull())
                    return Name;
                return $"{Name}  {BudgetItems.Sum(x => x.BudgetAmount).ToNumberFormat()}";
            }
        }

        public bool IsOverSpent
        {
            get
            {
                return Ratio > 100;
            }
        }

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
                var res = (Status * 100) / (Budget == 0 ? 100 : Budget);
                return res;
            }
        }

        public bool IsTotal { get; set; }
    }
}

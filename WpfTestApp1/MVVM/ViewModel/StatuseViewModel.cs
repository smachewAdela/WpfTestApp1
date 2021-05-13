using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp1.MVVM.ViewModel
{
    class StatuseViewModel
    {
        public List<BudgetGroup> Groups { get; set; }

        public StatuseViewModel()
        {
            var currentBudget = GlobalsProviderBL.GetLatestBudget();
            Groups = GlobalsProviderBL.Db.GetData<BudgetGroup>(new SearchParameters { });

            foreach (var g in Groups)
                g.BudgetItems = currentBudget.Items.Where(x => x.GroupId == g.Id).ToList();

            BudgetGroup total = new BudgetGroup
            {
                Name = "",
                BudgetItems = currentBudget.Items
            };
            Groups.Add(total);
        }
    }

}

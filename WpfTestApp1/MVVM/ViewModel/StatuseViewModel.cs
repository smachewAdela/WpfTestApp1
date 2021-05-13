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
            Groups = GlobalsProviderBL.Db.GetData<BudgetGroup>(new SearchParameters());
            BudgetGroup total = new BudgetGroup
            {
                Name = "",
                Budget = Groups.Sum(x => x.Budget),
                Status = Groups.Sum(x => x.Status)
            };
            Groups.Add(total);
        }
    }

}

using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestApp1.Core;
using WpfTestApp1.MVVM.Model;

namespace WpfTestApp1.MVVM.ViewModel
{
    public class StatuseViewModel : ObservableObject, IRefreshAble
    {
        private List<BudgetGroup> _g;

        public List<BudgetGroup> Groups
    {
            get { return _g; }
            set
            {
                _g = value;
                OnPropertyChanged();
                OnPropertyChanged("CurrentTitle");
            }
        }


        public StatuseViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            var currentBudget = GlobalsProviderBL.CurrentBudget;
            var gGroups = GlobalsProviderBL.Db.GetData<BudgetGroup>(new SearchParameters { });

            foreach (var g in gGroups)
                g.BudgetItems = currentBudget.Items.Where(x => x.GroupId == g.Id).ToList();

            BudgetGroup total = new BudgetGroup
            {
                Name = "",
                BudgetItems = currentBudget.Items
            };
            gGroups.Add(total);

            Groups = gGroups;
        }

        public void Refresh()
        {
            LoadData();
        }
    }

}

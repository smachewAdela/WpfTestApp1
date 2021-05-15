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
    public class BudgetViewModel : ObservableObject, IRefreshAble
    {
        public string CurrentTitle
        {
            get { return GlobalsProviderBL.CurrentBudget.Title; }
        }

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

        public BudgetViewModel()
        {
            UpdateBudgetCommand = new RelayCommand(parameter =>
            {
                var values = (object[])parameter;
                UpdateBudgetItem((BudgetItem)values[0]);
            });
            LoadData();
        }

        private void UpdateBudgetItem(BudgetItem o)
        {
            GlobalsProviderBL.Db.Update(o);
            LoadData();
        }

        public RelayCommand UpdateBudgetCommand { get; set; }
        private void RefreshView(object o)
        {
            int dir = Convert.ToInt32(o);
            GlobalsProviderBL.ProgressMonth(dir);
            LoadData();
        }

        private void LoadData()
        {
            var currentBudget = GlobalsProviderBL.CurrentBudget;
            var gGroups = GlobalsProviderBL.Db.GetData<BudgetGroup>(new SearchParameters { });

            foreach (var g in gGroups)
                g.BudgetItems = currentBudget.Items.Where(x => x.GroupId == g.Id).ToList();

            Groups = gGroups;
        }

        public void Refresh()
        {
            LoadData();
        }
    }
}

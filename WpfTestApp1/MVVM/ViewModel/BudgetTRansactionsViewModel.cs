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
    public class BudgetTRansactionsViewModel : ObservableObject, IRefreshAble
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
            }
        }

        public BudgetTRansactionsViewModel()
        {
            UpdateBudgetCommand = new RelayCommand(parameter =>
            {
                var values = (object[])parameter;
                int tran = Convert.ToInt32( values[1]);

                UpdateBudgetItem((BudgetItem)values[0], tran);
            });
            LoadData();
        }

        private void UpdateBudgetItem(BudgetItem o, int tran)
        {
            o.StatusAmount += tran;
            GlobalsProviderBL.Db.Update(o);
            LoadData();
        }

        public RelayCommand UpdateBudgetCommand { get; set; }

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

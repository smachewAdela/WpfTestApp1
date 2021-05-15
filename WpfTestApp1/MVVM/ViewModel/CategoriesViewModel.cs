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
    public class CategoriesViewModel : ObservableObject
    {
        public RelayCommand UpdateCategoryCommand { get; set; }
        public CategoriesViewModel()
        {
            Groups = GlobalsProviderBL.Db.GetData<BudgetGroup>(new SearchParameters { });
            LoadItems();
            UpdateCategoryCommand = new RelayCommand(parameter =>
            {
                UpdateCategory(parameter);
            });
        }

        private void LoadItems()
        {
            BudgetItems = GlobalsProviderBL.CurrentBudget.Items;
        }

        private void UpdateCategory(object parameter)
        {
            var values = (object[])parameter;
            var newGroupId = (int)values[0];
            var chAll = (bool)values[1];
            var cItem = (BudgetItem)values[2];

            if (chAll)
            {
                var allBudgetItems = GlobalsProviderBL.Db.GetData<BudgetItem>().Where(x => x.CategoryName == cItem.CategoryName).ToList();
                foreach (var allBudgetItem in allBudgetItems)
                {
                    allBudgetItem.GroupId = newGroupId;
                    GlobalsProviderBL.Db.Update(allBudgetItem);
                }
            }
            else
                GlobalsProviderBL.Db.Update(cItem);

            LoadItems();
        }


        public List<BudgetGroup> Groups { get; set; }


        private List<BudgetItem> _g;
        public List<BudgetItem> BudgetItems
        {
            get
            {
                return _g;
            }
            set
            {
                _g = value;
                OnPropertyChanged();
            }
        }

    }
}

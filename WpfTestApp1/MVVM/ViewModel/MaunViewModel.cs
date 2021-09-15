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
    public class MaunViewModel : ObservableObject
    {
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            {
                _currentView = value;
                OnPropertyChanged();
                OnPropertyChanged("CurrentTitle");
            }
        }
        public RelayCommand RefreshMonthCommand { get; set; }
        public string CurrentTitle
        {
            get { return GlobalsProviderBL.CurrentBudget.Title; }
        }


        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand CategoriesViewCommand { get; set; }
        public RelayCommand StatusViewCommand { get; set; }
        public RelayCommand IncomeViewCommand { get; set; }
        public RelayCommand BudgetViewCommand { get; set; }
        public RelayCommand BudgetTransactionsViewCommand { get; set; }
        public RelayCommand ReportViewCommand { get; set; }

        public MaunViewModel()
        {

            var statsVm = new StatuseViewModel();
            CurrentView = statsVm;

            RefreshMonthCommand = new RelayCommand(o =>
            {
                int dir = Convert.ToInt32(o);
                GlobalsProviderBL.ProgressMonthSelection(dir);
                CurrentView = _currentView;
                ((IRefreshAble)CurrentView).Refresh();
            });

            HomeViewCommand = new RelayCommand(o => 
            {
                CurrentView = new HomeViewModel();
            });
            CategoriesViewCommand = new RelayCommand(o =>
            {
                CurrentView = new CategoriesViewModel();
            });
            StatusViewCommand = new RelayCommand(o =>
            {
                CurrentView = statsVm;
            });
            IncomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = new IncomeViewModel();
            });
            BudgetViewCommand = new RelayCommand(o =>
            {
                CurrentView = new BudgetViewModel();
            });
            BudgetTransactionsViewCommand = new RelayCommand(o =>
            {
                CurrentView = new BudgetTRansactionsViewModel();
            });
            ReportViewCommand = new RelayCommand(o =>
            {
                CurrentView = new ReportViewModel();
            });
        }
    }
}

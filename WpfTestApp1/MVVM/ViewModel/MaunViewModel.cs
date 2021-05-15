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
            set { _currentView = value;
                OnPropertyChanged();
            }
        }

        public HomeViewModel HomeVM { get; set; }
        public CategoriesViewModel CatsVm { get; set; }
        public StatuseViewModel StatusVM { get; set; }
        public IncomeViewModel IncomeVM { get; set; }
        public BudgetViewModel BudgetVM { get; set; }
        public BudgetTRansactionsViewModel BudgetTransactionsVM { get; set; }

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand CategoriesViewCommand { get; set; }
        public RelayCommand StatusViewCommand { get; set; }
        public RelayCommand IncomeViewCommand { get; set; }
        public RelayCommand BudgetViewCommand { get; set; }
        public RelayCommand BudgetTransactionsViewCommand { get; set; }

        public MaunViewModel()
        {
            HomeVM = new HomeViewModel();
            CatsVm = new CategoriesViewModel();
            StatusVM = new StatuseViewModel();
            IncomeVM = new IncomeViewModel();
            BudgetVM = new BudgetViewModel();
            BudgetTransactionsVM = new BudgetTRansactionsViewModel();

            CurrentView = StatusVM;

            HomeViewCommand = new RelayCommand(o => 
            {
                CurrentView = HomeVM;
            });
            CategoriesViewCommand = new RelayCommand(o =>
            {
                CurrentView = CatsVm;
            });
            StatusViewCommand = new RelayCommand(o =>
            {
                CurrentView = StatusVM;
            });
            IncomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = IncomeVM;
            });
            BudgetViewCommand = new RelayCommand(o =>
            {
                CurrentView = BudgetVM;
            });
            BudgetTransactionsViewCommand = new RelayCommand(o =>
            {
                CurrentView = BudgetTransactionsVM;
            });
        }
    }
}

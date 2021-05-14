using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestApp1.Core;

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


        public string CurrentTitle
        {
            get { return GlobalsProviderBL.CurrentBudget.Title; }
        }

        public HomeViewModel HomeVM { get; set; }
        public CategoriesViewModel CatsVm { get; set; }
        public StatuseViewModel StatusVM { get; set; }

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand CategoriesViewCommand { get; set; }
        public RelayCommand StatusViewCommand { get; set; }

        public MaunViewModel()
        {
            HomeVM = new HomeViewModel();
            CatsVm = new CategoriesViewModel();
            StatusVM = new StatuseViewModel();

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
        }

        internal void RefreshView()
        {
            OnPropertyChanged();
        }
    }
}

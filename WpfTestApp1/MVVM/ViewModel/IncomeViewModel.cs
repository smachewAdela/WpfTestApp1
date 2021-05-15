﻿using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestApp1.Core;
using WpfTestApp1.MVVM.Model;

namespace WpfTestApp1.MVVM.ViewModel
{
    public class IncomeViewModel : ObservableObject
    {
        public IncomeViewModel()
        {
            LoadItems();
            UpdateIncomeCommand = new RelayCommand(parameter =>
            {
                UpdateIncome(parameter);
            });
            RefreshMonthCommand = new RelayCommand(o =>
            {
                RefreshView(o);
            });
        }

        private void UpdateIncome(object parameter)
        {
            var values = (object[])parameter;
            var incomeToUpdate = (BudgetIncomeItem)values[0];
            GlobalsProviderBL.Db.Update(incomeToUpdate);
            LoadItems();
        }

        private void RefreshView(object o)
        {
            int dir = Convert.ToInt32(o);
            GlobalsProviderBL.ProgressMonth(dir);
            LoadItems();
        }

        private void LoadItems()
        {
            BudgetIncomes = GlobalsProviderBL.CurrentBudget.Incomes;
        }

        public RelayCommand UpdateIncomeCommand { get; set; }
        public RelayCommand RefreshMonthCommand { get; set; }

        private List<BudgetIncomeItem> _g;
        public List<BudgetIncomeItem> BudgetIncomes
        {
            get
            {
                return _g;
            }
            set
            {
                var total = new BudgetIncomeItem
                {
                    Amount = value.Sum(x => x.Amount),
                    IsTotal = true
                };
                value.Add(total);
                _g = value;
                
                OnPropertyChanged();
                OnPropertyChanged("CurrentTitle");
            }
        }

        public string CurrentTitle
        {
            get { return GlobalsProviderBL.CurrentBudget.Title; }
        }
    }
}
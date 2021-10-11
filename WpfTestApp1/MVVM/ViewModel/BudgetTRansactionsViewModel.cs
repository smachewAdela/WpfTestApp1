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

        public List<TransactionCheckPoint> CheckPoints
        {
            get 
            {
                if (GlobalsProviderBL.CurrentBudget.TransactionCheckPoints.IsEmptyOrNull())
                {
                    var _t = GlobalsProviderBL.GenerateDefaultCheckPoints();
                    foreach (var checkPoint in _t)
                    {
                        checkPoint.BudgetId = GlobalsProviderBL.CurrentBudget.Id;
                        GlobalsProviderBL.Db.Add(checkPoint);
                    }
                    GlobalsProviderBL.CurrentBudget.TransactionCheckPoints = GlobalsProviderBL.Db.GetData<TransactionCheckPoint>(new SearchParameters { TransactionCheckPointBudgetId = GlobalsProviderBL.CurrentBudget.Id });
                }
                return GlobalsProviderBL.CurrentBudget.TransactionCheckPoints; 
            }
        }

        private List<BudgetGroup> _g;

        public List<BudgetGroup> Groups
        {
            get { return _g; }
            set
            {
                _g = value;
                OnPropertyChanged();
                OnPropertyChanged("CheckPoints");
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
            UpdateCheckPointsCommand = new RelayCommand(parameter =>
            {
                foreach (var cp in CheckPoints)
                    GlobalsProviderBL.Db.Update(cp);

            });
            RollBackTransactiontCommand = new RelayCommand(parameter =>
            {
                var values = (object[])parameter;

                RollBackLastTransaction((BudgetItem)values[0]);

            });
            LoadData();
        }

        private void RollBackLastTransaction(BudgetItem budgetItem)
        {
            BudgetItemLog log = GlobalsProviderBL.Db.GetSingle<BudgetItemLog>(new SearchParameters { BudgetItemLogBudgetItemId = budgetItem.Id });
            if (log != null && !log.RollBackExecuted)
            {
                budgetItem.StatusAmount -= log.Amount;
                GlobalsProviderBL.Db.Update(budgetItem);

                log.RollBackExecuted = true;
                GlobalsProviderBL.Db.Update(log);

                LoadData();
            }
        }

        private void UpdateBudgetItem(BudgetItem o, int tran)
        {
            o.StatusAmount += tran;
            GlobalsProviderBL.Db.Update(o);
            BudgetItemLog log = GlobalsProviderBL.Db.GetSingle<BudgetItemLog>(new SearchParameters { BudgetItemLogBudgetItemId = o.Id});
            if (log == null)
            {
                log = new BudgetItemLog { BudgetItemId = o.Id, Amount = tran };
                GlobalsProviderBL.Db.Add(log);
            }
            else
            {
                log.Amount = tran;
                log.RollBackExecuted = false;
                GlobalsProviderBL.Db.Update(log);
            }
           
            LoadData();
        }

        public RelayCommand UpdateBudgetCommand { get; set; }
        public RelayCommand UpdateCheckPointsCommand { get; set; }
        public RelayCommand RollBackTransactiontCommand { get; set; }

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

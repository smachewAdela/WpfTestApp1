using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp1.MVVM.Model.Automation
{
    public class AutomationHelper
    {
        public static void GenerateBudget(DbAccess db, Budget progressFrom)
        {
            var nextBudgetDate = progressFrom.Month.AddMonths(1);
            var existingBudget = db.GetData<Budget>( new SearchParameters { BudgetDate = nextBudgetDate }).First();
            if (existingBudget != null)
                throw new Exception("Next Budget Already Exists !");

            var nextBudget = new Budget { Month = nextBudgetDate };
            db.Insert(nextBudget);

            // incomes
            var existingIncomes = progressFrom.Incomes;
            foreach (var existingIncome in existingIncomes)
            {
                existingIncome.BudgetId = nextBudget.Id;
                existingIncome.Amount = 0;
                db.Insert(existingIncome);
            }

            // TransactionCheckPoints
            var existingTransactionCheckPoints = progressFrom.TransactionCheckPoints;
            foreach (var existingTransactionCheckPoint in existingTransactionCheckPoints)
            {
                existingTransactionCheckPoint.BudgetId = nextBudget.Id;
                existingTransactionCheckPoint.Description = string.Empty;
                db.Insert(existingTransactionCheckPoint);
            }

            // budget Categories
            var absCategories = db.GetData<AbstractCategory>();
            foreach (var absCategory in absCategories)
            {
                var nBudgetItem = new BudgetItem
                {
                    BudgetAmount = absCategory.DefaultAmount,
                    BudgetId = nextBudget.Id,
                    CategoryName = absCategory.CategoryName,
                    GroupId = absCategory.GroupId,
                    StatusAmount = 0,
                };
                db.Insert(nBudgetItem);
            }
        }
    }
}

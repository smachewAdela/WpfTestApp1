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
        public static Budget GenerateBudget(DbAccess db, Budget progressFrom)
        {
            var nextBudgetDate = progressFrom.Month.AddMonths(1);
            var existingBudget = db.GetData<Budget>(new SearchParameters { BudgetDate = nextBudgetDate }).FirstOrDefault();
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
                    AbstractCategoryId = absCategory.Id,
                    GroupId = absCategory.GroupId,
                    StatusAmount = 0,
                };
                db.Insert(nBudgetItem);
            }

            IMessage message = IMessage.Genertae(IMessageTypeEnum.Info);
            message.Title = "Budget Progressed";
            message.Message = progressFrom.Title;
            message.ExtraData = nextBudget.Title;
            db.Insert(message);

            return db.GetSingle<Budget>(new SearchParameters { BudgetDate = nextBudget.Month }); ;
        }

        public static void HandleAutoTransactions(DbAccess db, Budget budgetToHandle)
        {
            var autoTrans = db.GetData<AbstractAutoTransaction>(new SearchParameters { });

            foreach (var autoTran in autoTrans)
            {
                if (autoTran.BudgetId != budgetToHandle.Id && autoTran.DayOfTheMonth <= DateTime.Now.Day)
                {
                    BudgetItem matchingCategory = GetMatchingcategory(db, autoTran.AbstractCategoryId, budgetToHandle.Id);
                    if (matchingCategory != null)
                    {
                        matchingCategory.StatusAmount += autoTran.DefaultAmount;
                        db.Update(matchingCategory);

                        autoTran.BudgetId = budgetToHandle.Id;
                        autoTran.LastPaymentDate = budgetToHandle.Title;
                        db.Update(autoTran);

                        IMessage message = IMessage.Genertae( IMessageTypeEnum.Info);
                        message.Title = "Auto Transaction Applied";
                        message.Message = autoTran.Name;
                        message.ExtraData = autoTran.DefaultAmount.ToNumberFormat();
                        db.Insert(message);
                    }
                }
            }
        }

        private static BudgetItem GetMatchingcategory(DbAccess db, int abstractCategoryId, int budgetId)
        {
            return db.GetSingle<BudgetItem>(new SearchParameters
            {
                BudgetItemBudgetId = budgetId,
                BudgetItemAbstractCategoryId = abstractCategoryId
            });
        }
    }
}

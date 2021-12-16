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

        public static Budget GenerateBudget(DbAccess db, Budget progressFrom, DateTime? targetDate = null) 
        {

            try
            {
                var nextBudgetDate = targetDate ?? progressFrom.Month.AddMonths(1);
                var existingBudget = db.GetData<Budget>(new SearchParameters { BudgetDate = nextBudgetDate }).FirstOrDefault();
                if (existingBudget != null)
                    throw new DuplicateBudgetException("Next Budget Already Exists !");

                db.BeginTransaction();

                var nextBudget = new Budget { Month = nextBudgetDate };
                db.Add(nextBudget);
                
                // incomes
                var existingIncomes = progressFrom.Incomes;
                foreach (var existingIncome in existingIncomes)
                {
                    existingIncome.BudgetId = nextBudget.Id;
                    existingIncome.Amount = 0;
                    db.Add(existingIncome);
                }

                // TransactionCheckPoints
                var existingTransactionCheckPoints = progressFrom.TransactionCheckPoints;
                foreach (var existingTransactionCheckPoint in existingTransactionCheckPoints)
                {
                    existingTransactionCheckPoint.BudgetId = nextBudget.Id;
                    existingTransactionCheckPoint.Description = string.Empty;
                    db.Add(existingTransactionCheckPoint);
                }

                // budget Categories
                var absCategories = db.GetData<AbstractCategory>();
                foreach (var nBudgetItem in progressFrom.Items)
                {
                    //var nBudgetItem = new BudgetItem
                    //{
                    //    BudgetAmount = absCategory.DefaultAmount,
                    //    BudgetId = nextBudget.Id,
                    //    CategoryName = absCategory.CategoryName,
                    //    AbstractCategoryId = absCategory.Id,
                    //    GroupId = absCategory.GroupId,
                    //    StatusAmount = 0,
                    //};
                    nBudgetItem.BudgetId = nextBudget.Id;
                    nBudgetItem.StatusAmount = 0;
                    db.Add(nBudgetItem);
                }

                I_Message message = I_Message.Genertae(IMessageTypeEnum.Info);
                message.Title = "Budget Progressed";
                message.Message = progressFrom.Title;
                message.ExtraData = nextBudget.Title;
                db.Add(message);

                db.Commit();
                return db.GetSingle<Budget>(new SearchParameters { BudgetDate = nextBudget.Month });
            }
            catch (Exception ex)
            {
                db.RollBack();
                throw ex;
            }
        }

        public static void HandleAutoTransactions(DbAccess db)
        {
            var autoTrans = db.GetData<AbstractAutoTransaction>(new SearchParameters { });
            var currentBudget = db.GetSingle<Budget>(new SearchParameters { BudgetDate = DateTime.Now.FirstDayOfMonth() });

            if (currentBudget != null)
            {
                foreach (var autoTran in autoTrans.Where(a => a.Active).ToList())
                {
                    if (autoTran.BudgetId != currentBudget.Id && autoTran.DayOfTheMonth <= DateTime.Now.Day)
                    {
                        BudgetItem matchingCategory = GetMatchingcategory(db, autoTran.AbstractCategoryId, currentBudget.Id);
                        if (matchingCategory != null)
                        {

                            // add auto tran amount to status
                            matchingCategory.StatusAmount += autoTran.DefaultAmount;
                            db.Update(matchingCategory);

                            autoTran.BudgetId = currentBudget.Id;
                            autoTran.LastPaymentDate = currentBudget.Title;
                            db.Update(autoTran);

                            I_Message message = I_Message.Genertae(IMessageTypeEnum.Info);
                            message.Title = "Auto Transaction Applied";
                            message.Message = autoTran.Name;
                            message.ExtraData = autoTran.DefaultAmount.ToNumberFormat();
                            db.Add(message);
                        }
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

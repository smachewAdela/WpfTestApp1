using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBalanceTracker
{
    public partial class BudgetList :BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.XTitle = "רשימת תקציבים";
        }

        public List<BudgetMonth> Budgets
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return context.BudgetMonth.ToList();
            }
        }

        [WebMethod]
        public static string deleteBudget(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();
            int budgetId = req.budgetId;

            //var BudgetItemToDelete = Db.GetSingle<Budget>(new SearchParameters { BudgetId = (int)req.budgetId });
            //if (BudgetItemToDelete != null)
            //{
            //    var db = Global.Db;
            //    try
            //    {
            //        db.BeginTransaction();

            //        // incomes
            //        foreach (var ei in BudgetItemToDelete.Incomes)
            //        {
            //            db.Delete(ei);
            //        }
            //        // TransactionCheckPoints
            //        foreach (var ei in BudgetItemToDelete.TransactionCheckPoints)
            //        {
            //            db.Delete(ei);
            //        }

            //        // budget Categories
            //        foreach (var ei in BudgetItemToDelete.Items)
            //        {
            //            db.Delete(ei);
            //        }

            //        db.Delete(BudgetItemToDelete);

            //        db.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        db.RollBack();
            //        I_Message.HandleException(ex, db);
            //        throw new HttpException((int)HttpStatusCode.BadRequest, "Budget not created, check system log for more info");
            //    }
            //}

            Global.RefreshBudget();
            return "Posted";
        }


        [WebMethod]
        public static string generateBudget(string userdata)
        {
            var currentBudget = Global.CurrentBudget;
            if (currentBudget == null)
                currentBudget = Global.GetLatestBudget();

            if (currentBudget == null)
                currentBudget = Global.GenerateDefaultInitialBudget();

            try
            {
                using (var context = new BalanceAdmin_Entities())
                {
                    var latestBudgetDate = context.BudgetMonth.Max(x => x.Month).AddMonths(1);
                    //AutomationHelper.GenerateBudget(db, currentBudget, latestBudgetDate);
                    //Global.RefreshBudget();
                }
            }
            catch (Exception ex)
            {
                //I_Message.HandleException(ex, db);
                throw new HttpException((int)HttpStatusCode.BadRequest, "Budget not created, check system log for more info");
            }

            return "Posted";
        }
    }
}
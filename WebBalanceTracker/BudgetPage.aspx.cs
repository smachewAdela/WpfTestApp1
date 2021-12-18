using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBalanceTracker
{
    public partial class BudgetPage : BaseForm 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.XTitle = "הגדרת תקציב";
        }

        public List<BudgetGroup> BudgetGroups
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return context.BudgetGroup.ToList();
            }
        }

        [WebMethod]
        public static string updateBudget(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();

            //var BudgetItemToUpdate = Db.GetSingle<BudgetItem>(new SearchParameters { BudgetItemId = (int)req.budgetItemId });
            //BudgetItemToUpdate.BudgetAmount = (int)req.newBudgetAmount;
            //Global.Db.Update(BudgetItemToUpdate);

            Global.RefreshBudget();
            return "Posted";
        }

    }
}
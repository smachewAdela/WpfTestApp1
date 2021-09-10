using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public List<GroupData> BudgetGroups
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(5);
                var gData = new List<GroupData>();

                var currentBudget = Global.CurrentBudget;
                var gGroups = Db.GetData<BudgetGroup>(new SearchParameters { });

                foreach (var g in gGroups)
                {
                    g.BudgetItems = currentBudget.Items.Where(x => x.GroupId == g.Id).ToList();
                    gData.Add(new GroupData(g));
                }

                return gData;
            }
        }

        [WebMethod]
        public static string updateBudget(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();

            var BudgetItemToUpdate = Db.GetSingle<BudgetItem>(new SearchParameters { BudgetItemId = (int)req.budgetItemId });
            BudgetItemToUpdate.BudgetAmount = (int)req.newBudgetAmount;
            Global.Db.Update(BudgetItemToUpdate);

            Global.RefreshBudget();
            return "Posted";
        }

    }
}
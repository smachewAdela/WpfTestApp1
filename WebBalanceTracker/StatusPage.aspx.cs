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
    public partial class StatusPage : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.XTitle = "דוח מצב";
        }

        public DataTable BudgetGroups
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(5);

                var currentBudget = Global.CurrentBudget;
                var gGroups = Db.GetData<BudgetGroup>(new SearchParameters { });

                foreach (var g in gGroups)
                {
                    var rw = tbl.NewRow();
                    g.BudgetItems = currentBudget.Items.Where(x => x.GroupId == g.Id).ToList();

                    rw[0] = g.Name;
                    rw[1] = g.BudgetStr;
                    rw[2] = g.StatusStr;
                    rw[3] = $"{g.Ratio}%";
                    rw[4] = "0";
                    tbl.Rows.Add(rw);
                }
                var totalRow = tbl.NewRow();
                BudgetGroup total = new BudgetGroup
                {
                    Name = "",
                    BudgetItems = currentBudget.Items,
                    IsTotal = true
                };

                totalRow[0] = total.Name;
                totalRow[1] = total.BudgetStr;
                totalRow[2] = total.StatusStr;
                totalRow[3] = $"{total.Ratio}%";
                totalRow[4] = "1";
                tbl.Rows.Add(totalRow);
                //gGroups.Add(total);

                return tbl;
            }
        }

        [WebMethod]
        public static string updateSelection(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();
            int dir = 1;
            if (req.direction == "prev")
                dir = -1;

            Global.ProgressMonthSelection(dir);
            //HttpContext.Current.Response.Redirect(req.source.ToString());
            return "Posted";
        }
    }
}
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
                tbl.AddColumns(6);
                if (Global.CurrentBudget != null)
                {
                    using (var context = new BalanceAdmin_Entities())
                    {
                        var currentBudget = Global.CurrentBudget;
                        var gGroups = context.BudgetGroup.ToList();

                        if (gGroups.IsNotEmpty())
                        {
                            foreach (var g in gGroups)
                            {
                                var rw = tbl.NewRow();

                                rw[0] = g.Name;
                                rw[1] = currentBudget.BudgetForGroup(g.Id);// g.BudgetStr;
                                rw[2] = currentBudget.StatusForGroup(g.Id); 
                                rw[3] = currentBudget.RatioForGroup(g.Id); 
                                rw[4] = currentBudget.IsGroupOverSpent(g.Id) ? "1" : "0";
                                tbl.Rows.Add(rw);
                            }
                            var totalRow = tbl.NewRow();

                            totalRow[0] = currentBudget.Name;
                            totalRow[1] = currentBudget.TotalBudget;
                            totalRow[2] = currentBudget.TotalExpenses;
                            totalRow[3] = $"{currentBudget.Ratio}%";
                            totalRow[4] = currentBudget.IsOverSpent ? "1" : "0";
                            totalRow[5] = "1"; // is total
                            tbl.Rows.Add(totalRow);
                        }
                    }
                }

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


        [WebMethod]
        public static string changeGlobalBudget(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();

            int nextBudget = req.nextBudget;
            Global.ChangeGlobalBudget(nextBudget);
            //HttpContext.Current.Response.Redirect(req.source.ToString());
            return "Posted";
        }
    }
}
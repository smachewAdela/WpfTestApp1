using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBalanceTracker
{
    public partial class Categories : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "קטגוריות";
        }


        public DataTable BudgetCategories
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(3);

                var currentBudget = Global.CurrentBudget;
                var cats = currentBudget.Items;
                var Groups = Db.GetData<BudgetGroup>(new SearchParameters { }).ToDictionary(x => x.Id, x => x.Name);

                foreach (var g in cats)
                {
                    var rw = tbl.NewRow();

                    rw[0] = g.CategoryName;
                    rw[1] = Groups[g.GroupId];
                    rw[2] = g.Id;
                    tbl.Rows.Add(rw);
                }
             

                return tbl;
            }
        }
    }
}
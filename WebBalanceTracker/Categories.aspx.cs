using Newtonsoft.Json.Linq;
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
    public partial class Categories : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "קטגוריות";
            Groups = Db.GetData<BudgetGroup>(new SearchParameters { }).ToDictionary(x => x.Id, x => x.Name);
            foreach (var item in Groups)
            {
                cmbGroups.Items.Add(new System.Web.UI.WebControls.ListItem { Text = item.Value, Value = item.Key.ToString() });
            }
        }
        public Dictionary<int,string> Groups { get; set; }

        public DataTable BudgetCategories
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(4);

                var currentBudget = Global.CurrentBudget;
                var cats = currentBudget.Items;

                foreach (var g in cats)
                {
                    var rw = tbl.NewRow();

                    rw[0] = g.CategoryName;
                    rw[1] = Groups[g.GroupId];
                    rw[2] = g.Id;
                    rw[3] = g.GroupId;
                    tbl.Rows.Add(rw);
                }

                return tbl;
            }
        }


        [WebMethod]
        public static string updateSelection(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();

            //var values = (object[])parameter;
            //var newGroupId = (int)values[0];
            //var chAll = (bool)values[1];
            //var cItem = (BudgetItem)values[2];

            //if (chAll)
            //{
            //    var allBudgetItems = GlobalsProviderBL.Db.GetData<BudgetItem>().Where(x => x.CategoryName == cItem.CategoryName).ToList();
            //    foreach (var allBudgetItem in allBudgetItems)
            //    {
            //        allBudgetItem.GroupId = newGroupId;
            //        GlobalsProviderBL.Db.Update(allBudgetItem);
            //    }
            //}
            //else
            //    GlobalsProviderBL.Db.Update(cItem);

            return "Posted";
        }
    }
}
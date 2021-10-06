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
            this.XTitle = "קטגוריות";
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
                tbl.AddColumns(6);

                var currentBudget = Global.CurrentBudget;
                var cats = currentBudget.Items;

                var AbstractCategory = Global.Db.GetData<AbstractCategory>().ToDictionary(x => x.Id, x => x.CategoryName);
                AbstractCategory.Add(0, "לא הוגדר");

                foreach (var g in cats)
                {
                    var rw = tbl.NewRow();

                    rw[0] = g.CategoryName;
                    rw[1] = Groups[g.GroupId];
                    rw[2] = g.Id;
                    rw[3] = g.GroupId;
                    rw[4] = g.BudgetAmount;
                    rw[5] = AbstractCategory[g.AbstractCategoryId];
                    tbl.Rows.Add(rw);
                }

                return tbl;
            }
        }


        [WebMethod]
        public static string upsertCategory(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();
            var lBudget = Global.GetLatestBudget();
            if (req.editedId == 0)
            {
                BudgetItem upsertC = new BudgetItem
                {
                    CategoryName = req.catName,
                    GroupId = req.groupId,
                    BudgetAmount = req.budget,
                    StatusAmount = 0,
                    BudgetId = lBudget.Id,
                    Id = req.editedId
                };
                Db.Insert(upsertC);
            }
            else
            {
                var itemToUpdate = Db.GetSingle<BudgetItem>(new SearchParameters { BudgetItemId = req.editedId });
                itemToUpdate.CategoryName = req.catName;
                itemToUpdate.GroupId = req.groupId;
                itemToUpdate.BudgetAmount = req.budget;

                Db.Update(itemToUpdate);
            }

            Global.RefreshBudget();
            return "Posted";
        }
    }
}
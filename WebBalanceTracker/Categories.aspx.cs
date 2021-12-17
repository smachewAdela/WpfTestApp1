using Newtonsoft.Json.Linq;
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
            using (var context = new BalanceAdmin_Entities())
            {
                Groups = context.BudgetGroup.ToDictionary(x => x.Id, x => x.Name);
                foreach (var item in Groups)
                {
                    cmbGroups.Items.Add(new System.Web.UI.WebControls.ListItem { Text = item.Value, Value = item.Key.ToString() });
                }
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
                if (currentBudget != null)
                {
                    using (var context = new BalanceAdmin_Entities())
                    {
                        var cats = context.AbstractCategory.ToList();


                        foreach (var g in cats)
                        {
                            var rw = tbl.NewRow();

                            rw[0] = g.Name;
                            rw[1] = Groups[g.BudgetGroupId];
                            rw[2] = g.Id;
                            rw[3] = g.BudgetGroupId;
                            rw[4] = g.Amount;
                            rw[5] = string.Empty;
                            tbl.Rows.Add(rw);
                        }
                    }
                }

                return tbl;
            }
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
        public static string upsertCategory(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();
            var lBudget = Global.GetLatestBudget();
            //if (req.editedId == 0)
            //{
            //    BudgetItem upsertC = new BudgetItem
            //    {
            //        CategoryName = req.catName,
            //        GroupId = req.groupId,
            //        BudgetAmount = req.budget,
            //        StatusAmount = 0,
            //        BudgetId = lBudget.Id,
            //        Id = req.editedId
            //    };
            //    Db.Insert(upsertC);
            //}
            //else
            //{
            //    var itemToUpdate = Db.GetSingle<BudgetItem>(new SearchParameters { BudgetItemId = req.editedId });
            //    itemToUpdate.CategoryName = req.catName;
            //    itemToUpdate.GroupId = req.groupId;
            //    itemToUpdate.BudgetAmount = req.budget;

            //    Db.Update(itemToUpdate);
            //}

            Global.RefreshBudget();
            return "Posted";
        }
    }
}
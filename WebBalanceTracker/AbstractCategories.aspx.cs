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
    public partial class AbstractCategories : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.XTitle = "קטגוריות ברירת מחדל";
            using (var context = new BalanceAdmin_Entities())
            {
                Groups =context.BudgetGroup.ToDictionary(x => x.Id, x => x.Name);
                foreach (var item in Groups)
                {
                    cmbGroups.Items.Add(new System.Web.UI.WebControls.ListItem { Text = item.Value, Value = item.Key.ToString() });
                }
            }
        }

        public Dictionary<int, string> Groups { get; set; }
        public DataTable BudgetCategories
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(5);
                using (var context = new BalanceAdmin_Entities())
                {
                    var cats = context.AbstractCategory.OrderBy(x => x.Name).ToList();

                    foreach (var g in cats)
                    {
                        var rw = tbl.NewRow();

                        rw[0] = g.Name;
                        rw[1] = Groups[g.BudgetGroupId];
                        rw[2] = g.Id;
                        rw[3] = g.BudgetGroupId;
                        rw[4] = g.Amount;
                        tbl.Rows.Add(rw);
                    }
                }
                return tbl;
            }
        }

        [WebMethod]
        public static string upsertCategory(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();
            var lBudget = Global.GetLatestBudget();
            using (var context = new BalanceAdmin_Entities())
            {
                int entityId = req.req.editedId;
                if (entityId == 0)
                {
                    context.AbstractCategory.Add(new AbstractCategory
                    {
                        Name = req.catName,
                        BudgetGroupId = req.groupId,
                        Amount = req.budget,
                        Active = true,
                    });
                }
                else
                {
                    var existing = context.AbstractCategory.SingleOrDefault(x => x.Id == entityId);
                    existing.Name = req.catName;
                    existing.BudgetGroupId = req.groupId;
                    existing.Amount = req.budget;

                }
                context.SaveChanges();
            }
            Global.RefreshBudget();
            return "Posted";
        }
    }
}
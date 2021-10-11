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
    public partial class AbstractCategories : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.XTitle = "קטגוריות ברירת מחדל";
            Groups = Db.GetData<BudgetGroup>(new SearchParameters { }).ToDictionary(x => x.Id, x => x.Name);
            foreach (var item in Groups)
            {
                cmbGroups.Items.Add(new System.Web.UI.WebControls.ListItem { Text = item.Value, Value = item.Key.ToString() });
            }
        }

        public Dictionary<int, string> Groups { get; set; }
        public DataTable BudgetCategories
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(5);

                var cats = Db.GetData<AbstractCategory>(new SearchParameters { }).OrderBy(x => x.CategoryName).ToList();

                foreach (var g in cats)
                {
                    var rw = tbl.NewRow();

                    rw[0] = g.CategoryName;
                    rw[1] = Groups[g.GroupId];
                    rw[2] = g.Id;
                    rw[3] = g.GroupId;
                    rw[4] = g.DefaultAmount;
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
            var upsertC = new AbstractCategory
            {
                CategoryName = req.catName,
                GroupId = req.groupId,
                DefaultAmount = req.budget,
                DayInMonth = 10,
                Active = true,
                Id = req.editedId
            };
            if (req.editedId == 0)
                Global.Db.Add(upsertC);
            else
                Global.Db.Update(upsertC);

            Global.RefreshBudget();
            return "Posted";
        }
    }
}
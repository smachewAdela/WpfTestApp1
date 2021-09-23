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
    public partial class BudgetGroups : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XTitle = "קבוצות";
        }

        public DataTable BudgetGroupItems
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(2);

                var cats = Db.GetData<BudgetGroup>(new SearchParameters { }).ToList();

                foreach (var g in cats)
                {
                    var rw = tbl.NewRow();

                    rw[0] = g.Name;
                    rw[1] = g.Id;
                    tbl.Rows.Add(rw);
                }

                return tbl;
            }
        }

     

        [WebMethod]
        public static string upsertGroup(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();
            var g = new BudgetGroup
            {
                Name = req.groupName,
                Id = req.groupId
            };
            if (g.Id == 0)
                Global.Db.Insert(g);
            else
                Global.Db.Update(g);
            //Global.RefreshBudget();
            return "Posted";
        }


    }
}
using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WpfTestApp1.MVVM.Model;

namespace WebBalanceTracker
{
    public partial class AutoTransactions : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.XTitle = "תנועות אוטומטיות";
            AbstractCategories = Db.GetData<AbstractCategory>(new SearchParameters { }).ToDictionary(x => x.Id, x => x.CategoryName);
            foreach (var item in AbstractCategories)
            {
                cmbAbstractCategories.Items.Add(new System.Web.UI.WebControls.ListItem { Text = item.Value, Value = item.Key.ToString() });
            }
        }

        public Dictionary<int, string> AbstractCategories { get; set; }


        public DataTable AutoTransactionItems
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(8);

                var cats = Db.GetData<AbstractAutoTransaction>(new SearchParameters { });

                foreach (var g in cats)
                {
                    var rw = tbl.NewRow();

                    rw[0] = g.Name;
                    rw[1] = AbstractCategories[g.AbstractCategoryId];
                    rw[2] = g.DefaultAmount;
                    rw[3] = g.Active ? "פעיל" : "לא פעיל";
                    rw[4] = g.DayOfTheMonth;
                    rw[5] = string.IsNullOrEmpty( g.LastPaymentDate) ? string.Empty : g.LastPaymentDate;
                    rw[6] = g.Id;
                    rw[7] = g.AbstractCategoryId;
                    tbl.Rows.Add(rw);
                }

                return tbl;
            }
        }

        [WebMethod]
        public static string upsertTransaction(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();
            var lBudget = Global.GetLatestBudget();
            var upsertC = new AbstractAutoTransaction
            {
                Name = req.name,
                AbstractCategoryId = req.abstractCategoryId,
                DefaultAmount = req.defaultAmount,
                DayOfTheMonth = req.dayOfTheMonth,
                Active = bool.Parse( req.active.ToString()),
                Id = req.editedId
            };
            if (req.editedId == 0)
                Global.Db.Insert(upsertC);
            else
                Global.Db.Update(upsertC);

            Global.RefreshBudget();
            return "Posted";
        }

    }
}
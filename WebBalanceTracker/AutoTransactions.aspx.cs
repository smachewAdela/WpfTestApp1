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
    public partial class AutoTransactions : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.XTitle = "תנועות אוטומטיות";
            using (var context = new BalanceAdmin_Entities())
            {
                AbstractCategories = context.BudgetAutoTransaction.ToDictionary(x => x.Id, x => x.Description);
                foreach (var item in AbstractCategories)
                {
                    cmbAbstractCategories.Items.Add(new System.Web.UI.WebControls.ListItem { Text = item.Value, Value = item.Key.ToString() });
                }
            }
        }

        public Dictionary<int, string> AbstractCategories { get; set; }


        public DataTable AutoTransactionItems
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(8);

                using (var context = new BalanceAdmin_Entities())
                {
                    foreach (var g in context.BudgetAutoTransaction)
                    {
                        var rw = tbl.NewRow();

                        rw[0] = g.Description;
                        rw[1] = AbstractCategories[g.AbstractCatrgoryId];
                        rw[2] = g.Amount;
                        rw[3] = g.Active ? "פעיל" : "לא פעיל";
                        rw[4] = g.DayInMonth;
                        rw[5] =  string.Empty ;
                        rw[6] = g.Id;
                        rw[7] = g.AbstractCatrgoryId;
                        tbl.Rows.Add(rw);
                    }
                }

                return tbl;
            }
        }

        [WebMethod]
        public static string upsertTransaction(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();
            var lBudget = Global.GetLatestBudget();
            int entId = req.editedId;
            using (var context = new BalanceAdmin_Entities())
            {

                if (entId == 0)
                {
                    context.BudgetAutoTransaction.Add(new BudgetAutoTransaction 
                    {
                        Description = req.name,
                        AbstractCatrgoryId = req.abstractCategoryId,
                        Amount = req.defaultAmount,
                        DayInMonth = req.dayOfTheMonth,
                        Active = bool.Parse(req.active.ToString()),
                        Id = req.editedId
                    });
                }
                else
                {
                    var exsisting = context.BudgetAutoTransaction.SingleOrDefault(x => x.Id == entId);
                    exsisting.Description = req.name;
                    exsisting.AbstractCatrgoryId = req.abstractCategoryId;
                    exsisting.Amount = req.defaultAmount;
                    exsisting.DayInMonth = req.dayOfTheMonth;
                    exsisting.Active = bool.Parse(req.active.ToString());

                }
                context.SaveChanges();
            }
            Global.RefreshBudget();
            return "Posted";
        }

    }
}
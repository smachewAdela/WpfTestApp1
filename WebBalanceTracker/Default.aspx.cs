using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WpfTestApp1;

namespace WebBalanceTracker
{
    public partial class _Default : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "דף הבית";
        }

        public List<BudgetGroup> BudgetGroups
        {
            get
            {
                var currentBudget = Global.CurrentBudget;
                var gGroups = Db.GetData<BudgetGroup>(new SearchParameters { });

                foreach (var g in gGroups)
                    g.BudgetItems = currentBudget.Items.Where(x => x.GroupId == g.Id).ToList();

                BudgetGroup total = new BudgetGroup
                {
                    Name = "",
                    BudgetItems = currentBudget.Items,
                    IsTotal = true
                };
                gGroups.Add(total);
                return gGroups;
            }
        }
    }
}
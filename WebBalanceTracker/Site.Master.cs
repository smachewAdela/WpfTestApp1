using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBalanceTracker
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private static object obj = new object();

       

        public bool hideBudgetNavigator { get; set; }
        public string xTitle { get; set; }

        public string MyXTitle
        {
            get
            {
                var res = xTitle;
#if DEBUG
                res = " [TEST TEST TEST] " + res;
#endif
                return res;
                ;
            }
        }

        public Dictionary<int,string> Budgets
        {
            get
            {
                using (var context = new BalanceAdmin_Entities())
                    return context.BudgetMonth.ToDictionary(x => x.Id, x => x.Name);
            }
        }
    }
}
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
    }
}
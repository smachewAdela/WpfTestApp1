using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebBalanceTracker
{
    public class BaseForm : System.Web.UI.Page
    {

        public static string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["connectionString"];
        }

        bool hideBudgetNavigator;
        public virtual bool HideBudgetNavigator
        {
            get
            {
                return hideBudgetNavigator;
            }
            set
            {
                hideBudgetNavigator = value;
                (this.Master as SiteMaster) .hideBudgetNavigator = value;
            }
        }

        string xt;
        public virtual string XTitle
        {
            get
            {
                return xt;
            }
            set
            {
                xt = value;
                (this.Master as SiteMaster).xTitle = value;
            }
        }
    }
}
using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WpfTestApp1;

namespace WebBalanceTracker
{
    public class BaseForm : System.Web.UI.Page
    {
        static DbAccess bd;
        public static DbAccess Db
        {
            get
            {
                if (bd == null)
                {
                    string connStr = GetConnectionString();
                    bd = new DbAccess(connStr);
                }
                return bd;
            }
        }

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
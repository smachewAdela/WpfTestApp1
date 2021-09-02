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
#if DEBUG
            return ConfigurationManager.AppSettings["connectionString"];
#else
            return ConfigurationManager.AppSettings["prodconnectionString"];
#endif
        }

    }
}
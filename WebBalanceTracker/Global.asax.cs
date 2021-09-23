using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using WpfTestApp1.MVVM.Model.Automation;

namespace WebBalanceTracker
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

#if DEBUG
#else
            var subject = "WebBalanceTracker Started !";
            EmailHelper.SendMail(subject, string.Empty);
# endif
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

            // budgets
            var newestBudget = Db.GetData<Budget>().OrderByDescending(x => x.Month).First();
            var targetDate = DateTime.Now.FirstDayOfMonth();
            var dateToProcess = newestBudget.Month.AddMonths(1);
            while (dateToProcess.Month < targetDate.Month)
            {
                newestBudget = AutomationHelper.GenerateBudget(Db, newestBudget);
                dateToProcess = newestBudget.Month.AddMonths(1);
            }


            // AutoTransactions
            var date = DateTime.Now.FirstDayOfMonth();
            var latestBudget = Db.GetSingle<Budget>(new SearchParameters { BudgetDate = date });
            while (latestBudget == null)
            {
                date = date.AddMonths(-1);
                latestBudget = Db.GetSingle<Budget>(new SearchParameters { BudgetDate = date });
            }
            AutomationHelper.HandleAutoTransactions(Db, latestBudget);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if(ex is ThreadAbortException)
                return;

            var subject = "WebBalanceTracker error !";
            var body = ex.Message;

            EmailHelper.SendMail(subject, body);
        }

        static DbAccess budgetDb;
        public static DbAccess Db
        {
            get
            {
                if (budgetDb == null)
                {
                    string connStr = GetConnectionString();
                    budgetDb = new DbAccess(connStr);
                }
                return budgetDb;
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

        public static ISettings Settings
        {
            get
            {
                return Db.GetData<ISettings>().FirstOrDefault();
            }
        }

        internal static List<TransactionCheckPoint> GenerateDefaultCheckPoints()
        {
            var t = new List<TransactionCheckPoint>();
            var checkPontNames = ConfigurationManager.AppSettings["checkPontNames"].Split(',').ToList();
            foreach (var checkPontName in checkPontNames)
                t.Add(new TransactionCheckPoint { Name = checkPontName });

            return t;
        }

        internal static Budget GetLatestBudget()
        {

            var d = DateTime.Now.Date;
            Budget b = null;
            do
            {
                b = Db.GetSingle<Budget>(new SearchParameters { BudgetDate = d.FirstDayOfMonth() });
                d = d.AddMonths(-1);
            }
            while (b == null);

            return b;
        }

        internal static void RefreshBudget()
        {
            b = GetLatestBudget();
        }

        internal static void ProgressMonthSelection(int dir)
        {
            var nextDate = CurrentBudget.Month.AddMonths(dir);
            var nextB = Db.GetSingle<Budget>(new SearchParameters { BudgetDate = nextDate });
            if (nextB != null)
            {
                CurrentBudget = nextB;
            }
        }
        
        static Budget b;
        public static Budget CurrentBudget
        {
            get
            {
                if (b == null)
                    b = GetLatestBudget();
                return b;
            }
            private set
            {
                b = value;
            }
        }


        public static string CurrentTitle
        {
            get
            {
                return CurrentBudget.Title;
            }
        }
    }
}
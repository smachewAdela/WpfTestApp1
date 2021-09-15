using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace QBalanceDesktop
{
    public static class GlobalsProviderBL
    {
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
                {
                    //var d = DateTime.Now.Date;
                    //do
                    //{
                    //    b = Db.GetSingle<Budget>(new SearchParameters { BudgetDate = d.FirstDayOfMonth() });
                    //    d = d.AddMonths(-1);
                    //}
                    //while (b == null);
                    b = GetLatestBudget();
                }
                return b;
            }
            private set
            {
                b = value;
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

    }
}

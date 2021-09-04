﻿using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WebBalanceTracker
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
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

        internal static void ProgressMonth(int dir)
        {
            var nextDate = CurrentBudget.Month.AddMonths(dir);
            var nextB = Db.GetSingle<Budget>(new SearchParameters { BudgetDate = nextDate });
            if (nextB != null)
            {
                CurrentBudget = nextB;
            }
        }

        internal static void GenerateNextMonth()
        {
            var latestBudget = Db.GetData<Budget>().OrderByDescending(x => x.Month).First();
            var nextMonthI = new Budget
            {
                Month = latestBudget.Month.AddMonths(1)
            };

            Db.Insert(nextMonthI);

            foreach (var budgetItem in latestBudget.Items)
            {
                budgetItem.BudgetId = nextMonthI.Id;
                budgetItem.StatusAmount = 0;
                Db.Insert(budgetItem);
            }

            foreach (var income in latestBudget.Incomes)
            {
                income.BudgetId = nextMonthI.Id;
                income.Amount = 0;
                Db.Insert(income);
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


        public static string CurrentTitle
        {
            get
            {
                return CurrentBudget.Title;
            }
        }
    }
}
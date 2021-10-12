using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using WpfTestApp1.MVVM.Model;
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

            HandleOneTimeJobs();

            var backgroundWorker = new BackgroundWorkerDispatcher();
            backgroundWorker.Workers.Add(new MessagesWorker());
            backgroundWorker.Start();

#if DEBUG
#else
            I_Message message = I_Message.Genertae(IMessageTypeEnum.Info);
            message.Title = "WebBalanceTracker Started !";
            message.SendMail = true;
            Db.Insert(message);
#endif
        }

        private void HandleOneTimeJobs()
        {
            var performUniqueJobs = ConfigurationManager.AppSettings["OnStartUpJobs"];
            if (!string.IsNullOrEmpty(performUniqueJobs))
            {
                var jobs = performUniqueJobs.Split(';').ToList();
                if (jobs.Any(x => x.Equals("abstractCategoriesFromCurrentBudget")))
                {
                    var currentBudgetCategories = Global.CurrentBudget.Items;

                    Db.BeginTransaction();

                    try
                    {
                        foreach (var currentBudgetCategory in currentBudgetCategories)
                        {
                            var upsertC = new AbstractCategory
                            {
                                CategoryName = currentBudgetCategory.CategoryName,
                                GroupId = currentBudgetCategory.GroupId,
                                DefaultAmount = currentBudgetCategory.BudgetAmount,
                                DayInMonth = 10,
                                Active = true,
                            };
                            Db.Add(upsertC);

                            currentBudgetCategory.AbstractCategoryId = upsertC.Id;
                            Db.Update(currentBudgetCategory);
                        }
                        Db.Commit();
                    }
                    catch (Exception ex)
                    {
                        Db.RollBack();
                        I_Message message = I_Message.Genertae(IMessageTypeEnum.Error);
                        message.Title = "OnStartUpJobs";
                        message.Message = "Error on abstractCategoriesFromCurrentBudget";
                        message.ExtraData = ex.Message;
                        message.SendMail = true;
                        Db.Add(message);
                    }
                }
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

            Thread automationThread = new Thread(HandleAutomaticjobs);
            automationThread.Start();
        }
        private static object obj = new object();

        private void HandleAutomaticjobs()
        {
            lock (obj)
            {
                // budgets
                //var newestBudget = Db.GetData<Budget>().OrderByDescending(x => x.Month).First();
                //var targetDate = DateTime.Now.FirstDayOfMonth();
                //var dateToProcess = newestBudget.Month.AddMonths(1);
                //while (dateToProcess.Month <= targetDate.Month)
                //{
                //    newestBudget = AutomationHelper.GenerateBudget(Db, newestBudget);
                //    dateToProcess = newestBudget.Month.AddMonths(1);
                //}


                // AutoTransactions
                //var date = DateTime.Now.FirstDayOfMonth();
                //Budget latestBudget = null; // Global.GetLatestBudget();
                //while (latestBudget == null)
                //{
                //    latestBudget = Db.GetSingle<Budget>(new SearchParameters { BudgetDate = date });
                //    date = date.AddMonths(-1);
                //}
                //AutomationHelper.HandleAutoTransactions(Db, latestBudget);

                
                var oldestBudget = Global.GetLatestBudget();
                var lastFirstOfMonth = DateTime.Now.FirstDayOfMonth();

                //var newestBudget = Db.GetSingle<Budget>(new SearchParameters { BudgetDate = date });
                //if (newestBudget == null)
                //{
                //    AutomationHelper.GenerateBudget(Db, oldestBudget, date.Date);
                //    RefreshBudget();
                //}
                while (oldestBudget.Month.Date < lastFirstOfMonth)
                {
                    oldestBudget = AutomationHelper.GenerateBudget(Db, oldestBudget);
                }

            }
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if(ex is ThreadAbortException)
                return;

            var subject = "WebBalanceTracker error !";
            var body = ex.Message;

            I_Message message = I_Message.Genertae(IMessageTypeEnum.Error);
            message.Title = subject;
            message.Message = body;
            message.ExtraData = ex.StackTrace;
            message.SendMail = true;
            Db.Add(message);
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
            return ConfigurationManager.AppSettings["connectionString"];
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
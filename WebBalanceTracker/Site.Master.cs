using QBalanceDesktop;
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
            //lock (obj)
            //{
            //    //GenerateAbstractCategories();
            //}
        }

        private static object obj = new object();

        private void GenerateAbstractCategories()
        {
            var latestBudget = Global.Db.GetSingle<Budget>(new SearchParameters { BudgetDate = new DateTime(2021,09,01) });
            var ttl = latestBudget.Title;

            foreach (var req in latestBudget.Items)
            {
                var upsertC = new AbstractCategory
                {
                    CategoryName = req.CategoryName,
                    GroupId = req.GroupId,
                    DefaultAmount = req.BudgetAmount,
                    DayInMonth = 10,
                    Active = true,
                };
                Global.Db.Insert(upsertC);
            }
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
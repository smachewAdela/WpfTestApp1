using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBalanceTracker
{
    public partial class _Default : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.XTitle = "דף הבית";
            this.HideBudgetNavigator = true;
        }

        public string BudgetDataName
        {
            get
            {
                return Global.CurrentBudget.Name;
            }
        }

        public string TotalExpenses
        {
            get
            {
                return Global.CurrentBudget.TotalExpenses.ToNumberFormat();
            }
        }


        public string Totalincomes
        {
            get
            {
                return Global.CurrentBudget.Totalincomes.ToNumberFormat();
            }
        }

        

        public string LeftFromIncome
        {
            get
            {
                return Global.CurrentBudget.LeftFromIncome.ToNumberFormat();
            }
        }

        public string LefttoUse
        {
            get
            {
                return Global.CurrentBudget.LefttoUse.ToNumberFormat();
            }
        }
        public int Ratio
        {
            set { }
            get
            {
                // 75/100
                if (Global.CurrentBudget == null)
                    return 0;
                var status = Global.CurrentBudget.TotalExpenses;
                var budget = Global.CurrentBudget.TotalBudget;
                var res = (status * 100) / (budget == 0 ? 100 : budget);
                return res;
            }
        }


        [WebMethod]
        public static string getChartDataSource(string userdata)
        {
            if (Global.CurrentBudget == null)
                return string.Empty;

            dynamic req = userdata.ToDynamicJObject();
            var chartDdatasourceType = (string)req.datasourceType;
            dynamic cDs = ReportHandler.GenerateChartData(chartDdatasourceType);
            return cDs.ToString();
        }
    }
}
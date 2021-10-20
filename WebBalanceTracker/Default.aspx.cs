using Newtonsoft.Json.Linq;
using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WpfTestApp1;

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
                if (Global.CurrentBudget == null)
                    return string.Empty;
                return Global.CurrentBudget.Title;
            }
        }

        public string TotalExpenses
        {
            get
            {
                if (Global.CurrentBudget == null)
                    return string.Empty;
                return Global.CurrentBudget.Items.Sum(x => x.StatusAmount).ToNumberFormat();
            }
        }


        public string Totalincomes
        {
            get
            {
                if (Global.CurrentBudget == null)
                    return string.Empty;
                return Global.CurrentBudget.Incomes.Sum(x => x.Amount).ToNumberFormat();
            }
        }

        

        public string LeftFromIncome
        {
            get
            {
                if (Global.CurrentBudget == null)
                    return string.Empty;
                return (Global.CurrentBudget.Incomes.Sum(x => x.Amount) - Global.CurrentBudget.Items.Sum(x => x.StatusAmount)).ToNumberFormat();
            }
        }

        public string LefttoUse
        {
            get
            {
                if (Global.CurrentBudget == null)
                    return string.Empty;
                return (Global.CurrentBudget.Items.Sum(x => x.BudgetAmount) - Global.CurrentBudget.Items.Sum(x => x.StatusAmount)).ToNumberFormat();
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
                var status = Global.CurrentBudget.Items.Sum(x => x.StatusAmount);
                var budget = Global.CurrentBudget.Items.Sum(x => x.BudgetAmount);
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
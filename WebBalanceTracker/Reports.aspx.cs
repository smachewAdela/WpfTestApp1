using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBalanceTracker
{
    public partial class Reports : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XTitle = "דוחות";
        }

        public Dictionary<string, string> ReeportNames
        {
            get
            {
                return Enum.GetValues(typeof(ReportTypeEnum)).OfType< ReportTypeEnum>().ToDictionary(x => x.ToString(), x => x.GetEnumDescription());
            }
        }

        [WebMethod]
        public static string generateReport(string userdata)
        {
            try
            {
                dynamic req = userdata.ToDynamicJObject();
                var tbl = new DataTable();

                //var manager = new ReportManager();
                //ReportTypeEnum currentReport = (ReportTypeEnum)Enum.Parse(typeof(ReportTypeEnum), (string)req.reportType);
                //var CurrentReportContent = manager.GenerateReport(currentReport);

                //return JsonConvert.SerializeObject(CurrentReportContent.Table);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return string.Empty;
        }

    }

    public enum ReportTypeEnum
    {
        [Description("הכנסה מול תקציב")]
        IncomeVsBudget,
        [Description("הכנסה מול פעילות")]
        IncomeVsStatus,
        [Description("נתוני קבוצה")]
        GroupDetails,
        [Description("נתוני קטגוריה")]
        CategoryDetails,
        [Description("הכנסות")]
        IncomeDetails,
        [Description("הכנסות - מורחב")]
        IncomeDetailsExpanded,
        //[Description("יתרות")]
        //Balances,
        [Description("חריגות")]
        BudgetExceptions,
    }
}
using QBalanceDesktop;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp1.MVVM.Model
{
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


    public class ReportManager
    {

        public DbAccess db
        {
            get { return GlobalsProviderBL.Db; }
        }

        public ReportContent GenerateReport(ReportTypeEnum reportType)
        {
            IReportMaker reportMaker = GetMaker(reportType);
            ReportContent content = new ReportContent(reportType);

            var data = new ReportSourceData
            {
                Groups = db.GetData<BudgetGroup>(),
                Categories = db.GetData<BudgetItem>(),
                Incomes = db.GetData<BudgetIncomeItem>(),
                Months = db.GetData<Budget>()
            };
            reportMaker.FillData(content, db, data);

            return content;
        }

        private IReportMaker GetMaker(ReportTypeEnum reportType)
        {

            return new BaseReportMaker();
        }
    }
}

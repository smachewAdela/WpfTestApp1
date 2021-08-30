using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfTestApp1.MVVM.Model
{
    public class BaseReportMaker : IReportMaker
    {
        public virtual bool ExcludeCurrentMonth { get; set; }

        public BaseReportMaker()
        {
            ExcludeCurrentMonth = false;
        }

        public void FillData(ReportContent content, DbAccess db, ReportSourceData data)
        {
            FillHeaders(content, db, data);
            FillReportData(content, db, data);
        }

        private void FillReportData(ReportContent content, DbAccess db, ReportSourceData data)
        {
            var months = data.Months;
            var categories = data.Categories;
            var groups = data.Groups;
            var tbl = content.Table;
            var currentMonth = months.OrderByDescending(x => x.Month).First();

            switch (content.ReportType)
            {
                case ReportTypeEnum.IncomeVsBudget:
                    FillIncomeVsBudget(months, tbl, currentMonth);
                    break;
                case ReportTypeEnum.IncomeVsStatus:
                    FillIncomeVsStatus(months, tbl, currentMonth);
                    break;
                case ReportTypeEnum.GroupDetails:
                    FillGroupDetails(months, groups, tbl, currentMonth);
                    break;
                case ReportTypeEnum.IncomeDetailsExpanded:
                    FillIncomeDetailsExpanded(months, groups, tbl, currentMonth);
                    break;
                case ReportTypeEnum.CategoryDetails:
                    FillCategoryDetails(months, categories, tbl, currentMonth);
                    break;
                case ReportTypeEnum.IncomeDetails:
                    FillIncomeDetails(months, tbl, currentMonth);
                    break;
                //case ReportTypeEnum.Balances:
                //    break;
                case ReportTypeEnum.BudgetExceptions:
                    FillBudgetExceptions(months, tbl, currentMonth);
                    break;
                default:
                    break;
            }
        }

        private void FillIncomeDetailsExpanded(List<Budget> months, List<BudgetGroup> groups, System.Data.DataTable tbl, Budget currentMonth)
        {
            months.Remove(currentMonth);

            var allIncomes = GlobalsProviderBL.Db.GetData<BudgetIncomeItem>().GroupBy(x => x.Name).ToList();
            foreach (var allIncome in allIncomes)
            {
                var row = tbl.NewRow();
                row[0] = allIncome.Key;
                row[1] = allIncome.Count();
                row[2] = ((int)allIncome.Average(x => x.Amount)).ToNumberFormat();
                row[3] = ((int)allIncome.Sum(x => x.Amount)).ToNumberFormat();
                tbl.Rows.Add(row);
            }
        }

        private void FillBudgetExceptions(List<Budget> months, System.Data.DataTable tbl, Budget currentMonth)
        {
            months.Remove(currentMonth);

            var row = tbl.NewRow();
            var overSpentAvg = ((int)months.Average(x => x.OverSpentNumber));
            var overspentTotal = ((int)months.Sum(x => x.OverSpentNumber));

            var overspentAvgAmount = ((int)months.Average(x => x.OverSpentAmount));
            var overspentAmount = ((int)months.Sum(x => x.OverSpentAmount));

            row[0] = months.Count.ToNumberFormat();
            row[1] = overSpentAvg.ToNumberFormat();
            row[2] = (overspentTotal).ToNumberFormat();
            row[3] = overspentAvgAmount.ToNumberFormat();
            row[4] = (overspentAmount).ToNumberFormat();
            tbl.Rows.Add(row);
        }

        private void FillIncomeVsStatus(List<Budget> months, System.Data.DataTable tbl, Budget currentMonth)
        {
            months.Remove(currentMonth);

            var row = tbl.NewRow();
            var incomeAvg = ((int)months.Average(x => x.TotalIncomes));
            var statusAvg = ((int)months.Average(x => x.TotalExpenses));
            row[0] = incomeAvg.ToNumberFormat();
            row[1] = statusAvg.ToNumberFormat();
            row[2] = (incomeAvg - statusAvg).ToNumberFormat();
            row[3] = months.Count.ToNumberFormat();
            row[4] = ((incomeAvg - statusAvg) * months.Count).ToNumberFormat();
            tbl.Rows.Add(row);
        }

        private void FillIncomeVsBudget(List<Budget> months, System.Data.DataTable tbl, Budget currentMonth)
        {
            months.Remove(currentMonth);

            var row = tbl.NewRow();
            if (months.IsNotEmpty())
            {
                var incomeAvg = ((int)months.Average(x => x.TotalIncomes));
                var budget = ((int)months.First().TotalBudget);
                row[0] = incomeAvg.ToNumberFormat();
                row[1] = budget.ToNumberFormat();
                row[2] = (incomeAvg - budget).ToNumberFormat();
                row[3] = months.Count.ToNumberFormat();
                row[4] = ((incomeAvg - budget) * months.Count).ToNumberFormat();
            }
            tbl.Rows.Add(row);
        }

        private static void FillIncomeDetails(System.Collections.Generic.List<Budget> months, System.Data.DataTable tbl, Budget currentMonth)
        {
            months.Remove(currentMonth);

            var row = tbl.NewRow();
            row[0] = "הכנסות";
            row[1] = months.Count;
            row[2] = ((int)months.Average(x => x.TotalIncomes)).ToNumberFormat();
            row[3] = ((int)months.Average(x => x.TotalIncomes - x.TotalExpenses)).ToNumberFormat();
            row[4] = ((int)months.Sum(x => x.TotalIncomes)).ToNumberFormat();
            row[5] = ((int)months.Sum(x => x.TotalIncomes) - months.Sum(x => x.TotalExpenses)).ToNumberFormat();
            tbl.Rows.Add(row);
        }

        private static void FillCategoryDetails(System.Collections.Generic.List<Budget> months, System.Collections.Generic.List<BudgetItem> categories, System.Data.DataTable tbl, Budget currentMonth)
        {
            months.Remove(currentMonth);

            foreach (var c in currentMonth.Items.Where(x => x.Active).ToList())
            {
                var allCategoryInstances = categories.Where(x => x.CategoryName == c.CategoryName).ToList();

                var row = tbl.NewRow();
                row[0] = c.CategoryName;
                row[1] = c.BudgetAmount.ToNumberFormat();
                row[2] = ((int)allCategoryInstances.Average(x => x.StatusAmount)).ToNumberFormat();
                row[3] = ((int)allCategoryInstances.Sum(x => x.OverSpent)).ToNumberFormat();
                row[4] = ((int)allCategoryInstances.Sum(x => x.BudgetAmount) - allCategoryInstances.Sum(x => x.StatusAmount)).ToNumberFormat();

                if (allCategoryInstances.Sum(x => x.BudgetAmount) > 0)
                {
                    var ration = (allCategoryInstances.Sum(x => x.StatusAmount) * 100) / allCategoryInstances.Sum(x => x.BudgetAmount);
                    row[5] = $"{ ration }%";
                }
                else
                    row[5] = string.Empty;

                tbl.Rows.Add(row);
            }
        }

        private static void FillGroupDetails(System.Collections.Generic.List<Budget> months, System.Collections.Generic.List<BudgetGroup> groups, System.Data.DataTable tbl, Budget currentMonth)
        {
            foreach (var g in groups)
            {
                var row = tbl.NewRow();
                row[0] = g.Name;
                row[1] = currentMonth.Items.Where(x => x.GroupId == g.Id).Sum(x => x.BudgetAmount).ToNumberFormat();
                row[2] = ((int)months.Average(x => x.GroupStatusData[g.Id])).ToNumberFormat();
                row[3] = ((int)months.Sum(x => x.GroupOverSpentData[g.Id])).ToNumberFormat();
                row[4] = ((int)months.Sum(x => x.GroupBudgetData[g.Id]) - months.Sum(x => x.GroupStatusData[g.Id])).ToNumberFormat();

                var ration = (months.Sum(x => x.GroupStatusData[g.Id]) * 100) / months.Sum(x => x.GroupBudgetData[g.Id]);
                row[5] = $"{ ration }%";
                tbl.Rows.Add(row);
            }
        }

        private void FillHeaders(ReportContent content, DbAccess db, ReportSourceData data)
        {
            var tbl = content.Table;

            switch (content.ReportType)
            {
                case ReportTypeEnum.CategoryDetails:
                case ReportTypeEnum.GroupDetails:
                    tbl.Columns.Add("שם");
                    tbl.Columns.Add("תקציב");
                    tbl.Columns.Add("ממוצע");
                    tbl.Columns.Add("חריגות");
                    tbl.Columns.Add("מאזן");
                    tbl.Columns.Add("ניצול");
                    break;

                case ReportTypeEnum.IncomeDetails:
                    tbl.Columns.Add("נתוני הכנסות");
                    tbl.Columns.Add("חודשים");
                    tbl.Columns.Add("ממוצע");
                    tbl.Columns.Add("יתרה חודשית ממוצעת");
                    tbl.Columns.Add("הכנסה מצטברת");
                    tbl.Columns.Add("יתרה כוללת");
                    break;
                case ReportTypeEnum.IncomeDetailsExpanded:
                    tbl.Columns.Add("נתוני הכנסות");
                    tbl.Columns.Add("חודשים");
                    tbl.Columns.Add("ממוצע");
                    //tbl.Columns.Add("יתרה חודשית ממוצעת");
                    tbl.Columns.Add("הכנסה מצטברת");
                    //tbl.Columns.Add("יתרה כוללת");
                    break;
                case ReportTypeEnum.IncomeVsBudget:
                    tbl.Columns.Add("הכנסה ממוצעת");
                    tbl.Columns.Add("תקציב");
                    tbl.Columns.Add("הפרש ממוצע");
                    tbl.Columns.Add("חודשים");
                    tbl.Columns.Add("הפרש מצטבר");
                    break;

                case ReportTypeEnum.IncomeVsStatus:
                    tbl.Columns.Add("הכנסה ממוצעת");
                    tbl.Columns.Add("ממוצע הוצאות");
                    tbl.Columns.Add("הפרש ממוצע");
                    tbl.Columns.Add("חודשים");
                    tbl.Columns.Add("הפרש מצטבר");
                    break;

                case ReportTypeEnum.BudgetExceptions:
                    tbl.Columns.Add("חודשים");
                    tbl.Columns.Add("ממוצע חריגות לחודש");
                    tbl.Columns.Add("סה\"כ חריגות");
                    tbl.Columns.Add("סכום חריגות ממוצע לחודש");
                    tbl.Columns.Add("סכום חריגות מצטבר");
                    break;
                default:
                    break;
            }
        }
    }
}

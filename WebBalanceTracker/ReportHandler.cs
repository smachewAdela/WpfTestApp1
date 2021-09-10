using Newtonsoft.Json.Linq;
using QBalanceDesktop;
using System;
using System.Linq;

namespace WebBalanceTracker
{
    internal class ReportHandler
    {
        internal static dynamic GenerateChartData(string chartDdatasourceType)
        {
            dynamic chartDataSource = new JObject();
            // 
            if (chartDdatasourceType == "6MonthsDiff")
            {
                var db = Global.Db;
                var Groups = db.GetData<BudgetGroup>();
                var Categories = db.GetData<BudgetItem>();
                var Incomes = db.GetData<BudgetIncomeItem>();
                var Months = db.GetData<Budget>().OrderByDescending(x => x.Month).Take(6);
                var lenght = Months.Count() > 6 ? 6 : Months.Count();

                chartDataSource.labels = new JArray();
                var valuesArray = new int[lenght];
                int idx = 0;
                int minVal = 0;
                int maxVal = 0;
                foreach (var month in Months)
                {
                    chartDataSource.labels.Add(month.Title);
                    var monthDiff = month.TotalIncomes - month.TotalExpenses;

                    minVal = minVal < monthDiff ? minVal : monthDiff;
                    maxVal = maxVal > monthDiff ? maxVal : monthDiff;
                    valuesArray[idx++] = monthDiff;
                }

                chartDataSource.series = new JArray();
                chartDataSource.series.Add( valuesArray );
                chartDataSource.minVal = minVal;
                chartDataSource.maxVal = maxVal;
            }

            return chartDataSource;
        }

        //chartDataSource = {
        //                labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S'],
        //                series: [
        //                    [12, 17, 7, 17, 23, 18, 38]
        //                ]
        //            };
}
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfTestApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var migrationMode = ConfigurationManager.AppSettings["migrationMode"];
            if (migrationMode != null)
            {
                var db = GlobalsProviderBL.Db;
                var data = new
                {
                    groups = db.GetData<BudgetGroup>(),
                    categories = db.GetData<BudgetItem>(),
                    incomes = db.GetData<BudgetIncomeItem>(),
                    months = db.GetData<Budget>()
                };
                var migrationFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["migrationFilePath"]); // migrationData.json

                if (migrationMode == "toFile")
                {
                    using (FileStream fs = new FileStream(migrationFilePath, FileMode.Create))
                    { }

                    var jsonStr = JsonConvert.SerializeObject(data);
                    
                    File.WriteAllText(migrationFilePath, jsonStr);
                }
                else if (migrationMode == "fromFile")
                {
                    var proceed = CheckDbAndTables();
                    if (proceed)
                    {
                        var jsonStr = File.ReadAllText(migrationFilePath, Encoding.GetEncoding("Windows-1255"));
                        dynamic dynamicData = JObject.Parse(jsonStr);

                        var groups = (List<BudgetGroup>)JsonConvert.DeserializeObject<List<BudgetGroup>>(dynamicData.groups.ToString());
                        foreach (var g in groups)
                            db.Insert(g);
                        var categories = (List<BudgetItem>)JsonConvert.DeserializeObject<List<BudgetItem>>(dynamicData.categories.ToString());
                        foreach (var c in categories)
                            db.Insert(c);
                        var incomes = (List<BudgetIncomeItem>)JsonConvert.DeserializeObject<List<BudgetIncomeItem>>(dynamicData.incomes.ToString());
                        foreach (var i in incomes)
                            db.Insert(i);
                        var months = (List<Budget>)JsonConvert.DeserializeObject<List<Budget>>(dynamicData.months.ToString());
                        foreach (var m in months)
                            db.Insert(m);
                    }
                }
            }
        }

        private bool CheckDbAndTables()
        {
            return !GlobalsProviderBL.Db.GetData<BudgetGroup>().IsNotEmpty();
        }
    }
}

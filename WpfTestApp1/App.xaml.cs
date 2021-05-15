using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
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
                var migrationFilePath = ConfigurationManager.AppSettings["migrationFilePath"]; // migrationData.json

                if (migrationMode == "tofile")
                {
                    using (FileStream fs = new FileStream(migrationFilePath, FileMode.Create))
                    { }

                    var jsonStr = JsonConvert.SerializeObject(data);
                    File.WriteAllText(migrationFilePath, jsonStr);
                }
                else if (migrationMode == "fromfile")
                {
                    CheckDbAndTables();
                    var jsonStr = File.ReadAllText(migrationFilePath);
                    dynamic dynamicData = JObject.Parse(jsonStr);
                }
            }
        }

        private void CheckDbAndTables()
        {
           
        }
    }
}

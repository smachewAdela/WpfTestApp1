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
    public partial class Transactions : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "תנועות אחרונות";
        }
        

        public List<GroupData> BudgetGroups
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(5);
                var gData = new List<GroupData>();

                var currentBudget = Global.CurrentBudget;
                var gGroups = Db.GetData<BudgetGroup>(new SearchParameters { });

                foreach (var g in gGroups)
                {
                    g.BudgetItems = currentBudget.Items.Where(x => x.GroupId == g.Id).ToList();
                    gData.Add(new GroupData(g));
                }

                return gData;
            }
        }

        [WebMethod]
        public static string addTransaction(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();

            var BudgetItemToUpdate = Db.GetSingle<BudgetItem>(new SearchParameters { BudgetItemId = (int)req.budgetItemId });
            BudgetItemToUpdate.StatusAmount += (int)req.amountToAdd;
            Global.Db.Update(BudgetItemToUpdate);

            Global.RefreshBudget();
            return "Posted";
        }

        protected void btnDownloadFile_Click(object sender, EventArgs e)
        {
            var lpKeys = new ExcelParameters();
            lpKeys.FileName = "trans.xls";
            var tbData = lpKeys.GetNewtableParameter();
            var cats = Global.CurrentBudget.CategoryStatusData;
            tbData.table = new System.Data.DataTable();
            tbData.table.Columns.Add("קטגוריה");
            tbData.table.Columns.Add("תקציב");
            tbData.table.Columns.Add("מצב");
            tbData.table.Columns.Add("תוספת");

            foreach (var item in Global.CurrentBudget.Items)
            {
                var row = tbData.table.NewRow();
                row[0] = item.CategoryName;
                row[1] = item.BudgetAmount.ToNumberFormat();
                row[2] = Global.CurrentBudget.CategoryStatusData[item.Id].ToNumberFormat();
                row[3] = string.Empty;
                tbData.table.Rows.Add(row);
            }
            ExcelUtlity.DownLoadAsExcel(HttpContext.Current.Response, lpKeys);
        }

        protected void btnUploadFile_Click(object sender, EventArgs e)
        {

        }
    }

    public class GroupData
    {
        public GroupData(BudgetGroup g)
        {
            this.GroupName = g.Name;
            BudgetGroups = g.BudgetItems.Where(x => x.Active).Select(x => new BudgetData(x)).ToList();
        }

        public string GroupName { get; set; }
        public List<BudgetData> BudgetGroups { get; set; }
    }

    public class BudgetData
    {
        public string CategoryName { get; set; }
        public int BudgetAmount { get; set; }
        public int Id { get; set; }
        public int StatusAmount { get; set; }
        public int GroupId { get; set; }
        public int BudgetId { get; set; }
        public int OverSpent
        {
            get
            {
                return BudgetAmount < StatusAmount ? 1 : 0;
            }
        }
        

        public int Ratio
        {
            set { }
            get
            {
                // 75/100
                return (StatusAmount * 100) / BudgetAmount;
            }
        }


        public BudgetData(BudgetItem x)
        {
            this.BudgetAmount = x.BudgetAmount;
            this.BudgetId = x.BudgetId;
            this.CategoryName = x.CategoryName;
            this.GroupId = x.GroupId;
            this.StatusAmount = x.StatusAmount;
            this.Id = x.Id;
        }
    }
}
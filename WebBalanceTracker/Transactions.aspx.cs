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
            XTitle = "תנועות אחרונות";
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


        [WebMethod]
        public static string saveCheckpoint(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();

            var transactionCheckPoint = Db.GetSingle<TransactionCheckPoint>(new SearchParameters { TransactionCheckPointId = (int)req.checkPointId });
            transactionCheckPoint.Description = (string)req.checkPointDescription;
            Global.Db.Update(transactionCheckPoint);

            Global.RefreshBudget();
            return "Posted";
        }


        public List<CheckPointData> CheckPoints
        {
            get
            {
                if (Global.CurrentBudget == null)
                    return new List<CheckPointData>();

                if (Global.CurrentBudget.TransactionCheckPoints.IsEmptyOrNull())
                {
                    var _t = Global.GenerateDefaultCheckPoints();
                    foreach (var checkPoint in _t)
                    {
                        checkPoint.BudgetId = Global.CurrentBudget.Id;
                        Global.Db.Insert(checkPoint);
                    }
                    Global.CurrentBudget.TransactionCheckPoints = Global.Db.GetData<TransactionCheckPoint>(new SearchParameters { TransactionCheckPointBudgetId = Global.CurrentBudget.Id });
                }
                return Global.CurrentBudget.TransactionCheckPoints.Select(x => new CheckPointData(x)).ToList();
            }
        }
    }

    public class GroupData
    {
        public GroupData(BudgetGroup g)
        {
            this.GroupName = g.Name;
            this.Id = g.Id;
            BudgetGroups = g.BudgetItems.Where(x => x.Active).Select(x => new BudgetData(x)).ToList();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }
        public List<BudgetData> BudgetGroups { get; set; }

        public string Status
        {
            get
            {
                return BudgetGroups.IsNotEmpty() ? BudgetGroups.Sum(x => x.StatusAmount).ToNumberFormat() : string.Empty;
            }
        }
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

    public class CheckPointData
    {
        public CheckPointData(TransactionCheckPoint x)
        {
            this.Id = x.Id;
            this.Name = x.Name;
            this.Description = x.Description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class BudgetInfo
    {
        public int Id { get; set; }
        public int Incomes { get; set; }
        public int CheckPoints { get; set; }
        public int BudgetItems { get; set; }
        public string Title { get; set; }

        public BudgetInfo(Budget x)
        {
            this.Id = x.Id;
            this.Incomes = x.Incomes.Count();
            this.CheckPoints = x.TransactionCheckPoints.Count();
            this.BudgetItems = x.Items.Count();
            this.Title = x.Title;
        }
    }
}
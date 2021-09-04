﻿using QBalanceDesktop;
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
    public partial class Incomes : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "הכנסות";
        }

        public DataTable BudgetIncomes
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(5);

                var currentBudget = Global.CurrentBudget;
                var items = currentBudget.Incomes;

                foreach (var g in items)
                {
                    var rw = tbl.NewRow();

                    rw[0] = g.Id;
                    rw[1] = g.BudgetId;
                    rw[2] = g.Name;
                    rw[3] = g.Amount.ToNumberFormat();
                    rw[4] = "0";
                    tbl.Rows.Add(rw);
                }

                var totalRow = tbl.NewRow();
                totalRow[0] = null;
                totalRow[1] = currentBudget.Id;
                totalRow[2] = currentBudget.Title;
                totalRow[3] = items.Sum(x => x.Amount).ToNumberFormat();
                totalRow[4] = "1";
                tbl.Rows.Add(totalRow);

                return tbl;
            }
        }
        
        [WebMethod]
        public static string appendIncome(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();
            var lBudget = Global.GetLatestBudget();

            int idToFinfd = req.incomeId;
            var iIncome = Global.CurrentBudget.Incomes.First(x => x.Id == idToFinfd);
            iIncome.Amount += (int)req.amountToAdd;

            Global.Db.Update(iIncome);

            Global.RefreshBudget();
            return "Posted";
        }

        [WebMethod]
        public static string addIncome(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();
            var lBudget = Global.GetLatestBudget();

            var incomeName = req.incomeName;

            BudgetIncomeItem bi = new BudgetIncomeItem
            {
                Amount = 0,
                Name = incomeName,
                BudgetId = lBudget.Id
            };
            Global.Db.Insert(bi);

            return "Posted";
        }
    }
}
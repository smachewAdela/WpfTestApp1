using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBalanceTracker
{
    public partial class BudgetGroups : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XTitle = "קבוצות";
        }

        public DataTable BudgetGroupItems
        {
            get
            {
                var tbl = new DataTable();
                tbl.AddColumns(2);
                using (var context = new BalanceAdmin_Entities())
                {
                    foreach (var g in context.BudgetGroup)
                    {
                        var rw = tbl.NewRow();

                        rw[0] = g.Name;
                        rw[1] = g.Id;
                        tbl.Rows.Add(rw);
                    }
                }
                return tbl;
            }
        }

     

        [WebMethod]
        public static string upsertGroup(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();
            int entId = req.groupId;
            var g = new BudgetGroup
            {
                Name = req.groupName,
                Id = req.groupId
            };
            using (var context = new BalanceAdmin_Entities())
            {
                if (g.Id == 0)
                {
                    context.BudgetGroup.Add(new BudgetGroup
                    {
                        Name = req.name,
                        AbstractCategory = req.abstractCategoryId,
                    });
                }
                else
                {
                    var exsisting = context.BudgetGroup.SingleOrDefault(x => x.Id == entId);
                    exsisting.Name = req.name;
                    exsisting.AbstractCategory = req.abstractCategoryId;
                }
                context.SaveChanges();
            }

            //Global.RefreshBudget();
            return "Posted";
        }


        [WebMethod]
        public static string deleteGroup(string userdata)
        {
            dynamic req = userdata.ToDynamicJObject();

            var groupToDelete = Db.GetSingle<BudgetGroup>(new SearchParameters { BudgetGroupId = (int)req.groupId });
            var groupItemsToDelete = Db.GetData<BudgetItem>(new SearchParameters { BudgetItemGroupId = (int)req.groupId }).ToList();

            try
            {
                Db.BeginTransaction();

                if (groupToDelete != null)
                    Db.Delete(groupToDelete);

                foreach (var groupItemToDelete in groupItemsToDelete)
                    Db.Delete(groupItemToDelete);

                Db.Commit();

                Global.RefreshBudget();
            }
            catch (Exception ex)
            {
                Db.RollBack();
                I_Message.HandleException(ex, Db);
                throw new HttpException((int)HttpStatusCode.BadRequest, "Budget not Deleted, check system log for more info");
            }


            return "Posted";
        }

    }
}
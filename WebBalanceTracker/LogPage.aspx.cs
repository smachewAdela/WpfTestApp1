using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBalanceTracker
{
    public partial class LogPage : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.XTitle = "יומן אירועים";
        }

        public List<LogInfo> Logs
        {
            get
            {
                //var budgets = Db.GetData<I_Message>();
                //return budgets.OrderByDescending(x => x.Date).Select(x => 
                //new LogInfo
                //{
                //    Date = x.Date.ToString(),
                //    Title = x.Title,
                //    Message = x.Message,
                //    ExtraData = x.ExtraData,
                //    Type = x.IType.ToString()

                //}).ToList();

                return new List<LogInfo>();
            }
        }
    }

    public class LogInfo
    {
        public string Date { get; internal set; }
        public string Title { get; internal set; }
        public string Message { get; internal set; }
        public string ExtraData { get; internal set; }
        public string Type { get; internal set; }
    }
}
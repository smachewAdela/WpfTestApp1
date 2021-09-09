using Newtonsoft.Json.Linq;
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
    public partial class _Default : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "דף הבית";
            this.HideBudgetNavigator = true;
        }

    }
}
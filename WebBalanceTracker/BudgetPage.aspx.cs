﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebBalanceTracker
{
    public partial class BudgetPage : BaseForm
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "הגדרת תקציב";
        }
    }
}
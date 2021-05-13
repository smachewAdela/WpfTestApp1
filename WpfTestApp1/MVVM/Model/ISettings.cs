using System;
using System.Collections.Generic;
using System.Text;

namespace QBalanceDesktop
{
    [DbEntity("Settings")]
    public class ISettings : BaseDbItem
    {
        [DbField()]
        public bool ShowUnbudgetedCategories { get; set; }

    }
}

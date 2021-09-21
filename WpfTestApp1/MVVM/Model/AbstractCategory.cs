using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBalanceDesktop
{
    /// <summary>
    /// template for category 
    /// </summary>
    [DbEntity("AbstractCategories")]
    public class AbstractCategory : BaseDbItem
    {
        [DbField()]
        public string CategoryName { get; set; }

        [DbField()]
        public int DefaultAmount { get; set; }

        [DbField()]
        public int GroupId { get; set; }

        [DbField()]
        public int DayInMonth { get; set; }

        [DbField()]
        public bool Active { get; set; }
    }
}

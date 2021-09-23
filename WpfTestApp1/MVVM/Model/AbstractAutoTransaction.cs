using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp1.MVVM.Model
{
    [DbEntity("AbstractAutoTransactions")]
    public class AbstractAutoTransaction : BaseDbItem 
    {
        [DbField()]
        public string Name { get; set; }

        [DbField()]
        public int DefaultAmount { get; set; }

        [DbField()]
        public int AbstractCategoryId { get; set; }

        [DbField()]
        public int BudgetId { get; set; }

        [DbField()]
        public int DayOfTheMonth { get; set; }

        [DbField()]
        public string  LastPaymentDate { get; set; }

        [DbField()]
        public bool Active { get; set; }
    }
}

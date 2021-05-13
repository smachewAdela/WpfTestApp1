using System;
using System.Collections.Generic;
using System.Text;

namespace QBalanceDesktop
{
    [DbEntity("FinandaSyncLog")]
    public class FinandaSyncLog : BaseDbItem
    {
        [DbField()]
        public DateTime SyncStart { get; set; }
        [DbField()]
        public DateTime SyncEnd { get; set; }
        [DbField()]
        public string LogInfo { get; set; }

        [DbField()]
        public int NewTransactions { get;  set; }
        [DbField()]
        public bool Success { get;  set; }
        [DbField()]
        public DateTime TransactionsFromDate { get; internal set; }

        [DbField()]
        public DateTime TransactionsToDate { get; internal set; }
    }
}

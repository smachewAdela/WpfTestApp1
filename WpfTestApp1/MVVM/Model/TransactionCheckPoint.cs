namespace QBalanceDesktop
{
    public class TransactionCheckPoint : BaseDbItem
    {
        [DbField()]
        public int BudgetId { get; set; }

        [DbField()]
        public string Name { get; set; }

        [DbField()]
        public string Description { get; set; }
        [DbField()]
        public int LastTransactionAmount { get; set; }
    }
}
namespace QBalanceDesktop
{
    [DbEntity("BudgetItems")]
    public class BudgetItem : BaseDbItem
    {
        [DbField()]
        public string CategoryName { get; set; }

        [DbField()]
        public int BudgetAmount { get; set; }

        [DbField()]
        public int StatusAmount { get; set; }

        [DbField()]
        public int GroupId { get; set; }

        [DbField()]
        public int BudgetId { get; set; }
        public int OverSpent
        {
            get
            {
                return BudgetAmount < StatusAmount ? 1 : 0;
            }
        }

        public bool Active
        {
            get
            {
                return BudgetAmount > 0;
            }
        }
    }
}
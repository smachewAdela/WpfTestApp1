namespace QBalanceDesktop
{
    [DbEntity("BudgetIncomes")]
    public class BudgetIncomeItem : BaseDbItem
    {
        [DbField()]
        public string Name { get; set; }

        [DbField()]
        public int Amount { get; set; }

        [DbField()]
        public int BudgetId { get; set; }
    }
}
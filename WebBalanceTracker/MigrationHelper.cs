using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WebBalanceTracker
{
    internal class MigrationHelper
    {
        internal static void Perform()
        {
            ThreadStart ts = new ThreadStart(Start);
            ts.Invoke();
        }

        private static BugetGroupMigrationItemList bugetGroupMigrationItems = new BugetGroupMigrationItemList();
        private static BugetMonthMigrationItemList bugetMonthMigrationItems = new BugetMonthMigrationItemList();

        private static void Start()
        {
            LoadBudgetMonths();
            LoadBudgetGroups();
            LoadAbstractCategories();
            LoadAbstractIncomes();
            LoadLegacyIncomeTransactions();
            LoadLegacyTransactions();
        }

        private static void LoadAbstractIncomes()
        {
            using (var context = new BalanceAdmin_Entities())
            using (var legacyContext = new Legacy_prodEntities())
            {
                if (context.AbstractIncome.Count() == 0)
                {
                    foreach (var legacyIncome in legacyContext.BudgetIncomes.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.ToList().Average(g => g.Amount)))
                    {
                        context.AbstractIncome.Add(new AbstractIncome
                        {
                            Active = true,
                            DefaultAmount = (int)legacyIncome.Value,
                            Name = legacyIncome.Key
                        });
                    }
                    context.SaveChanges();
                }
            }
        }

        private static void LoadLegacyIncomeTransactions()
        {
            using (var context = new BalanceAdmin_Entities())
            using (var legacyContext = new Legacy_prodEntities())
            {
                if (context.IncomeTransaction.Count() == 0)
                {
                    foreach (var legacyIncome in legacyContext.BudgetIncomes)
                    {
                        var newIncomeTran = new IncomeTransaction
                        {
                            Amount = legacyIncome.Amount,
                            Name = legacyIncome.Name,
                            BudgetMonthId = bugetMonthMigrationItems.TransformToNewId(legacyIncome.BudgetId),
                            AbstractIncomeId = context.AbstractIncome.First(x => x.Name.Equals(legacyIncome.Name)).Id
                        };
                        context.IncomeTransaction.Add(newIncomeTran);
                    }
                    context.SaveChanges();
                }
            }
        }

        private static void LoadLegacyTransactions()
        {
            using (var context = new BalanceAdmin_Entities())
            using (var legacyContext = new Legacy_prodEntities())
            {
                if (context.BudgetTransaction.Count() == 0)
                {
                    foreach (var item in legacyContext.BudgetItems)
                    {
                        var matchingAbstractCategory = context.AbstractCategory.SingleOrDefault(x => x.Name.Equals(item.CategoryName));
                        if (matchingAbstractCategory != null)
                        {
                            var newB = new BudgetTransaction
                            {
                                AbstractCatrgoryId = matchingAbstractCategory.Id,
                                Amount = item.StatusAmount,
                                BudgetMonthId = bugetMonthMigrationItems.TransformToNewId(item.BudgetId),
                                Description = $"Migrated transaction",
                            };
                            context.BudgetTransaction.Add(newB);
                        }
                        else
                        {
                            //throw new Exception("UnKnown category fort transaction");
                        }
                    }
                    context.SaveChanges();
                }
            }
        }

        private static void LoadBudgetGroups()
        {
            using (var context = new BalanceAdmin_Entities())
            using (var legacyContext = new Legacy_prodEntities())
            {
                if (context.BudgetGroup.Count() == 0)
                {
                    foreach (var item in legacyContext.BudgetItemGroup)
                    {
                        var newB = new BudgetGroup
                        {
                            Name = item.Name
                        };
                        context.BudgetGroup.Add(newB);
                    }
                    context.SaveChanges();
                }
                foreach (var legacyGroup in legacyContext.BudgetItemGroup)
                {
                    var matchingGroup = context.BudgetGroup.OrderBy(x => x.Id).SingleOrDefault(x => x.Name.Equals(legacyGroup.Name));
                    bugetGroupMigrationItems.Add(new BugetMigrationItem
                    {
                        LegacyId = legacyGroup.Id,
                        NewId = matchingGroup.Id
                    });
                }
            }
        }

        private static void LoadAbstractCategories()
        {
            using (var context = new BalanceAdmin_Entities())
            using (var legacyContext = new Legacy_prodEntities())
            {
                if (context.AbstractCategory.Count() == 0)
                {
                    foreach (var item in legacyContext.AbstractCategories)
                    {
                        context.AbstractCategory.Add(new AbstractCategory
                        {
                            Active = true,
                            Amount = item.DefaultAmount,
                            Name = item.CategoryName,
                            BudgetGroupId = bugetGroupMigrationItems.TransformToNewId(item.GroupId)
                        });
                    }
                    context.SaveChanges();
                }
            }
        }

        private static void LoadBudgetMonths()
        {
            using (var context = new BalanceAdmin_Entities())
            using (var legacyContext = new Legacy_prodEntities())
            {
                if (context.BudgetMonth.Count() == 0)
                {
                    foreach (var item in legacyContext.BudgetForMonth)
                    {
                        var Month = item.Month;

                        context.BudgetMonth.Add(new BudgetMonth
                        {
                            Month = item.Month,
                            Name = $"{Month.ToString("MMMM")} {Month.Year.ToString().Substring(2, 2)}"
                        });
                    }
                    context.SaveChanges();
                }
                foreach (var legacyMonth in legacyContext.BudgetForMonth)
                {
                    var matchingMonth = context.BudgetMonth.SingleOrDefault(x => x.Month.Equals(legacyMonth.Month));
                    bugetMonthMigrationItems.Add(new BugetMigrationItem
                    {
                        LegacyId = legacyMonth.Id,
                        NewId = matchingMonth.Id
                    });
                }

            }
        }
    }

    public class BugetGroupMigrationItemList : List<BugetMigrationItem>
    {
        internal int TransformToNewId(int gId)
        {
            var EX = this.FirstOrDefault(x => x.LegacyId == gId);
            if (EX == null)
                return 0;
            return EX.NewId;
        }
    }

    public class BugetMonthMigrationItemList : List<BugetMigrationItem>
    {
        internal int TransformToNewId(int gId)
        {
            var EX = this.FirstOrDefault(x => x.LegacyId == gId);
            if (EX == null)
                return 0;
            return EX.NewId;
        }
    }



    public class BugetMigrationItem
    {
        public int LegacyId { get; set; }
        public int NewId { get; set; }
    }
}
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

        private static void Start()
        {
            LoadBudgetMonths();
            LoadBudgetGroups();
            LoadAbstractCategories();
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
                    bugetGroupMigrationItems.Add(new BugetGroupMigrationItem
                    {
                        LegacyId = legacyGroup.Id,
                        Name = legacyGroup.Name,
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
            }
        }
    }

    public class BugetGroupMigrationItemList : List<BugetGroupMigrationItem>
    {
        internal int TransformToNewId(int gId)
        {
            var EX = this.FirstOrDefault(x => x.LegacyId == gId);
            if (EX == null)
                return 0;
            return EX.NewId;
        }
    }

    public class BugetGroupMigrationItem
    {
        public int LegacyId { get; set; }
        public int NewId { get; set; }
        public string Name { get; set; }
    }
}
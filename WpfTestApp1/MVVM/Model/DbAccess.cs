using System;
using System.Collections.Generic;
using System.Linq;

namespace QBalanceDesktop
{
    public class DbAccess : DbAccessProvider
    {
        public DbAccess(string connStr) : base(connStr)
        {

        }

        public void Add(BaseDbItem item)
        {
            Insert(item);
        }

        public void Delete(BaseDbItem item)
        {
            base.Delete(item);
        }

        public T GetSingle<T>(SearchParameters parameters) where T : BaseDbItem
        {
            return GetData<T>(parameters).FirstOrDefault();
        }

        public List<T> GetData<T>(SearchParameters parameters = null) where T : BaseDbItem
        {
            if (parameters == null)
                parameters = new SearchParameters();
            List<DbParam> innerParams = new List<DbParam>();
            var i = (T)Activator.CreateInstance(typeof(T));
            var query = $"select * from {i.GetTableName()}";
            List<string> paramsText = ExtractParameters(parameters, innerParams);
            if (paramsText.Count > 0)
            {
                query += $" where { string.Join(" and ", paramsText)}";
            }

            return Get<T>(query, innerParams.ToArray());
        }

        private List<string> ExtractParameters(SearchParameters parameters, List<DbParam> innerParams)
        {
            List<string> param = new List<string>();

            if (parameters.CategoryGroupId.HasValue)
            {
                param.Add("Id=@Id");
                innerParams.Add(new DbParam("@Id", parameters.CategoryGroupId.Value));
            }
            if (!string.IsNullOrEmpty(parameters.TransID))
            {
                param.Add("TransID=@TransID");
                innerParams.Add(new DbParam("@TransID", parameters.TransID));
            }
            if (!string.IsNullOrEmpty(parameters.CategoryCode))
            {
                param.Add("Code=@Code");
                innerParams.Add(new DbParam("@Code", parameters.CategoryCode));
            }
            if (!string.IsNullOrEmpty(parameters.CategoryGroupName))
            {
                param.Add("Name=@Name");
                innerParams.Add(new DbParam("@Name", parameters.CategoryGroupName));
            }
            if (parameters.BudgetCategoryMonthId.HasValue)
            {
                param.Add("MonthId=@MonthId");
                innerParams.Add(new DbParam("@MonthId", parameters.BudgetCategoryMonthId.Value));
            }
            if (parameters.TranFromDate.HasValue)
            {
                param.Add("CreateDate>=@CreateDate");
                innerParams.Add(new DbParam("@CreateDate", parameters.TranFromDate.Value));
            }
            if (parameters.TranToDate.HasValue)
            {
                param.Add("CreateDate<@ToCreateDate");
                innerParams.Add(new DbParam("@ToCreateDate", parameters.TranToDate.Value));
            }
            if (parameters.BudgetDate.HasValue)
            {
                param.Add("Month=@Month");
                innerParams.Add(new DbParam("@Month", parameters.BudgetDate.Value));
            }
            if (parameters.BudgetItemBudgetId.HasValue)
            {
                param.Add("BudgetId=@BudgetId");
                innerParams.Add(new DbParam("@BudgetId", parameters.BudgetItemBudgetId.Value));
            }
            if (parameters.BudgetIncomeId.HasValue)
            {
                param.Add("Id=@Id");
                innerParams.Add(new DbParam("@Id", parameters.BudgetIncomeId.Value));
            }
            if (parameters.BudgetItemGroupId.HasValue)
            {
                param.Add("GroupId=@GroupId");
                innerParams.Add(new DbParam("@GroupId", parameters.BudgetItemGroupId.Value));
            }
            return param;
        }
    }

}
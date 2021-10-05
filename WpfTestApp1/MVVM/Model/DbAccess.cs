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

            
            AddIdentityFilter(param, innerParams, parameters.CategoryGroupId);
            AddFilter(param, innerParams, parameters.TransID, "TransID");
            AddFilter(param, innerParams, parameters.CategoryCode, "Code");
            AddFilter(param, innerParams, parameters.CategoryGroupName, "Name");
            AddFilter(param, innerParams, parameters.BudgetCategoryMonthId, "MonthId");

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
           

            AddFilter(param, innerParams, parameters.BudgetDate, "Month");
            AddFilter(param, innerParams, parameters.BudgetItemBudgetId, "BudgetId");
            AddIdentityFilter(param, innerParams, parameters.BudgetIncomeId);
            AddIdentityFilter(param, innerParams, parameters.BudgetItemId);
            AddIdentityFilter(param, innerParams, parameters.TransactionCheckPointId);
            AddIdentityFilter(param, innerParams, parameters.BudgetGroupId);
            AddIdentityFilter(param, innerParams, parameters.BudgetId);
            AddIdentityFilter(param, innerParams, parameters.AbstractAutoTransactionId);
            AddFilter(param, innerParams, parameters.BudgetItemGroupId, "GroupId");
            AddFilter(param, innerParams, parameters.TransactionCheckPointBudgetId, "BudgetId");
            AddFilter(param, innerParams, parameters.BudgetItemLogBudgetItemId, "BudgetItemId");
            AddFilter(param, innerParams, parameters.BudgetItemAbstractCategoryId, "AbstractCategoryId");
            AddFilter(param, innerParams, parameters.IMessageSendMail, "SendMail");

            return param;
        }

        private void AddFilter(List<string> param, List<DbParam> innerParams, DateTime? parameter, string field)
        {
            if (parameter.HasValue)
            {
                param.Add($"{field}=@{field}");
                innerParams.Add(new DbParam($"@{field}", parameter.Value));
            }
        }

        private void AddFilter(List<string> param, List<DbParam> innerParams, string parameter, string field)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                param.Add($"{field}=@{field}");
                innerParams.Add(new DbParam($"@{field}", parameter));
            }
        }

        private void AddFilter(List<string> param, List<DbParam> innerParams, int? parameter, string field)
        {
            if (parameter.HasValue)
            {
                param.Add($"{field}=@{field}");
                innerParams.Add(new DbParam($"@{field}", parameter.Value));
            }
        }

        private void AddFilter(List<string> param, List<DbParam> innerParams, bool? parameter, string field)
        {
            if (parameter.HasValue)
            {
                param.Add($"{field}=@{field}");
                innerParams.Add(new DbParam($"@{field}", parameter.Value ? 1 : 0));
            }
        }

        private void AddIdentityFilter(List<string> param, List<DbParam> innerParams, int? parameter)
        {
            if (parameter.HasValue)
            {
                param.Add("Id=@Id");
                innerParams.Add(new DbParam("@Id", parameter.Value));
            }
        }
    }

}
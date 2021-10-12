using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBalanceDesktop
{
    public class DbAccess
    {
        
        private string connectionString;

        public DbAccess(string connStr)
        {
            connectionString = connStr;
        }

        public void Insert(BaseDbItem item)
        {
            Add(item);
        }

        public void Add(BaseDbItem item)
        {
            CheckTable(item);

            var table = item.GetTableName();
            var iFields = item.GetInsertFields();

            using (var _connection = new SqlConnection(connectionString))
            using (var _command = _connection.GetCommand())
            {
                CheckTransaction(_command);
                _command.CommandText = $"INSERT INTO {table}({string.Join(",", iFields)})   VALUES({string.Join(",", iFields.Select(x => $"@{x}").ToList())})";
                foreach (var DbInsertField in iFields)
                    _command.Parameters.AddWithValue($"@{DbInsertField}", item.GetValue(DbInsertField));

                _command.ExecuteNonQuery();
            }
            SetNewDbIdentity(item);
        }

        public void Update(IDbItem item)
        {
            CheckTable(item);

            var table = item.GetTableName();
            string updateScriptFormat = "UPDATE {0} SET {1}  WHERE {2}";

            var iFields = item.GetInsertFields();
            var fieldsSet = string.Join(",", iFields.Select(x => $"{x}=@{x}").ToList());

            var idf = item.GetIdentityField();
            string where = $"{idf}=@{idf}";

            using (var _connection = new SqlConnection(connectionString))
            using (var _command = _connection.CreateCommand())
            {
                foreach (var DbInsertField in iFields)
                    _command.Parameters.AddWithValue($"@{DbInsertField}", item.GetValue(DbInsertField));

                CheckTransaction(_command);

                _command.Parameters.AddWithValue($"@{idf}", item.GetValue(idf));
                _command.CommandText = string.Format(updateScriptFormat, table, fieldsSet, where);
                _command.ExecuteNonQuery();
            }
        }

        public void Delete(BaseDbItem item)
        {
            var table = item.GetTableName();
            var idCol = item.GetIdentityField();

            CheckTable(item);

            using (var _connection = new SqlConnection(connectionString))
            using (var _command = _connection.GetCommand())
            {
                CheckTransaction(_command);
                _command.CommandText = $"delete from {table} where {idCol}=@{idCol}";
                _command.Parameters.AddWithValue($"@{idCol}", item.GetDbIdentity());
                _command.ExecuteNonQuery();
            }
        }


        public DataTable ListTableBySql(string sql)
        {
            DataTable dt = new DataTable();

            using (var _connection = new SqlConnection(connectionString))
            using (SqlDataAdapter sda = new SqlDataAdapter(sql, _connection))
            {
                sda.Fill(dt);
            }
            return dt;
        }

        public List<T> Get<T>(string sql, params DbParam[] parameters) where T : IDbItem
        {
            List<T> lst = new List<T>();
            CheckTable((T)Activator.CreateInstance(typeof(T)));

            using (var _connection = new SqlConnection(connectionString))
            using (var _command = _connection.GetCommand())
            {
                _command.CommandText = sql;
                CheckTransaction(_command);
                if (parameters != null)
                {
                    foreach (var item in parameters)
                        _command.Parameters.AddWithValue(item.Name, item.Value);
                }
                using (SqlDataReader reader = _command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var i = (T)Activator.CreateInstance(typeof(T));
                        i.Load(reader);
                        lst.Add(i);
                    }
                }
            }
            return lst;
        }


        static object obj = new object();
        public void CheckTable(IDbItem dbItem)
        {
            List<string> cols = dbItem.GetInsertFields();
            string tblName = dbItem.GetTableName();

            lock (obj)
            {
                using (var _connection = new SqlConnection(connectionString))
                using (var _command = _connection.GetCommand())
                {
                    CheckTransaction(_command);
                    var tableScript = string.Format(createTableFormat, tblName);
                    var colsScript = BuildTableColumns(dbItem);
                    tableScript = tableScript.Replace("[[cols]]", colsScript);
                    string creatScript = $"If not exists (select name from sysobjects where name = '{tblName}') {tableScript}";

                    _command.CommandText = creatScript;
                    _command.ExecuteNonQuery();
                }
            }
        }

        private void SetNewDbIdentity(IDbItem item)
        {
            var table = item.GetTableName();
            var idCol = item.GetIdentityField();

            CheckTable(item);
            using (var _connection = new SqlConnection(connectionString))
            using (var _command = _connection.GetCommand())
            {
                CheckTransaction(_command);
                _command.CommandText = $"Select max({idCol}) from {table} ";
                using (SqlDataReader reader = _command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var newId = reader.GetInt32(0);
                        item.SetDbIdentity(newId);
                    }
                }
            }
        }

        private void CheckTransaction(SqlCommand command)
        {
            if (_transaction != null)
            {
                command.Transaction = _transaction;
                command.Connection = _transactionConnection;
            }
        }

        private SqlTransaction _transaction;
        private SqlConnection _transactionConnection;

        public void BeginTransaction()
        {
            _transactionConnection = new SqlConnection(connectionString);
            _transactionConnection.Open();
            _transaction = _transactionConnection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
            ClearTransaction();
        }

        private void ClearTransaction()
        {
            _transaction.Dispose();
            _transaction = null;

            _transactionConnection.Close();
            _transactionConnection.Dispose();
            _transactionConnection = null;
        }

        public void RollBack()
        {
            _transaction.Rollback();
            ClearTransaction();
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

        private string BuildTableColumns(IDbItem dbItem)
        {
            List<string> b = new List<string>();
            List<string> cols = dbItem.GetInsertFields();
            foreach (var col in cols)
            {
                try
                {
                    var prop = dbItem.GetType().GetProperty(col);
                    b.Add($"[{col}] {GetSqlType(prop.PropertyType)}");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return string.Join(",", b);
        }


        const string createTableFormat = @"CREATE TABLE [dbo].[{0}](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[[cols]]
 CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] ";

        public static Type GetClrType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return typeof(long?);

                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return typeof(bool?);

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return typeof(string);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    return typeof(DateTime?);

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return typeof(decimal?);

                case SqlDbType.Float:
                    return typeof(double?);

                case SqlDbType.Int:
                    return typeof(int?);

                case SqlDbType.Real:
                    return typeof(float?);

                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid?);

                case SqlDbType.SmallInt:
                    return typeof(short?);

                case SqlDbType.TinyInt:
                    return typeof(byte?);

                case SqlDbType.Variant:
                case SqlDbType.Udt:
                    return typeof(object);

                case SqlDbType.Structured:
                    return typeof(DataTable);

                case SqlDbType.DateTimeOffset:
                    return typeof(DateTimeOffset?);

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }

        public static string GetSqlType(Type clrType)
        {
            if (clrType.IsEnum)
                clrType = typeof(int);
            var _clrTypeToSqlTypeMaps = new Dictionary<Type, string>
            {
                {typeof (Boolean), "[bit] NOT NULL"},
                {typeof (Boolean?), "[bit] NULL"},
                {typeof (String), "[nvarchar](max) NULL"},
                {typeof (DateTime), "[datetime] NOT NULL"},
                {typeof (DateTime?), "[datetime] NULL"},
                {typeof (Int32), "[int] NOT NULL"},
                {typeof (Int32?), "[int] NULL"},
                {typeof (Decimal), "decimal (10,2) NOT NULL"},
                {typeof (Decimal?), "decimal (10,2) NULL"},
                {typeof (TimeSpan), "TIME (0) NOT NULL"},
                {typeof (Guid), " UNIQUEIDENTIFIER PRIMARY KEY default NEWID()"},
                //{typeof (Byte[]), SqlDbType.Binary},
                //{typeof (Byte?[]), SqlDbType.Binary},
            };
            return _clrTypeToSqlTypeMaps[clrType];
        }

    }

    public static class DbAccess2Extensions
    {
        public static SqlCommand GetCommand(this SqlConnection _connection)
        {
            var _com = _connection.CreateCommand();
            //if (conn)
            //{

            //}
            if (_connection.State != ConnectionState.Open && _connection.State != ConnectionState.Connecting)
                _connection.Open();
            return _com;
        }
    }
}

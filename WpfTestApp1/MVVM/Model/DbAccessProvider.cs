using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace QBalanceDesktop
{
    public class DbAccessProvider
    {
        #region Table Handling

        public string _connectionString { get; set; }

        public DbAccessProvider(string connStr)
        {
            _connectionString = connStr;
        }

        static object obj = new object();
        public void CheckTable(IDbItem dbItem)
        {
            List<string> cols = dbItem.GetInsertFields();
            string tblName = dbItem.GetTableName();

            lock (obj)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var tableScript = string.Format(createTableFormat, tblName);
                    var colsScript = BuildTableColumns(dbItem);
                    tableScript = tableScript.Replace("[[cols]]", colsScript);
                    string creatScript = $"If not exists (select name from sysobjects where name = '{tblName}') {tableScript}";
                    SqlCommand command = new SqlCommand(creatScript, conn);
                    command.ExecuteNonQuery();
                }
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

        #endregion

        public void Update(IDbItem item)
        {
            CheckTable(item);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var table = item.GetTableName();
                string updateScriptFormat = "UPDATE {0} SET {1}  WHERE {2}";

                var iFields = item.GetInsertFields();
                var fieldsSet = string.Join(",", iFields.Select(x => $"{x}=@{x}").ToList());

                var idf = item.GetIdentityField();
                string where = $"{idf}=@{idf}";

                SqlCommand command = new SqlCommand(string.Format(updateScriptFormat, table, fieldsSet, where), conn);

                foreach (var DbInsertField in iFields)
                    command.Parameters.AddWithValue($"@{DbInsertField}", item.GetValue(DbInsertField));

                command.Parameters.AddWithValue($"@{idf}", item.GetValue(idf));
                command.ExecuteNonQuery();
            }
        }

        public void Insert(IDbItem item)
        {
            CheckTable(item);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var table = item.GetTableName();
                var iFields = item.GetInsertFields();
                SqlCommand command = new SqlCommand($"INSERT INTO {table}({string.Join(",", iFields)})   VALUES({string.Join(",", iFields.Select(x => $"@{x}").ToList())})", conn);

                foreach (var DbInsertField in iFields)
                    command.Parameters.AddWithValue($"@{DbInsertField}", item.GetValue(DbInsertField));

                command.ExecuteNonQuery();
            }

            SetNewDbIdentity(item);
        }

        private void SetNewDbIdentity(IDbItem item)
        {
            var table = item.GetTableName();
            var idCol = item.GetIdentityField();

            CheckTable(item);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand($"Select max({idCol}) from {table} ", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var newId = reader.GetInt32(0);
                        item.SetDbIdentity(newId);
                    }
                }
            }
        }

        public void Delete(IDbItem item)
        {
            var table = item.GetTableName();
            var idCol = item.GetIdentityField();

            CheckTable(item);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand($"delete from {table} where {idCol}=@{idCol}", conn);
                command.Parameters.AddWithValue($"@{idCol}", item.GetDbIdentity());
                command.ExecuteNonQuery();
            }
        }

        public DataTable ListTableBySql(string sql)
        {
            DataTable dt = new DataTable();
            using (SqlConnection c = new SqlConnection(_connectionString))
            using (SqlDataAdapter sda = new SqlDataAdapter(sql, c))
            {
                c.Open();
                sda.Fill(dt);
            }
            return dt;
        }

        public List<T> GetEx<T>(params DbParam[] parameters) where T : IDbItem
        {
            var ins = Activator.CreateInstance(typeof(T)) as IDbItem;
            var sql = $"select * from {ins.GetTableName()}";
            return Get<T>(sql, parameters);
        }

        public List<T> Get<T>(string sql, params DbParam[] parameters) where T : IDbItem
        {
            List<T> lst = new List<T>();
            CheckTable((T)Activator.CreateInstance(typeof(T)));
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);
                if (parameters != null)
                {
                    foreach (var item in parameters)
                        command.Parameters.AddWithValue(item.Name, item.Value);
                }
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var i = (T)Activator.CreateInstance(typeof(T));
                        i.Load(reader);
                        //i.LoadAccess(this);
                        lst.Add(i);
                    }
                }
            }
            return lst;
        }

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

}

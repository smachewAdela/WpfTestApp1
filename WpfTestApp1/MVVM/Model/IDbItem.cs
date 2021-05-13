using System.Collections.Generic;
using System.Data.SqlClient;

namespace QBalanceDesktop
{
    public interface IDbItem
    {
        List<string> GetInsertFields();
        string GetTableName();
        string GetIdentityField();
        object GetValue(string dbInsertField);
        void SetDbIdentity(int newId);
        void Load(SqlDataReader reader);
        int GetDbIdentity();
        //void LoadAccess(DbAccess dbAccessProvider);
    }
}
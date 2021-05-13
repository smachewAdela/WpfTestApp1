using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace QBalanceDesktop
{
    public class BaseDbItem : IDbItem
    {
        [DbField(Identity = true)]
        public int Id { get; set; }

        public void LoadAccess(DbAccess dbAccess)
        {
            db = dbAccess;
        }

        internal DbAccess db { get; set; }

        public string GetIdentityField()
        {
            return DbFieldsDictionary().Where(x => x.Value.Identity).FirstOrDefault().Key.Name;
        }

        public List<string> GetInsertFields()
        {
            return DbFieldsDictionary().Where(x => !x.Value.Identity).Select(x => x.Key.Name).ToList();
        }

        public virtual string GetTableName()
        {
            var dbEntity = GetType().GetCustomAttributes(typeof(DbEntityAttribute), true).FirstOrDefault();
            if (dbEntity != null)
            {
                return (dbEntity as DbEntityAttribute).TableName;
            }
            return GetType().Name;
        }

        public object GetValue(string dbInsertField)
        {
            var iProp = DbFieldsDictionary().Where(x => x.Key.Name == dbInsertField).FirstOrDefault();
            return iProp.Key.GetValue(this) ?? DBNull.Value;
        }

        public void Load(SqlDataReader reader)
        {
            var dbFields = DbFieldsDictionary().Keys.ToList();
            foreach (var dbField in dbFields)
            {
                var dbVal = reader[dbField.Name.ToString()];
                if (dbVal != null)
                {
                    if (dbField.PropertyType.IsEnum)
                        dbVal = Convert.ChangeType(dbVal, typeof(int));
                    else if (dbField.PropertyType == typeof(bool))
                        dbVal = dbVal.ToString() == "1" || dbVal.ToString().ToLower() == "true";
                    else
                        dbVal = Convert.ChangeType(dbVal, dbField.PropertyType);
                }
                dbField.SetValue(this, dbVal);
            }

            LoadExtraData();
        }

        public virtual void LoadExtraData()
        {
            
        }

        public void SetDbIdentity(int newId)
        {
            var idProp = DbFieldsDictionary().Where(x => x.Value.Identity).FirstOrDefault();
            idProp.Key.SetValue(this, newId);
        }

        public int GetDbIdentity()
        {
            var idProp = DbFieldsDictionary().Where(x => x.Value.Identity).FirstOrDefault();
            return (int)idProp.Key.GetValue(this);
        }

        private Dictionary<PropertyInfo, DbFieldAttribute> DbFieldsDictionary()
        {
            var lst = new Dictionary<PropertyInfo, DbFieldAttribute>();
            var props = GetType().GetProperties();
            foreach (var prop in props)
            {
                var pa = prop.GetCustomAttributes(typeof(DbFieldAttribute), true).FirstOrDefault();
                if (pa != null)
                {
                    var paa = pa as DbFieldAttribute;
                    lst.Add(prop, paa);
                }
            }
            return lst;
        }
    }

}
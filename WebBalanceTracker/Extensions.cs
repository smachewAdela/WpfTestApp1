using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebBalanceTracker
{
    public static class Extensions
    {
        public static void AddColumns(this DataTable tbl , int colsNumber, params string[] cols)
        {
            for (int i = 0; i < colsNumber; i++)
            {
                if (cols != null && cols.Length <= colsNumber)
                    tbl.Columns.Add();
                else
                    tbl.Columns.Add(cols[i]);
            }
        }

        public static dynamic ToDynamicJObject(this string str)
        {
            return JObject.Parse(str);
        }

        public static int ToInteger(this object str)
        {
            return Convert.ToInt32(str);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QBalanceDesktop
{
    public static class Ext
    {
        public static string ToNumberFormat(this int num)
        {
            return String.Format("{0:n0}", num); 
        }

        public static string ToFormatedDate(this DateTime d)
        {
            return $"{d.Year}-{d.Month.ToString().PadLeft(2,'0')}-{d.Day.ToString().PadLeft(2, '0')}";
        }

        #region Date

        public static DateTime FirstDayOfMonth(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, 1);
        }

        public static int NumberOfWeeksInMonth(this DateTime d)
        {
            //extract the month
            int daysInMonth = DateTime.DaysInMonth(d.Year, d.Month);
            DateTime firstOfMonth = new DateTime(d.Year, d.Month, 1);
            //days of week starts by default as Sunday = 0
            int firstDayOfMonth = (int)firstOfMonth.DayOfWeek;
            int weeksInMonth = (int)Math.Ceiling((firstDayOfMonth + daysInMonth) / 7.0);
            return weeksInMonth;
        }

        public static DateTime lastDayOfMonth(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, DateTime.DaysInMonth(d.Year, d.Month));
        }

        public static DateTime FirstDayOfTheWeek(this DateTime d)
        {
            var tmpD = new DateTime(d.Year, d.Month, d.Day);

            while (tmpD.DayOfWeek != DayOfWeek.Sunday)
            {
                tmpD = tmpD.AddDays(-1);
            }
            return tmpD;
        }

        public static DateTime FirstDayOfTheNextWeek(this DateTime d)
        {
            var tmpD = new DateTime(d.Year, d.Month, d.Day + 1);

            while (tmpD.DayOfWeek != DayOfWeek.Sunday)
            {
                tmpD = tmpD.AddDays(1);
            }
            return tmpD;
        }

        public static DateTime LastDayOfTheWeek(this DateTime d)
        {
            var tmpD = new DateTime(d.Year, d.Month, d.Day, 23, 59, 59);

            while (tmpD.DayOfWeek != DayOfWeek.Saturday)
            {
                tmpD = tmpD.AddDays(1);
            }
            return tmpD;
        }


        #endregion


        public static bool IsNotEmpty(this IList lst)
        {
            return lst != null && lst.Count > 0;
        }

        public static String GetEnumDescription(this Enum obj)
        {
            try
            {
                System.Reflection.FieldInfo fieldInfo =
                    obj.GetType().GetField(obj.ToString());

                object[] attribArray = fieldInfo.GetCustomAttributes(false);

                if (attribArray.Length > 0)
                {
                    var attrib = attribArray[0] as DescriptionAttribute;

                    if (attrib != null)
                        return attrib.Description;
                }
                return obj.ToString();
            }
            catch (NullReferenceException ex)
            {
                return "Unknown";
            }
        }
    }
}

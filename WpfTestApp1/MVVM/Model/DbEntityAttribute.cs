using System;

namespace QBalanceDesktop
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DbEntityAttribute : Attribute
    {
        public DbEntityAttribute(string v)
        {
            this.TableName = v;
        }

        public string TableName { get; set; }
    }
}
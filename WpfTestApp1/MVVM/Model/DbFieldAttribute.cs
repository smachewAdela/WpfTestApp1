using System;

namespace QBalanceDesktop
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbFieldAttribute : Attribute
    {
        public bool Identity { get; set; }
    }
}
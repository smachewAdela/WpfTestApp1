﻿namespace QBalanceDesktop
{
    public class DbParam
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public DbParam(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
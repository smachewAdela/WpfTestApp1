using System;
using System.Collections.Generic;
using System.Text;

namespace QBalanceDesktop
{
   public  class ListItem
    {
        public int Key { get; set; }
        public string Display { get; set; }
        public object ExtraData { get; set; }

        public override string ToString()
        {
            return Display;
        }
    }
}

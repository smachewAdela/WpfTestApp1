using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp1.MVVM.Model
{
    public class DuplicateBudgetException : Exception
    {
        public DuplicateBudgetException(string message) : base(message)
        {
        }
    }
}

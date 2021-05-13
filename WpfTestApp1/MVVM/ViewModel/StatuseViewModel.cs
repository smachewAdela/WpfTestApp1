using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp1.MVVM.ViewModel
{
    class StatuseViewModel
    {
        public List<Group> A
        {
            get;
            set;
        }

        Random rnd = new Random();
        public StatuseViewModel()
        {
            A = new List<Group>();

            for (int i = 1; i < 10; i++)
            {
                Group iv = new Group();
                iv.Heading = "Group " + i;
                iv.BudgetAmount = rnd.Next(4500);
                iv.StatusAmount = rnd.Next(4500);
                A.Add(iv);
            }
        }
    }

    public class Group
    {
        public int StatusAmount { get; set; }
        public int BudgetAmount { get; set; }
        public string Heading { get; set; }

        public List<string> Values
        {
            get;
            set;
        }
    }

}

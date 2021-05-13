using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp1.MVVM.ViewModel
{
    class StatuseViewModel
    {
        public List<Inventory> A
        {
            get;
            set;
        }

        public StatuseViewModel()
        {
            A = new List<Inventory>();

            for (int i = 1; i < 10; i++)
            {
                Inventory iv = new Inventory();
                iv.Heading = "Group " + i;
                iv.Values = new List<string>();
                for (int j = 0; j < 3; j++)
                {
                    iv.Values.Add("G Sum ");
                }
                A.Add(iv);
            }
        }
    }

    public class Inventory
    {
        public string Heading
        {
            get;
            set;
        }

        public List<string> Values
        {
            get;
            set;
        }
    }

}

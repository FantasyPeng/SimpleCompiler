using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR0
{
    public class Production
    {
        string left, right;

        public string Right
        {
            get { return right; }
            set { right = value; }
        }

        public string Left
        {
            get { return left; }
            set { left = value; }
        }
        public Boolean equals(Production p)
        {
            if (p.Right == right && p.Left == left)
                return true;
            else
                return false;
        }
    }
}

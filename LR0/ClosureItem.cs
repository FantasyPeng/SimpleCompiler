using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR0
{
    public class ClosureItem
    {
        private string left, right;
        private int fromitem;

        public int Fromitem
        {
            get { return fromitem; }
            set { fromitem = value; }
        }
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
        private int dot;

        public int Dot
        {
            get { return dot; }
            set { dot = value; }
        }
        public String getStrClo()
        {
            String str = "";
            String str1 = "";
            str += left + "->";
            String rightB = right.Insert(dot,".");
            str += rightB;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != '-' && str[i] != '>' && str[i] != '.')
                    str1 += ((int)str[i]).ToString() + " ";
                else
                {
                    str1 += str[i];
                }
            }
            return str1;
        }
        public Boolean equal(ClosureItem clo1)
        {
            if (clo1.Dot == dot && clo1.Left.Equals(left) && clo1.Right.Equals(right))
                return true;
            else
                return false;
        }
    }
}

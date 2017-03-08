using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR0
{
    public class Closure
    {
        private List<ClosureItem> clo = new List<ClosureItem>();

        public List<ClosureItem> Clo
        {
            get { return clo; }
            set { clo = value; }
        }
        private Dictionary<String, int> dic = new Dictionary<String, int>();

        public Dictionary<String, int> Dic
        {
            get { return dic; }
            set { dic = value; }
        }
        private int item;

        public int Item
        {
            get { return item; }
            set { item = value; }
        }
        public String getStrCLosure()
        {
            String str = "";
            str += "----Closure-I"+item+"---\r\n";
            foreach(ClosureItem c in clo)
            {
                str += c.getStrClo();
                str += "\r\n";
            }
            str += "路径：\r\n";
            foreach(String key in dic.Keys)
            {
                str += (int)key[0] + "->" + dic[key];
                str += "\r\n";
            }
            return str;
        }
        public Boolean equals(Closure cc)
        {
            List<ClosureItem> lc = cc.Clo;
            Boolean flag1 = false;
            foreach(ClosureItem c in clo)
            {
                foreach(ClosureItem y in lc)
                {
                    if(y.equal(c))
                    {
                        flag1 = true;
                        break;
                    }
                }
                if (flag1 == false)
                    return false;
            }
            foreach (ClosureItem c in lc)
            {
                foreach (ClosureItem y in clo)
                {
                    if (y.equal(c))
                    {
                        flag1 = true;
                        break;
                    }
                }
                if (flag1 == false)
                    return false;
            }
            return true;
        }
    }
}

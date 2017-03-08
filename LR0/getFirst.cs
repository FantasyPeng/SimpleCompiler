using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR0
{
    class getFirst
    {
        public static List<string> l_grammar;
        public static List<string> l_vt;
        public static List<string> l_vn;
        public static Dictionary<string, List<string>> d_grammer;
        public static Dictionary<string, List<string>> get_First(String grammar, String strVT, String strVN, String strS)
        {
            //List<Dictionary<string, string>> list_first = new List<Dictionary<string, string>>();
            Dictionary<string, List<string>> d_first = new Dictionary<string, List<string>>();
            d_grammer = new Dictionary<string, List<string>>();
            //d_grammer的key值为非终结符，而右边是他可以推出的产生式

            List<string> s = new List<string>();  //s用来存储刚被->和|划分完的
            //将产生式、符号分割为一条一条的语句或字符//
            l_grammar = grammar.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            l_vt = strVT.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            l_vn = strVN.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //将产生式以->和|为界划分为左右两部分
            foreach (string i in l_grammar)
            {
                List<string> behind_1 = new List<string>();//behind存储->右边的式子
                s = i.Split(new string[] { "->"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (d_grammer.ContainsKey(s[0]))
                {
                    behind_1 = d_grammer[s[0]];
                    behind_1.Add(s[1]);
                    d_grammer[s[0]] = behind_1;
                }
                else
                {
                    behind_1.Add(s[1]);
                    d_grammer.Add(s[0], behind_1);
                }

                //本来是写成下面一样的清空数组，但是情况不太对，一旦把behind清空，那么d_grammer中的values
                //值也会被清空，这是C#的特色吗！？？？？只好在每次循环开始都重新声明一次behind_1
                //behind_1.Clear();
            }

            //foreach (string key in d_grammer.Keys)
            //{
            //    foreach(string i in d_grammer[key])
            //        Console.WriteLine("key: {0}  value:  {1}", key, i);
            //}

            for (int i = 0; i < l_vt.Count; i++)
            {
                List<string> vt = new List<string>();
                vt.Add(l_vt[i]);
                d_first.Add(l_vt[i], vt);
            }
            foreach (string i in l_vn)
            {
                List<string> vn_1 = new List<string>();
                //foreach (string j in d_grammer[i])
                //{
                //    if (l_vt.Contains(j[0].ToString()))//当j即产生式的第一个字符为vt时，first（i）集加入该字符
                //        vn_1.Add(j[0].ToString());     //该vt中包括$
                //}
                d_first.Add(i, vn_1);
            }
            bool if_stop = false;
            int count = 0;
            while (!if_stop)
            {

                foreach (string i in l_vn)
                {
                    foreach (string j in d_grammer[i])
                    {
                        int m = 0;
                        bool flag = true;
                        while (m < j.Length && l_vn.Contains(j[m].ToString())) //循环判断每个产生式的字串
                        {
                            if (d_first[j[m].ToString()].Contains("$"))//当产生式当前面对的字符为VN且，该字符可推出$
                            {
                                if (m == j.Length - 1)   //若直到最后一个非中结字符都可以推出$,则将该终结符的first集
                                {                        //加入i的first集，其中包括了$
                                    d_first[i].AddRange(d_first[j[m].ToString()]);
                                }
                                else                       //若字符不是最后一个，则将$去除，再加入i的first集
                                {
                                    List<string> vn_3 = new List<string>();
                                    vn_3.AddRange(d_first[j[m].ToString()]);
                                    //剔除$
                                    for (int l = 0; l < vn_3.Count; l++)
                                    {
                                        if (vn_3[l].Equals("$"))
                                        {
                                            vn_3.RemoveAt(l);
                                        }
                                    }
                                    d_first[i].AddRange(vn_3);
                                }
                            }

                            else //当产生式当前循环到的字符为VN且，该字符的首符集中无$
                            {
                                d_first[i].AddRange(d_first[j[m].ToString()]);
                                flag = false;
                                break;
                            }
                            m++;
                        }
                        if (m != j.Length && flag)//当产生式是S->ABa,循环到a时跳出循环，并且将a加入i的fisrt集
                        {
                            d_first[i].Add(j[m].ToString());
                        }
                    }
                }
                //计算first集是否发生了改变                
                int count_1 = count;
                count = 0;
                //使用了linq方法便捷，由于foreach方法中无法直接更改d_first,需要用for
                for (int n = 0; n < d_first.Keys.Count; n++)
                {
                    int idx = 0;
                    string key = "";
                    foreach (string i in d_first.Keys)
                    {
                        if (idx == n)
                        {
                            key = string.Copy(i);
                            break;
                        }
                        idx++;
                    }
                    d_first[key] = d_first[key].Distinct().ToList();
                }
                foreach (string i in d_first.Keys)
                {
                    foreach (string sk in d_first[i])
                    {
                        count++;
                    }
                }
                if (count_1 == count)
                {
                    if_stop = true;
                }
            }
            foreach (string i in l_vn)
            {
                foreach (string j in d_grammer[i])
                {
                    if (j.Length != 1)
                    {
                        List<string> vn_1 = new List<string>();
                        d_first.Add(j, vn_1);
                    }
                }
            }
            foreach (string i in l_vn)
            {
                foreach (string j in d_grammer[i])
                {
                    if (j.Length != 1)
                    {
                        Dictionary<string, List<string>> d_first3 = new Dictionary<string, List<string>>();
                        d_first3 = getFollow.get_First2(j, d_first, l_vn);
                        d_first[j].AddRange(d_first3[j]);
                    }
                }
            }

            return d_first;
        }
    }
}

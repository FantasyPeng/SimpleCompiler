using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR0
{
    class getFollow
    {
        public static List<string> l_grammar;
        public static List<string> l_vt;
        public static List<string> l_vn;
        public static Dictionary<string, List<string>> d_grammer;
        private static Dictionary<string, List<string>> d_first2;
        public static Dictionary<string, List<string>> get_Follow(String grammar, String strVT, String strVN,String strS)
        {
            Dictionary<string, List<string>> d_follow = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> d_grammer = new Dictionary<string, List<string>>();
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
                s = i.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries).ToList();
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
            }
            foreach (string i in l_vn)//初始化d_follow
            {
                List<string> vn_1 = new List<string>();
                d_follow.Add(i, vn_1);
            }
            int m;
            int count = -1;
            int count1 = 0;
            foreach (string i in d_follow.Keys)
            {
                foreach (string sk in d_follow[i])
                {
                    count1++;
                }
            }
            while (count != count1)
            {
                count = count1;
                List<string> l1 = d_follow[strS];
                l1.Add("#");
                l1 = l1.Distinct().ToList();
                d_follow[strS] = l1;
                foreach (string i in l_vn)
                {
                    foreach (string j in d_grammer[i])
                    {
                        m = 0;
                        while (m < j.Length)
                        {
                            String ss = j[m].ToString();
                            if (l_vn.Contains(ss))
                            {
                                string strbe = j.Substring(m + 1, j.Length - m - 1);
                                if (strbe != "")
                                {
                                    List<string> l2 = d_follow[ss];
                                    List<string> l3 = new List<string>();
                                    Dictionary<string, List<string>> d1 = get_First2(strbe,Form1.d_first,l_vn);
                                    l3.AddRange(d1[strbe]);
                                    l2.AddRange(l3);
                                    l2 = l2.Distinct().ToList();
                                    d_follow[ss] = l2;
                                }
                                else
                                {
                                    List<string> l2 = d_follow[i];
                                    List<string> l3 = d_follow[ss];
                                    l3.AddRange(l2);
                                    l3 = l3.Distinct().ToList();
                                    d_follow[ss] = l3;
                                }
                            }
                            m++;
                        }

                    }
                }
                count1 = 0;
                foreach (string i in d_follow.Keys)
                {
                    foreach (string sk in d_follow[i])
                    {
                        count1++;
                    }
                }
            }
            return d_follow;
        }
        public static Dictionary<string, List<string>> get_First2(String str, Dictionary<string, List<string>> d_first, List<string> l_vn1)
        {
            d_first2 = new Dictionary<string, List<string>>();
            //初始化d_first2

            List<string> s = new List<string>();
            d_first2.Add(str, s);
            int i = 0;
            bool flag = true;
            while (i < str.Length && l_vn1.Contains(str[i].ToString()))
            {
                if (d_first[str[i].ToString()].Contains("$"))
                {
                    if (i == str.Length - 1)
                    {
                        d_first2[str].AddRange(d_first[str[i].ToString()]);
                    }
                    else
                    {
                        List<string> vn_3 = new List<string>();
                        vn_3.AddRange(d_first[str[i].ToString()]);
                        //剔除$
                        for (int l = 0; l < vn_3.Count; l++)
                        {
                            if (vn_3[l].Equals("$"))
                            {
                                vn_3.RemoveAt(l);
                            }
                        }
                        d_first2[str].AddRange(vn_3);
                    }
                }
                else
                {
                    d_first2[str].AddRange(d_first[str[i].ToString()]);
                    flag = false;
                    break;
                }
                i++;
            }

            if (i != str.Length && flag)
            {
                d_first2[str].Add(str[i].ToString());
            }
            d_first2[str] = d_first2[str].Distinct().ToList();
            return d_first2;
        }
    }
}

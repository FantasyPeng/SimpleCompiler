﻿using System;
using System.IO;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace LR0
{
    public partial class Form1 : Form
    {
        public static List<Production> lp = new List<Production>();
        public static Dictionary<string, List<string>> d_first = new Dictionary<string, List<string>>();
        public static Dictionary<string, List<string>> d_follow = new Dictionary<string, List<string>>();
        public static List<Dictionary<string, string>> Table = new List<Dictionary<string, string>>();//Action和GOTO表
        public static List<ClosureItem> ClosureI = new List<ClosureItem>();
        public static List<Closure> ClosureP = new List<Closure>();
        public static List<String> VN;
        public static List<String> VT;
        public static String beginC;

        int success = 0;
        public Form1()
        {
            InitializeComponent();

            String filename = "E:\\Table.txt";


            MyFile file1 = new MyFile();
            //如果文件不存在，就创建文件             

            getClosureItem(pro);
            if (!File.Exists(filename))
            {
                getAllClosure(ClosureI[0]);
                getLRTable();
                file1.write();
            }
            else
            {
                file1.read();
                Console.WriteLine("读取结束");
            }
            PrintTable();
        }
        String grammar = "";
        String strVT = "";
        String strVN = "";
        

        public static Dictionary<String, String> convert = new Dictionary<String, string> { 
        { ((char)Symbol.TYPE.program).ToString(), "program" } ,
        { ((char)Symbol.TYPE.myconst).ToString(), "const" } ,
        { ((char)Symbol.TYPE.var).ToString(), "var" } ,
        { ((char)Symbol.TYPE.procedure).ToString(), "procedure" } ,
        { ((char)Symbol.TYPE.begin).ToString(), "begin" } ,
        { ((char)Symbol.TYPE.end).ToString(), "end" } ,
        { ((char)Symbol.TYPE.myif).ToString(), "if" } ,
        { ((char)Symbol.TYPE.then).ToString(), "then" } ,
        { ((char)Symbol.TYPE.myelse).ToString(), "else" } ,
        { ((char)Symbol.TYPE.mydo).ToString(), "do" } ,
        { ((char)Symbol.TYPE.mywhile).ToString(), "while" } ,
        { ((char)Symbol.TYPE.call).ToString(), "call" } ,
        { ((char)Symbol.TYPE.read).ToString(), "read" } ,
        { ((char)Symbol.TYPE.write).ToString(), "write" } ,
        { ((char)Symbol.TYPE.repeat).ToString(), "repeat" } ,
        { ((char)Symbol.TYPE.unti).ToString(), "unti" } ,

        { ((char)Symbol.TYPE.PROG).ToString(), "<prog>" } ,
        { ((char)Symbol.TYPE.PROC).ToString(), "<proc>" } ,
        { ((char)Symbol.TYPE.ID).ToString(), "<id>" } ,
        { ((char)Symbol.TYPE.BLOCK).ToString(), "<block>" } ,
        { ((char)Symbol.TYPE.CONDECL).ToString(), "<condecl>" } ,
        { ((char)Symbol.TYPE.VARDECL).ToString(), "<vardecl>" } ,
        { ((char)Symbol.TYPE.BODY).ToString(), "<body>" } ,
        { ((char)Symbol.TYPE.STATEMENT).ToString(), "<statement>" } ,
        { ((char)Symbol.TYPE.EXP).ToString(), "<exp>" } ,
        { ((char)Symbol.TYPE.CONST).ToString(), "<const>" } ,
        { ((char)Symbol.TYPE.INTEGER).ToString(), "<integer>" } ,
        { ((char)Symbol.TYPE.LEXP).ToString(), "<lexp>" } ,
        { ((char)Symbol.TYPE.LOP).ToString(), "<lop>" } ,
        { ((char)Symbol.TYPE.TERM).ToString(), "<term>" } ,
        { ((char)Symbol.TYPE.AOP).ToString(), "<aop>" } ,
        { ((char)Symbol.TYPE.FACTOR).ToString(), "<factor>" } ,
        { ((char)Symbol.TYPE.MOP).ToString(), "<mop>" } ,

        
        { ((char)Symbol.TYPE.DOU).ToString(), "," } ,
        { ((char)Symbol.TYPE.FEN).ToString(), ";" } ,
        { ((char)Symbol.TYPE.FUZHI).ToString(), ":=" } ,
        { ((char)Symbol.TYPE.ADD).ToString(), "+" } ,
        { ((char)Symbol.TYPE.SUB).ToString(), "-" } ,
        { ((char)Symbol.TYPE.MUL).ToString(), "*" } ,
        { ((char)Symbol.TYPE.DIV).ToString(), "/" } ,
        { ((char)Symbol.TYPE.LEFT_P).ToString(), "(" } ,
        { ((char)Symbol.TYPE.RIGHT_P).ToString(), ")" } ,
        { ((char)Symbol.TYPE.N_EQUAL).ToString(), "<>" } ,
        { ((char)Symbol.TYPE.EQUAL).ToString(), "=" } ,
        { ((char)Symbol.TYPE.LESS).ToString(), "<" } ,
        { ((char)Symbol.TYPE.LESS_E).ToString(), "<=" } ,
        { ((char)Symbol.TYPE.BIG).ToString(), ">" } ,
        { ((char)Symbol.TYPE.BIG_E).ToString(), ">=" } ,


        { ((char)Symbol.TYPE.PCONDECL).ToString(), "Pcondecl" } , 
        { ((char)Symbol.TYPE.NULL).ToString(), "$" } ,
        { ((char)Symbol.TYPE.PVARDECL).ToString(), "P<vardecl>" } ,
        { ((char)Symbol.TYPE.PM).ToString(), "Pm" } ,
        { ((char)Symbol.TYPE.PPROC).ToString(), "P<proc>" } ,
        { ((char)Symbol.TYPE.PBODY ).ToString(), "P<body>" } ,
        { ((char)Symbol.TYPE.PSTATEMENT).ToString(), "P<statement>" } ,
        { ((char)Symbol.TYPE.PSTATEMENT1).ToString(), "P<statement>1" } ,
         // { ((char)Symbol.TYPE.PEX).ToString(), "P<integer>" } ,
          //  { ((char)Symbol.TYPE.PSTATEMENT_EXP).ToString(), "P<id>" } ,
            { ((char)Symbol.TYPE.PTERM).ToString(), "P<term>" } ,
            { ((char)Symbol.TYPE.PEXP).ToString(), "P<exp>" } ,
            { ((char)Symbol.TYPE.PSTATEMENT_EXP).ToString(), "P<STATEMENT_EXP>" } ,
            { ((char)Symbol.TYPE.odd).ToString(), "odd" } ,
            { ((char)Symbol.TYPE.jinhao).ToString(), "#" } ,
            { ((char)Symbol.TYPE.Pmyelse).ToString(), "P<else>" } ,
        };
        List<String> pro = new List<string>{
            //终结符
            ((char)Symbol.TYPE.program).ToString() + "," + ((char)Symbol.TYPE.myconst).ToString() + "," +((char)Symbol.TYPE.FEN).ToString() + ","  + ((char)Symbol.TYPE.FUZHI).ToString() + ","  + ((char)Symbol.TYPE.DOU).ToString() + ","  + 
            ((char)Symbol.TYPE.procedure).ToString() + "," +((char)Symbol.TYPE.begin).ToString() + "," + ((char)Symbol.TYPE.end).ToString() + ","  + ((char)Symbol.TYPE.myif).ToString() + ","  + ((char)Symbol.TYPE.then).ToString() + ","  + 
            ((char)Symbol.TYPE.myelse).ToString() + "," +((char)Symbol.TYPE.mywhile).ToString() + "," + ((char)Symbol.TYPE.mydo).ToString() + ","  + ((char)Symbol.TYPE.call).ToString() + ","  + ((char)Symbol.TYPE.LEFT_P).ToString() + ","  + 
            ((char)Symbol.TYPE.RIGHT_P).ToString() + "," +((char)Symbol.TYPE.read).ToString() + "," + ((char)Symbol.TYPE.write).ToString() + ","  + ((char)Symbol.TYPE.odd).ToString() + ","  + ((char)Symbol.TYPE.ADD).ToString() + ","  +
            ((char)Symbol.TYPE.SUB).ToString() + "," +((char)Symbol.TYPE.EQUAL).ToString() + "," + ((char)Symbol.TYPE.N_EQUAL).ToString() + ","  + ((char)Symbol.TYPE.LESS).ToString() + ","  + ((char)Symbol.TYPE.LESS_E).ToString() + ","  + 
            ((char)Symbol.TYPE.BIG).ToString() + "," +((char)Symbol.TYPE.BIG_E).ToString() + ","  + ((char)Symbol.TYPE.MUL).ToString() + ","  + ((char)Symbol.TYPE.DIV).ToString() + ","  + ((char)Symbol.TYPE.var).ToString()  + ","  + 
            ((char)Symbol.TYPE.INTEGER).ToString() + "," + ((char)Symbol.TYPE.ID).ToString()+ "," + ((char)Symbol.TYPE.jinhao).ToString()+ ","  + ((char)Symbol.TYPE.NULL).ToString(),

            //非终结符
            ((char)Symbol.TYPE.PROG).ToString() + "," + ((char)Symbol.TYPE.BLOCK).ToString() + "," +((char)Symbol.TYPE.CONDECL).ToString() + ","  + ((char)Symbol.TYPE.CONST).ToString() + ","  + ((char)Symbol.TYPE.VARDECL).ToString() + ","  + 
            ((char)Symbol.TYPE.PROC).ToString() + "," +((char)Symbol.TYPE.BODY).ToString() + "," + ((char)Symbol.TYPE.STATEMENT).ToString() + ","  + ((char)Symbol.TYPE.LEXP).ToString() + ","  + ((char)Symbol.TYPE.EXP).ToString() + ","  +
            ((char)Symbol.TYPE.TERM).ToString() + "," +((char)Symbol.TYPE.FACTOR).ToString() + "," + ((char)Symbol.TYPE.LOP).ToString() + ","  + ((char)Symbol.TYPE.AOP).ToString() + ","  + ((char)Symbol.TYPE.MOP).ToString() + ","  + 
            ((char)Symbol.TYPE.PCONDECL).ToString()  + ","  + ((char)Symbol.TYPE.PVARDECL).ToString() + ","  + ((char)Symbol.TYPE.PM).ToString() + "," +((char)Symbol.TYPE.PPROC).ToString() + "," + 
            ((char)Symbol.TYPE.PBODY).ToString() + ","  + ((char)Symbol.TYPE.PSTATEMENT).ToString() + ","  + ((char)Symbol.TYPE.PSTATEMENT1).ToString() + ","  + ((char)Symbol.TYPE.PTERM).ToString() + ","  + ((char)Symbol.TYPE.PEXP).ToString() + ","  + 
            ((char)Symbol.TYPE.PSTATEMENT_EXP).ToString() + "," + ((char)Symbol.TYPE.Pmyelse).ToString(),
            
            //开始符
            ((char)Symbol.TYPE.PROG).ToString(),
            ((char)Symbol.TYPE.PROG).ToString() + "->" + ((char)Symbol.TYPE.program).ToString() + ((char)Symbol.TYPE.ID).ToString() + ((char)Symbol.TYPE.FEN).ToString() + ((char)Symbol.TYPE.BLOCK).ToString(),
            ((char)Symbol.TYPE.BLOCK).ToString() + "->" + ((char)Symbol.TYPE.BODY).ToString(),
            ((char)Symbol.TYPE.BLOCK).ToString() + "->" + ((char)Symbol.TYPE.CONDECL).ToString() + ((char)Symbol.TYPE.BODY).ToString(),
            ((char)Symbol.TYPE.BLOCK).ToString() + "->" + ((char)Symbol.TYPE.VARDECL).ToString() + ((char)Symbol.TYPE.BODY).ToString(),
            ((char)Symbol.TYPE.BLOCK).ToString() + "->" + ((char)Symbol.TYPE.PROC).ToString() + ((char)Symbol.TYPE.BODY).ToString(),
            ((char)Symbol.TYPE.BLOCK).ToString() + "->" + ((char)Symbol.TYPE.CONDECL).ToString() + ((char)Symbol.TYPE.VARDECL).ToString() + ((char)Symbol.TYPE.BODY).ToString(),
            ((char)Symbol.TYPE.BLOCK).ToString() + "->" + ((char)Symbol.TYPE.CONDECL).ToString() + ((char)Symbol.TYPE.PROC).ToString() + ((char)Symbol.TYPE.BODY).ToString(),
            ((char)Symbol.TYPE.BLOCK).ToString() + "->" + ((char)Symbol.TYPE.VARDECL).ToString() + ((char)Symbol.TYPE.PROC).ToString() + ((char)Symbol.TYPE.BODY).ToString(),
            ((char)Symbol.TYPE.BLOCK).ToString() + "->" + ((char)Symbol.TYPE.CONDECL).ToString() + ((char)Symbol.TYPE.VARDECL).ToString() + ((char)Symbol.TYPE.PROC).ToString() + ((char)Symbol.TYPE.BODY).ToString(),

            //<condecl> → const <const>{,<const>}
            ((char)Symbol.TYPE.CONDECL).ToString() + "->" + ((char)Symbol.TYPE.myconst).ToString() + ((char)Symbol.TYPE.CONST).ToString() + ((char)Symbol.TYPE.PCONDECL).ToString(),
            ((char)Symbol.TYPE.PCONDECL).ToString() + "->" + ((char)Symbol.TYPE.DOU).ToString() + ((char)Symbol.TYPE.CONST).ToString() + ((char)Symbol.TYPE.PCONDECL).ToString(),
            ((char)Symbol.TYPE.PCONDECL).ToString() + "->" + ((char)Symbol.TYPE.NULL).ToString(),

            //<const> → <id>:=<integer>
            ((char)Symbol.TYPE.CONST).ToString() + "->" + ((char)Symbol.TYPE.ID).ToString() + ((char)Symbol.TYPE.FUZHI).ToString() + ((char)Symbol.TYPE.INTEGER).ToString(),

            //<vardecl> → var <id>{,<id>}
            ((char)Symbol.TYPE.VARDECL).ToString() + "->" + ((char)Symbol.TYPE.var).ToString() + ((char)Symbol.TYPE.ID).ToString() + ((char)Symbol.TYPE.PVARDECL).ToString(),
            ((char)Symbol.TYPE.PVARDECL).ToString() + "->" + ((char)Symbol.TYPE.DOU).ToString() + ((char)Symbol.TYPE.ID).ToString() + ((char)Symbol.TYPE.PVARDECL).ToString(),
            ((char)Symbol.TYPE.PVARDECL).ToString() + "->" + ((char)Symbol.TYPE.NULL).ToString(),

            //<proc> → procedure <id>（[<id>{,<id>}]）;<block>{;<proc>}
            ((char)Symbol.TYPE.PROC).ToString() + "->" + ((char)Symbol.TYPE.procedure).ToString() + ((char)Symbol.TYPE.ID).ToString() + ((char)Symbol.TYPE.LEFT_P).ToString() + ((char)Symbol.TYPE.PM).ToString() + ((char)Symbol.TYPE.RIGHT_P).ToString() + ((char)Symbol.TYPE.FEN).ToString() + ((char)Symbol.TYPE.BLOCK).ToString() + ((char)Symbol.TYPE.PPROC).ToString(),
            ((char)Symbol.TYPE.PPROC).ToString() + "->" + ((char)Symbol.TYPE.FEN).ToString() + ((char)Symbol.TYPE.PROC).ToString() + ((char)Symbol.TYPE.PPROC).ToString(),
            ((char)Symbol.TYPE.PPROC).ToString() + "->" + ((char)Symbol.TYPE.NULL).ToString(),
            ((char)Symbol.TYPE.PM).ToString() + "->" + ((char)Symbol.TYPE.NULL).ToString(),
            ((char)Symbol.TYPE.PM).ToString() + "->" +  ((char)Symbol.TYPE.ID).ToString() + ((char)Symbol.TYPE.PVARDECL).ToString(),

            //<body> → begin <statement>{;<statement>}end
            ((char)Symbol.TYPE.BODY).ToString() + "->" +  ((char)Symbol.TYPE.begin).ToString() + ((char)Symbol.TYPE.STATEMENT).ToString() + ((char)Symbol.TYPE.PBODY).ToString() + ((char)Symbol.TYPE.end).ToString(),
            ((char)Symbol.TYPE.PBODY).ToString() + "->" + ((char)Symbol.TYPE.FEN).ToString() + ((char)Symbol.TYPE.STATEMENT).ToString() + ((char)Symbol.TYPE.PBODY).ToString(),
            ((char)Symbol.TYPE.PBODY).ToString() + "->" + ((char)Symbol.TYPE.NULL).ToString(),

            //<statement> → <id> := <exp>
            ((char)Symbol.TYPE.STATEMENT).ToString() + "->" + ((char)Symbol.TYPE.ID).ToString() + ((char)Symbol.TYPE.FUZHI).ToString() + ((char)Symbol.TYPE.EXP).ToString(),
            //<statement> → if <lexp> then <statement>[else <statement>]
            ((char)Symbol.TYPE.STATEMENT).ToString() + "->" + ((char)Symbol.TYPE.myif).ToString() + ((char)Symbol.TYPE.LEXP).ToString() + ((char)Symbol.TYPE.then).ToString() + ((char)Symbol.TYPE.STATEMENT).ToString() + ((char)Symbol.TYPE.Pmyelse).ToString(),
            ((char)Symbol.TYPE.Pmyelse).ToString() + "->" + ((char)Symbol.TYPE.NULL).ToString(),
            ((char)Symbol.TYPE.Pmyelse).ToString() + "->" + ((char)Symbol.TYPE.myelse).ToString() + ((char)Symbol.TYPE.STATEMENT).ToString(),
            //<statement> → while <lexp> do <statement>
            ((char)Symbol.TYPE.STATEMENT).ToString() + "->" + ((char)Symbol.TYPE.mywhile).ToString() + ((char)Symbol.TYPE.LEXP).ToString() + ((char)Symbol.TYPE.mydo).ToString() + ((char)Symbol.TYPE.STATEMENT).ToString(),
            //<statement> → call <id>[（<exp>{,<exp>}）]
            ((char)Symbol.TYPE.STATEMENT).ToString() + "->" + ((char)Symbol.TYPE.call).ToString() + ((char)Symbol.TYPE.ID).ToString() + ((char)Symbol.TYPE.PSTATEMENT).ToString(),
            ((char)Symbol.TYPE.PSTATEMENT).ToString()  + "->" + ((char)Symbol.TYPE.NULL).ToString(),
            ((char)Symbol.TYPE.PSTATEMENT).ToString()  + "->" + ((char)Symbol.TYPE.LEFT_P).ToString() + ((char)Symbol.TYPE.EXP).ToString() + ((char)Symbol.TYPE.PSTATEMENT1).ToString() + ((char)Symbol.TYPE.RIGHT_P).ToString(),
            ((char)Symbol.TYPE.PSTATEMENT1).ToString() + "->" + ((char)Symbol.TYPE.NULL).ToString(),
            ((char)Symbol.TYPE.PSTATEMENT1).ToString() + "->" + ((char)Symbol.TYPE.DOU).ToString() + ((char)Symbol.TYPE.EXP).ToString() + ((char)Symbol.TYPE.PSTATEMENT1).ToString(),
             //<statement> → <body>
            ((char)Symbol.TYPE.STATEMENT).ToString()  + "->" + ((char)Symbol.TYPE.BODY).ToString(),
            //<statement> → read (<id>{，<id>})
            ((char)Symbol.TYPE.STATEMENT).ToString()  + "->" + ((char)Symbol.TYPE.read).ToString() + ((char)Symbol.TYPE.LEFT_P).ToString() + ((char)Symbol.TYPE.ID).ToString() + ((char)Symbol.TYPE.PVARDECL).ToString() + ((char)Symbol.TYPE.RIGHT_P).ToString(),

            //<mop> → *|/
            ((char)Symbol.TYPE.MOP).ToString() + "->" + ((char)Symbol.TYPE.MUL).ToString(),
            ((char)Symbol.TYPE.MOP).ToString() + "->" + ((char)Symbol.TYPE.DIV).ToString(),

            //<aop> → +|-
            ((char)Symbol.TYPE.AOP).ToString() + "->" + ((char)Symbol.TYPE.ADD).ToString(),
            ((char)Symbol.TYPE.AOP).ToString() + "->" + ((char)Symbol.TYPE.SUB).ToString(),

            //<lop> → =|<>|<|<=|>|>=
            ((char)Symbol.TYPE.LOP).ToString() + "->" + ((char)Symbol.TYPE.EQUAL).ToString(),
            ((char)Symbol.TYPE.LOP).ToString() + "->" + ((char)Symbol.TYPE.N_EQUAL).ToString(),
            ((char)Symbol.TYPE.LOP).ToString() + "->" + ((char)Symbol.TYPE.LESS).ToString(),
            ((char)Symbol.TYPE.LOP).ToString() + "->" + ((char)Symbol.TYPE.LESS_E).ToString(),
            ((char)Symbol.TYPE.LOP).ToString() + "->" + ((char)Symbol.TYPE.BIG).ToString(),
            ((char)Symbol.TYPE.LOP).ToString() + "->" + ((char)Symbol.TYPE.BIG_E).ToString(),

            //<factor>→<id>|<integer>|(<exp>)
            ((char)Symbol.TYPE.FACTOR).ToString() + "->" + ((char)Symbol.TYPE.ID).ToString(),
            ((char)Symbol.TYPE.FACTOR).ToString() + "->" + ((char)Symbol.TYPE.INTEGER).ToString(),
            ((char)Symbol.TYPE.FACTOR).ToString() + "->" + ((char)Symbol.TYPE.LEFT_P).ToString() + ((char)Symbol.TYPE.EXP).ToString() + ((char)Symbol.TYPE.RIGHT_P).ToString(),

            //<term> → <factor>{<mop><factor>}
 //            ((char)Symbol.TYPE.TERM).ToString() + "->" + ((char)Symbol.TYPE.FACTOR).ToString(),
            ((char)Symbol.TYPE.TERM).ToString() + "->" + ((char)Symbol.TYPE.FACTOR).ToString() + ((char)Symbol.TYPE.PTERM).ToString(),
            ((char)Symbol.TYPE.PTERM).ToString() + "->" + ((char)Symbol.TYPE.MOP).ToString() + ((char)Symbol.TYPE.FACTOR).ToString() + ((char)Symbol.TYPE.PTERM).ToString(),
            ((char)Symbol.TYPE.PTERM).ToString() + "->" + ((char)Symbol.TYPE.NULL).ToString(),

            //<exp> → [+|-]<term>{<aop><term>}
           
   //         ((char)Symbol.TYPE.EXP).ToString() + "->" + ((char)Symbol.TYPE.TERM).ToString(),
            ((char)Symbol.TYPE.EXP).ToString() + "->" + ((char)Symbol.TYPE.ADD).ToString() + ((char)Symbol.TYPE.TERM).ToString() + ((char)Symbol.TYPE.PEXP).ToString(),
            ((char)Symbol.TYPE.EXP).ToString() + "->" + ((char)Symbol.TYPE.SUB).ToString() + ((char)Symbol.TYPE.TERM).ToString() + ((char)Symbol.TYPE.PEXP).ToString(),
            ((char)Symbol.TYPE.EXP).ToString() + "->" + ((char)Symbol.TYPE.TERM).ToString() + ((char)Symbol.TYPE.PEXP).ToString(),
            ((char)Symbol.TYPE.PEXP).ToString() + "->" + ((char)Symbol.TYPE.NULL).ToString(),
            ((char)Symbol.TYPE.PEXP).ToString() + "->" + ((char)Symbol.TYPE.AOP).ToString() + ((char)Symbol.TYPE.TERM).ToString() + ((char)Symbol.TYPE.PEXP).ToString(),

            //<lexp> → <exp> <lop> <exp>|odd <exp>
            ((char)Symbol.TYPE.LEXP).ToString() + "->" + ((char)Symbol.TYPE.EXP).ToString() + ((char)Symbol.TYPE.LOP).ToString() + ((char)Symbol.TYPE.EXP).ToString(),
            ((char)Symbol.TYPE.LEXP).ToString() + "->" + ((char)Symbol.TYPE.odd).ToString() + ((char)Symbol.TYPE.EXP).ToString(),

            //<statement> → write (<exp>{,<exp>}) 
            ((char)Symbol.TYPE.STATEMENT).ToString() + "->" + ((char)Symbol.TYPE.write).ToString() + ((char)Symbol.TYPE.LEFT_P).ToString() + ((char)Symbol.TYPE.EXP).ToString() + ((char)Symbol.TYPE.PSTATEMENT_EXP).ToString() + ((char)Symbol.TYPE.RIGHT_P).ToString(),
            ((char)Symbol.TYPE.PSTATEMENT_EXP).ToString() + "->" + ((char)Symbol.TYPE.DOU).ToString() + ((char)Symbol.TYPE.EXP).ToString() + ((char)Symbol.TYPE.PSTATEMENT_EXP).ToString(),
            ((char)Symbol.TYPE.PSTATEMENT_EXP).ToString() + "->" + ((char)Symbol.TYPE.NULL).ToString(),
        };

        //获取所有的项目，存储在 ClosureI 这个list中
        public void getClosureItem(List<String> LineList)
        {
            listView2.Clear();
            ColumnHeader ch = new ColumnHeader();
            ch.Text = "Syntax:";   //设置列标题  
            ch.Width = 1000;    //设置列宽度  
            ch.TextAlign = HorizontalAlignment.Center;   //设置列的对齐方式  

            listView2.Columns.Add(ch);    //将列头添加到ListView控件。
            int row = 1;
            foreach (String aline in LineList)
            {
                if (row == 1)
                {
                    strVT = aline;
                    VT = aline.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    String str1 = "";
                    for (int i = 0; i < aline.Length; i++)
                    {
                        if (aline[i] != ',')
                            str1 += convert[aline[i].ToString()] + " ";
                    }

                    String vt = "VT:" + str1;

                    ListViewItem lvi = listView2.Items.Add(vt);
                }
                else if (row == 2)
                {
                    strVN = aline;
                    VN = aline.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    String str1 = "";
                    int i = 0;
                    for (; i < aline.Length/2; i++)
                    {
                        if (aline[i] != ',')
                            str1 += convert[aline[i].ToString()] + " ";
                    }
                    String vt = "VN:" + str1;
                    ListViewItem lvi = listView2.Items.Add(vt);
                    str1 = "";
                    for (; i < aline.Length ; i++)
                    {
                        if (aline[i] != ',')
                            str1 += convert[aline[i].ToString()] + " ";
                    }
                    lvi = listView2.Items.Add(str1);
                }
                else if (row == 3)
                {
                    beginC = aline;
                    String vt = "S:" + convert[aline[0].ToString()];
                    ListViewItem lvi = listView2.Items.Add(vt);
                    ListViewItem lv = listView2.Items.Add("Productions:");
                }
                else
                {
                    int count = row - 4;
                    String aline1 = count.ToString() + " ";
                    
                    for (int i = 0; i < aline.Length; i++)
                    {
                        if (aline[i] != '-' && aline[i] != '>')
                            aline1 += convert[aline[i].ToString()] + " ";
                        else
                        {
                            aline1 += aline[i];
                        }
                    }
                    ListViewItem lvi = listView2.Items.Add(aline1);
                    grammar += aline + "\r\n";
                    Production p = new Production();
                    List<String> temp = new List<string>();
                    temp = aline.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    p.Left = temp[0];
                    p.Right = temp[1];
                    lp.Add(p);
                    int lenth = temp[1].Length;


                    for (int i = 0; i <= lenth; i++)
                    {
                        ClosureItem clo = new ClosureItem();
                        clo.Left = temp[0];
                        clo.Right = temp[1];
                        clo.Dot = i;
                        clo.Fromitem = row - 4;
                        ClosureI.Add(clo);
                    }
      
                }
                row++;
            }

            foreach (ClosureItem c in ClosureI)
            {
                Console.WriteLine(c.getStrClo());
            }
        }
        //获取某个项目集闭包
        public List<ClosureItem> getClosure(ClosureItem clo)
        {
            List<ClosureItem> lco = new List<ClosureItem>();
            lco.Add(clo);
            int count = 0;
            while (count != lco.Count)
            {
                count = lco.Count;
                for (int i = 0; i < count; i++)
                {
                    ClosureItem c1 = lco[i];
                    String right = c1.Right;
                    String a = "";
                    int dot = c1.Dot;
                    if (dot != c1.Right.Length)
                        a = right[dot].ToString();
                    foreach (ClosureItem key in ClosureI)
                    {
                        if (key.Left.Equals(a) && key.Dot == 0)
                        {
                            lco.Add(key);
                        }
                    }
                }
                lco = lco.Distinct().ToList(); //清除List中的相同项，只保留一个
            }
            return lco;
        }
        public Dictionary<string, List<ClosureItem>> getKind(Closure clo)
        {
            List<Closure> ln = new List<Closure>();
            Dictionary<string, List<ClosureItem>> d = new Dictionary<string, List<ClosureItem>>();
            List<string> l = new List<string>();
            List<ClosureItem> cc = clo.Clo;
            foreach (ClosureItem key in cc)
            {
                string right = key.Right;
                if (key.Dot != right.Length)
                {
                    string a = right[key.Dot].ToString();
                    l.Add(a);
                }
            }
            l = l.Distinct().ToList();
            foreach (String key in l)
            {
                List<ClosureItem> cc1 = new List<ClosureItem>();
                foreach (ClosureItem item in cc)
                {
                    string right = item.Right;
                    if (item.Dot != right.Length)
                    {
                        if (key.Equals(right[item.Dot].ToString()))
                        {
                            cc1.Add(item);
                        }
                    }
                }
                d.Add(key, cc1);
            }
            return d;
        }
        public void getAllClosure(ClosureItem firstclo)
        {
            Closure clo = new Closure();
            List<ClosureItem> lc = new List<ClosureItem>();
            lc = getClosure(firstclo);
            clo.Clo = lc;
            clo.Item = 0;
            ClosureP.Add(clo);
            int countp = 0;
            int count = -1;
            foreach (Closure cc in ClosureP)
            {
                countp += cc.Dic.Count;
            }
            countp += ClosureP.Count;
            int item = 1;
            while (count != countp)
            {
                count = countp;
                for (int i = 0; i < ClosureP.Count; i++)
                {
                    Dictionary<string, List<ClosureItem>> begin = getKind(ClosureP[i]);
                    List<String> f = begin.Keys.ToList();
                    int count1 = ClosureP[i].Clo.Count;
                    Dictionary<String, int> d = ClosureP[i].Dic;
                    for (int j = 0; j < begin.Count; j++)
                    {
                        Closure cc = new Closure();
                        List<ClosureItem> lc1 = new List<ClosureItem>();
                        String a = f[j];
                        lc1 = begin[a];
                        cc.Item = item;
                        ClosureItem c = lc1[0];
                        string right = c.Right;
                        int dot = c.Dot;
                        Dictionary<string, int> dd = new Dictionary<string, int>();

                        for (int m = 0; m < lc1.Count; m++)
                        {
                            ClosureItem ci = lc1[m];
                            ClosureItem tclo1 = new ClosureItem();
                            List<ClosureItem> lco = new List<ClosureItem>();
                            tclo1.Left = ci.Left;
                            tclo1.Right = ci.Right;
                            tclo1.Dot = ci.Dot + 1;
                            tclo1.Fromitem = ci.Fromitem;
                            Console.WriteLine("the closure:");
                            lco = getClosure(tclo1);
                            foreach (ClosureItem key in lco)
                            {
                                String c2 = key.getStrClo();
                                Console.WriteLine(c2);
                            }
                            cc.Clo.AddRange(lco);
                            lco = cc.Clo.Distinct().ToList();
                            cc.Clo = lco;
                        }
                        int n = 0;
                        //判断状态是否已经存在
                        for (; n < ClosureP.Count; n++)
                        {
                            Closure clo1 = ClosureP[n];
                            if (clo1.equals(cc))
                            {
                                if (!ClosureP[i].Dic.ContainsKey(right[dot].ToString()) && right[dot] != '$')
                                    ClosureP[i].Dic.Add(right[dot].ToString(), n);
                                break;
                            }
                        }
                        if (n == ClosureP.Count && !ClosureP[i].Dic.ContainsKey(right[dot].ToString()) && right[dot] != '$')
                        {
                            ClosureP[i].Dic.Add(right[dot].ToString(), item);
                            ClosureP.Add(cc);
                            item++;
                        }
                    }
                }
                countp = 0;
                foreach (Closure cc in ClosureP)
                {
                    countp += cc.Dic.Count;
                }
                countp += ClosureP.Count;

            }
            String str = "";
            foreach (Closure cl in ClosureP)
            {
                str += cl.getStrCLosure();
            }

                
        }
        public void getLRTable()
        {
            List<string> vn1 = VN;
            vn1.Remove(beginC);
            for (int j = 0; j < ClosureP.Count; j++) //分析表初始化全部为error
            {
                Dictionary<string, string> s = new Dictionary<string, string>();
                Table.Add(s);
                foreach (string key in VT)
                {
                    Table[j].Add(key, "er");
                }
                foreach (string key in vn1)
                {
                    Table[j].Add(key, "er");
                }
            }
            int i = 0;
            foreach (Closure c in ClosureP)
            {
                Dictionary<string, int> d1 = c.Dic;
                if (d1.Count == 0)
                {
                    if (c.Clo[0].Fromitem == 0) //是第一条产生式
                    {
                        Table[i]["#"] = "acc";
                    }
                    else
                    {
                        string r = "r";
                        r += c.Clo[0].Fromitem;
                        foreach (string key in VT)
                        {
                            Table[i][key] = r;
                        }
                    }
                }
                else
                {
                    foreach (ClosureItem clo in c.Clo)
                    {
                        if (clo.Right.Equals("$"))
                        {
                            if (clo.Fromitem == 0) //是第一条产生式
                            {
                                Table[i]["#"] = "acc";
                            }
                            else
                            {
                                List<String> VT1 = new List<string>();
                                foreach(string key in d1.Keys)
                                {
                                    VT1.Add(key);
                                }
                                String left = clo.Left;
                                string r = "r";
                                r += clo.Fromitem;
                                foreach (string key in VT)
                                {
                                    if (!VT1.Contains(key))
                                     Table[i][key] = r;
                                }
                            }
                        }
                    }
                    foreach (string key in d1.Keys)
                    {
                        if (VN.Contains(key))
                        {
                            Table[i][key] = d1[key].ToString();
                        }
                        else if (VT.Contains(key))
                        {
                            string s = "s";
                            s += d1[key].ToString();
                            Table[i][key] = s;
                        }
                        else
                        {
                            MessageBox.Show("未知错误");
                        }
                    }
                }
                i++;
            }

        }

        public void clear()
        {
            lp = new List<Production>();
            d_first = new Dictionary<string, List<string>>();
            d_follow = new Dictionary<string, List<string>>();
            Table = new List<Dictionary<string, string>>();//表
            ClosureI = new List<ClosureItem>();
            ClosureP = new List<Closure>();
            VN = new List<string>();
            VT = new List<string>();
            beginC = "";
            grammar = "";
            strVT = "";
            strVN = "";
        }
        public void PrintTable()
        {

            listView1.Clear();
            List<string> vn1 = VN;
            vn1.Remove(beginC);
            ColumnHeader ch = new ColumnHeader();
            ch.Text = "状态";   //设置列标题  
            ch.Width = 45;    //设置列宽度  
            ch.TextAlign = HorizontalAlignment.Center;   //设置列的对齐方式  

            listView1.Columns.Add(ch);    //将列头添加到ListView控件。
            foreach (string key in VT)
            {
                ColumnHeader ch1 = new ColumnHeader();
                ch1.Text = convert[key[0].ToString()];   //设置列标题  
                ch1.Width = 70;    //设置列宽度  
                ch1.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式                
                listView1.Columns.Add(ch1);    //将列头添加到ListView控件。
            }
            foreach (string key in vn1)
            {
                ColumnHeader ch1 = new ColumnHeader();
                ch1.Text = convert[key[0].ToString()];   //设置列标题  
                ch1.Width = 100;    //设置列宽度  
                ch1.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式                
                listView1.Columns.Add(ch1);    //将列头添加到ListView控件。
            }
            for (int j = 0; j < Table.Count; j++) //遍历输出预测分析表
            {
                ListViewItem lvi = listView1.Items.Add(j.ToString());
                foreach (string key in VT)
                {
                    lvi.SubItems.Add(Table[j][key]);
                }
                foreach (string key in vn1)
                {
                    lvi.SubItems.Add(Table[j][key]);
                }
            }
        }



        public void setTextBox3(String msg)
        {
            listView6.Clear();
            ColumnHeader ch1 = new ColumnHeader();
            ch1.Text = "Addr";   //设置列标题  
            ch1.Width = 100;    //设置列宽度  
            ch1.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式                
            listView6.Columns.Add(ch1);    //将列头添加到ListView控件。
            ColumnHeader ch2 = new ColumnHeader();
            ch2.Text = "Pcode";
            ch2.Width = 100;    //设置列宽度  
            ch2.TextAlign = HorizontalAlignment.Left;
            listView6.Columns.Add(ch2);
            ColumnHeader ch3 = new ColumnHeader();
            ch3.Text = "L";
            ch3.Width = 100;    //设置列宽度  
            ch3.TextAlign = HorizontalAlignment.Left;
            listView6.Columns.Add(ch3);
            ColumnHeader ch4 = new ColumnHeader();
            ch4.Text = "A";
            ch4.Width = 100;    //设置列宽度  
            ch4.TextAlign = HorizontalAlignment.Left;
            listView6.Columns.Add(ch4);
            int i = 0;
            int count = 0;
            String str = "";

            ListViewItem lvi = new ListViewItem();
            while (msg[i] != '#')
            {
                str += msg[i];
                if (msg[i] == '$')
                {
                    int l = str.Length;
                    String str1 = str.Remove(l - 1);
                    if (count%4 == 0)
                    {
                        lvi = listView6.Items.Add(str1);
                    }
                    else
                    {
                        lvi.SubItems.Add(str1);
                    }
                    count++;
                    str = "";
                }                
                i++;
            }
            //textBox1.Text = msg;

        }

        Parse parse = new Parse();

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            List<String> LineList = new List<String>();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;
                StreamReader sr = File.OpenText(file);
                String sLine = "";
                String strFile = "";

                while (sLine != null)
                {
                    sLine = sr.ReadLine();
                    if (sLine != null && !sLine.Equals(""))
                    {
                        LineList.Add(sLine);
                        strFile += sLine;
                        strFile += "\r\n";
                    }
                }
                textBox5.Text = strFile;
            }
        }

        private void printChifa(String msg)
        {
            listView3.Clear();
            ColumnHeader ch1 = new ColumnHeader();
            ch1.Text = "Id";   //设置列标题  
            ch1.Width = 130;    //设置列宽度  
            ch1.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式                
            listView3.Columns.Add(ch1);    //将列头添加到ListView控件。
            ColumnHeader ch2 = new ColumnHeader();
            ch2.Text = "Value";
            ch2.Width = 130;    //设置列宽度  
            ch2.TextAlign = HorizontalAlignment.Left;
            listView3.Columns.Add(ch2);
            ColumnHeader ch3 = new ColumnHeader();
            ch3.Text = "Type";
            ch3.Width = 130;    //设置列宽度  
            ch3.TextAlign = HorizontalAlignment.Left;
            listView3.Columns.Add(ch3);

            int i = 0;
            int count = 0;
            String str = "";

            ListViewItem lvi = new ListViewItem();
            while (msg[i] != '#')
            {
                str += msg[i];
                if (msg[i] == '$')
                {
                    int l = str.Length;
                    String str1 = str.Remove(l - 1);
                    if (count % 3 == 0)
                    {
                        lvi = listView3.Items.Add(str1);
                    }
                    else
                    {
                        lvi.SubItems.Add(str1);
                    }
                    count++;
                    str = "";
                }
                i++;
            }
        }

        public void printSymTable()
        {

            ColumnHeader ch1 = new ColumnHeader();
            ch1.Text = "Name";   //设置列标题  
            ch1.Width = 70;    //设置列宽度  
            ch1.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式                
     //将列头添加到ListView控件。

            ColumnHeader ch2 = new ColumnHeader();
            ch2.Text = "Type";
            ch2.Width = 50;    //设置列宽度  
            ch2.TextAlign = HorizontalAlignment.Left;


            ColumnHeader ch3 = new ColumnHeader();
            ch3.Text = "Value";
            ch3.Width = 50;    //设置列宽度  
            ch3.TextAlign = HorizontalAlignment.Left;

            ColumnHeader ch4 = new ColumnHeader();
            ch4.Text = "Lev";
            ch4.Width = 50;    //设置列宽度  
            ch4.TextAlign = HorizontalAlignment.Left;


            ColumnHeader ch5 = new ColumnHeader();
            ch5.Text = "Addr";
            ch5.Width = 50;    //设置列宽度  
            ch5.TextAlign = HorizontalAlignment.Left;


            ColumnHeader ch6 = new ColumnHeader();
            ch6.Text = "Size";
            ch5.Width = 50;    //设置列宽度  
            ch6.TextAlign = HorizontalAlignment.Left;
            listView4.Clear();
            listView4.Columns.Add(ch1);
            listView4.Columns.Add(ch2);
            listView4.Columns.Add(ch3);
            listView4.Columns.Add(ch4);
            listView4.Columns.Add(ch5);
            listView4.Columns.Add(ch6);

            ListViewItem lvi = new ListViewItem();
            for (int it = 1; it < parse.symTable.tablePtr + 1; it++)
            {
                lvi = listView4.Items.Add(parse.symTable.table[it].name);
                lvi.SubItems.Add((parse.symTable.table[it].type).ToString());
                lvi.SubItems.Add((parse.symTable.table[it].value).ToString());
                lvi.SubItems.Add((parse.symTable.table[it].lev).ToString());
                lvi.SubItems.Add((parse.symTable.table[it].addr).ToString());
                lvi.SubItems.Add((parse.symTable.table[it].size).ToString());
            }
        }
        private void compileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "";
            MyFile my = new MyFile();
            my.delete("E:\\test.txt");
            parse.clear();
            if (textBox5.Text != null)
            {

                Scanner scanner = new Scanner(textBox5.Text);
                String strCifa = "";
                Symbol tmp = scanner.getsym();
                while (tmp.symtype != 0 && tmp.id != ".")
                {

                    //str = ((char)tmp.symtype).ToString();
                    //str + = ((char)tmp.symtype).ToString();

                    parse.AnasisCh(tmp, this, false);
                    strCifa +=  tmp.id + "$" + tmp.value + "$"  + tmp.symtype + "$";
                    tmp = scanner.getsym();
                }
                int res =  parse.AnasisCh(null, this, true);
                if (res == 1)
                {
                    success = 1;
                    textBox1.Text = "编译成功!" + "\r\n";
                }
                else
                {
                    success = 0;
                    textBox1.Text = "**************" + "编译失败，请检查程序后再编译" + "**************" + "\r\n" + parse.getErrMsg(); ;                 
                }
                strCifa += "#";
                printChifa(strCifa);
                printSymTable();
            }
            else
            {
                textBox5.Text = "输入信息为空";
            }
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try{
                if (success == 1 && parse.interp.interpret(textBox4.Text) != "")
                    textBox1.Text += "运行结果：" + parse.interp.interpret(textBox4.Text) + "\r\n";
                else if (success == 0)
                {
                    textBox1.Text = "**************" + "运行失败，请编译后再运行" + "**************";
                }
            }
            catch
            {
                textBox1.Text = "**************" + "运行失败，请编译后再运行" + "**************";
            }
           
        }


        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
        private void label5_Click_1(object sender, EventArgs e)
        {

        }
    }
}

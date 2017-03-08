using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR0
{
    public class Symbol
    {
        public enum TYPE:int
        {
            nul=0,
            ID = 1, ADD, SUB, MUL, DIV,
            EQUAL, N_EQUAL, LESS, BIG_E, BIG, 
            LESS_E, LEFT_P, RIGHT_P, DOU, FEN, //15
            DOT, FUZHI, INTEGER,   // 18
            //申明
            PROC, PROG, BLOCK, CONDECL, VARDECL,//23
            BODY, CONST,  STATEMENT, EXP,//27
            LEXP, LOP, TERM, AOP, FACTOR,//32
            MOP,   //33   
           
           
            // L字母  D数字  ，个数35
            
            //辅助字
            PCONDECL,jinhao, NULL = '#' + 1, PVARDECL, PM, PPROC,//39
            PBODY, PSTATEMENT, PSTATEMENT1, PTERM, PEXP = ',' + 2, //45
            PSTATEMENT_EXP, Pmyelse,  //个数 47
            // 关键字
            myconst, program, var, procedure, begin,//52
            end, myif, then, myelse, mydo,
            mywhile, call, read, write, repeat,
            unti, //个数 63
            //PID, PINTEGER
            //奇怪的
            odd, //判断奇数偶数  ，个数 65
        };
        //符号码的个数
        public static int symnum = 35;
        public int lineCount = -1;
        //设置操作符名字，
        public static String[] opt = new String[]{
        "null","id","+" , "-" , "*" , "/" , //5
        "=", "<>","<", ">=", ">", //10
        "<=", "(", ")",",", ";", //15
         ".", ":=" ,"number"//17
         };
        //设置保留字名字，按照字母顺序，便于折半查找
        public static String[] word = new String[]{
        "begin","call" , "const"    , "do" ,
        "else"  ,"end" ,"if"   , "odd",  
        "procedure","program", "read","repeat",
        "then", "until" , "var", "while"    , 
        "write" };
        //保留字对应的符号值
        public static int[] wsym = new int[]{
        (int)TYPE.begin, (int)TYPE.call, (int)TYPE.myconst, (int)TYPE.mydo,
        (int)TYPE.myelse, (int)TYPE.end, (int)TYPE.myif,(int)TYPE.odd,
        (int)TYPE.procedure,(int)TYPE.program, (int)TYPE.read,(int)TYPE.repeat,
        (int)TYPE.then,(int)TYPE.unti,(int)TYPE.var,(int)TYPE.mywhile,
        (int)TYPE.write};

        //符号码
        public int symtype;
        //标志符号名字；
        public String id;
        //数值的大小
        public int value;

        /**
         * 构造具有特定符号码的符号
         *
         * @param stype
         */
        public Symbol(int stype)
        {
            symtype = stype;
            if(stype==(int)TYPE.nul)
                id = "";
            else if (stype < (int)TYPE.PROC && stype > (int)TYPE.nul)
            {
                id = opt[stype];
            }
            value = 0;
        }
    }
}

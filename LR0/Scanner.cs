using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
//using System.Text.StringBuilder;

namespace LR0
{
    class Scanner
    {
        public int lineCnt = 0;
        private char curCh = ' ';
        private String line;
        public int lineLength = 0;
        public int chCount = 0;
        private int[] ssym;
        StringReader In;

        public Scanner(String text)
        {
            //StreamReader In;
            
           
            In = new StringReader(text);
           
            //设置单字符
            ssym = new int[256];
            
            ssym['+'] = (int)Symbol.TYPE.ADD;
            ssym['-'] = (int)Symbol.TYPE.SUB;
            ssym['*'] = (int)Symbol.TYPE.MUL;
            ssym['/'] = (int)Symbol.TYPE.DIV;
            ssym['('] = (int)Symbol.TYPE.LEFT_P;
            ssym[')'] = (int)Symbol.TYPE.RIGHT_P;
            ssym['='] = (int)Symbol.TYPE.EQUAL;
            ssym[','] = (int)Symbol.TYPE.DOU;
            ssym['.'] = (int)Symbol.TYPE.DOT;
            ssym[';'] = (int)Symbol.TYPE.FEN;
           
        }
     

        //读取一个字符，为减少磁盘I/O次数，每次读取一行
        void getch()
        {
            if (chCount == lineLength)
            {
                try
                {//如果读到行末尾，就重新读入一行
                    String tmp = "";
                    while (tmp.Equals(""))
                    {
                        tmp = In.ReadLine();
                        if (tmp == null) //读到文件末尾
                        {
                            curCh = '\0';
                            return;
                        }
                        tmp =tmp.Trim() + ' ';               //除去空行
                    }
                    line = tmp;
                    lineCnt++;
                }
                catch (Exception e)
                {
                    
                    
                    Console.WriteLine("***reading character meet with error!***"+e.StackTrace);
                }
                lineLength = line.Length;
                chCount = 0;
                Console.WriteLine(line);
            }
            curCh = line[chCount++];
        }
        //词法分析，获取一个词法分析符号，是词法分析器的重点
        public Symbol getsym()
        {
            Symbol sym;
            while (curCh == ' ') //去掉空格
            {
                getch();
            }
            if(curCh == 0)
            {
                sym = new Symbol(0);
            }
            else if ((curCh >= 'a' && curCh <= 'z') || (curCh >= 'A' && curCh <= 'Z'))
            {
                sym = matchKeywordOrIdentifier();                                     //关键字或者一般标识符
            }
            else if (curCh >= '0' && curCh <= '9')
            {
                sym = matchNumber();                                                       //数字
            }
            else
            {
                sym = matchOperator();                                                     //操作符
            }
            sym.lineCount = lineCnt;
            return sym;
        }

        private Symbol matchKeywordOrIdentifier()
        {
            StringBuilder sb = new StringBuilder();
            do
            {
                sb.Append(curCh);
                getch();
            } while ((curCh >= 'a' && curCh <= 'z') || (curCh >= 'A' && curCh <= 'Z') || (curCh >= '0' && curCh <= '9'));

            String token = sb.ToString();
            int index = Array.BinarySearch(Symbol.word, token);                           //搜索是不是保留字
            Symbol sym = null;
            if (index < 0)
            {
                sym = new Symbol((int)Symbol.TYPE.ID);                                            //一般标识符
                sym.id = token;
            }
            else
            {
                sym = new Symbol(Symbol.wsym[index]);                                    //保留字对应的符号值0-31
                sym.id = token;
            }
            return sym;
        }

        private Symbol matchNumber()
        {
            //统计数字位数
            Symbol sym = new Symbol((int)Symbol.TYPE.INTEGER);
            do
            {
                sym.value = 10 * sym.value + curCh - '0';                                    // 获取数字的值
                getch();
            } while (curCh >= '0' && curCh <= '9');                                    //!!!
            
            return sym;
        }

        private Symbol matchOperator()
        {
            Symbol sym = null;
            switch (curCh)
            {
                case ':':                                                                       // 赋值符号
                    getch();
                    if (curCh == '=')
                    {
                        sym = new Symbol((int)Symbol.TYPE.FUZHI);
                        getch();
                    }
                    else
                    {
                        sym = new Symbol((int)Symbol.TYPE.nul);                               //不能识别的符号
                    }
                    break;
                case '<':                                                                    //小于或者小于等于
                    getch();
                    if (curCh == '=')
                    {
                        sym = new Symbol((int)Symbol.TYPE.LESS_E);                             //是<=
                        getch();
                    }
                    else if (curCh == '>')
                    {
                        sym = new Symbol((int)Symbol.TYPE.N_EQUAL);                           //是<>
                        getch();
                    }
                    else
                    {
                        sym = new Symbol((int)Symbol.TYPE.LESS);                             //是<
                    }
                    break;
                case '>':                                                      //大于或者大于等于
                    getch();
                    if (curCh == '=')
                    {
                        sym = new Symbol((int)Symbol.TYPE.BIG_E);                           //大于等于
                        getch();
                    }
                    else
                    {
                        sym = new Symbol((int)Symbol.TYPE.BIG);                            //大于
                    }
                    break;
                default:
                    sym = new Symbol(ssym[curCh]);
                    if (sym.symtype != (int)Symbol.TYPE.DOT)
                    {
                        getch();
                    }
                    break;
            }
            return sym;
        }
    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace LR0
{
    public class Pcode
    {
        public Pcode(int f, int l, int a)
        {
            this.f = f;
            this.l = l;
            this.a = a;
        }
        public const  int LIT = 0;
        public const  int OPR = 1;
        public const  int LOD = 2;
        public const  int STO = 3;
        public const  int CAL = 4;
        public const  int INT = 5;
        public const  int JMP = 6;
        public const  int JPC = 7;
        public static int convert (String str) {
            switch (str)
            {
                case "LIT":
                    return 0;
                case "OPR":
                    return 1;
                case "LOD":
                    return 2;
                case "STO":
                    return 3;
                case "CAL":
                    return 4;
                case "INT":
                    return 5;
                case "JMP":
                    return 6;
                case "JPC":
                    return 7;
            }
            return -1;
        }
        //各符号的名字
        public static String[] pcode = new String[] { "LIT", "OPR", "LOD", "STO", "CAL", "INT", "JMP", "JPC" };
        //虚拟机代码指令
        public int f;
        //引用层与声明层的层次差
        public int l;
        //指令参数
        public int a;
    }
    class Interpreter
    {
        //运行栈上限
        private static int stackSize = 1000;
        //pcode数组上线
        private static int arraySize = 500;
        //虚拟机代码指针，取值范围[0,arraySize-1]
        public int arrayPtr = 0;
        //存放虚拟机代码的数组
        public Pcode[] pcodeArray;
        //显示虚拟代码与否
        public static Boolean listswitch = true;
        public Interpreter()
        {
            pcodeArray = new Pcode[arraySize];
        }
        /**
        * 生成虚拟机代码
        *
        * @param x Pcodeuction.f
        * @param y Pcodeuction.l
        * @param z Pcodeuction.a
        */
        public void gen(int f, int l, int a)
        {
            if (arrayPtr >= arraySize)
            {                                                                          //超出堆栈的上限
                throw new Exception("***ERROR:Program too long***");
            }
            pcodeArray[arrayPtr++] = new Pcode(f, l, a);

        }
        public Interpreter(String code)
        {
            pcodeArray = new Pcode[arraySize];
            List<String> tmp = new List<string>();
            String[] list = code.Split(new char[] { '\r','\n' });
            for(int i = 0; i < list.Length; i++)
            {
                if (list[i].Trim() != "")
                {
                    tmp.Add(list[i]);
                    String[] goal = list[i].Split(' ');
                    int f = Pcode.convert(goal[1]);
                    int l = Convert.ToInt32(goal[2]);
                    int a = Convert.ToInt32(goal[3]);
                    if (f == -1)
                    {
                        Console.WriteLine("不存在此操作码");
                        return;
                    }
                    pcodeArray[Convert.ToInt32(goal[0])] = new Pcode(f,l,a);
                    //Console.WriteLine("f:"+f+" l:"+l+" a:"+a );
                }
                
            }
        }
       public String printCode()
        {
            String res = "";

            for(int i=0;i< arrayPtr; i++)
            {
                res += i + "$" + Pcode.pcode[pcodeArray[i].f] + "$" + pcodeArray[i].l + "$" + pcodeArray[i].a + "$";
            }
            return res + "#";
        }
        public  String interpret(String str)
        {
            String stdout = "";
            StringReader stdin = null;
            if(str!=null)
                stdin = new StringReader(str);
            int[] runtimeStack = new int[stackSize];
            Console.WriteLine("**Start Interpret the P_code**");
            int pc = 0, // pc:指令指针，
                bp = 0, //bp:指令基址，
                sp = 0;//sp:栈顶指针
            do
            {

                Pcode index = pcodeArray[pc++];// index :存放当前指令, 读当前指令
                //Console.WriteLine(pc + "  " + Pcode.pcode[index.f] + " " + index.l + " " + index.a);
                //Console.WriteLine(Pcode.pcode[index.f]);
                switch (index.f)
                {
                    case Pcode.LIT:                                                   // 将a的值取到栈顶
                        runtimeStack[sp++] = index.a;
                        break;
                    case Pcode.OPR:                                                   // 数学、逻辑运算
                        switch (index.a)
                        {
                            case 0:                                                          //OPR 0 0;RETURN 返回
                                sp = bp;
                                pc = runtimeStack[sp + 2];
                                bp = runtimeStack[sp + 1];
                                break;
                            case 1:                                                           //OPR 0 1 ;NEG取反
                                runtimeStack[sp - 1] = -runtimeStack[sp - 1];
                                break;
                            case 2:                                                           //OPR 0 2;ADD加法
                                sp--;
                                runtimeStack[sp - 1] += runtimeStack[sp];
                                
                                break;
                            case 3:                                                             //OPR 0 3;SUB减法
                                sp--;
                                runtimeStack[sp - 1] -= runtimeStack[sp];
                                
                                break;
                            case 4:                                                             //OPR 0 4;MUL乘法
                                sp--;
                                runtimeStack[sp - 1] = runtimeStack[sp - 1] * runtimeStack[sp];
                                
                                break;
                            case 5:                                                             //OPR 0 5;DIV除法
                                sp--;
                                runtimeStack[sp - 1] /= runtimeStack[sp];
                                
                                break;
                            case 6:                                                              //OPR 0 6;ODD对2取模mod 2
                                runtimeStack[sp - 1] %= 2;
                                break;
                            case 7:                                                              //OPR 0 7;MOD取模
                                sp--;
                                runtimeStack[sp - 1] %= runtimeStack[sp];
                                
                                break;
                            case 8:                                                             //OPR 0 8;==判断相等
                                sp--;
                                runtimeStack[sp - 1] = (runtimeStack[sp - 1] == runtimeStack[sp ] ? 1 : 0);
                                break;
                            case 9:                                                                //OPR 0 9;!=判断不相等
                                sp--;
                                runtimeStack[sp - 1] = (runtimeStack[sp - 1] != runtimeStack[sp] ? 1 : 0);
                                break;
                            case 10:                                                               //OPR 0 10;<判断小于
                                sp--;
                                runtimeStack[sp - 1] = (runtimeStack[sp - 1] < runtimeStack[sp] ? 1 : 0);
                                break;
                            case 11:                                                                //OPR 0 11;>=判断大于等于
                                sp--;
                                runtimeStack[sp - 1] = (runtimeStack[sp - 1] >= runtimeStack[sp] ? 1 : 0);
                                break;
                            case 12:                                                                //OPG 0 12;>判断大于
                                sp--;
                                runtimeStack[sp - 1] = (runtimeStack[sp-1] > runtimeStack[sp ] ? 1 : 0);
                                break;
                            case 13:                                                                 //OPG 0 13;<=判断小于等于
                                sp--;
                                runtimeStack[sp - 1] = (runtimeStack[sp - 1] <= runtimeStack[sp ] ? 1 : 0);
                                
                                break;
                            case 14:                                                                 //OPG 0 14;输出栈顶值
                                Console.WriteLine("runtimeStack[sp - 1]" + runtimeStack[sp - 1] + ' ');
                                try
                                {
                                    stdout += " " + runtimeStack[sp - 1] + ' ';
                                    //stdout.Flush();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("***case 14 meet with error***");
                                }
                                sp--;
                                break;
                            case 15:                                                                 //OPG 0 15;输出换行
                                Console.WriteLine("\n");
                                try
                                {
                                    stdout += ("\n");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("***case 15 meet with error***");
                                }
                                break;
                            case 16:                                                                 //OPG 0 16;读入一行输入，置入栈顶
                                Console.WriteLine("Please Input a Integer : ");
                                runtimeStack[sp] = 0;
                                try
                                {
                                    runtimeStack[sp] = Int32.Parse(stdin.ReadLine().Trim());       //读入一个整型数字
                                    Console.WriteLine(runtimeStack[sp]);
                                    sp++;
                                }
                                catch (Exception e)
                                {
                                    //e.printStackTrace();
                                    Console.WriteLine("***read data meet with error***");
                                }
                                
                                break;
                        }
                        break;
                    case Pcode.LOD:                                          //取相对当前过程的数据基地址为a的内存的值到栈顶
                        runtimeStack[sp] = runtimeStack[Base(index.l, bp) + index.a];
                        sp++;
                        break;
                    case Pcode.STO:                                         //栈顶的值存到相对当前的过程的数据基地址为a的内存
                        sp--;
                        runtimeStack[Base(index.l,  bp) + index.a] = runtimeStack[sp];
                        break;
                    case Pcode.CAL:                                                                 //调用子程序
                        runtimeStack[sp] = Base(index.l,  bp);            //将静态作用域基地址入栈
                        runtimeStack[sp + 1] = bp;                                                //将动态作用域基地址
                        runtimeStack[sp + 2] = pc;                                               //将当前指针入栈
                        bp = sp;                                                                        //改变基地址指针值为新过程的基地址
                        pc = index.a;                                                                 //跳转至地址a
                        break;
                    case Pcode.INT:                                                               //开辟空间大小为a
                        sp += index.a;
                        break;
                    case Pcode.JMP:                                                               //直接跳转至a
                        pc = index.a;
                        break;
                    case Pcode.JPC:
                        sp--;
                        if (runtimeStack[sp] == 0) //条件跳转至a(当栈顶指针为0时)
                        {
                            pc = index.a;
                        }
                        break;
                }
            } while (pc != 0);
           
            return stdout;
        }
        /**
        * 通过给定的层次差来获得该层的堆栈帧基址
        *
        * @param l 目标层次与当前层次的层次差
        * @param runtimeStack 运行栈
         * @param b 当前层堆栈帧基地址
        * @return 目标层次的堆栈帧基地址
        */
        public int[] runtimeStack = new int[stackSize];
        public int Base(int l,  int b) {
            while (l > 0) {                                                       //向上找l层
                b = runtimeStack[b];
                l--;
            }
            return b;
        }
    }
}

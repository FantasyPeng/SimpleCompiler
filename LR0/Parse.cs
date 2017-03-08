using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR0
{
    class Parse 
    {
        public class Node
        {
            Symbol sym=null;
            int level=0;
            public String op;
            public int type;
            public int count=0;
            public bool flag = false; //节点是否有操作

            public Symbol Sym
            {
                get
                {
                    return sym;
                }

                set
                {
                    sym = value;
                }
            }

            public int Level
            {
                get
                {
                    return level;
                }

                set
                {
                    level = value;
                }
            }

            public Node(String op)
            {//无意义的符号 “#”  赋值标识等
                this.op = op;
            }
            public Node(Symbol sym)
            {
                this.Sym = sym;
                this.op = ((char)sym.symtype).ToString();
            }
            public String getMsg()
            {
                if (this.sym != null)
                    return "op: " + Convert.ToInt32(this.op[0]) + " id: " + this.sym.id + "  value: " + this.sym.value;
                else if (this.op != "#")
                {
                    return "op: " + Convert.ToInt32(this.op[0]) + " id: " + Form1.convert[this.op];
                }
                else
                {
                    return "op: " + this.op;
                }

            }
        }
        public List<Node> nodes; //存储所有的node, 最后应该会只剩下一个
        public SymbolTable symTable;
        Stack<int> var_addr;

        Stack<string> sts ;
        Err myerr;
        Stack<Node> ops;
        String AnasStr ;
        int i = 0;
        public Parse()
        {
           
        }
        public void clear()
        {//对不同的词法进行分析
            symTable = new SymbolTable();
            nodes = new List<Node>();
            sts = new Stack<string>();
            ops = new Stack<Node>();
            myerr = new Err();
            var_addr = new Stack<int>();
            sts.Push("0");
            ops.Push(new LR0.Parse.Node("#"));
            AnasStr = "";
            i = 0;
            dx = 3;//DL静态链
            //DL动态链
            //主程序返回
            interp = new Interpreter();
            //生成分配内存代码，program 的内存分配，具体大小需要后面来填
            interp.gen(Pcode.INT, 0, 0);
            //interp.gen(Pcode.LIT, 0, 0);
            //interp.gen(Pcode.LIT, 0, 0);
            //interp.gen(Pcode.LIT, 0, 0); 
        }
        public String getErrMsg()
        {
            return myerr.getReport();
        }
        public int AnasisCh(Symbol sym,Form1 form1,bool last)
        {
            String str; 
            int res = -1;
            String str2;
            if (last) //sym == null
            {
                str2 = AnasStr + "#";
                str = "#";
                nodes.Add(new Node("#"));
            }
            else
            {
                if(sym.symtype == (int)Symbol.TYPE.INTEGER)//常量
                {
                    symTable.enter(sym,0,0,0);
                }
                str = ((char)sym.symtype).ToString();
                nodes.Add(new Node(sym));

                str2 = AnasStr + str + ",";
            }
            AnasStr = AnasStr + str;
            
            string right = str2;
            
            Boolean flag = true;
            
           
            while (flag)
            {
                
                string current = str2[i].ToString();
                if (current == ",")
                {
                    break;
                }
                String topSts = sts.Peek();
                int s = int.Parse(topSts);
                String action = "";
                try
                {
                    action = Form1.Table[s][current];
                }
                catch
                {
                    Console.WriteLine("无法识别该字符串0");
                    //MessageBox.Show("无法识别该字符串0");
                    res = 0;
                    break;
                }
                if (action.Equals("er"))
                {
                    Console.WriteLine("无法识别该字符串1");
                    res = 0;
                    break;
                }
                else if (action.Equals("acc"))
                {
                    res = 1;
                    interp.pcodeArray[0].a = dx;//过程入口地址填写在pcodeArray中的int 的第二个参数
                    interp.gen(Pcode.OPR, 0, 0);
                    break;
                }
                else if (action[0].ToString().Equals("s"))
                {
                    Node current_node = nodes[i];
                    String nexts = action.Remove(0, 1);
                    sts.Push(nexts);
                    ops.Push(current_node);
                    switch (current_node.Sym.symtype)
                    {
                        case (char)Symbol.TYPE.myelse://回填if判断否定跳到 else的位置
                            elseEnd_pos = interp.arrayPtr;
                            interp.gen(Pcode.JMP, 0, 0);
                            interp.pcodeArray[else_pos].a = interp.arrayPtr;
                            break;
                        case (char)Symbol.TYPE.read://回填if判断否定跳到 else的位置
                            read_flag = 1;
                            break;
                        case (char)Symbol.TYPE.write:// write的参数，其中每个exp都要输出
                            write_flag = 1;
                            break;
                        case (char)Symbol.TYPE.mywhile:// write的参数，其中每个exp都要输出
                            lexp_begin = interp.arrayPtr;
                            break;
                        case (char)Symbol.TYPE.mydo:// write的参数，其中每个exp都要输出
                            while_pos = interp.arrayPtr;
                            interp.gen(Pcode.JPC, 0, 0);
                            break;
                        case (char)Symbol.TYPE.then:// write的参数，其中每个exp都要输出
                            else_pos = interp.arrayPtr;

                            interp.gen(Pcode.JPC, 0, 0);
                            break;
                    }
                    right = right.Remove(0, 1);
                    i++;                  
                }
                else if (action[0].ToString().Equals("r"))
                {
                    int k = int.Parse(action.Remove(0, 1).ToString());  //注意这里很可能错误
                    String left = Form1.lp[k].Left;
                    String right_1 = Form1.lp[k].Right;
                    int n = Form1.lp[k].Right.Length;
                    if (!right_1.Equals("$"))
                    {
                        List<Node> tmp = new List<Node>();
                        for (int count = 0; count < n; count++)
                        {
                            sts.Pop();
                            tmp.Add(ops.Peek());
                            ops.Pop();
                        }
                        Node left_node = Boss(left,tmp,k);
                        ops.Push(left_node);
                    }
                    else
                    {
                        Node left_node = new Node(left);
                        Boss(left, null, k);
                        ops.Push(left_node);
                    }
                    String top0 = ops.Peek().op;
                    String top = sts.Peek();
                    int t = int.Parse(top);
                    String new1 = Form1.Table[t][top0];
                    sts.Push(new1);
                }
                else
                {
                    //MessageBox.Show("无法识别该字符串2");
                    Console.WriteLine("无法识别该字符串2");
                    res = 0;
                    break;
                }
            }
            string strops = "";
            List<Node> bStack = ops.ToList();
            for (int si = bStack.Count - 1; si >= 0; si--)
                strops += bStack[si].getMsg() + " ";
            MyFile myf = new MyFile();
            myf.write1("E:\\test.txt", strops);
            if (last)
            {
                form1.setTextBox3(interp.printCode());
            }
            return  res==0 || (myerr.errCount>0) ? 0:1;

        }
        /**
         * 当前作用域的堆栈帧大小，或者说数据大小(data size)
         * 计算每个变量在运行栈中相对本过程基地址的偏移量，
         * 放在symbolTable中的address域，
         * 生成目标代码时再放在code中的a域
         */
        private int dx;
        public Interpreter interp;
        Node Boss(String op,List<Node> lst_node,int pro_id)
        {//如何进行规约！！同时属性计算
            Node ret = new Node(op);
            if(lst_node!=null)
                lst_node.Reverse();
            int index = -1;
            int num;
            switch (pro_id)
            {
                case 0://<prog>->program <id> ; <block>
                    
                    break;
                case 9://<condecl> -> const <const> Pcondecl
                    
                    break;
                case 12://<const>->< id >:=< integer >  ???
                    lst_node[0].Sym.value = lst_node[2].Sym.value;
                    symTable.enter(lst_node[0].Sym, SymbolTable.Item.constant, 0, 0);
                    break;
                case 13://<vardecl>->var <id> P<vardecl>
                    symTable.enter(lst_node[1].Sym, SymbolTable.Item.variable, 0, dx);
                    dx++;
                    break;
                
                case 16://<proc>->procedure <id> (Pm); <block> P<proc>
                    symTable.enter(lst_node[1].Sym, SymbolTable.Item.procedure, 0, 0);
                    break;
                case 20://<Pm>-><id> P<vardecl> 确定是var,并且是应当查表

                    break;
                case 24:// <statement>-><id>:=<exp> 调用 id，所以要先定义的，这里应当查表
                    index = symTable.position(lst_node[0].Sym.id);
                    if (index > 0)
                    {
                        SymbolTable.Item item = symTable.get(index);
                        //expression将执行一系列指令，
                        //但最终结果将会保存在栈顶，
                        //执行sto命令完成赋值  lev - item.lev
                        interp.gen(Pcode.STO,0 , item.addr);
                    }   
                    else
                    {
                        myerr.report(4, lst_node[0].Sym.lineCount);
                    }

                    break;
                case 29:// <statement>-><call> <id> P<statement>  调用 id，所以要先定义的，这里应当查表
                    
                    break;
                case 14://P<vardecl>->,<id> P<vardecl>
                    if (read_flag!=0)
                    {
                        index = symTable.position(lst_node[1].Sym.id);
                        if (index > 0)
                        {
                            SymbolTable.Item item = symTable.get(index);
                            switch (item.type)
                            {
                                //如果这个标识符对应的是常量，值为val，生成lit指令，把val放到栈顶
                                case SymbolTable.Item.constant:                        //名字为常量
                                    myerr.report(29, lst_node[1].Sym.lineCount);

                                    break;
                                case SymbolTable.Item.variable:                         //名字为变量
                                    var_addr.Push(item.addr);                                                    //interp.gen(Pcode.LOD, lev - item.lev, item.addr);
                                   

                                    break;
                                case SymbolTable.Item.procedure:                     //
                                                                                     //表达式内不可有过程标识符
                                    myerr.report(30, lst_node[1].Sym.lineCount);
                                    break;
                            }
                        }
                        else
                        {
                            myerr.report(35, lst_node[1].Sym.lineCount);
                        }
                    }
                    else
                    {
                        symTable.enter(lst_node[1].Sym, SymbolTable.Item.variable, 0, dx);
                        dx++;
                    }
                    
                    break;
                case 35://<statement>-><read> (<id> P<vardel>) 调用 id，所以要先定义的，这里应当查表
                    index = symTable.position(lst_node[2].Sym.id);
                    if (index > 0)
                    {
                        SymbolTable.Item item = symTable.get(index);
                        switch (item.type)
                        {
                            //如果这个标识符对应的是常量，值为val，生成lit指令，把val放到栈顶
                            case SymbolTable.Item.constant:                        //名字为常量
                                myerr.report(29, lst_node[2].Sym.lineCount);
                                
                                break;
                            case SymbolTable.Item.variable:                         //名字为变量
                                //interp.gen(Pcode.LOD, lev - item.lev, item.addr);
                                
                                interp.gen(Pcode.OPR, 0, 16);                            //OPR 0 16:读入一个数据
                                interp.gen(Pcode.STO, 0, item.addr);   //STO L A;存储变量
                                while (var_addr.Count>0)
                                {
                                    interp.gen(Pcode.OPR, 0, 16);                            //OPR 0 16:读入一个数据
                                    interp.gen(Pcode.STO, 0, var_addr.Peek());   //STO L A;存储变量
                                    var_addr.Pop();
                                }
                                break;
                            case SymbolTable.Item.procedure:                     //
                                  //表达式内不可有过程标识符
                                myerr.report(30, lst_node[2].Sym.lineCount);
                                break;
                        }
                        
                    }
                    else
                    {
                        myerr.report(35, lst_node[2].Sym.lineCount);
                    }
                    read_flag = 0;
                    break;
                case 52:// <exp>-> + <term> P<exp>
                    
                    if (write_flag == 1)
                    {
                        interp.gen(Pcode.OPR, 0, 14);                                     //OPR 0 14:输出栈顶的值
                    }
                    break;
                case 53:// <exp>-> - <term> P<exp>
                    interp.gen(Pcode.OPR, 0, 1); //取反
                    if (write_flag == 1)
                    {
                        interp.gen(Pcode.OPR, 0, 14);                                     //OPR 0 14:输出栈顶的值
                    }
                    break;
                case 54://<exp>-><term> P<exp>  直接取得数字
                    if (write_flag == 1)
                    {
                        interp.gen(Pcode.OPR, 0, 14);                                     //OPR 0 14:输出栈顶的值
                    }
                    break;
               
                case 36: // <mop> -> *
                    mul_div = 4;
                    break;
                case 37: // <mop> -> /
                    mul_div = 5;
                    break;
                case 38://<aop> -> +
                    add_sub = 2;
                    break;
                case 39://<aop> -> -
                    add_sub = 3;
                    break;
                case 56://<exp> -> <aop> <term> P<exp>
                    //加法:OPR 0 2,减法:OPR 0 3
                    interp.gen(Pcode.OPR, 0, add_sub);
                    break;
                case 49: //<term> - > <factor> P<term>
                    
                    break;
                case 50: //P<term> - > <mop> <factor> P<term>
                    break;
                case 51: //P<term> - > NULL
                   
                    break;
                case 47://<factor>-><integer>  直接取得数字
                    num = lst_node[0].Sym.value;
                    if (num > SymbolTable.addrMax)
                    {                                   //数越界
                        num = 0;
                    }
                    interp.gen(Pcode.LIT, 0, num);                     //生成lit指令，把这个数值字面常量放到栈顶
                    if (mul_div != 0)
                    {//乘法:OPR 0 4 ,除法:OPR 0 5
                        interp.gen(Pcode.OPR, 0, mul_div);
                        mul_div = 0;
                    }
                    break;
                case 46://<factor>-><id> 调用 id，所以要先定义的，这里应当查表
                    index = symTable.position(lst_node[0].Sym.id);
                    
                    if (index > 0)
                    {
                        SymbolTable.Item item = symTable.get(index);
                        switch (item.type)
                        {
                            //如果这个标识符对应的是常量，值为val，生成lit指令，把val放到栈顶
                            case SymbolTable.Item.constant:                        //名字为常量

                                interp.gen(Pcode.LIT, 0, item.value);           //生成lit指令，把这个数值字面常量放到栈顶
                                break;
                            case SymbolTable.Item.variable:                         //名字为变量
                                 //interp.gen(Pcode.LOD, lev - item.lev, item.addr);
                                //把位于距离当前层level的层的偏移地址为adr的变量放到栈顶
                                interp.gen(Pcode.LOD, 0, item.addr);
                                break;
                            case SymbolTable.Item.procedure:                     //常量
                                                                                 //表达式内不可有过程标识符
                                break;
                        }
                        if (mul_div != 0)
                        {//乘法:OPR 0 4 ,除法:OPR 0 5
                            interp.gen(Pcode.OPR, 0, mul_div);
                            mul_div = 0;
                        }
                    }
                    else
                    {
                        myerr.report(5, lst_node[0].Sym.lineCount);
                    }
                    break;
                case 1: //<block>-> <constdecl> <vardecl> <body>
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    
                    break;
                case 59://<statement>-> write( <exp> P<statement_exp> )
                    interp.gen(Pcode.OPR, 0, 15);                                     //OPR 0 14:输出换行
                    write_flag = 0;
                    break;
                case 60: // P<statement_exp> -> ,<exp> P<statement_exp>
                    
                    break;
                case 40: // <lop> -> =
                    ret.type = 8;
                    break;
                case 41: // <lop> -> <>
                    ret.type = 9;
                    break;
                case 42: // <lop> -> <
                    ret.type = 10;
                    break;
                case 43: // <lop> -> <=
                    ret.type = 13;
                    break;
                case 44: // <lop> -> >
                    ret.type = 12;
                    break;
                case 45: // <lop> -> >=
                    ret.type = 11;
                    break;
                case 57:// <lexp> -> <exp> <lop> <exp>
                    //type=eql... leq与7... 13相对应
                    
                    interp.gen(Pcode.OPR, 0, lst_node[1].type);
                    //生成条件跳转指令，跳转地址位置，暂时写0
                    
                    break;
                case 58:// <lexp> -> odd <exp>
                    
                    interp.gen(Pcode.OPR, 0, 6);                        //OPR 0 6:判断栈顶元素是否为奇数
                    //生成条件跳转指令，跳转地址位置，暂时写0
                    
                    break;
                case 25: // <statement>-> if<lexp> then <statement> P<else>
                                                              
                    break;
                case 26:// P<else> -> null
                    interp.pcodeArray[else_pos].a = interp.arrayPtr;
                    break;
                case 27://27 P<else> -> else <statement>
                    interp.pcodeArray[elseEnd_pos].a = interp.arrayPtr;
                    break;
                case 28:// while <lexp> do <statement>
                    interp.gen(Pcode.JMP, 0, lexp_begin);
                    //回填lexp否定的跳转地址
                    interp.pcodeArray[while_pos].a = interp.arrayPtr;
                    break;
            }
            return ret;
        }
        int mul_div = 0; //0 不存在  4 乘法  5除法
        int add_sub = 0; //0 不存在  2 加法  3减法
        int else_pos = 0;
        int while_pos = 0;
        int elseEnd_pos = 0;
        int lexp_begin = 0;
        int read_flag = 0; //如果是var 定义就为0
        int write_flag = 0; //如果在write内 定义就为1
    }
}


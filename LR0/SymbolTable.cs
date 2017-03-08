﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR0
{
    
    public class SymbolTable
    {

        /**
         * 当前名字表项指针(有效的符号表大小)table size
         */
        public int tablePtr = 0;
        /**
         * 符号表的大小
         */
        public static  int tableMax = 100;
        public static  int symMax = 10;            //符号的最大长度
        public static  int addrMax = 1000000;        //最大允许的数值
        public static  int levMax = 3;            //最大允许过程嵌套声明层数[0,levmax]
        public static  int numMax = 14;           //number的最大位数
        public static bool tableswitch;           //显示名字表与否
                                                     //名字表
        public Item[] table = new Item[tableMax];

        public class Item
        {

            public const  int constant = 0;
            public const int variable = 1;
            public const int procedure = 2;
            public const int program = 3;
            public String name;                                             //名字
            public int type;                                               //类型，const \var \ procedur\program
            public int value;                                                 //数值，const使用
            public int lev;                                                 //所处层，var和procedur使用
            public int addr;                                                //地址，var和procedur使用
            public int size;                                               //需要分配的数据区空间，仅procedure使用

            public Item()
            {
              
                this.name = "";
            }

        }

        /**
         * 获得名字表某一项的内容
         *
         * @param i 名字表中的位置
         * @return 名字表第i项的内容
         */
        public Item get(int i)
        {
            if (table[i] == null)
            {
                table[i] = new Item();
            }
            return table[i];
        }

        /**
         * 把某个符号登录到名字表中 名字表从1开始填，0表示不存在该项符号
         *
         * @param sym 要登记到名字表的符号
         * @param type 该符号的类型：const,var,procedure,program
         * @param lev 名字所在的层次
         * @param dx 当前应分配的变量的相对地址，注意调用enter()后dx要加一
         */
        public void enter(Symbol sym, int type, int lev, int dx)
        {
            tablePtr++;
            Item item = get(tablePtr);
            item.name = sym.id;
            item.type = type;
            switch (type)
            {
                case Item.constant:                                     //常量名字
                    item.value = sym.value;                               //记录下常数值的大小
                    break;
                case Item.variable:                                      //变量名字
                    item.lev = lev;                                          //变量所在的层
                    item.addr = dx;                                            //变量的偏移地址
                    break;
                case Item.procedure:                                    //过程名字
                    item.lev = lev;
                    break;
                case Item.program:                                    //程序名字
                    item.lev = lev;
                    break;
            }
        }

        /**
         * 在名字表中查找某个名字的位置 查找符号表是从后往前查， 这样符合嵌套分程序名字定义和作用域的规定
         *
         * @param idt 要查找的名字
         * @return 如果找到则返回名字项的下标，否则返回0
         */
        public int position(String idt)
        {
            for (int i = tablePtr; i > 0; i--) //必须从后往前找
            {
                if (get(i).name == idt )
                {
                    return i;
                }
            }
            return 0;
        }

        /**
         * 输出符号表内容，摘自block()函数
         *
         * @param start 当前符号表区间的左端
         */
        void debugTable(int start)
        {
            if (tableswitch) //显示名字表与否
            {
                return;
            }
            Console.WriteLine("**** Symbol Table ****");
            if (start > tablePtr)
            {
                Console.WriteLine("  NULL");
            }
            for (int i = start + 1; i <= tablePtr; i++)
            {
                try
                {
                    String msg = "unknown table item !";
                    switch (table[i].type)
                    {
                        case Item.constant:
                            msg = "   " + i + "  const: " + table[i].name + "  val: " + table[i].value;
                            break;
                        case Item.variable:
                            msg = "    " + i + "  var: " + table[i].name + "  lev: " + table[i].lev + "  addr: " + table[i].addr;
                            break;
                        case Item.procedure:
                            msg = "    " + i + " proc: " + table[i].name + "  lev: " + table[i].lev + "  addr: " + table[i].size;
                            break;
                        case Item.program:
                            msg = "    " + i + " program: " + table[i].name + "  lev: " + table[i].lev + "  addr: " + table[i].size;
                            break;
                    }
                    Console.WriteLine(msg);
                    //PL0.tableWriter.write(msg + '\n');
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine("***write table intfo meet with error***"+ ex.StackTrace);
                }
            }
        }


    }
}

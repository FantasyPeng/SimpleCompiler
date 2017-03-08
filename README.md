# SimpleCompiler
一个简单的PL/0编译器  
## 程序实现要求    
### 1.设计符号表 
确定符号表的组织方式，一般应包括名字栏和信息栏，其中名字栏作为关键字。要考虑能够存储有关名字的信息，并可以高效地完成如下操作：

a.查找：根据给定的名字，在符号表中查找其信息。如果该名字在符号表中不存在，则将其加入到符号表中，否则返回指向该名字的指针；  
b.删除：从符号表中删除给定名字的表项。

### 2.设计词法分析器
设计各单词的状态转换图，并为不同的单词设计种别码。将词法分析器设计成供语法分析器调用的子程序。功能包括：

a.  具备预处理功能。将不翻译的注释等符号先滤掉，只保留要翻译的符号串，即要求设计一个供词法分析调用的预处理子程序；  
b.  能够拼出语言中的各个单词；  
c.  将拼出的标识符填入符号表；  
d.  返回（种别码， 属性值）。

### 3.语法分析与中间代码产生器
　　要求用预测分析法、算符优先分析法、SLR分析法，实现对表达式、各种说明语句、控制语句进行语法分析。  
若语法正确，则用语法制导翻译法进行语义翻译：对说明语句，要求将说明的各符号记录到相应符号表中；对可执行语句，应产生出四元式中间代码并填写到三地址码表中；  
若语法错误，要求指出出错性质和出错位置（行号）。出错处理应设计成一个出错处理子程序

### 4.中间代码生成
可生成基本的四元式表示的中间代码，也可以生成虚拟机规定的汇编语言代码（相关材料以后再给）  
##  PL/0语言的BNF描述（扩充的巴克斯范式表示法）

```
<prog> → program <id>；<block>
<block> → [<condecl>][<vardecl>][<proc>]<body>
<condecl> → const <const>{,<const>}
<const> → <id>:=<integer>
<vardecl> → var <id>{,<id>}
<proc> → procedure <id>（[<id>{,<id>}]）;<block>{;<proc>}
<body> → begin <statement>{;<statement>}end
<statement> → <id> := <exp>               
|if <lexp> then <statement>[else <statement>]
               |while <lexp> do <statement>
               |call <id>[（<exp>{,<exp>}）]
               |<body>
               |read (<id>{，<id>})
               |write (<exp>{,<exp>})
<lexp> → <exp> <lop> <exp>|odd <exp>
<exp> → [+|-]<term>{<aop><term>}
<term> → <factor>{<mop><factor>}
<factor>→<id>|<integer>|(<exp>)
<lop> → =|<>|<|<=|>|>=
<aop> → +|-
<mop> → *|/
<id> → l{l|d}   （注：l表示字母）
<integer> → d{d}

```
注释：　　　



<prog>：程序 ；<block>：块、程序体 ；<condecl>：常量说明 ；<const>：常量；<vardecl>：变量说明 ；<proc>：分程序 ； <body>：复合语句 ；<statement>：语句；<exp>：表达式 ；<lexp>：条件 ；<term>：项 ； <factor>：因子 ；<aop>：加法运算符；<mop>：乘法运算符； <lop>：关系运算符。
- ## 	假想目标机的代码
- LIT 0 ，a 取常量a放入数据栈栈顶
- OPR 0 ，a 执行运算，a表示执行某种运算
- LOD L ，a 取变量（相对地址为a，层差为L）放到数据栈的栈顶
- STO L ，a 将数据栈栈顶的内容存入变量（相对地址为a，层次差为L）
- CAL L ，a 调用过程（转子指令）（入口地址为a，层次差为L）
- INT 0 ，a 数据栈栈顶指针增加a
- JMP 0 ，a无条件转移到地址为a的指令
- JPC 0 ，a 条件转移指令，转移到地址为a的指令
- RED L ，a 读数据并存入变量（相对地址为a，层次差为L）
- WRT 0 ，0 将栈顶内容输出



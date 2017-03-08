using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR0
{
    class MyFile
    {
        int countVtVn = 59;
        string filename = "E:\\Table.txt"; //获取输入文件名
        FileStream fs; //声明FileStream对象
        public void write()
        {
            try
            {
                fs = new FileStream(filename, FileMode.Create); //初始化FileStream对象
                BinaryWriter bw = new BinaryWriter(fs); //创建BinaryWriter对象
                //写入文件
                foreach (Dictionary<string, string> d in Form1.Table)
                {
                    foreach (string key in d.Keys)
                    {
                        bw.Write(key);
                        bw.Write(d[key]);
                    }
                }
                Console.WriteLine("成功写入");
                bw.Close(); //关闭BinaryWriter对象
                fs.Close(); //关闭文件流
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void read()
        {
            fs = new FileStream(filename, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            String command = Console.ReadLine();
            int i = 0;
       //     从磁盘上读取信息
            try
            {
                while (true)
                {
                    Dictionary<String, String> dt = new Dictionary<string, string>();
                    for(int j = 0; j < 59;j++)
                    {
                        String key = br.ReadString();
                        String value = br.ReadString();
                        dt.Add(key, value);
                    }
                    Form1.Table.Add(dt);
                    i++;
                    dt = null;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("\n\n读取结束！");
            }
            
            br.Close();
            fs.Close();
        }
        public void write1(String filename, String con)
        {
            try
            {
                fs = new FileStream(filename, FileMode.Append); //初始化FileStream对象
                BinaryWriter bw = new BinaryWriter(fs); //创建BinaryWriter对象
                //写入文件
                bw.Write(con + "\r\n");
                bw.Close(); //关闭BinaryWriter对象
                fs.Close(); //关闭文件流
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void delete(String filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("Text文件不存在");
            }
            else
            {
                Console.WriteLine("Text文件删除");
                File.Delete(filename);
            }
        }
    }
}

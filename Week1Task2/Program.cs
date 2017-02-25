using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week1Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = File.OpenText("input.txt"))
            using (var writer = new StreamWriter(File.OpenWrite("output.txt")))
            {
                var str = reader.ReadLine();
                string[] val = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var a = long.Parse(val[0]);
                var b = long.Parse(val[1]);
                writer.Write(a + b * b);
            }
        }
    }
}

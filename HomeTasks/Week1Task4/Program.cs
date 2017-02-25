using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week1Task4
{
    class Program
    {
        struct Val
        {
            public double val;
            public int idx;
        }

        static void Main(string[] args)
        {
            using (var reader = File.OpenText("input.txt"))
            using (var writer = new StreamWriter(File.OpenWrite("output.txt")))
            {
                var n = int.Parse(reader.ReadLine());
                var strArr = reader.ReadLine().Split(' ');
                Val[] arr = new Val[n];
                for (int i = 0; i < n; i++)
                {
                    arr[i] = new Val() { idx = i + 1, val = double.Parse(strArr[i], CultureInfo.InvariantCulture) };
                }

                for (int i = 1; i < n; i++)
                {
                    int j = i;
                    while (j > 0 && arr[j].val < arr[j - 1].val)
                    {
                        var temp = arr[j - 1];
                        arr[j - 1] = arr[j];
                        arr[j] = temp;

                        j--;
                    }
                }

                writer.Write(arr[0].idx);
                writer.Write(" ");
                writer.Write(arr[n / 2].idx);
                writer.Write(" ");
                writer.Write(arr[n - 1].idx);
                writer.Write(" ");
            }
        }
    }
}

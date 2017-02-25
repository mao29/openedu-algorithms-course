using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week1Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = File.OpenText("input.txt"))
            using (var writer = new StreamWriter(File.OpenWrite("output.txt")))
            {
                var n = int.Parse(reader.ReadLine());
                int[] arr = reader.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
                writer.Write("1 ");
                for (int i = 1; i < n; i++)
                {
                    int j = i;
                    while (j > 0 && arr[j] < arr[j - 1])
                    {
                        var temp = arr[j - 1];
                        arr[j - 1] = arr[j];
                        arr[j] = temp;

                        j--;
                    }
                    writer.Write(j + 1);
                    writer.Write(" ");
                }

                writer.WriteLine();

                for (int i = 0; i < n; i++)
                {
                    writer.Write(arr[i]);
                    writer.Write(" ");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week1Task5
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

                for (int i = 0; i < n; i++)
                {
                    int k = i;
                    for (int j = i + 1; j < n; j++)
                    {
                        if (arr[j] < arr[k])
                        {
                            k = j;
                        }
                    }
                    if (i != k)
                    {
                        writer.WriteLine("Swap elements at indices {0} and {1}.", i + 1, k + 1);
                        var temp = arr[i];
                        arr[i] = arr[k];
                        arr[k] = temp;
                    }
                }
                writer.WriteLine("No more swaps needed.");

                for (int i = 0; i < n; i++)
                {
                    writer.Write(arr[i]);
                    writer.Write(" ");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week2Task1
{
    static class Extensions
    {
        public static char? ReadChar(this StreamReader reader)
        {
            int nextChar = reader.Read();

            if (nextChar == -1)
            {
                return null;
            }

            return (char)nextChar;
        }

        public static string ReadToken(this StreamReader reader)
        {
            char? c = reader.ReadChar();
            while (c.HasValue && char.IsWhiteSpace(c.Value))
            {
                c = reader.ReadChar();
            }

            StringBuilder builder = new StringBuilder();
            while (c.HasValue && !char.IsWhiteSpace(c.Value))
            {
                builder.Append(c.Value);
                c = reader.ReadChar();
            }
            return builder.ToString();
        }

        public static int ReadInt(this StreamReader reader)
        {
            var token = reader.ReadToken();
            if (!string.IsNullOrEmpty(token))
            {
                return int.Parse(token);
            }
            throw new IOException();
        }

        public static long ReadLong(this StreamReader reader)
        {
            var token = reader.ReadToken();
            if (!string.IsNullOrEmpty(token))
            {
                return long.Parse(token);
            }
            throw new IOException();
        }

        public static double ReadDouble(this StreamReader reader)
        {
            var token = reader.ReadToken();
            if (!string.IsNullOrEmpty(token))
            {
                return double.Parse(token, CultureInfo.InvariantCulture);
            }
            throw new IOException();
        }

        public static int[] ReadIntArray(this StreamReader reader, int length)
        {
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = reader.ReadInt();
            }
            return result;
        }

        public static long[] ReadLongArray(this StreamReader reader, int length)
        {
            long[] result = new long[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = reader.ReadLong();
            }
            return result;
        }

        public static double[] ReadDoubleArray(this StreamReader reader, int length)
        {
            double[] result = new double[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = reader.ReadDouble();
            }
            return result;
        }

        public static void WriteArray(this StreamWriter writer, int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
                writer.Write(" ");
            }
        }
    }


    class Program
    {
        struct Val
        {
            public int val;
            public int idx;
        }

        static StreamReader reader;
        static StreamWriter writer;

        static void Main(string[] args)
        {
            using (reader = File.OpenText("input.txt"))
            using (writer = new StreamWriter(File.OpenWrite("output.txt")))
            {
                var n = reader.ReadInt();
                var valArr = reader.ReadIntArray(n);
                //Val[] arr = new Val[n];
                //for (int i = 0; i < n; i++)
                //{
                //    arr[i] = new Val() { idx = i + 1, val = valArr[i] };
                //}

                  //    Val[] arr1 = MergeSort(arr);
                int[] arr2 = MergeSort(valArr);
                //writer.WriteArray(arr1.Select(x => x.val).ToArray());
                writer.WriteArray(arr2);
            }
        }

        static int[] MergeSort(int[] arr)
        {
            if (arr.Length == 1)
                return arr;

            int[] newArr = new int[arr.Length];
            for (int k = 2; k / 2 < arr.Length; k *= 2)
            {
                for (int c = 0; c < arr.Length / k + (arr.Length % k > 0 ? 1 : 0); c++)
                {
                    int f = c * k;
                    int l = Math.Min((c + 1) * k, arr.Length);
                    int middle = f + k / 2;
                    if (middle >= l)
                    {
                        Array.Copy(arr, f, newArr, f, l - f);
                        continue;
                    }

                    int j = middle;
                    int i = f;
                    int m = f;
                    while (i < middle || j < l)
                    {
                        if (i == middle || (j < l && arr[j] < arr[i]))
                        {
                            newArr[m] = arr[j];
                            m++;
                            j++;
                        }
                        else
                        {
                            newArr[m] = arr[i];
                            m++;
                            i++;
                        }
                    }
                    writer.WriteArray(new int[] { f + 1, l, newArr[f], newArr[l - 1] });
                    writer.WriteLine();
                }
                Array.Copy(newArr, arr, arr.Length);
            }

            return newArr;
        }

        static Val[] MergeSort(Val[] arr)
        {
            if (arr.Length == 1)
                return arr;

            Val[] arr1 = new Val[arr.Length / 2];
            Array.Copy(arr, 0, arr1, 0, arr1.Length);

            Val[] arr2 = new Val[arr.Length - arr.Length / 2];
            Array.Copy(arr, arr.Length / 2, arr2, 0, arr2.Length);

            arr1 = MergeSort(arr1);
            arr2 = MergeSort(arr2);
            return Merge(arr1, arr2);
        }

        static Val[] Merge(Val[] arr1, Val[] arr2)
        {
            Val[] res = new Val[arr1.Length + arr2.Length];
            int i = 0;
            int j = 0;

            while (i < arr1.Length || j < arr2.Length)
            {
                if (i == arr1.Length || (j < arr2.Length && arr2[j].val <= arr1[i].val))
                {
                    res[i + j] = arr2[j];
                    j++;
                }
                else
                {
                    res[i + j] = arr1[i];
                    i++;
                }
            }
            writer.WriteArray(new int[] { res.Select(x => x.idx).Min(), res.Select(x => x.idx).Max(), res.First().val, res.Last().val });
            writer.WriteLine();
            return res;
        }
    }
}

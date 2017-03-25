using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week2Task2
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
        static StreamReader reader;
        static StreamWriter writer;

        static void Main(string[] args)
        {

            using (reader = File.OpenText("input.txt"))
            using (writer = new StreamWriter(File.OpenWrite("output.txt")))
            {
                var n = reader.ReadInt();
                var arr = reader.ReadIntArray(n);
                var res = MergeSort(arr);
                writer.Write(res);
            }
        }

        static long MergeSort(int[] arr)
        {
            long inverseCount = 0;
            if (arr.Length == 1)
                return inverseCount;

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
                    long localInverseCount = 0;
                    while (i < middle || j < l)
                    {
                        if (i == middle || (j < l && arr[j] < arr[i]))
                        {
                            if (i < middle)
                            {
                                localInverseCount++;
                            }

                            newArr[m] = arr[j];
                            m++;
                            j++;
                        }
                        else
                        {
                            inverseCount += localInverseCount;
                            newArr[m] = arr[i];
                            m++;
                            i++;
                        }
                    }
                }
                Array.Copy(newArr, arr, arr.Length);
            }

            return inverseCount;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week1Task5
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

        public static void WriteArray(this StreamWriter writer, long[] array)
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
        static void Main(string[] args)
        {
            using (var reader = File.OpenText("input.txt"))
            using (var writer = new StreamWriter(File.OpenWrite("output.txt")))
            {                
                var n = reader.ReadInt();
                int[] arr = reader.ReadIntArray(n);
                
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

                writer.WriteArray(arr);                
            }
        }
    }
}

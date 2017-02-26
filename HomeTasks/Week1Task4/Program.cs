using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week1Task4
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
            public double val;
            public int idx;
        }

        static void Main(string[] args)
        {
            using (var reader = File.OpenText("input.txt"))
            using (var writer = new StreamWriter(File.OpenWrite("output.txt")))
            {
                var n = reader.ReadInt();
                var valArr = reader.ReadDoubleArray(n);
                Val[] arr = new Val[n];
                for (int i = 0; i < n; i++)
                {
                    arr[i] = new Val() { idx = i + 1, val = valArr[i] };
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

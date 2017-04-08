using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week6Task2
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

        public static void WriteArray(this StreamWriter writer, ushort[] array)
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
        static double e = 1e-7;

        static void Main(string[] args)
        {
            using (reader = File.OpenText("input.txt"))
            using (writer = new StreamWriter(File.Create("output.txt")))
            {
                int n = reader.ReadInt();
                double a = reader.ReadDouble();

                double l = a;
                double r = 0;
                double minB = ComputeB(a, r, n);

                while (Math.Abs(l - r) > e)
                {
                    double m = (l + r) / 2;
                    double b = ComputeB(a, m, n);
                    if (b <= minB)
                    {
                        r = m;
                        minB = b;
                    }
                    else
                    {
                        l = m;
                    }
                }
                writer.Write("{0:F7}", minB);
            }
        }

        private static double ComputeB(double h1, double h2, int n)
        {
            bool rise = false;
            for (int i = 2; i <= n; i++)
            {
                double hNext = 2 * h2 + 2 - h1;
                if (!rise && hNext > h2)
                {
                    return double.MaxValue;
                }
                if (hNext < 0 && Math.Abs(hNext) > e)
                {
                    return double.MaxValue;
                }
                if (Math.Abs(hNext) < e)
                    rise = true;
                h1 = h2;
                h2 = hNext;
            }
            return h2;
        }
    }
}

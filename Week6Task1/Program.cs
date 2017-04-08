using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week6Task1
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

        class Element
        {
            public int StartIndex;
            public int Value;
            public int Count;
        }

        static void Main(string[] args)
        {
            using (reader = File.OpenText("input.txt"))
            using (writer = new StreamWriter(File.Create("output.txt")))
            {
                int n = reader.ReadInt();

                Element[] arr = new Element[n];
                int val = reader.ReadInt();
                arr[0] = new Element()
                {
                    Count = 1,
                    StartIndex = 0,
                    Value = val
                };
                int length = 1;
                for (int i = 1; i < n; i++)
                {
                    val = reader.ReadInt();
                    if (arr[length - 1].Value == val)
                    {
                        arr[length - 1].Count++;
                    }
                    else
                    {
                        arr[length] = new Element()
                        {
                            Count = 1,
                            Value = val,
                            StartIndex = arr[length - 1].StartIndex + arr[length - 1].Count
                        };
                        length++;
                    }
                }

                int m = reader.ReadInt();
                while (m > 0)
                {
                    int search = reader.ReadInt();
                    var found = BinarySearch(arr, length, search);
                    if (found == -1)
                    {
                        writer.WriteLine("-1 -1");
                    }
                    else
                    {
                        writer.WriteLine("{0} {1}", arr[found].StartIndex + 1, arr[found].StartIndex + arr[found].Count);
                    }
                    m--;
                }
            }
        }

        static int BinarySearch(Element[] arr, int length, int search)
        {
            int l = -1;
            int r = length;

            while (l + 1 < r)
            {
                int m = (l + r) / 2;
                if (arr[m].Value < search)
                {
                    l = m;
                }
                else
                {
                    r = m;
                }
            }

            if (r < length && arr[r].Value == search)
            {
                return r;
            }
            else
            {
                return -1;
            }
        }
    }
}

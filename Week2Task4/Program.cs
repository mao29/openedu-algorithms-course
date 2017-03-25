using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week2Task4
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
            using (writer = new StreamWriter(File.Create("output.txt")))
            {
                int n = reader.ReadInt();
                int[] arr = new int[n];
                int k1 = reader.ReadInt();
                int k2 = reader.ReadInt();

                int a = reader.ReadInt();
                int b = reader.ReadInt();
                int c = reader.ReadInt();
                int a1 = reader.ReadInt();
                int a2 = reader.ReadInt();

                //int n = 100000;
                //int[] arr = new int[n];
                //int k1 = 1;
                //int k2 = 200;

                //int a = 0;
                //int b = 1;
                //int c = 0;
                //int a1 = 2;
                //int a2 = 2;

                //if (a1 > a2)
                //{
                //    arr[0] = a2;
                //    arr[1] = a1;
                //}
                //else
                //{
                //    arr[0] = a1;
                //    arr[1] = a2;
                //}

                arr[0] = a1;
                arr[1] = a2;

                for (int i = 2; i < n; i++)
                {
                    int a3 = a * a1 + b * a2 + c;
                    arr[i] = a3;
                    a1 = a2;
                    a2 = a3;
                }

                //QSort(arr, 0, arr.Length - 1);

                //writer.WriteArray(arr);

                //writer.WriteLine();

                for (int i = k1 - 1; i < k2; i++)
                {
                    int kth = KthStatistic(arr, 0, arr.Length - 1, i);
                    writer.Write(kth);
                    writer.Write(" ");
                }
            }
        }

        static int KthStatistic(int[] arr, int l, int r, int k)
        {
            if (l == r)
                return arr[l];

            int m = Partition(arr, l, r);

            if (m == k)
                return arr[m];
            else if (m > k)
                return KthStatistic(arr, l, m, k);
            else
                return KthStatistic(arr, m + 1, r, k);
        }

        static void QSort(int[] arr, int l, int r)
        {
            if (l < r)
            {
                int m = Partition(arr, l, r);
                QSort(arr, l, m);
                QSort(arr, m + 1, r);
            }
        }

        static int Partition(int[] arr, int l, int r)
        {
            int key = arr[(l + r) / 2];

            int i = l;
            int j = r;

            while (i <= j)
            {
                while (arr[i] < key)
                    i++;
                while (arr[j] > key)
                    j--;

                if (i <= j)
                {
                    int temp = arr[j];
                    arr[j] = arr[i];
                    arr[i] = temp;
                    i++;
                    j--;
                }
            }

            return j + 1;// Math.Max(j, l);
        }

        static void OrderedInsert(int[] arr, int val, int currLen)
        {
            int len = currLen / 2;
            int idx = len;
            while (len > 0)//&& arr[idx] != val)
            {
                if (arr[idx] <= val)
                {
                    idx += len / 2 + len % 2;
                }
                else
                {
                    idx -= len / 2 + len % 2;
                }
                len = len / 2;
            }

            if (idx < currLen && arr[idx] < val)
                idx++;

            for (int i = currLen; i > idx; i--)
            {
                arr[i] = arr[i - 1];
            }

            arr[idx] = val;
        }
    }
}

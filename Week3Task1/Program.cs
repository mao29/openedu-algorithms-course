using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3Task1
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

        class Val
        {
            public ushort value;
            public ushort count;
            public ushort secondIdx;
        }

        static void Main(string[] args)
        {
            using (reader = File.OpenText("input.txt"))
            using (writer = new StreamWriter(File.Create("output.txt")))
            {
                int n = reader.ReadInt();
                int m = reader.ReadInt();
                //ushort i = 0;
                //ushort j = 0;

                int[] a = new int[n];
                int[] b = new int[m];

                for (int i = 0; i < n; i++)
                {
                    a[i] = reader.ReadInt();
                }
                for (int i = 0; i < m; i++)
                {
                    b[i] = reader.ReadInt();
                }
                long sum = 0;
                int[] resarr = new int[n * m];
                a = CountSort(a, 40001);
                b = CountSort(b, 40001);
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        resarr[i * m + j] = a[i] * b[j];
                    }
                }
                Array.Sort(resarr);
                
                for (int i = 0; i < n * m; i += 10)
                    sum += resarr[i];
                writer.Write(sum);


                int pos = 0;
                //a = CountSort(a, 40001);
                //b = CountSort(b, 40001);
                //writer.WriteArray(a);
                //writer.WriteLine();
                //writer.WriteArray(b);

                //if (n < m)
                //{
                //    b = CountSort(b, 40001);// Array.Sort(b);
                //    Dictionary<int, ushort> mergingArrays = new Dictionary<int, ushort>();
                //    for (ushort i = 0; i < n; i++)
                //    {
                //        mergingArrays.Add(i, 0);
                //    }

                //    //ushort[] mergePositions = new ushort[n];
                //    // int lastMinIdx = 0;
                //    while (pos < n * m)
                //    {
                //        int minIdx = mergingArrays.Keys.First();// lastMinIdx;
                //        int minVal = a[minIdx] * b[mergingArrays[minIdx]];// b[mergePositions[minIdx]];
                //        foreach (var pair in mergingArrays)
                //        {
                //            if (a[pair.Key] * b[pair.Value] < minVal)
                //            {
                //                minVal = a[pair.Key] * b[pair.Value];
                //                minIdx = pair.Key;
                //            }
                //        }
                //        //for (int i = 1; i < n; i++)
                //        //{
                //        //    if (mergePositions[i] < m && a[i] * b[mergePositions[i]] < minVal)
                //        //    {
                //        //        minVal = a[i] * b[mergePositions[i]];
                //        //        minIdx = i;
                //        //    }
                //        //}

                //        mergingArrays[minIdx]++;
                //        if (mergingArrays[minIdx] == m)
                //        {
                //            mergingArrays.Remove(minIdx);
                //        }

                //        //mergePositions[minIdx]++;
                //        //if (minIdx == lastMinIdx)
                //        //{
                //        //    while (mergePositions[lastMinIdx] == m)
                //        //    {
                //        //        lastMinIdx++;
                //        //    }
                //        //}
                //        //    resarr[pos] = minVal;
                //        if (pos % 10 == 0)
                //        {
                //            sum += minVal;
                //        }
                //        pos++;
                //    }
                //}
                //else
                //{
                //    a = CountSort(a, 40001);
                //    //Array.Sort(a);
                //    Dictionary<int, ushort> mergingArrays = new Dictionary<int, ushort>();
                //    for (ushort i = 0; i < m; i++)
                //    {
                //        mergingArrays.Add(i, 0);
                //    }
                //    //ushort[] mergePositions = new ushort[m];
                //    //int lastMinIdx = 0;
                //    while (pos < n * m)
                //    {
                //        int minIdx = mergingArrays.Keys.First();
                //        int minVal = b[minIdx] * a[mergingArrays[minIdx]];
                //        foreach (var pair in mergingArrays)
                //        {
                //            if (b[pair.Key] * a[pair.Value] < minVal)
                //            {
                //                minVal = b[pair.Key] * a[pair.Value];
                //                minIdx = pair.Key;
                //            }
                //        }
                //        mergingArrays[minIdx]++;
                //        if (mergingArrays[minIdx] == n)
                //        {
                //            mergingArrays.Remove(minIdx);
                //        }
                //        //int minIdx = lastMinIdx;
                //        //int minVal = b[minIdx] * a[mergePositions[minIdx]];
                //        //for (int i = 1; i < m; i++)
                //        //{
                //        //    if (mergePositions[i] < n && b[i] * a[mergePositions[i]] < minVal)
                //        //    {
                //        //        minVal = b[i] * a[mergePositions[i]];
                //        //        minIdx = i;
                //        //    }
                //        //}
                //        //resarr[pos] = minVal;
                //        //mergePositions[minIdx]++;
                //        //if (minIdx == lastMinIdx)
                //        //{
                //        //    while (mergePositions[lastMinIdx] == n)
                //        //    {
                //        //        lastMinIdx++;
                //        //    }
                //        //}
                //        //       resarr[pos] = minVal;
                //        if (pos % 10 == 0)
                //        {
                //            sum += minVal;
                //        }
                //        pos++;
                //    }
                //}
                //writer.Write(sum);
                //// writer.WriteArray(resarr);



                //  ushort[] a = new ushort[40000];
                //for (i = 0; i < n; i++)
                //{
                //    int val = reader.ReadInt();
                //    a[val]++;
                //}



                //ushort[] b = new ushort[40000];

                //for (i = 0; i < m; i++)
                //{
                //    int val = reader.ReadInt();
                //    b[val]++;
                //}
                //int[] arr = new int[6000 * 6000];


                //List<Val> sortedA = new List<Val>();
                //List<Val> sortedB = new List<Val>();
                //for (i = 0; i < 40000; i++)
                //{
                //    if (a[i] > 0)
                //    {
                //        sortedA.Add(new Val() { count = a[i], secondIdx = 0, value = i });
                //    }

                //    if (b[i] > 0)
                //    {
                //        sortedB.Add(new Val() { count = b[i], secondIdx = 0, value = i });
                //    }
                //}

                //for (i = 0; i < sortedA.Count; i++)
                //{
                //    for (j = 0; j < sortedB.Count; j++)
                //    {

                //    }
                //}

                //i = 0;
                //j = 0;
                //long value = long.MaxValue;
                //long sum = 0;
                //int totalCount = 0;

                //while (i < sortedA.Count || j < sortedB.Count)
                //{
                //    long currVal = ((long)sortedA[i].value) * ((long)sortedB[j].value);

                //    if (currVal < value)
                //    {
                //        int p = totalCount % 10;
                //        int currCount = ((int)sortedA[i].count) * ((int)sortedB[j].count);
                //        sum += currVal * ((p + currCount) % 10);
                //        totalCount += currCount;
                //        sortedA[i].secondIdx = j;
                //    }
                //}

            }
        }

        static int[] CountSort(int[] arr, int maxVal)
        {
            int[] result = new int[arr.Length];
            ushort[] counts = new ushort[maxVal];
            for (int i = 0; i < arr.Length; i++)
            {
                counts[arr[i]]++;
            }

            ushort lastPos = 0;
            for (int i = 0; i < maxVal; i++)
            {
                if (counts[i] > 0)
                {
                    counts[i] += lastPos;
                    lastPos = counts[i];
                }
            }

            for (int i = 0; i < arr.Length; i++)
            {
                result[counts[arr[i]] - 1] = arr[i];
                counts[arr[i]]--;
            }
            return result;
        }
    }
}

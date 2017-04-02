using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week5Task2
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

        public class QueueElement
        {
            public int Position;
            public int Value;

            public QueueElement(int position, int value)
            {
                Position = position;
                Value = value;
            }
        }

        public class PriorityQueue
        {
            int _capacity;
            QueueElement[] _heap;
            int _size;

            public PriorityQueue(int capacity)
            {
                _capacity = capacity;
                _heap = new QueueElement[_capacity];
                _size = 0;
            }

            public QueueElement Add(int value)
            {
                int insertedIdx = _size;
                _size++;
                _heap[insertedIdx] = new QueueElement(insertedIdx, int.MaxValue);

                return Decrease(insertedIdx, value);
            }

            public QueueElement Decrease(int idx, int newValue)
            {
                int insertedIdx = idx;
                _heap[insertedIdx].Value = newValue;
                int parentIdx = (insertedIdx - 1) / 2;
                while (parentIdx >= 0 && _heap[parentIdx].Value > _heap[insertedIdx].Value)
                {
                    var temp = _heap[parentIdx];
                    _heap[parentIdx] = _heap[insertedIdx];
                    _heap[parentIdx].Position = parentIdx;
                    _heap[insertedIdx] = temp;
                    _heap[insertedIdx].Position = insertedIdx;
                    insertedIdx = parentIdx;
                    parentIdx = (insertedIdx - 1) / 2;
                }
                return _heap[insertedIdx];
            }

            public int Extract()
            {
                int result = _heap[0].Value;
                _size--;
                _heap[0] = _heap[_size];
                _heap[0].Position = 0;
                Heapify(1);
                return result;
            }

            public bool IsEmpty()
            {
                return _size == 0;
            }

            private void Heapify(int idx)
            {
                int minIdx = idx - 1;
                int left = idx * 2 - 1;
                int right = idx * 2;
                if (left < _size && _heap[left].Value < _heap[minIdx].Value)
                {
                    minIdx = left;
                }
                if (right < _size && _heap[right].Value < _heap[minIdx].Value)
                {
                    minIdx = right;
                }

                if (minIdx != idx - 1)
                {
                    var temp = _heap[minIdx];
                    _heap[minIdx] = _heap[idx - 1];
                    _heap[minIdx].Position = minIdx;
                    _heap[idx - 1] = temp;
                    _heap[idx - 1].Position = idx - 1;
                    Heapify(minIdx + 1);
                }
            }
        }


        static void Main(string[] args)
        {
            using (reader = File.OpenText("input.txt"))
            using (writer = new StreamWriter(File.Create("output.txt")))
            {
                int n = reader.ReadInt();
                var queue = new PriorityQueue(n + 1);
                QueueElement[] elements = new QueueElement[n + 1];
                int i = 0;
                while (i < n)
                {
                    var str = reader.ReadToken();
                    switch (str)
                    {
                        case "A":
                            int val = reader.ReadInt();
                            elements[i] = queue.Add(val);
                            break;
                        case "X":
                            if (queue.IsEmpty())
                            {
                                writer.WriteLine("*");
                            }
                            else
                            {
                                writer.WriteLine(queue.Extract());
                            }
                            break;
                        case "D":
                            int pos = reader.ReadInt();
                            int newVal = reader.ReadInt();
                            queue.Decrease(elements[pos - 1].Position, newVal);
                            break;
                    }

                    i++;
                }
            }
        }
    }
}

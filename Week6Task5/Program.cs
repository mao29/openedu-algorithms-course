using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week6Task5
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

        class Node<T> where T : IComparable
        {
            public T Key;
            public Node<T> Left, Right, Parent;


            public Node<T> GetNext()
            {
                var next = Right;
                if (next == null)
                {
                    next = this;
                    while (next.Parent != null && next.Parent.Right == next)
                    {
                        next = next.Parent;
                    }
                    return next.Parent;
                }
                else
                {
                    while (next.Left != null)
                    {
                        next = next.Left;
                    }
                }
                return next;
            }
            //public T MaxRight;
            //public T MinLeft;
        }

        class CustomQueue<T>
        {
            int _capacity;
            T[] _queue;
            int _head, _tail;
            public CustomQueue(int capacity)
            {
                _capacity = capacity;
                _queue = new T[_capacity];
                _tail = 0;
                _head = 0;
            }

            public T Dequeue()
            {
                T value = _queue[_head];
                _head = (_head + 1) % _capacity;
                return value;
            }

            public void Enqueue(T value)
            {
                _queue[_tail] = value;
                _tail = (_tail + 1) % _capacity;
            }

            public bool IsEmpty()
            {
                return _head == _tail;
            }
        }

        static void Main(string[] args)
        {
            using (reader = File.OpenText("input.txt"))
            using (writer = new StreamWriter(File.Create("output.txt")))
            {
                int n = reader.ReadInt();
                if (n == 0)
                {
                    writer.Write("YES");
                    return;
                }

                Node<int>[] arr = new Node<int>[n];
                //CustomQueue<Node<int>> queue = new CustomQueue<Node<int>>(n + 1);
                for (int i = 0; i < n; i++)
                {
                    arr[i] = new Node<int>();
                    //arr[i].MaxRight = int.MinValue;
                    //arr[i].MinLeft = int.MaxValue;
                }

                for (int i = 0; i < n; i++)
                {
                    int key = reader.ReadInt();
                    int leftIdx = reader.ReadInt();
                    int rightIdx = reader.ReadInt();

                    arr[i].Key = key;
                    if (leftIdx != 0)
                    {
                        arr[i].Left = arr[leftIdx - 1];
                        arr[leftIdx - 1].Parent = arr[i];
                    }
                    //else
                    //{
                    //    arr[i].MinLeft = arr[i].Key;
                    //}

                    if (rightIdx != 0)
                    {
                        arr[i].Right = arr[rightIdx - 1];
                        arr[rightIdx - 1].Parent = arr[i];
                    }
                    //else
                    //{
                    //    arr[i].MaxRight = arr[i].Key;
                    //}

                    //if (leftIdx == 0 && rightIdx == 0)
                    //{
                    //    queue.Enqueue(arr[i]);
                    //}
                }

                Node<int> root = null;
                for (int i = 0; i < n && root == null; i++)
                {
                    if (arr[i].Parent == null)
                    {
                        root = arr[i];
                    }
                }

                Node<int> current;
                current = root;
                while (current.Left != null)
                {
                    current = current.Left;
                }

                var next = current.GetNext();
                while (next != null && current.Key.CompareTo(next.Key) < 0)
                {
                    current = next;
                    next = next.GetNext();
                }

                if (next != null)
                {
                    writer.Write("NO");
                }
                else
                {
                    writer.Write("YES");
                }

                //while (!queue.IsEmpty())
                //{
                //    var node = queue.Dequeue();
                //    var parent = node.Parent;
                //    if (parent != null)
                //    {
                //        if (parent.Left == node)
                //        {
                //            if (parent.Key.CompareTo(node.MaxRight) < 0)
                //            {
                //                writer.Write("NO");
                //                return;
                //            }
                //            parent.MinLeft = node.MinLeft;
                //            queue.Enqueue(parent); 
                //        }

                //        if (parent.Right == node)
                //        {
                //            if (parent.Key.CompareTo(node.MinLeft) > 0)
                //            {
                //                writer.Write("NO");
                //                return;
                //            }
                //            parent.MaxRight = node.MaxRight;
                //            queue.Enqueue(parent);
                //        }
                //    }
                //}

                //writer.Write("YES");
            }
        }
    }
}

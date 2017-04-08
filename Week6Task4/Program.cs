using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week6Task4
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

            private int _size = 0;

            public int Size
            {
                get
                {
                    if (_size == 0)
                    {
                        InitSize();
                    }
                    return _size;
                }
                set
                {
                    _size = value;
                }
            }

            private void InitSize()
            {
                _size = 1;
                if (Left != null)
                {
                    _size += Left.Size;
                }
                if (Right != null)
                {
                    _size += Right.Size;
                }
            }

            public static Node<T> FindKey(Node<T> root, T key)
            {
                if (root == null || root.Key.CompareTo(key) == 0)
                {
                    return root;
                }

                if (root.Key.CompareTo(key) > 0)
                {
                    return FindKey(root.Left, key);
                }

                return FindKey(root.Right, key);
            }
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

                Node<int>[] arr = new Node<int>[n];
                //CustomQueue<Node<int>> queue = new CustomQueue<Node<int>>(51);

                for (int i = 0; i < n; i++)
                {
                    arr[i] = new Node<int>();
                }
                Node<int> root = arr[0];
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
                    if (rightIdx != 0)
                    {
                        arr[i].Right = arr[rightIdx - 1];
                        arr[rightIdx - 1].Parent = arr[i];
                    }
                    while (root.Parent != null)
                    {
                        root = root.Parent;
                    }

                    //if (leftIdx == 0 && rightIdx == 0)
                    //{
                    //    queue.Enqueue(arr[i]);
                    //}
                }

                //while (!queue.IsEmpty())
                //{
                //    var leafNode = queue.Dequeue();
                //    var parent = leafNode.Parent;
                //    while (parent != null)
                //    {
                //        parent.Size++;
                //        parent = parent.Parent;
                //    }
                //    if (leafNode.Parent != null && (leafNode.Parent.Left == null || leafNode.Parent.Right == null || leafNode.Parent.Right == leafNode))
                //    {
                //        queue.Enqueue(leafNode.Parent);
                //    }
                //    if (leafNode.Parent == null)
                //    {
                //        root = leafNode;
                //    }
                //}
                int initSize = root.Size;
                int m = reader.ReadInt();
                while (m > 0)
                {
                    int key = reader.ReadInt();

                    var keyNode = Node<int>.FindKey(root, key);
                    if (keyNode != null)
                    {
                        var parent = keyNode.Parent;
                        if (parent.Left == keyNode)
                        {
                            parent.Left = null;
                        }
                        else
                        {
                            parent.Right = null;
                        }
                        while (parent != null)
                        {
                            parent.Size -= keyNode.Size;
                            parent = parent.Parent;
                        }

                    }
                    writer.WriteLine(root.Size);

                    m--;
                }
            }
        }
    }
}

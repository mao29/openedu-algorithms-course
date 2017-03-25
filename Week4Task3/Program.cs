using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week4Task3
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

        class CustomStack<T>
        {
            int _capacity;
            T[] _stack;
            int tail;
            public CustomStack(int capacity)
            {
                _capacity = capacity;
                _stack = new T[_capacity];
                tail = -1;
            }

            public T Pop()
            {
                T value = _stack[tail];
                tail--;
                return value;
            }

            public void Push(T value)
            {
                tail++;
                _stack[tail] = value;
            }

            public bool IsEmpty()
            {
                return tail < 0;
            }

            public void Clear()
            {
                tail = -1;
            }
        }

        static void Main(string[] args)
        {
            using (reader = File.OpenText("input.txt"))
            using (writer = new StreamWriter(File.Create("output.txt")))
            {
                int n = reader.ReadInt();
                CustomStack<char> stack = new CustomStack<char>(10001);
                while (n > 0)
                {
                    stack.Clear();
                    string str = reader.ReadToken();
                    bool right = true;
                    for (int i = 0; i < str.Length && right; i++)
                    {
                        switch (str[i])
                        {
                            case '(':
                            case '[':
                                stack.Push(str[i]);
                                break;
                            case ')':
                                if (stack.IsEmpty())
                                {
                                    right = false;
                                }
                                else
                                {
                                    char c = stack.Pop();
                                    right = (c == '(');
                                }
                                break;
                            case ']':
                                if (stack.IsEmpty())
                                {
                                    right = false;
                                }
                                else
                                {
                                    char c = stack.Pop();
                                    right = (c == '[');
                                }
                                break;
                        }
                    }

                    right = right && stack.IsEmpty();

                    writer.WriteLine(right ? "YES" : "NO");
                    n--;
                }
            }
        }
    }
}
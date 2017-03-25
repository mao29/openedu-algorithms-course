using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week4Task6
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
                var stack = new CustomStack<int>(n);
                int operand1 = 0, operand2 = 0;
                while (n > 0)
                {
                    var str = reader.ReadToken();

                    switch (str)
                    {
                        case "*":
                            operand1 = stack.Pop();
                            operand2 = stack.Pop();
                            stack.Push(operand1 * operand2);
                            break;
                        case "-":
                            operand1 = stack.Pop();
                            operand2 = stack.Pop();
                            stack.Push(operand2 - operand1);
                            break;
                        case "+":
                            operand1 = stack.Pop();
                            operand2 = stack.Pop();
                            stack.Push(operand2 + operand1);
                            break;
                        case "/":
                            operand1 = stack.Pop();
                            operand2 = stack.Pop();
                            stack.Push(operand2 / operand1);
                            break;
                        default:
                            stack.Push(int.Parse(str));
                            break;
                    }
                    n--;
                }
                writer.Write(stack.Pop());
            }
        }
    }
}

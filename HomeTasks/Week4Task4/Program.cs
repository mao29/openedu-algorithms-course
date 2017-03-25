using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week4Task4
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

        class LinkedListItem<T>
        {
            public T value;
            public LinkedListItem<T> prev;
            public LinkedListItem<T> next;

            public LinkedListItem(T value)
            {
                this.value = value;
            }
        }

        class CustomSortedList<T> where T : IComparable
        {
            LinkedListItem<T> _head = null;

            public LinkedListItem<T> Add(T value)
            {
                LinkedListItem<T> current = _head;
                LinkedListItem<T> prev = null;
                while (current != null && current.value.CompareTo(value) < 0)
                {
                    prev = current;
                    current = current.next;
                }

                LinkedListItem<T> newItem = new LinkedListItem<T>(value);
                newItem.next = current;
                if (current != null)
                {
                    current.prev = newItem;
                }
                newItem.prev = prev;
                if (prev != null)
                {
                    prev.next = newItem;
                }

                if (current == _head)
                {
                    _head = newItem;
                }
                return newItem;
            }

            public void Remove(LinkedListItem<T> element)
            {
                if (element == _head)
                {
                    _head = _head.next;
                }

                if (element.prev != null)
                {
                    element.prev.next = element.next;
                }

                if (element.next != null)
                {
                    element.next.prev = element.prev;
                }
            }

            public T Min()
            {
                return _head.value;
            }
        }

        class CustomQueue<T> where T : IComparable //where T : IComparable
        {
            int _capacity;
            //LinkedListItem<T>[] _queue;
            //CustomSortedList<T> _sortedList;
            int _head, _tail;
            T[] _queue;

            int _ohead, _otail;
            T[] _orderedQueue;

            //T _min;
            public CustomQueue(int capacity)
            {
                _capacity = capacity;
                //_queue = new LinkedListItem<T>[_capacity];
                //_sortedList = new CustomSortedList<T>();
                _queue = new T[_capacity];
                _orderedQueue = new T[_capacity];
                _tail = 0;
                _head = 0;
                _ohead = 0;
                _otail = 0;
            }

            public void Enqueue(T value)
            {
                //if (IsEmpty())
                //{
                //    _min = value;
                //}
                //if (_min.CompareTo(value) > 0)
                //{
                //    _min = value;
                //}

                if (IsEmpty())
                {
                    _orderedQueue[_otail] = value;
                    _otail = (_otail + 1) % _capacity;
                }
                else
                {
                    int opos = (_otail + _capacity - 1) % _capacity;
                    while (opos != _ohead && _orderedQueue[opos].CompareTo(value) > 0)
                    {
                        opos = (opos + _capacity - 1) % _capacity;
                    }
                    if (opos == _ohead && _orderedQueue[opos].CompareTo(value) > 0)
                    {
                        _orderedQueue[opos] = value;
                        _otail = (opos + 1) % _capacity;
                    }
                    else
                    {
                        opos = (opos + 1) % _capacity;
                        _orderedQueue[opos] = value;
                        _otail = (opos + 1) % _capacity;
                    }
                }
                //else
                //{
                //    int opos = _ohead;
                //    while (opos != _otail)
                //    {
                //        if (_orderedQueue[opos].CompareTo(value) > 0)
                //        {
                //            _orderedQueue[opos] = value;
                //            _otail = (opos + 1) % _capacity;
                //            break;
                //        }
                //        opos = (opos + 1) % _capacity;
                //    }
                //    if (opos == _otail)
                //    {
                //        _orderedQueue[_otail] = value;
                //        _otail = (_otail + 1) % _capacity;
                //    }
                //}

                _queue[_tail] = value;
                _tail = (_tail + 1) % _capacity;
            }

            public T Dequeue()
            {
                T value = _queue[_head];
                _head = (_head + 1) % _capacity;
                if (_orderedQueue[_ohead].CompareTo(value) == 0)
                {
                    _ohead = (_ohead + 1) % _capacity;
                }

                //if (value.CompareTo(_min) == 0)
                //{
                //    _min = FindMin();
                //}
                return value;
            }

            public bool IsEmpty()
            {
                return _head == _tail;
            }

            public T Min()
            {
                return _orderedQueue[_ohead];

                // return _min;
            }

            //private T FindMin()
            //{
            //    int i = _head;
            //    var min = _queue[i];                
            //    while (i != _tail)
            //    {                    
            //        if (min.CompareTo(_queue[i]) > 0)
            //        {
            //            min = _queue[i];
            //        }
            //        i = (i + 1) % _capacity;
            //    }
            //    return min;
            //}

            //public T Dequeue()
            //{
            //    var element = _queue[_head];
            //    _sortedList.Remove(element);
            //    T value = _queue[_head].value;
            //    _head = (_head + 1) % _capacity;
            //    return value;
            //}

            //public void Enqueue(T value)
            //{
            //    var newElement = _sortedList.Add(value);
            //    _queue[_tail] = newElement;
            //    _tail = (_tail + 1) % _capacity;
            //}

            //public T Min()
            //{
            //    return _sortedList.Min();
            //}
        }

        static void Main(string[] args)
        {
            using (reader = File.OpenText("input.txt"))
            using (writer = new StreamWriter(File.Create("output.txt")))
            {
                int m = reader.ReadInt();
                CustomQueue<int> queue = new CustomQueue<int>(m + 1);
                while (m > 0)
                {
                    var cmd = reader.ReadToken();
                    int value;
                    switch (cmd)
                    {
                        case "+":
                            int number = reader.ReadInt();
                            queue.Enqueue(number);
                            break;
                        case "-":
                            value = queue.Dequeue();
                            break;
                        case "?":
                            value = queue.Min();
                            writer.WriteLine(value);
                            break;
                    }
                    m--;
                }

            }
        }
    }
}

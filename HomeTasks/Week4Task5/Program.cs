using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week4Task5
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
        }

        interface ICommand
        {
            void Execute(QuackVM vm, Workflow workflow);
        }

        abstract class QuackCommand : ICommand
        {
            public abstract void Execute(QuackVM vm, Workflow workflow);
        }

        class PlusCommand : QuackCommand
        {
            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int operand1 = vm.Get();
                int operand2 = vm.Get();
                vm.Put((operand1 + operand2) % 65536);
                workflow.GoToNext();
            }

        }

        class MinusCommand : QuackCommand
        {
            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int operand1 = vm.Get();
                int operand2 = vm.Get();
                vm.Put((operand1 - operand2 + 65536) % 65536);
                workflow.GoToNext();
            }

        }

        class MultiplyCommand : QuackCommand
        {
            public override void Execute(QuackVM vm, Workflow workflow)
            {
                long operand1 = vm.Get();
                long operand2 = vm.Get();
                vm.Put((int)((operand1 * operand2) % 65536));
                workflow.GoToNext();
            }

        }

        class DivideCommand : QuackCommand
        {

            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int operand1 = vm.Get();
                int operand2 = vm.Get();
                if (operand2 == 0)
                {
                    vm.Put(0);
                }
                else
                {
                    vm.Put(operand1 / operand2);
                }
                workflow.GoToNext();
            }

        }

        class ModuloCommand : QuackCommand
        {

            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int operand1 = vm.Get();
                int operand2 = vm.Get();
                if (operand2 == 0)
                {
                    vm.Put(0);
                }
                else
                {
                    vm.Put(operand1 % operand2);
                }
                workflow.GoToNext();
            }

        }

        class SetRegisterCommand : QuackCommand
        {
            char _register;
            public SetRegisterCommand(char register)
            {
                _register = register;
            }

            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int operand1 = vm.Get();
                vm.PutRegistry(_register, operand1);
                workflow.GoToNext();
            }
        }

        class GetRegisterCommand : QuackCommand
        {
            char _register;
            public GetRegisterCommand(char register)
            {
                _register = register;
            }

            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int operand1 = vm.GetRegistry(_register);
                vm.Put(operand1);
                workflow.GoToNext();
            }
        }

        class PrintCommand : QuackCommand
        {

            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int operand1 = vm.Get();
                writer.WriteLine(operand1);
                workflow.GoToNext();
            }

        }

        class PrintRegisterCommand : QuackCommand
        {
            char _register;
            public PrintRegisterCommand(char register)
            {
                _register = register;
            }
            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int operand1 = vm.GetRegistry(_register);
                writer.WriteLine(operand1);
                workflow.GoToNext();
            }
        }

        class PrintCharCommand : QuackCommand
        {

            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int operand1 = vm.Get();
                char c = (char)(operand1 % 256);
                writer.Write(c);
                workflow.GoToNext();
            }

        }

        class PrintRegisterCharCommand : QuackCommand
        {
            char _register;
            public PrintRegisterCharCommand(char register)
            {
                _register = register;
            }
            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int operand1 = vm.GetRegistry(_register);
                char c = (char)(operand1 % 256);
                writer.Write(c);
                workflow.GoToNext();
            }
        }

        class LabelCommand : QuackCommand
        {
            string _labelName;
            public LabelCommand(string labelName)
            {
                _labelName = labelName;
            }

            public string LabelName { get { return _labelName; } }


            public override void Execute(QuackVM vm, Workflow workflow)
            {
                workflow.GoToNext();
            }
        }

        class GoToLabelCommand : QuackCommand
        {
            string _labelName;

            public GoToLabelCommand(string labelName)
            {
                _labelName = labelName;
            }

            public override void Execute(QuackVM vm, Workflow workflow)
            {
                workflow.GoToLabel(_labelName);
            }
        }

        class GoToLabelZCommand : QuackCommand
        {
            string _labelName;

            char _register;
            public GoToLabelZCommand(char register, string labelName)
            {
                _labelName = labelName;
                _register = register;
            }

            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int value = vm.GetRegistry(_register);
                if (value == 0)
                {
                    workflow.GoToLabel(_labelName);
                }
                else
                {
                    workflow.GoToNext();
                }
            }
        }

        class GoToLabelECommand : QuackCommand
        {
            string _labelName;

            char _register1, _register2;
            public GoToLabelECommand(char register1, char register2, string labelName)
            {
                _labelName = labelName;
                _register1 = register1;
                _register2 = register2;
            }

            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int value1 = vm.GetRegistry(_register1);
                int value2 = vm.GetRegistry(_register2);
                if (value1 == value2)
                {
                    workflow.GoToLabel(_labelName);
                }
                else
                {
                    workflow.GoToNext();
                }
            }
        }

        class GoToLabelGCommand : QuackCommand
        {
            string _labelName;

            char _register1, _register2;
            public GoToLabelGCommand(char register1, char register2, string labelName)
            {
                _labelName = labelName;
                _register1 = register1;
                _register2 = register2;
            }

            public override void Execute(QuackVM vm, Workflow workflow)
            {
                int value1 = vm.GetRegistry(_register1);
                int value2 = vm.GetRegistry(_register2);
                if (value1 > value2)
                {
                    workflow.GoToLabel(_labelName);
                }
                else
                {
                    workflow.GoToNext();
                }
            }
        }

        class QuitCommand : QuackCommand
        {
            public override void Execute(QuackVM vm, Workflow workflow)
            {
                workflow.Quit();
            }
        }

        class NumberCommand : QuackCommand
        {
            int _number;
            public NumberCommand(int number)
            {
                _number = number;
            }

            public override void Execute(QuackVM vm, Workflow workflow)
            {
                vm.Put(_number);
                workflow.GoToNext();
            }
        }

        class Workflow
        {
            QuackVM _vm;

            ICommand[] _commands;
            int _lastPosition;

            Dictionary<string, int> _labelPositions;

            int _currentPosition;

            public Workflow(QuackVM vm)
            {
                _vm = vm;
                _commands = new ICommand[100001];
                _labelPositions = new Dictionary<string, int>();
                _currentPosition = 0;
                _lastPosition = 0;
            }

            public void AddCommand(ICommand command)
            {
                _commands[_lastPosition] = command;                
                if (command is LabelCommand)
                {
                    _labelPositions.Add((command as LabelCommand).LabelName, _lastPosition);
                }
                _lastPosition++;
            }

            public void GoToLabel(string labelName)
            {
                _currentPosition = _labelPositions[labelName];
            }

            public void GoToNext()
            {
                _currentPosition++;
            }

            public bool IsCompleted()
            {
                return _currentPosition == _lastPosition;
            }

            public void Quit()
            {
                _currentPosition =_lastPosition;
            }

            public void ExecuteCurrentCommand()
            {
                var command = _commands[_currentPosition];
                command.Execute(_vm, this);
            }
        }

        class QuackVM
        {
            CustomQueue<int> _memory = new CustomQueue<int>(100001);

            Dictionary<char, int> _registry = new Dictionary<char, int>();

            public QuackVM()
            {
                for (char c = 'a'; c <= 'z'; c++)
                {
                    _registry[c] = 0;
                }
            }

            public int Get()
            {
                return _memory.Dequeue();
            }

            public void Put(int value)
            {
                _memory.Enqueue(value);
            }

            public int GetRegistry(char registry)
            {
                return _registry[registry];
            }

            public void PutRegistry(char registry, int value)
            {
                _registry[registry] = value;
            }
        }

        static void Main(string[] args)
        {
            using (reader = File.OpenText("input.txt"))
            using (writer = new StreamWriter(File.Create("output.txt")))
            {
                QuackVM vm = new QuackVM();
                Workflow wf = new Workflow(vm);

                while (!reader.EndOfStream)
                {
                    string str = reader.ReadLine();
                    ICommand cmd = null;
                    switch (str[0])
                    {
                        case '+':
                            cmd = new PlusCommand();
                            break;
                        case '-':
                            cmd = new MinusCommand();
                            break;
                        case '*':
                            cmd = new MultiplyCommand();
                            break;
                        case '/':
                            cmd = new DivideCommand();
                            break;
                        case '%':
                            cmd = new ModuloCommand();
                            break;
                        case '>':
                            cmd = new SetRegisterCommand(str[1]);
                            break;
                        case '<':
                            cmd = new GetRegisterCommand(str[1]);
                            break;
                        case 'P':
                            if (str.Length > 1)
                            {
                                cmd = new PrintRegisterCommand(str[1]);
                            }
                            else
                            {
                                cmd = new PrintCommand();
                            }
                            break;
                        case 'C':
                            if (str.Length > 1)
                            {
                                cmd = new PrintRegisterCharCommand(str[1]);
                            }
                            else
                            {
                                cmd = new PrintCharCommand();
                            }
                            break;
                        case ':':
                            cmd = new LabelCommand(str.Substring(1));
                            break;
                        case 'J':
                            cmd = new GoToLabelCommand(str.Substring(1));
                            break;
                        case 'Z':
                            cmd = new GoToLabelZCommand(str[1], str.Substring(2));
                            break;
                        case 'E':
                            cmd = new GoToLabelECommand(str[1], str[2], str.Substring(3));
                            break;
                        case 'G':
                            cmd = new GoToLabelGCommand(str[1], str[2], str.Substring(3));
                            break;
                        case 'Q':
                            cmd = new QuitCommand();
                            break;
                        default:
                            cmd = new NumberCommand(int.Parse(str));
                            break;
                    }
                    wf.AddCommand(cmd);
                }

                while (!wf.IsCompleted())
                {
                    wf.ExecuteCurrentCommand();
                }
            }
        }
    }
}

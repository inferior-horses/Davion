using System.Collections.Generic;
using System.Text;

namespace IR
{
    public interface IOperand
    {
        int Value { get; }
    }

    public class Immdiate : IOperand
    {
        public int Value { get; }

        public Immdiate(int number)
        {
            Value = number;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Variable : IOperand
    {
        public string Name;

        public Dictionary<string, int> SymbolTable;

        public int Value => SymbolTable[Name];

        public Variable(string name, Dictionary<string, int> symbolTable)
        {
            Name = name;
            SymbolTable = symbolTable;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Temporary : IOperand
    {
        public int Value { get; }
        private static int _number = 0;
        private readonly string _name;

        public Temporary(int value)
        {
            Value = value;
            var sb = new StringBuilder();
            sb.Append('t').Append(_number);
            _name = sb.ToString();
            _number += 1;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
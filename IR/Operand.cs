using System.Collections.Generic;

namespace IR
{
    public interface IOperand
    {
        int Value { get; }
    }

    public class Immdiate : IOperand
    {
        public int Value { get; private set; }
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
}
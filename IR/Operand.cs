using System;
using System.Collections.Generic;

namespace IR
{
    public interface IOperand
    {
        long Value { get; }
    }

    public class Immdiate : IOperand
    {
        public long Value { get; private set; }
        public Immdiate(long number)
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

        public Dictionary<string, long> SymbolTable;

        public long Value => SymbolTable[Name];

        public Variable(string name, Dictionary<string, long> symbolTable)
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
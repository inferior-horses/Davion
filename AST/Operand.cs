using System;
using System.Collections.Generic;

namespace AST
{
    public interface IOperand
    {
        Int64 Value { get; }
    }

    public class Immdiate : IOperand
    {
        public Int64 Value { get; private set; }
        public Immdiate(Int64 number)
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
        public String Name;

        public Dictionary<String, Int64> SymbolTable;

        public Int64 Value => SymbolTable[Name];

        public Variable(String name, Dictionary<String, Int64> symbolTable)
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
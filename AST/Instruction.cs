using System;
using System.Collections.Generic;

namespace AST
{
    public abstract class Instruction
    {
        public Opcode Opcode;
        public IOperand[] Operands;

        public string MakeString(string spliter)
        {
            string str = "";
            for (int i = 0; i < Operands.Length; ++i)
            {
                if (i > 0)
                {
                    str += spliter;
                }

                str += Operands[i];
            }

            return str;
        }
    }

    public class Neg : Instruction
    {
        public Neg(IOperand first, IOperand second)
        {
            Opcode = Opcode.Neg;
            Operands = new IOperand[] {first};
        }

        public override string ToString()
        {
            return "Neg: " + MakeString(", ");
        }
    }

    public class Add : Instruction
    {
        public Add(IOperand first, IOperand second)
        {
            Opcode = Opcode.Add;
            Operands = new IOperand[] {first, second};
        }

        public override string ToString()
        {
            return "Add: " + MakeString(", ");
        }
    }

    public class Sub : Instruction
    {
        public Sub(IOperand first, IOperand second)
        {
            Opcode = Opcode.Sub;
            Operands = new IOperand[] {first, second};
        }

        public override string ToString()
        {
            return "Sub: " + MakeString(", ");
        }
    }

    public class Mul : Instruction
    {
        public Mul(IOperand first, IOperand second)
        {
            Opcode = Opcode.Mul;
            Operands = new IOperand[] {first, second};
        }

        public override string ToString()
        {
            return "Mul: " + MakeString(", ");
        }
    }

    public class Div : Instruction
    {
        public Div(IOperand first, IOperand second)
        {
            Opcode = Opcode.Div;
            Operands = new IOperand[] {first, second};
        }

        public override string ToString()
        {
            return "Div: " + MakeString(", ");
        }
    }

    public class Cmp : Instruction
    {
        public Cmp(IOperand first, IOperand second)
        {
            Opcode = Opcode.Cmp;
            Operands = new IOperand[] {first, second};
        }

        public override string ToString()
        {
            return "Cmp: " + MakeString(", ");
        }
    }

    public class Adda : Instruction
    {
        public Adda(IOperand first, IOperand second)
        {
            Opcode = Opcode.Adda;
            Operands = new IOperand[] {first, second};
        }

        public override string ToString()
        {
            return "Adda: " + MakeString(", ");
        }
    }

    public class Load : Instruction
    {
        public Load(IOperand first)
        {
            Opcode = Opcode.Load;
            Operands = new IOperand[] {first};
        }

        public override string ToString()
        {
            return "Load: " + MakeString(", ");
        }
    }

    public class Store : Instruction
    {
        public Store(IOperand first, IOperand second)
        {
            Opcode = Opcode.Store;
            Operands = new IOperand[] {first, second};
        }

        public override string ToString()
        {
            return "Store: " + MakeString(", ");
        }
    }

    public class Move : Instruction
    {
        public Move(IOperand first, IOperand second)
        {
            Opcode = Opcode.Move;
            Operands = new IOperand[] {first, second};
        }

        public override string ToString()
        {
            return "Move: " + MakeString(", ");
        }
    }

    public class Phi : Instruction
    {
        public Phi(List<IOperand> operands)
        {
            Opcode = Opcode.Phi;
            Operands = operands.ToArray();
        }

        public override string ToString()
        {
            return "Phi: " + MakeString(", ");
        }
    }
}
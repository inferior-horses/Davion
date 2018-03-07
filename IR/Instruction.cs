using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IR
{
    public abstract class Instruction
    {
        public Opcode Opcode;
        public readonly IOperand[] Operands;
        public int Address;

        public Instruction(IOperand[] operands)
        {
            Operands = operands;
        }

        public string MakeString(string prefix, string spliter)
        {
            var strings = Operands.Select(x => x.ToString());
            StringBuilder sb = new StringBuilder(prefix, prefix.Length + strings.Sum(x => x.Length));
            foreach (var operand in Operands)
            {
                sb.Append(operand);
            }

            return sb.ToString();
        }

        public bool IsExpression()
        {
            switch (Opcode)
            {
                case Opcode.Neg:
                case Opcode.Add:
                case Opcode.Sub:
                case Opcode.Mul:
                case Opcode.Div: return true;
                default:
                    return false;
            }
        }
    }

    public class Neg : Instruction
    {
        public Neg(IOperand x) : base(new[] {x})
        {
            Opcode = Opcode.Neg;
        }

        public override string ToString()
        {
            return MakeString("Neg: ", ", ");
        }

        public override bool Equals(object obj)
        {
            if (obj is Neg rhs)
            {
                return rhs.Operands.Equals(Operands);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Operands[0].GetHashCode();
        }
    }

    public class Add : Instruction
    {
        public Add(IOperand x, IOperand y) : base(new[] {x, y})
        {
            Opcode = Opcode.Add;
        }

        public override string ToString()
        {
            return MakeString("Add: ", ", ");
        }

        public override bool Equals(object obj)
        {
            if (obj is Add rhs)
            {
                return rhs.Operands.Equals(Operands) ||
                       (rhs.Operands[0].Equals(Operands[1]) &&
                        rhs.Operands[1].Equals(Operands[0]));
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Operands[0].GetHashCode() ^ Operands[1].GetHashCode();
        }
    }

    public class Sub : Instruction
    {
        public Sub(IOperand x, IOperand y) : base(new[] {x, y})
        {
            Opcode = Opcode.Sub;
        }

        public override string ToString()
        {
            return MakeString("Sub: ", ", ");
        }

        public override bool Equals(object obj)
        {
            if (obj is Sub rhs)
            {
                return rhs.Operands.Equals(Operands);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Operands[0].GetHashCode() - Operands[1].GetHashCode();
            }
        }
    }

    public class Mul : Instruction
    {
        public Mul(IOperand x, IOperand y) : base(new[] {x, y})
        {
            Opcode = Opcode.Mul;
        }

        public override string ToString()
        {
            return MakeString("Mul: ", ", ");
        }

        public override bool Equals(object obj)
        {
            if (obj is Mul rhs)
            {
                return rhs.Operands.Equals(Operands) ||
                       (rhs.Operands[0].Equals(Operands[1]) &&
                        rhs.Operands[1].Equals(Operands[0]));
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Operands[0].GetHashCode() + Operands[1].GetHashCode();
            }
        }
    }

    public class Div : Instruction
    {
        public Div(IOperand x, IOperand y) : base(new[] {x, y})
        {
            Opcode = Opcode.Div;
        }

        public override string ToString()
        {
            return MakeString("Div: ", ", ");
        }

        public override bool Equals(object obj)
        {
            if (obj is Div rhs)
            {
                return rhs.Operands.Equals(Operands);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Operands[1].GetHashCode() - Operands[0].GetHashCode();
            }
        }
    }

    public class Cmp : Instruction
    {
        public Cmp(IOperand x, IOperand y) : base(new[] {x, y})
        {
            Opcode = Opcode.Cmp;
        }

        public override string ToString()
        {
            return MakeString("Cmp: ", ", ");
        }
    }

    public class Adda : Instruction
    {
        public Adda(IOperand x, IOperand y) : base(new[] {x, y})
        {
            Opcode = Opcode.Adda;
        }

        public override string ToString()
        {
            return MakeString("Adda: ", ", ");
        }
    }

    public class Load : Instruction
    {
        public Load(IOperand x) : base(new[] {x})
        {
            Opcode = Opcode.Load;
        }

        public override string ToString()
        {
            return MakeString("Load: ", ", ");
        }
    }

    public class Store : Instruction
    {
        public Store(IOperand x, IOperand y) : base(new[] {x, y})
        {
            Opcode = Opcode.Store;
        }

        public override string ToString()
        {
            return MakeString("Store: ", ", ");
        }
    }

    public class Move : Instruction
    {
        public Move(IOperand x, IOperand y) : base(new[] {x, y})
        {
            Opcode = Opcode.Move;
        }

        public override string ToString()
        {
            return MakeString("Move: ", ", ");
        }
    }

    public class Phi : Instruction
    {
        public Phi(List<IOperand> operands) : base(operands.ToArray())
        {
            Opcode = Opcode.Phi;
        }

        public override string ToString()
        {
            return MakeString("Phi: ", ", ");
        }
    }

    public class End : Instruction
    {
        public End() : base(new IOperand[] { })
        {
            Opcode = Opcode.End;
        }

        public override string ToString()
        {
            return MakeString("End: ", ", ");
        }
    }

    public class Bra : Instruction
    {
        public Bra(Immdiate immdiate) : base(new IOperand[] {immdiate})
        {
            Opcode = Opcode.Bra;
        }

        public Bra(int target)
            : this(new Immdiate(target))
        {
        }
    }

    public class Bne : Instruction
    {
        public Bne(IOperand x, Immdiate y) : base(new[] {x, y})
        {
            Opcode = Opcode.Bne;
        }

        public Bne(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Beq : Instruction
    {
        public Beq(IOperand x, Immdiate y) : base(new[] {x, y})
        {
            Opcode = Opcode.Beq;
        }

        public Beq(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Ble : Instruction
    {
        public Ble(IOperand x, Immdiate y) : base(new[] {x, y})
        {
            Opcode = Opcode.Ble;
        }

        public Ble(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Blt : Instruction
    {
        public Blt(IOperand x, Immdiate y) : base(new[] {x, y})
        {
            Opcode = Opcode.Blt;
        }

        public Blt(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Bge : Instruction
    {
        public Bge(IOperand x, Immdiate y) : base(new[] {x, y})
        {
            Opcode = Opcode.Bge;
        }

        public Bge(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Bgt : Instruction
    {
        public Bgt(IOperand x, Immdiate y) : base(new[] {x, y})
        {
            Opcode = Opcode.Bgt;
        }

        public Bgt(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Read : Instruction
    {
        public Read() : base(new IOperand[] { })
        {
            Opcode = Opcode.Read;
        }
    }

    public class Write : Instruction
    {
        public Write(IOperand x) : base(new[] {x})
        {
            Opcode = Opcode.Write;
        }
    }

    public class WriteNL : Instruction
    {
        public WriteNL() : base(new IOperand[] { })
        {
            Opcode = Opcode.WriteNL;
        }
    }

    public sealed class Nop : Instruction
    {
        private Nop() : base(new IOperand[] { })
        {
            Opcode = Opcode.Nop;
        }

        public static Nop Instance { get; } = new Nop();
    }
}
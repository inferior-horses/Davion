using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IR
{
    
    public abstract class Instruction
    {
        public Opcode Opcode;
        public IOperand[] Operands;
        public int Address;

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
    }

    public class Neg : Instruction
    {
        public Neg(IOperand x)
        {
            Opcode = Opcode.Neg;
            Operands = new IOperand[] {x};
        }

        public override string ToString()
        {
            return MakeString("Neg: ", ", ");
        }
    }

    public class Add : Instruction
    {
        public Add(IOperand x, IOperand y)
        {
            Opcode = Opcode.Add;
            Operands = new IOperand[] {x, y};
        }

        public override string ToString()
        {
            return MakeString("Add: ", ", ");
        }
    }

    public class Sub : Instruction
    {
        public Sub(IOperand x, IOperand y)
        {
            Opcode = Opcode.Sub;
            Operands = new IOperand[] {x, y};
        }

        public override string ToString()
        {
            return MakeString("Sub: ", ", ");
        }
    }

    public class Mul : Instruction
    {
        public Mul(IOperand x, IOperand y)
        {
            Opcode = Opcode.Mul;
            Operands = new IOperand[] {x, y};
        }

        public override string ToString()
        {
            return MakeString("Mul: ", ", ");
        }
    }

    public class Div : Instruction
    {
        public Div(IOperand x, IOperand y)
        {
            Opcode = Opcode.Div;
            Operands = new IOperand[] {x, y};
        }

        public override string ToString()
        {
            return MakeString("Div: ", ", ");
        }
    }

    public class Cmp : Instruction
    {
        public Cmp(IOperand x, IOperand y)
        {
            Opcode = Opcode.Cmp;
            Operands = new IOperand[] {x, y};
        }

        public override string ToString()
        {
            return MakeString("Cmp: ", ", ");
        }
    }

    public class Adda : Instruction
    {
        public Adda(IOperand x, IOperand y)
        {
            Opcode = Opcode.Adda;
            Operands = new IOperand[] {x, y};
        }

        public override string ToString()
        {
            return MakeString("Adda: ", ", ");
        }
    }

    public class Load : Instruction
    {
        public Load(IOperand x)
        {
            Opcode = Opcode.Load;
            Operands = new IOperand[] {x};
        }

        public override string ToString()
        {
            return MakeString("Load: ", ", ");
        }
    }

    public class Store : Instruction
    {
        public Store(IOperand x, IOperand y)
        {
            Opcode = Opcode.Store;
            Operands = new IOperand[] {x, y};
        }

        public override string ToString()
        {
            return MakeString("Store: ", ", ");
        }
    }

    public class Move : Instruction
    {
        public Move(IOperand x, IOperand y)
        {
            Opcode = Opcode.Move;
            Operands = new IOperand[] {x, y};
        }

        public override string ToString()
        {
            return MakeString("Move: ", ", ");
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
            return MakeString("Phi: ", ", ");
        }
    }

    public class End : Instruction
    {
        public End()
        {
            Opcode = Opcode.End;
            Operands = new IOperand[] { };
        }

        public override string ToString()
        {
            return MakeString("End: ", ", ");
        }
    }

    public class Bra : Instruction
    {
        public Bra(Immdiate immdiate)
        {
            Opcode = Opcode.Bra;
            Operands = new IOperand[] {immdiate};
        }

        public Bra(int target)
            : this(new Immdiate(target))
        {
        }
    }

    public class Bne : Instruction
    {
        public Bne(IOperand x, Immdiate y)
        {
            Opcode = Opcode.Bne;
            Operands = new IOperand[] {x, y};
        }

        public Bne(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Beq : Instruction
    {
        public Beq(IOperand x, Immdiate y)
        {
            Opcode = Opcode.Beq;
            Operands = new IOperand[] {x, y};
        }

        public Beq(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Ble : Instruction
    {
        public Ble(IOperand x, Immdiate y)
        {
            Opcode = Opcode.Ble;
            Operands = new IOperand[] {x, y};
        }

        public Ble(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Blt : Instruction
    {
        public Blt(IOperand x, Immdiate y)
        {
            Opcode = Opcode.Blt;
            Operands = new IOperand[] {x, y};
        }

        public Blt(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Bge : Instruction
    {
        public Bge(IOperand x, Immdiate y)
        {
            Opcode = Opcode.Bge;
            Operands = new IOperand[] {x, y};
        }

        public Bge(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Bgt : Instruction
    {
        public Bgt(IOperand x, Immdiate y)
        {
            Opcode = Opcode.Bgt;
            Operands = new IOperand[] {x, y};
        }

        public Bgt(IOperand x, int y)
            : this(x, new Immdiate(y))
        {
        }
    }

    public class Read : Instruction
    {
        public Read()
        {
            Opcode = Opcode.Read;
            Operands = new IOperand[] { };
        }
    }

    public class Write : Instruction
    {
        public Write(IOperand x)
        {
            Opcode = Opcode.Write;
            Operands = new IOperand[] {x};
        }
    }

    public class WriteNL : Instruction
    {
        public WriteNL()
        {
            Opcode = Opcode.WriteNL;
            Operands = new IOperand[] { };
        }
    }
}
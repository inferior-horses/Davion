using System.Collections.Generic;
using System.Text;

namespace IR
{
    public interface IOperand
    {
    }

    public class Immdiate : IOperand
    {
        public readonly int Value;

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
        public readonly string Name;
        private static int _number = 0;

        public static Variable GetTemporary()
        {
            var sb = new StringBuilder();
            sb.Append('t').Append(_number);
            _number += 1;
            return new Variable(sb.ToString());
        }

        public Variable(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
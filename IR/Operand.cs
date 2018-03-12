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

        public override bool Equals(object obj)
        {
            if (obj is Immdiate rhs)
            {
                return rhs.Value == Value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    public class Variable : IOperand
    {
        public readonly string Name;

        // actaully have id2name mapping in scanner
        public Variable(string name)
        {
            Name = name;
        }

        public Variable(int number)
        {
            var sb = new StringBuilder();
            sb.Append('t').Append(number);
            Name = sb.ToString();
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is Variable rhs)
            {
                return rhs.Name.Equals(Name);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
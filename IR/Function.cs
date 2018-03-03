using System.Collections.Generic;

namespace IR
{
    public class Function
    {
        public List<Block> Blocks;
        public List<Instruction> Instructions;

        public Function()
        {
            Blocks = new List<Block>();
            Instructions = new List<Instruction>();
        }
    }
}
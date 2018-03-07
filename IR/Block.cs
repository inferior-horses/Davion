using System.Collections;
using System.Collections.Generic;

namespace IR
{
    public class Block : IEnumerable<Instruction>
    {
        public int Begin { get; }
        public int End { get; }
        private List<Instruction> Instructions;
        public Block[] Pred;
        public Block[] Succ;

        public Block(List<Instruction> instructions, int begin, int end, Block[] pred = null, Block[] succ = null)
        {
            Instructions = instructions;
            Begin = begin;
            End = end;
            Pred = pred;
            Succ = succ;
        }

        public IEnumerator<Instruction> GetEnumerator()
        {
            for (var i = Begin; i < End; ++i)
            {
                yield return Instructions[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // AppendInstruction(int ins_id)
    }
}
using System.Collections;
using System.Collections.Generic;

namespace IR
{
    public class Block : IEnumerable<Instruction>
    {
        private long Begin;
        private long End;
        private Instruction[] Instructions;
        public Block[] Pred;
        public Block[] Succ;

        public Block(Instruction[] instructions, long begin, long end, Block[] pred = null, Block[] succ = null)
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
    }
}
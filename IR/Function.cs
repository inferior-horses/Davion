using System.Collections.Generic;

namespace IR
{
    public class Function
    {
        public class MappedValue
        {
            public int FuncAddr { get; set; }
            public int FuncId { get; set; } // function index in function list of main(may be no need for function list)
        }
        public List<Block> Blocks;
        public List<Instruction> Instructions;
        public List<IOperand> Results;

        public Dictionary<int, int> FuncAddrTable; // identifier_id, func_addr

        private int next_block_start_;

        public Function()
        {
            Blocks = new List<Block>();
            Instructions = new List<Instruction>();
            FuncAddrTable = new Dictionary<int, int>();
            next_block_start_ = 0;
        }

        public int AddInstruction(Instruction instru)
        {
            Instructions.Add(instru);
            return Instructions.Count - 1;
        }

        // TODO: appending may not be the only way to add block
        public int AppendBlock(){
            Blocks.Add(new Block(Instructions, next_block_start_, Instructions.Count - 1));
            next_block_start_ = Instructions.Count;
            //Instructions.Add(new Block(...));
            return Blocks.Count - 1;
        }

        // public int AddIdentifier(int id, TYPE type)
        public int AddIdentifier(int id, IdType type, int array_capacity){
            // Identifier_table
            return 0;
        }

        public int AddFunctionAddr(int id, FuncType type, int func_addr){
            FuncAddrTable.Add(id, func_addr);
            return FuncAddrTable.Count - 1;
        }

        public int GetNextInstruAddr(){
            return Instructions.Count;
        }

        public bool ModifyInstruction(int instru_addr, Instruction new_instru){
            if(instru_addr >= 0 && instru_addr < Instructions.Count){
                Instructions[instru_addr] = new_instru;
                return true;
            }

            return false;
        }
    }

    public enum IdType{
        Var,
        Array
    }

    public enum FuncType
    {
        Func,
        Proc
    }
}
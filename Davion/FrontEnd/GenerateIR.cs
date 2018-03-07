using System;
using System.Collections.Generic;
using System.Text;
using IR.Block
using IR.Operand
using IR.Instructions
namespace Davion.FrontEnd
{
	// Class GenerateIR{}

    public void GenerateIR(Function fuction, int block_id, Opcode op, IOperand[] oprands, out IOperand result){
    	result = Variable.GetTemporary();
    	Instruction instruction;
		switch (op)
            {
                case Opcode.Neg:{
                	instruction = new Neg(oprands[0]);
                	break;
                }
                case Opcode.Add:{
                	instruction = new Add(oprands[0], oprands[1]);
                	break;
                }
                case Opcode.Sub:{
                	instruction = new Sub();
                	break;
                }
                case Opcode.Mul:{
                	instruction = new Mul();
                	break;
                }
                case Opcode.Div:{
                	instruction = new Div();
                	break;
                }
                default:
                    return false;
            }

    	Function.Instructions.Add(instruction);
    	Function.Results.Add(result);
    	Blocks[block_id].AppendInstruction(Instructions.Length() - 1);
    }
}

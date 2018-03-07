using System.Collections.Generic;
using IR;

namespace Optimization
{
    public class CseAndCP : Pass
    {
        public void Opt(Function function)
        {
            List<Instruction> instructions = function.Instructions;
            var expressionTable = new Dictionary<Instruction, Variable>();
            var variableTable = new Dictionary<Variable, Variable>();
            for (int i = 0, l = instructions.Count; i < l; ++i)
            {
                Instruction instruction = instructions[i];
                for (int j = 0, e = instruction.Operands.Length; j < e; ++j)
                {
                    IOperand operand = instruction.Operands[j];
                    if (operand is Variable v)
                    {
                        if (variableTable.TryGetValue(v, out Variable old))
                        {
                            instruction.Operands[j] = old;
                        }
                    }
                }

                if (instruction.IsExpression())
                {
                    int num = instruction.Address;
                    if (expressionTable.TryGetValue(instruction, out Variable temp))
                    {
                        variableTable[new Variable(num)] = temp;
                        instructions[i] = Nop.Instance;
                    }
                    else
                    {
                        expressionTable[instruction] = new Variable(num);
                    }
                }
            }
        }
    }
}
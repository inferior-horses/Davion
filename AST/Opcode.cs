﻿namespace AST
{
    public enum Opcode
    {
        Neg,
        Add,
        Sub,
        Mul,
        Div,
        Cmp,
        Adda,
        Load,
        Store,
        Move,
        Phi,
        End,
        Bra,
        Bne,
        Beq,
        Ble,
        Blt,
        Bge,
        Bgt,
        Read,
        Write,
        WriteNL
    }
}
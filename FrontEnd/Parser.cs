using System;
using System.Collections.Generic;
using System.Text;
using IR;
using FrontEnd;

namespace FrontEnd
{
    public class Parser
    {
        private Scanner scanner_;
        private Tokens scanner_sym_;
        static public List<Function> Functions; // Todo : Do I Need during optimization?

        public Parser(string file_name) // opoen file and scan the first token into .input_sym_
        {
            this.scanner_ = new Scanner(file_name);
        }

        private void Next()
        {
            scanner_sym_ = scanner_.GetSym(); // advance to the next token
        }
        private Tokens PreFetchSym()
        {
            return scanner_.PreFetchSym();
        }

        public void Error(string error_msg)
        {
            Console.Error.WriteLine("-- Parser ERROR: {0} -- ", 
                error_msg);
        }

        public bool EatToken(Tokens token_enum)
        {
            Next();
            if(scanner_sym_ != token_enum){
                Error("Expected " + Enum.GetName(typeof(Tokens), token_enum));
                return false;
            }
            return true;
        }

        // TODO: How do I made a function call and go branch, how do I add function to indentifier table.
        public int AddFunction(int func_id){
            Functions.Add(new Function());
            return Functions.Count - 1;
        }

        /*
            -------------------------------------------
            computation = “main” { varDecl } { funcDecl } “{” statSequence “}” “.” 
            create function : main
        */
        public bool MainComputation()
        {
            if(!EatToken(Tokens.kMainToken)){
                return false;
            }

            int fun_id = AddFunction(-1);

            // Next(); // look forward into VarDecl
            while(VarDecl(Functions[fun_id]) != false)
            {
            }

            while(FuncDecl(Functions[fun_id]) != false)
            {
            }

            if(!EatToken(Tokens.kBeginToken)){
                return false;
            }

            if(StatSequence (Functions[fun_id]) == false){
                Error("Expected StatSequence");
                return false;
            }

            if(!EatToken(Tokens.kEndToken)){
                return false;
            }

            if(!EatToken(Tokens.kPeriodToken)){
                return false;
            }

            return true;

            // GENERATE IR here;
            
            // for main, should end with no word
        }


        /*
            -------------------------------------------
        */
        /*
            varDecl  =  typeDecl ident { “,” ident } “;” 
        */
        public bool VarDecl(Function function)
        {
            if(TypeDecl(function) == false){// TODO: deal with the ARRAY
                Error("Expected TypeDecl");
                return false;
            }

            if(!EatToken(Tokens.kIdent)){
                return false;
            }

            while(PreFetchSym() == Tokens.kCommaToken){
                Next(); // eat

                if(!EatToken(Tokens.kIdent)){
                    return false;
                }
            }

            if(!EatToken(Tokens.kSemiToken)){
                return false;
            }
            return true;
        }

        /*
             funcDecl  =  (“function” | “procedure”) ident [formalParam] “;” funcBody “;”
             create function : the func decled
        */
        // TODO: add function decl into Identifier_table
        public bool FuncDecl(Function function)
        {
            if(PreFetchSym() != Tokens.kFuncToken && PreFetchSym() != Tokens.kProcToken){
                Error("Expected FuncToken or ProcToken");
                return false;
            }
            FuncType func_type = PreFetchSym() == Tokens.kFuncToken ? FuncType.Func : FuncType.Proc;
            Next();

            // TODO: Is GetNextInstruAddr the right function adress?
            if(!EatToken(Tokens.kIdent)){
                return false;
            }
            function.AddFunctionAddr(scanner_.Id, func_type, function.GetNextInstruAddr());// Is the adress right?
            //function.AddIdentifier(scanner_.Id, func_type, function.GetNextInstruAddr()); 

            if(PreFetchSym() != Tokens.kSemiToken){
                if(FormalParam(function) == false){ // TODO: pass in type: func/proc
                    Error("Expected FormalParam");
                    return false;
                }
            }

            if(!EatToken(Tokens.kSemiToken)){
                return false;
            }

            if(FuncBody(function) == false){
                Error("Expected FuncBody");
                return false;
            }

            if(!EatToken(Tokens.kSemiToken)){
                return false;
            }

            // WRITE IDENT TABLE (for outer function)
            return true;
        }
        /* 
            typeDecl  =  “var” | “array” “[“ number “]” { “[“ number “]”} 
        */
        // TODO: return the TYPE
        // TODO: Deal with The Array
        public bool TypeDecl(Function function)
        {
            // Next();
            if(PreFetchSym() == Tokens.kArrayToken)
            {
                Next();

                if(!EatToken(Tokens.kOpenbracketToken)){
                    return false;
                }
                if(!EatToken(Tokens.kNumber)){
                    return false;
                }
                if(!EatToken(Tokens.kClosebracketToken)){
                    return false;
                }

                // higher dimension
                while(PreFetchSym() == Tokens.kOpenbracketToken)
                {
                    Next();
                    if(!EatToken(Tokens.kNumber)){
                        return false;
                    }

                    if(!EatToken(Tokens.kClosebracketToken)){
                        return false;
                    }
                }
            }
            else if(PreFetchSym() == Tokens.kVarToken){
                Next();
                // do sth.
            }
            else{
                Error("Expected ArrayToken or VarToken");
                return false;
            }

            return true;
        }
        /* 
            formalParam  = “(“[ident { “,” ident }] “)”
        */
        public bool FormalParam(Function function)
        {
            if(PreFetchSym() != Tokens.kOpenparenToken){
                return false;
            }

            if(!EatToken(Tokens.kOpenparenToken)){
                return false;
            }

            // TODO: declare function params?
            if(PreFetchSym() == Tokens.kIdent){
                Next();

                while(PreFetchSym() == Tokens.kCommaToken){
                    Next();// Eat Comma

                    if(!EatToken(Tokens.kIdent)){
                        return false;
                    }
                }
            }

            if(!EatToken(Tokens.kCloseparenToken)){
                return false;
            }


            return true;            
        }

        /* 
            funcBody  =  { varDecl } “{” [ statSequence ] “}”
        */
        public bool FuncBody(Function function)
        {
            while(VarDecl(function) != false){
                // eat {varDecl}, do sth.
            }

            if(!EatToken(Tokens.kBeginToken)){
                return false;
            }

            if(PreFetchSym() != Tokens.kEndToken){
                while(StatSequence(function) != false){
                    // eat{statSequence}
                }
            }

            if(!EatToken(Tokens.kEndToken)){
                return false;
            }

            return true;
        }

        /*
            -------------------------------------------
        */
        /*
            statSequence  =  statement { “;” statement }
        */
        public bool StatSequence(Function function)
        {
            if(Statement(function) == false){
                Error("Expected Statement");
                return false;
            }

            while(PreFetchSym() == Tokens.kSemiToken){
                // eat
                Next();

                if(Statement(function) == false){
                    Error("Expected Statement");
                    return false;
                }
            }

            return true;
        }
        /*
             statement = assignment | funcCall | ifStatement | whileStatement | returnStatement
        */
        public bool Statement(Function function)
        {
            if(Assignment(function)){

            }
            // FuncCall(function, out IOperand func_result)
            else if (FuncCall(function)){

            }
            else if(IfStatement(function)){

            }
            else if (WhileStatement(function)){

            }
            else if (ReturnStatement(function)){

            }
            else{
                Error("Expected Assignment or FuncCall or any other Statement");
                return false;
            }

            return true;
        }


        /*
            -------------------------------------------             
        */
        /*
             assignment  =  “let” designator “<-” expression
        */
        public bool Assignment(Function function)
        {
            if(PreFetchSym() != Tokens.kLetToken){
                return false;
            }

            // Variable dest;
            // IOperand source;
            if(!EatToken(Tokens.kLetToken)){
                return false;
            }

            if(Designator(function, out IOperand dest) == false){
                Error("Expected Designator");
                return false;
            }

            if(Expression(function, out IOperand source) == false){
                Error("Expected Expression");
                return false;
            }

            function.AddInstruction(new Move(source, dest));

            return true;
        }
        /*
             funcCall  =  “call” ident [ “(“ [expression { “,” expression } ] “)” ]
        */
        // TODO: Instructions that set the params();
        // TODO: Ioperad func_result and Return Statement(Returned Expresstion is func_result)
        public bool FuncCall(Function function, out IOperand func_result)
        {
            func_result = null;

            if(PreFetchSym() != Tokens.kCallToken){
                return false;
            }

            if(!EatToken(Tokens.kCallToken)){
                return false;
            }

            if(!EatToken(Tokens.kIdent)){
                return false;
            }
            int id_func = scanner_.Id;

            if(PreFetchSym() == Tokens.kOpenparenToken){
                Next(); // eat "("

                if(PreFetchSym() != Tokens.kCloseparenToken){ // assume expression won't begin with "")", due to EBNF
                    // Expression(function, out IOperand param)
                    if (Expression() == false){
                        Error("Expected Expression");
                        return false;
                    }

                    while(PreFetchSym() == Tokens.kCommaToken){
                        Next(); // eat

                        // Expression(function, out IOperand extra_param)
                        if (Expression() == false){
                            Error("Expected Expression");
                            return false;
                        }
                    }
                }

                if(!EatToken(Tokens.kCloseparenToken)){
                    return false;
                }
                
            }

            // Important: one function is enough! when you call, just go branch,
            // use function table to go branch:
            var func_addr_num = new Immdiate(function.FuncAddrTable[id_func]);
            int result_addr = function.AddInstruction(new Bra(func_addr_num));

            // return will return result here
            // how to pass this result_addr? during param set we can set a variable for return_addr
            func_result = new Variable(result_addr);
            return true;
        } 
        /*
            ifStatement  =  “if” relation “then” statSequence [ “else” statSequence ] “fi”             
        */
        public bool IfStatement(Function function)
        {
            if(PreFetchSym() != Tokens.kIfToken){
                return false;
            }

            if(!EatToken(Tokens.kIfToken)){
                return false;
            }

            // then branch according to rel_result
            if(Relation(function, out Tokens rel_op ,out IOperand rel_result) == false){
                Error("Expected Relation");
                return false;
            }

            if(!EatToken(Tokens.kThenToken)){
                return false;
            }

            // modify later
            int instru_addr_go_notif = function.AddInstruction(Nop.Instance);

            if(StatSequence(function) == false){
                Error("Expected StatSequence");
                return false;
            }

            // modify later
            int instru_addr_goout = function.AddInstruction(Nop.Instance);

            // modify
            // IOperand go_notif_instru;
            // var notif_addr = new Immediate(function.GetNextInstruAddr()); // next Instruction is else entry
            // switch(rel_op){
            //     case (Tokens.kEqlToken): go_notif_instru = new Bne(rel_result, notif_addr); break;
            //     case (Tokens.kNeqToken): go_notif_instru = new Beq(rel_result, notif_addr); break;
            //     case (Tokens.kIssToken): go_notif_instru = new Bge(rel_result, notif_addr); break;
            //     case (Tokens.kGeqToken): go_notif_instru = new Blt(rel_result, notif_addr); break;
            //     case (Tokens.kLeqToken): go_notif_instru = new Bgt(rel_result, notif_addr); break;
            //     case (Tokens.kGtrToken): go_notif_instru = new Ble(rel_result, notif_addr); break;
            //     default: go_notif_instru = new Nop();
            // }
            GenBranchInstru(rel_op, rel_result, function.GetNextInstruAddr(), out Instruction go_notif_instru);
            function.ModifyInstruction(instru_addr_go_notif, go_notif_instru);

            if(PreFetchSym() == Tokens.kElseToken){
                Next(); // eat else

                if(StatSequence(function) == false){
                    Error("Expected StatSequence");
                    return false;
                }
            }

            var out_addr = new Immdiate(function.GetNextInstruAddr()); // next Instruction is else entry
            Instruction go_out_instru = new Bra(out_addr);
            function.ModifyInstruction(instru_addr_goout, go_out_instru);

            if(!EatToken(Tokens.kFiToken)){
                return false;
            }

            return true;
        }

        /*
             whileStatement  =  “while” relation “do” StatSequence “od”
        */
        public bool WhileStatement(Function function)
        {
            if(PreFetchSym() != Tokens.kWhileToken){
                return false;
            }

            if(!EatToken(Tokens.kWhileToken)){
                return false;
            }

            // then branch according to rel_result
            if(Relation(function, out Tokens rel_op ,out IOperand rel_result) == false){
                Error("Expected Relation");
                return false;
            }
            
            // +2 line is the dest addr. that is skipping the goout instruction
            GenBranchInstru(rel_op, rel_result, function.GetNextInstruAddr() + 1, out Instruction go_whilebody_instru);
            function.AddInstruction(go_whilebody_instru);

            // modify bra later
            int goout_instru_addr = function.AddInstruction(Nop.Instance);

            if(!EatToken(Tokens.kDoToken)){
                return false;
            }

            if(StatSequence(function) == false){
                Error("Expected StatSequence");
                return false;
            }

            function.ModifyInstruction(goout_instru_addr, new Bra(new Immdiate(function.GetNextInstruAddr())));

            if(!EatToken(Tokens.kOdToken)){
                return false;
            }

            return true;
        } 
        /*
             returnStatement  =  “return” [ expression ] 
        */
        // save return result somewhere
        public bool ReturnStatement(Function function)
        {
            if(PreFetchSym() != Tokens.kReturnToken){
                return false;
            }

            if(!EatToken(Tokens.kReturnToken)){
                return false;
            }

            if(PreFetchSym() == Tokens.kCallToken || 
                PreFetchSym() == Tokens.kOpenparenToken || 
                PreFetchSym() == Tokens.kIdent || 
                PreFetchSym() == Tokens.kNumber)
            {
                if(Expression() == false){
                    Error("Expected Expression");
                    return false;
                }
            }

            return true;
        }


        /*
            -------------------------------------------             
        */
        /*
             designator = ident{ "[" expression "]" }
        */
        public bool Designator(Function function,  out IOperand variable)
        {
            variable = null;

            if(PreFetchSym() != Tokens.kIdent){
                return false;
            }

            if(!EatToken(Tokens.kIdent)){
                return false;
            }
            
            // is not array
            // necessary to devide into two situation?
            if(PreFetchSym() != Tokens.kOpenbracketToken){
                // current variable name is scanner_.id
                variable = new Variable(Scanner.identifier_table[scanner_.Id]);
                return true;
            }

            // TODO: when array, use temp variable?
            while(PreFetchSym() == Tokens.kOpenbracketToken){
                Next();

                //Expression(function, out IOperand capacity_exp)
                if (Expression() == false){
                    Error("Expected Expression");
                    return false;
                }

                if(!EatToken(Tokens.kClosebracketToken)){
                    return false;
                }
            }

            return true;
        } 
        /*
             factor  =  designator |  number  |  “(“ expression “)”  | funcCall
        */
        public bool Factor(Function function, out IOperand fac_result)
        {
            // IOperand fac;
            fac_result = null;

            if(PreFetchSym() == Tokens.kIdent){
                if(Designator(function, out fac_result) == false){
                    Error("Expected Designator");
                    return false;
                }
            }
            else if(PreFetchSym() == Tokens.kNumber){
                Next(); // eat number
                fac_result = new Immdiate(scanner_.Val);
            }
            else if(PreFetchSym() == Tokens.kOpenparenToken){
                Next();
                if(Expression(function, out fac_result) == false){
                    Error("Expected Expression");
                    return false;
                }
                if(!EatToken(Tokens.kCloseparenToken)){
                    return false;
                }
            }
            else if(PreFetchSym() == Tokens.kCallToken){
                if(FuncCall(function, out fac_result) == false){
                    Error("Expected FuncCall");
                    return false;
                }
            }
            else{
                Error("No match in Factor");
                return false;
            }

            return true;
        } 
        /*
             term =  factor { (“*” | “/”) factor}.
        */
        public bool Term(Function function, out IOperand term_result)
        {
            term_result = null;
            if(Factor(function, out IOperand cur_term) == false){
                Error("Expected Factor");
                return false;
            }

            while(PreFetchSym() == Tokens.kTimesTocken || 
                PreFetchSym() == Tokens.kDivToken){
                Tokens op = PreFetchSym();

                Next();

                if(Factor(function, out IOperand extra_fac) == false){
                    Error("Expected Factor");
                    return false;
                }

                int instru_addr;
                if(op == Tokens.kTimesTocken){
                    instru_addr = function.AddInstruction(new Mul(cur_term, extra_fac));
                }
                else{
                    instru_addr = function.AddInstruction(new Div(cur_term, extra_fac));
                }

                // update cur_term, it's the ith Result: Var ti
                cur_term = new Variable(instru_addr);
            }
            term_result = cur_term;

            return true;
        } 
        /*
             expression  =  term {(“+” | “-”) term}
        */
        public bool Expression(Function function, out IOperand exp_result)
        {
            // exp_result = Variable.GetTemporary();

            exp_result = null;

            if(Term(function, out IOperand cur_expression) == false){
                Error("Expected Term");
                return false;
            }
                        
            while(PreFetchSym() == Tokens.kPlusToken ||
                PreFetchSym() == Tokens.kMinusToken){

                Tokens op = PreFetchSym();
                Next();

                if(Term(function, out IOperand extra_term) == false){
                    Error("Expected Term");
                    return false;
                }

                int instru_addr;
                if(op == Tokens.kPlusToken){
                    instru_addr = function.AddInstruction(new Add(cur_expression, extra_term));
                }
                else{
                    instru_addr = function.AddInstruction(new Sub(cur_expression, extra_term));
                }

                // update cur_expression, it's the ith Result: Var ti
                cur_expression = new Variable(instru_addr); // ti
            }

            exp_result = cur_expression;
            return true;
        }
        /*
             relation = expression relOp expression
        */
        public bool Relation(Function function, out Tokens relop, out IOperand relation_result)
        {
            relop = Tokens.kErrorToken;
            relation_result = null;
            if(Expression(function, out IOperand lhs) == false){
                Error("Expected Expression");
                return false;
            }

            if(RelOp(function, out relop) == false){
                Error("Expected RelOp");
                return false;
            }

            if(Expression(function, out IOperand rhs) == false){
                Error("Expected Expression");
                return false;
            }

            int instru_addr = function.AddInstruction(new Cmp(lhs, rhs));
            relation_result = new Variable(instru_addr);

            return true;
        }

        public bool RelOp(Function function, out Tokens relop)
        {
            relop = Tokens.kErrorToken;
            Next();

            if(scanner_sym_ == Tokens.kEqlToken ||
                scanner_sym_ == Tokens.kNeqToken ||
                scanner_sym_ == Tokens.kIssToken ||
                scanner_sym_ == Tokens.kGeqToken ||
                scanner_sym_ == Tokens.kLeqToken ||
                scanner_sym_ == Tokens.kGtrToken )
            {
                relop = scanner_sym_;
                return true;
            }
            else {
                Error("No Op matched");
                return false;
            }
        }

        // case
        // {@"==", Tokens.kEqlToken},
        // {@"!=", Tokens.kNeqToken},
        // {@"<", Tokens.kIssToken},
        // {@">=", Tokens.kGeqToken},
        // {@"<=", Tokens.kLeqToken},
        // {@">", Tokens.kGtrToken},
        private void GenBranchInstru(Tokens rel_op, IOperand rel_result, int goto_addr, out Instruction branch_instru){
            var goto_addr_var = new Immdiate(goto_addr); // next Instruction is else entry
            switch(rel_op){
                case (Tokens.kEqlToken): branch_instru = new Bne(rel_result, goto_addr_var); break;
                case (Tokens.kNeqToken): branch_instru = new Beq(rel_result, goto_addr_var); break;
                case (Tokens.kIssToken): branch_instru = new Bge(rel_result, goto_addr_var); break;
                case (Tokens.kGeqToken): branch_instru = new Blt(rel_result, goto_addr_var); break;
                case (Tokens.kLeqToken): branch_instru = new Bgt(rel_result, goto_addr_var); break;
                case (Tokens.kGtrToken): branch_instru = new Ble(rel_result, goto_addr_var); break;
                default: branch_instru = Nop.Instance; return;
            }
            return;
        }

/*
        public void MainComputation(Function function, )
        {
            Next();
            if(scanner_sym_ != kMainToken){
                Error("Main Error");
                return;
            }

            Next(); // look forward into VarDecl 
            while(scanner_sym_ == kVarToken || scanner_sym_ == kArrayToken ){
                VarDecl(); 
            }

            // already scan forward into FuncDecl inside last process.
            while(scanner_sym_ == kFuncToken || scanner_sym_ == kProcToken ){
                FuncDecl();
            }

            if(scanner_sym_ != kBeginToken){
                Error("Expected BeginToken");
                return;
            }
            StatSequence ();
            if(scanner_sym_ != kEndToken){
                Error("Expected EndToken");
                return;
            }
            Next();

            if(scanner_sym_ != kPeriodToken){
                Error("Expected PeriodToken");
                return;
            }

            // GENERATE IR here;

            // for main, end with nothing else
        }
*/

    }

}

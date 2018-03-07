using System;
using System.Collections.Generic;
using System.Text;
using IR.Block
using IR.Operand
using IR.Instructions
using FrontEnd.GenerateIR

namespace Davion.FrontEnd
{
    public class Parser
    {
        public Parser(string file_name) // opoen file and scan the first token into .input_sym_
        {
            this.scanner_ = new Scanner(file_name);
        }
        private Scanner scanner_;
        private int scanner_sym_;

        private void Next()
        {
            scanner_sym_ = scanner_.GetSym(); // advance to the next token
        }
        private int PreFetchSym()
        {
            return scanner_.PreFetchSym();
        }

        public void Error(string error_msg)
        {
            Console.WriteLine("-- ERROR: {0} -- ", 
                error_msg);
        }

        public bool EatToken(uint token_enum)
        {
            Next();
            if(scanner_sym_ != token_enum){
                Error("Expected " + Enum.GetName(typeof(Tokens), token_enum));
                return false;
            }
            return true;
        }


        /*
            -------------------------------------------             
        */
        public bool MainComputation()
        {
            if(!EatToken(Tokens.kMainToken)){
                return false;
            }

            // Next(); // look forward into VarDecl 
            while(VarDecl() != false)
            {
            }
            while(FuncDecl() != false)
            {
            }

            if(!EatToken(Tokens.kBeginToken)){
                return false;
            }

            if(StatSequence () == false){
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
        public bool VarDecl()
        {
            if(TypeDecl() == false){
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
        */
        public bool FuncDecl()
        {
            if(PreFetchSym() != Tokens.kFuncToken && PreFetchSym() != Tokens.kProcToken){
                Error("Expected FuncToken or ProcToken");
                return false;
            }
            Next();

            if(!EatToken(Tokens.kIdent)){
                return false;
            }

            if(PreFetchSym() != Tokens.kSemiToken){
                if(FormalParam() == false){
                    Error("Expected FormalParam");
                    return false;
                }
            }

            if(!EatToken(Tokens.kSemiToken)){
                return false;
            }

            if(FuncBody() == false){
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
        public bool TypeDecl()
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
        public bool FormalParam()
        {
            if(!EatToken(Tokens.kOpenparenToken)){
                return false;
            }

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
        public bool FuncBody()
        {
            while(VarDecl() != false){
                // eat {varDecl}, do sth.
            }

            if(!EatToken(Tokens.kBeginToken)){
                return false;
            }
            if(PreFetchSym() != Tokens.kEndToken){
                while(StatSequence() != false){
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
        public bool StatSequence()
        {
            if(Statement() == false){
                Error("Expected Statement");
                return false;
            }

            while(PreFetchSym() == Tokens.kSemiToken){
                // eat
                Next();

                if(Statement() == false){
                    Error("Expected Statement");
                    return false;
                }
            }


            return true;
        }
        /*
             statement = assignment | funcCall | ifStatement | whileStatement | returnStatement
        */
        public bool Statement()
        {
            if(Assignment()){

            }
            else if(FuncCall()){

            }
            else if(IfStatement()){

            }
            else if (WhileStatement()){

            }
            else if (ReturnStatement()){

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
        public bool Assignment()
        {
            Variable dest;
            IOperand source;
            if(!EatToken(Tokens.kLetToken)){
                return false;
            }

            if(Designator(dest) == false){
                Error("Expected Designator");
                return false;
            }

            if(Expression(source) == false){
                Error("Expected Expression");
                return false;
            }


            return true;
        }
        /*
             funcCall  =  “call” ident [ “(“ [expression { “,” expression } ] “)” ]
        */
        public bool FuncCall(out IOperand func_result)
        {
            if(!EatToken(Tokens.kCallToken)){
                return false;
            }

            if(!EatToken(Tokens.kIdent)){
                return false;
            }

            if(PreFetchSym() == Tokens.kOpenparenToken){
                Next(); // eat "("

                if(PreFetchSym() != Tokens.kCloseparenToken){ // assume expression won't begin with "")", due to EBNF
                    if(Expression() == false){
                        Error("Expected Expression");
                        return false;
                    }

                    while(PreFetchSym() == Tokens.kCommaToken){
                        Next(); // eat

                        if(Expression() == false){
                            Error("Expected Expression");
                            return false;
                        }
                    }
                }

                if(!EatToken(Tokens.kCloseparenToken)){
                    return false;
                }
                
            }
            
            return true;
        } 
        /*
            ifStatement  =  “if” relation “then” statSequence [ “else” statSequence ] “fi”             
        */
        public bool IfStatement()
        {
            if(!EatToken(Tokens.kIfToken)){
                return false;
            }

            if(Relation() == false){
                Error("Expected Relation");
                return false;
            }

            if(!EatToken(Tokens.kThenToken)){
                return false;
            }

            if(StatSequence() == false){
                Error("Expected StatSequence");
                return false;
            }

            if(PreFetchSym() == Tokens.kElseToken){
                Next(); // eat else

                if(StatSequence() == false){
                    Error("Expected StatSequence");
                    return false;
                }
            }

            if(!EatToken(Tokens.kFiToken)){
                return false;
            }

            return true;
        } 
        /*
             whileStatement  =  “while” relation “do” StatSequence “od”
        */
        public bool WhileStatement()
        {
            if(!EatToken(Tokens.kWhileToken)){
                return false;
            }

            if(Relation() == false){
                Error("Expected Relation");
                return false;
            }

            if(!EatToken(Tokens.kDoToken)){
                return false;
            }

            if(StatSequence() == false){
                Error("Expected StatSequence");
                return false;
            }

            if(!EatToken(Tokens.kOdToken)){
                return false;
            }

            return true;
        } 
        /*
             returnStatement  =  “return” [ expression ] 
        */
        public bool ReturnStatement()
        {
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
        public bool Designator(out IOperand variable)
        {
            if(!EatToken(Tokens.kIdent)){
                return false;
            }
            
            // is not array
            // necessary to devide into two situation?
            if(PreFetchSym() != Tokens.kOpenbracketToken){
                // current variable name is scanner_.id
                variable = New Variable(scanner_.id);
            }

            while(PreFetchSym() == Tokens.kOpenbracketToken){
                Next();

                if(Expression() == false){
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
        public bool Factor(out IOperand fac_result)
        {
            // IOperand fac;
            if(PreFetchSym() == Tokens.kIdent){
                if(Designator(fac_result) == false){
                    Error("Expected Designator");
                    return false;
                }
            }
            else if(PreFetchSym() == Tokens.kNumber){
                fac_result = new Immdiate(scanner_.val);
                Next(); // eat number
            }
            else if(PreFetchSym() == Tokens.kOpenparenToken){
                Next();
                if(Expression(fac_result) == false){
                    Error("Expected Expression");
                    return false;
                }
                if(!EatToken(Tokens.kCloseparenToken)){
                    return false;
                }
            }
            else if(PreFetchSym() = Tokens.kCallToken){
                if(FuncCall(fac_result) == false){
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
        public bool Term(out IOperand term_result)
        {
            IOperand fac_0;
            if(Factor(fac_0) == false){
                Error("Expected Factor");
                return false;
            }
            term_result = fac_0;

            while(PreFetchSym() == Tokens.kTimesTocken || 
                PreFetchSym() == Tokens.kDivToken){
                int op;
                if(PreFetchSym() == Tokens.kTimesTocken){
                    op = Opcode.Mul;
                }
                else{
                    op = Opcode.Div;
                }

                Next();

                IOperand temp_fac;
                if(Factor(temp_fac) == false){
                    Error("Expected Factor");
                    return false;
                }

                GenerateIR(function, block_id, op, 
                            new IOperand[2]{term_result, temp_fac},
                            term_result); // for some BasicBlock : exp_result = exp_result op temp_term
            }

            return true;
        } 
        /*
             expression  =  term {(“+” | “-”) term}
        */
        public bool Expression(out IOperand exp_result)
        {
            // exp_result = Variable.GetTemporary();

            IOperand term_0;
            if(Term(term_0) == false){
                Error("Expected Term");
                return false;
            }

            exp_result = term_0;

            while(PreFetchSym() == Tokens.kPlusToken ||
                PreFetchSym() == Tokens.kMinusToken){

                int op;
                if(PreFetchSym() == Tokens.kPlusToken){
                    op = Opcode.Add;
                }
                else{
                    op = Opcode.Sub
                }

                Next();

                IOperand temp_term;
                if(Term(temp_term) == false){
                    Error("Expected Term");
                    return false;
                }

                GenerateIR(function, block_id, op, 
                        new IOperand[2]{exp_result, temp_term},
                        exp_result); // for some BasicBlock : exp_result = exp_result op temp_term
            }
            return true;
        }
        /*
             relation = expression relOp expression
        */
        public bool Relation()
        {
            if(Expression() == false){
                Error("Expected Expression");
                return false;
            }

            if(RelOp() == false){
                Error("Expected RelOp");
                return false;
            }

            return true;
        }

        public bool RelOp()
        {
            Next();

            if(scanner_sym_ == Tokens.kEqlToken ||
                scanner_sym_ == Tokens.kNeqToken ||
                scanner_sym_ == Tokens.kIssToken ||
                scanner_sym_ == Tokens.kGeqToken ||
                scanner_sym_ == Tokens.kLeqToken ||
                scanner_sym_ == Tokens.kGtrToken ||)
            {
                return true;
            }
            else {
                Error("No Op matched");
                return false;
            }
        }

/*
        public void MainComputation()
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

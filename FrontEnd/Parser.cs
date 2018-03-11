using System;
using System.Collections.Generic;
using System.Text;
using IR;
using FrontEnd;

namespace FrontEnd
{
    public class Parser
    {
        public Parser(string file_name) // opoen file and scan the first token into .input_sym_
        {
            this.scanner_ = new Scanner(file_name);
        }
        private Scanner scanner_;
        private TokenHelper.Tokens scanner_sym_;

        private void Next()
        {
            scanner_sym_ = scanner_.GetSym(); // advance to the next token
        }
        private TokenHelper.Tokens PreFetchSym()
        {
            return scanner_.PreFetchSym();
        }

        public void Error(string error_msg)
        {
            Console.Error.WriteLine("-- Parser ERROR: {0} -- ", 
                error_msg);
        }

        public bool EatToken(TokenHelper.Tokens token_enum)
        {
            Next();
            if(scanner_sym_ != token_enum){
                Error("Expected " + Enum.GetName(typeof(TokenHelper.Tokens), token_enum));
                return false;
            }
            return true;
        }


        /*
            -------------------------------------------
        */
        public bool MainComputation()
        {
            if(!EatToken(TokenHelper.Tokens.kMainToken)){
                return false;
            }

            // Next(); // look forward into VarDecl
            while(VarDecl() != false)
            {
            }
            while(FuncDecl() != false)
            {
            }

            if(!EatToken(TokenHelper.Tokens.kBeginToken)){
                return false;
            }

            if(StatSequence () == false){
                Error("Expected StatSequence");
                return false;
            }

            if(!EatToken(TokenHelper.Tokens.kEndToken)){
                return false;
            }

            if(!EatToken(TokenHelper.Tokens.kPeriodToken)){
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
            if(!EatToken(TokenHelper.Tokens.kIdent)){
                return false;
            }

            while(PreFetchSym() == TokenHelper.Tokens.kCommaToken){
                Next(); // eat

                if(!EatToken(TokenHelper.Tokens.kIdent)){
                    return false;
                }
            }

            if(!EatToken(TokenHelper.Tokens.kSemiToken)){
                return false;
            }
            return true;
        }

        /*
             funcDecl  =  (“function” | “procedure”) ident [formalParam] “;” funcBody “;”
        */
        public bool FuncDecl()
        {
            if(PreFetchSym() != TokenHelper.Tokens.kFuncToken && PreFetchSym() != TokenHelper.Tokens.kProcToken){
                Error("Expected FuncToken or ProcToken");
                return false;
            }
            Next();

            if(!EatToken(TokenHelper.Tokens.kIdent)){
                return false;
            }

            if(PreFetchSym() != TokenHelper.Tokens.kSemiToken){
                if(FormalParam() == false){
                    Error("Expected FormalParam");
                    return false;
                }
            }

            if(!EatToken(TokenHelper.Tokens.kSemiToken)){
                return false;
            }

            if(FuncBody() == false){
                Error("Expected FuncBody");
                return false;
            }

            if(!EatToken(TokenHelper.Tokens.kSemiToken)){
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
            if(PreFetchSym() == TokenHelper.Tokens.kArrayToken)
            {
                Next();

                if(!EatToken(TokenHelper.Tokens.kOpenbracketToken)){
                    return false;
                }
                if(!EatToken(TokenHelper.Tokens.kNumber)){
                    return false;
                }
                if(!EatToken(TokenHelper.Tokens.kClosebracketToken)){
                    return false;
                }

                // higher dimension
                while(PreFetchSym() == TokenHelper.Tokens.kOpenbracketToken)
                {
                    Next();
                    if(!EatToken(TokenHelper.Tokens.kNumber)){
                        return false;
                    }

                    if(!EatToken(TokenHelper.Tokens.kClosebracketToken)){
                        return false;
                    }
                }
            }
            else if(PreFetchSym() == TokenHelper.Tokens.kVarToken){
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
            if(!EatToken(TokenHelper.Tokens.kOpenparenToken)){
                return false;
            }

            if(PreFetchSym() == TokenHelper.Tokens.kIdent){
                Next();

                while(PreFetchSym() == TokenHelper.Tokens.kCommaToken){
                    Next();// Eat Comma

                    if(!EatToken(TokenHelper.Tokens.kIdent)){
                        return false;
                    }
                }
            }

            if(!EatToken(TokenHelper.Tokens.kCloseparenToken)){
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

            if(!EatToken(TokenHelper.Tokens.kBeginToken)){
                return false;
            }
            if(PreFetchSym() != TokenHelper.Tokens.kEndToken){
                while(StatSequence() != false){
                    // eat{statSequence}
                }
            }

            if(!EatToken(TokenHelper.Tokens.kEndToken)){
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

            while(PreFetchSym() == TokenHelper.Tokens.kSemiToken){
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
            if(!EatToken(TokenHelper.Tokens.kLetToken)){
                return false;
            }

            if(Designator(out dest) == false){
                Error("Expected Designator");
                return false;
            }

            if(Expression(out source) == false){
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
            if(!EatToken(TokenHelper.Tokens.kCallToken)){
                return false;
            }

            if(!EatToken(TokenHelper.Tokens.kIdent)){
                return false;
            }

            if(PreFetchSym() == TokenHelper.Tokens.kOpenparenToken){
                Next(); // eat "("

                if(PreFetchSym() != TokenHelper.Tokens.kCloseparenToken){ // assume expression won't begin with "")", due to EBNF
                    if(Expression() == false){
                        Error("Expected Expression");
                        return false;
                    }

                    while(PreFetchSym() == TokenHelper.Tokens.kCommaToken){
                        Next(); // eat

                        if(Expression() == false){
                            Error("Expected Expression");
                            return false;
                        }
                    }
                }

                if(!EatToken(TokenHelper.Tokens.kCloseparenToken)){
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
            if(!EatToken(TokenHelper.Tokens.kIfToken)){
                return false;
            }

            if(Relation() == false){
                Error("Expected Relation");
                return false;
            }

            if(!EatToken(TokenHelper.Tokens.kThenToken)){
                return false;
            }

            if(StatSequence() == false){
                Error("Expected StatSequence");
                return false;
            }

            if(PreFetchSym() == TokenHelper.Tokens.kElseToken){
                Next(); // eat else

                if(StatSequence() == false){
                    Error("Expected StatSequence");
                    return false;
                }
            }

            if(!EatToken(TokenHelper.Tokens.kFiToken)){
                return false;
            }

            return true;
        } 
        /*
             whileStatement  =  “while” relation “do” StatSequence “od”
        */
        public bool WhileStatement()
        {
            if(!EatToken(TokenHelper.Tokens.kWhileToken)){
                return false;
            }

            if(Relation() == false){
                Error("Expected Relation");
                return false;
            }

            if(!EatToken(TokenHelper.Tokens.kDoToken)){
                return false;
            }

            if(StatSequence() == false){
                Error("Expected StatSequence");
                return false;
            }

            if(!EatToken(TokenHelper.Tokens.kOdToken)){
                return false;
            }

            return true;
        } 
        /*
             returnStatement  =  “return” [ expression ] 
        */
        public bool ReturnStatement()
        {
            if(!EatToken(TokenHelper.Tokens.kReturnToken)){
                return false;
            }

            if(PreFetchSym() == TokenHelper.Tokens.kCallToken || 
                PreFetchSym() == TokenHelper.Tokens.kOpenparenToken || 
                PreFetchSym() == TokenHelper.Tokens.kIdent || 
                PreFetchSym() == TokenHelper.Tokens.kNumber)
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
            if(!EatToken(TokenHelper.Tokens.kIdent)){
                return false;
            }
            
            // is not array
            // necessary to devide into two situation?
            if(PreFetchSym() != TokenHelper.Tokens.kOpenbracketToken){
                // current variable name is scanner_.id
                variable = new Variable(scanner_.identifier_table[scanner_.id]);
            }

            while(PreFetchSym() == TokenHelper.Tokens.kOpenbracketToken){
                Next();

                if(Expression() == false){
                    Error("Expected Expression");
                    return false;
                }

                if(!EatToken(TokenHelper.Tokens.kClosebracketToken)){
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
            if(PreFetchSym() == TokenHelper.Tokens.kIdent){
                if(Designator(out fac_result) == false){
                    Error("Expected Designator");
                    return false;
                }
            }
            else if(PreFetchSym() == TokenHelper.Tokens.kNumber){
                fac_result = new Immdiate(scanner_.val);
                Next(); // eat number
            }
            else if(PreFetchSym() == TokenHelper.Tokens.kOpenparenToken){
                Next();
                if(Expression(out fac_result) == false){
                    Error("Expected Expression");
                    return false;
                }
                if(!EatToken(TokenHelper.Tokens.kCloseparenToken)){
                    return false;
                }
            }
            else if(PreFetchSym() = TokenHelper.Tokens.kCallToken){
                if(FuncCall(out fac_result) == false){
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
            if(Factor(out fac_0) == false){
                Error("Expected Factor");
                return false;
            }
            term_result = fac_0;

            while(PreFetchSym() == TokenHelper.Tokens.kTimesTocken || 
                PreFetchSym() == TokenHelper.Tokens.kDivToken){
                Opcode op;
                if(PreFetchSym() == TokenHelper.Tokens.kTimesTocken){
                    op = Opcode.Mul;
                }
                else{
                    op = Opcode.Div;
                }

                Next();

                IOperand temp_fac;
                if(Factor(out temp_fac) == false){
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
            if(Term(out term_0) == false){
                Error("Expected Term");
                return false;
            }

            exp_result = term_0;

            while(PreFetchSym() == TokenHelper.Tokens.kPlusToken ||
                PreFetchSym() == TokenHelper.Tokens.kMinusToken){

                Opcode op;
                if(PreFetchSym() == TokenHelper.Tokens.kPlusToken){
                    op = Opcode.Add;
                }
                else{
                    op = Opcode.Sub;
                }

                Next();

                IOperand temp_term;
                if(Term(out temp_term) == false){
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

            if(scanner_sym_ == TokenHelper.Tokens.kEqlToken ||
                scanner_sym_ == TokenHelper.Tokens.kNeqToken ||
                scanner_sym_ == TokenHelper.Tokens.kIssToken ||
                scanner_sym_ == TokenHelper.Tokens.kGeqToken ||
                scanner_sym_ == TokenHelper.Tokens.kLeqToken ||
                scanner_sym_ == TokenHelper.Tokens.kGtrToken )
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

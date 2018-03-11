using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace FrontEnd
{
    public class Scanner
    {
        private char input_sym_; // current character on the input, 0x00=error, 0xff=EOF
        private FileReader file_reader_;
        private TokenHelper.Tokens current_token_;

        public int Val { get; private set; } // public int number; // value of the last number encountered
        // private int next_val_;
        public int Id { get; private set; } // id of the last identifier encountered
        // private int next_id_;
        // this is the list of identifiers
        public static List<string> identifier_table { get; set; } // public static SortedDictionary<uint, string> identifier_table; // 
        private uint cur_ident_num_;

        public Scanner(string file_name) // opoen file and scan the first token into .input_sym_
        {
            identifier_table = new List<string>();
            this.file_reader_ = new FileReader(file_name);

            this.current_token_ = TokenHelper.Tokens.kNullToken;
            Next();
            GetSym();
        }
        private bool ScanNumber()
        {
            bool is_number = false;
            // this.Val = this.next_val_;
            if(Char.IsNumber(input_sym_)){
                is_number = true;
                this.Val = input_sym_ - '0';   
                Next();
            }
            while(Char.IsNumber(input_sym_)){
                is_number = true;
                this.Val = 10 * this.Val + input_sym_ - '0';
                Next();
            }

            if(is_number){
                current_token_ = TokenHelper.Tokens.kNumber;
            }

            return is_number;
        }

        private bool ScanIdentifier()
        {
            StringBuilder sb = new StringBuilder();
            if (!Char.IsLetter(input_sym_))
            {
                return false;
            }
            sb.Append(input_sym_);
            Next();
            while (Char.IsNumber( input_sym_) || (Char.IsLetter( input_sym_) && input_sym_ < (char)128))
            {
                sb.Append(input_sym_);
                Next();
            }
            string token_str = sb.ToString();

            // try to match with a keyword
            if(TokenHelper.MatchKeyword(token_str, out current_token_)){
                return true;
            }

            int pos = identifier_table.IndexOf(sb.ToString());
            if (pos < 0)
            {
                pos = identifier_table.Count;
                identifier_table.Add(sb.ToString());
            }

            // Id = next_id_;
            Id = pos;
            current_token_ = TokenHelper.Tokens.kIdent;
            return true;
        }

        private bool Next()  // advanced to the next character
        {
            if(input_sym_ == FileReader.kEndSymbol){
                return false;
            }

            if((input_sym_ = file_reader_.GetSym()) == FileReader.kEndSymbol){
                return false; // end char of file
            }
            return true;
        }
        public bool SkipSpace(){
            while (input_sym_ == '\t' || input_sym_ == '\r' || input_sym_ == '\n' || input_sym_ == ' ' || input_sym_ == FileReader.kEOLSymbol) {
                Next();
            }
            // if come to the EOF
            if(input_sym_ == FileReader.kEndSymbol)
            {
                return false;
            }
            else{
                return true;
            }
        }
		public TokenHelper.Tokens GetSym() // public int sym; // the current token on the input, 0 = error token, 255 = end-of-file token
		{
            TokenHelper.Tokens r = current_token_;
            if(r == TokenHelper.Tokens.kEndofToken){
                Next();
                return r;
            }

            if (!SkipSpace()){
                current_token_ = TokenHelper.Tokens.kEndofToken;
                return r;
            }

            if(input_sym_ == FileReader.kEndSymbol){
                current_token_ = TokenHelper.Tokens.kEndofToken;
                return r;
            }

            if(ScanIdentifier() != false){
                return r;
            }

            if(ScanNumber() != false){
                return r;
            }

            switch(input_sym_){
                case '/':{
                    Next();
                    if(input_sym_ == '/'){
                        //Next();
                        file_reader_.SkipLine();
                        Next();
                        GetSym(); // OR: SkipCommentAndSpace
                        return r;
                    }
                    else{
                        current_token_ = TokenHelper.Tokens.kDivToken;
                        return r;
                    }
                    break;
                }
                case '=':{
                    Next();
                    if(input_sym_ == '='){
                        Next();
                        current_token_ = TokenHelper.Tokens.kEqlToken;
                        return r;
                    }
                    else{
                        Error(@"'=' should be followed by '='");
                        current_token_ = TokenHelper.Tokens.kErrorToken;
                        return r;
                    }
                    break;
                }
                case '!':{
                    Next();
                    if(input_sym_ == '='){
                        Next();
                        current_token_ = TokenHelper.Tokens.kNeqToken;
                        return r;
                    }
                    else{
                        Error(@"'!' should be followed by '='");
                        current_token_ = TokenHelper.Tokens.kErrorToken;
                        return r;
                    }
                    break;
                }
                case '>':{
                    Next();
                    if(input_sym_ == '='){
                        Next();
                        current_token_ = TokenHelper.Tokens.kGeqToken;
                        return r;
                    }
                    else{
                        current_token_ = TokenHelper.Tokens.kGtrToken;
                        return r;
                    }
                    break;
                }
                case '<':{
                    Next();
                    if(input_sym_ == '='){
                        Next();
                        current_token_ = TokenHelper.Tokens.kLeqToken;
                        return r;
                    }
                    else if(input_sym_ == '-'){
                        Next();
                        current_token_ = TokenHelper.Tokens.kBecomesToken;
                        return r;
                    }
                    else{
                        current_token_ = TokenHelper.Tokens.kIssToken;
                        return r;
                    }
                    break;
                }
                case '+':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kPlusToken;
                    return r;
                }
                case '-':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kMinusToken;
                    return r;
                }
                case '*':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kTimesTocken;
                    return r;
                }
                case '.':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kPeriodToken;
                    return r;
                }
                case ',':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kCommaToken;
                    return r;
                }
                case ';':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kSemiToken;
                    return r;
                }
                case '(':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kOpenparenToken;
                    return r;
                }
                case ')':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kCloseparenToken;
                    return r;
                }
                case '[':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kOpenbracketToken;
                    return r;
                }
                case ']':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kClosebracketToken;
                    return r;
                }
                case '{':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kBeginToken;
                    return r;
                }
                case '}':{
                    Next();
                    current_token_ = TokenHelper.Tokens.kEndToken;
                    return r;
                }
            }
            {
                current_token_ = TokenHelper.Tokens.kErrorToken;
            }
            return r;
        }

        public void Error(string error_msg)
        {
            Console.Error.WriteLine("-- Scanner ERROR: {0} -- ", 
                error_msg);
        }

        public TokenHelper.Tokens PreFetchSym()
        {
            return current_token_;
        }


        public string Token2String(TokenHelper.Tokens token){
            string key_str = TokenHelper.TOKENSET.FirstOrDefault(x => x.Value == token).Key;
            return key_str;
        }

        public uint GetFileLine(){
            return file_reader_.NumberOfLine;
        }

        public char DebugCurChar(){
            return input_sym_;
        }

        // TODO: handle removed ID
        public string Id2String(int id)
        {
            if (id < identifier_table.Count)
                if (identifier_table[id] != "") // check null string
                {
                    return identifier_table[id];
                }

            return ""; // error when
        }

        public int String2Id(string name)
        {
            int pos = identifier_table.IndexOf(name);
            if (pos < 0)
            {
                return -1;
            }
            else return pos;
        }

    }
}

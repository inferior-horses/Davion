using System;

namespace FrontEnd{
    public class FileReader{ // encapsulates streams of characters
        private System.IO.StreamReader file_stream_;
        private string[] words_;
        private uint pos_;
        private uint num_word_;

        private string[] inner_words_;
        private uint inner_pos_;

        private string pre_fetch_str_;
        // private uint num_inner_word_;

        public FileReader(string file_name) // open file and read the first char into .sym
        {
            this.file_stream_ = new System.IO.StreamReader(@file_name);
            this.words_ = file_stream_.ReadToEnd().Split(' ');
            this.pos_ = 0;
            this.num_word_ = words_.Length();

            this.pre_fetch_str_ = "";
            this.inner_words_ = new string[] {}; // length == zero
            this.inner_pos_ = 0;

            GetSym(); // in order to prefetch a sym
        }

        // not parsing properly, need to rewrite
        public string GetSym() // return current character on the input, 0x00=error, 0xff=EOF
        {
            string r = pre_fetch_str_;

            if (pos_ < num_word_)
            {
                if(inner_pos_ < inner_words_.Length()){
                    pre_fetch_str_ = inner_words_[inner_pos_++];
                }// else empty words
                else{
                    inner_pos_ = 0;
                    pos++;

                    if (pos_ == num_word_){
                        pre_fetch_str_ = (char)0xff;
                    }else if (pos > num_word_){
                        pre_fetch_str_ = (char)0x00;
                    }else{
                        inner_words_ = Regex.Split(words_[pos_], @"();[]+-*/");
                    }

                    pre_fetch_str_ = inner_words_[inner_pos_++];
                }
            }
            else if (pos_ == num_word_)
            {
                pre_fetch_str_ = (char)0xff;
                //return (char)0xff;
            }
            else
            {
                pre_fetch_str_ = (char)0x00;
                // return (char)0x00;
            }

            return r;
        }

        public void Error(string error_msg); // signal an error with current file position

        public string PreFetch() // return current character on the input, 0x00=error, 0xff=EOF
        {
            // change: return pre_fetch_str_
            return pre_fetch_str_;
        }

    }

    public class Scanner {
        private string input_sym_; // current character on the input, 0x00=error, 0xff=EOF
        private FileReader file_reader_;

        public int val { get; private set; } // public int number; // value of the last number encountered
        public uint id { get { return (int)id; } private set; } // id of the last identifier encountered

        // this is the list of identifiers
        public static ArrayList<string> identifier_table; // public static SortedDictionary<uint, string> identifier_table; // 
        private uint cur_ident_num_;

        public Scanner(string file_name) // opoen file and scan the first token into .input_sym_
        {
            this.file_reader_ = new FileReader(file_name);
        }

        private bool ScanNumber(string token_str)
        {
            this.val = 0;

            foreach (char c in token_str)
            {
                if (Char.IsNumber(c))
                {
                    this.val = 10 * this.val + c - '0';
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private bool ScanIdentifier(string token_str)
        {
            StringBuilder sb = new StringBuilder();
            if (!Char.IsLetter(token_str[0]))
            {
                return false;
            }
            foreach (char c in token_str)
            {
                if (!(Char.IsNumber(c) || Char.IsLetter(c)))
                {
                    sb.Append(c);
                    return false;
                }
            }

            int pos = identifier_table.Indexof(sb.ToString());
            if (pos < 0)
            {
                pos = identifier_table.Length();
                identifier_table.Add(sb.ToString());

                id = (uint)pos;
            }

            return true;
        }


        public void Next() {  // advanced to the next character
            input_sym_ = file_reader_.GetSym();
        }

        public int GetSym() // public int sym; // the current token on the input, 0 = error token, 255 = end-of-file token
        {
            Next(); // for current or Next()? depends on caller
            
            // handle ERROR and EOF (already in dictionary)
            // handle keywords (dictionary)
            // handle number and id

            uint token;

            if (token_set.TryGetValue(input_sym_, out token))
            {
                return (int)token;
            }
            else
            {
                if (ScanNumber(input_sym_)) // number
                {
                    token = kNumber;
                    return (int)token;
                }
                else if (ScanIdentifier(input_sym_))
                {
                    token = kIdent;
                    return (int)token;
                }
            }
        }

        public void Error(string error_msg);

        // TODO: handle removed ID
        public string Id2String(int id)
        {
            if ((id < identifier_table.Length()))
                if (identifier_table[id] != "") // check null string
                {
                    return identifier_table[id];
                }
        }

        public int String2Id(string name)
        {
            int pos = identifier_table.Indexof(name);
            if (pos < 0)
            {
                return -1;
            }
            else return pos;
        }

        public int PreFetchSym()
        {
            next_sym = file_reader_.PreFetch();

            // could pack into a method
            uint token;

            if (token_set.TryGetValue(next_sym, out token))
            {
                return (int)token;
            }
            else
            {
                if (ScanNumber(next_sym)) // number
                {
                    token = kNumber;
                    return (int)token;
                }
                else if (ScanIdentifier(next_sym))
                {
                    token = kIdent;
                    return (int)token;
                }
            }
        }

    }

}
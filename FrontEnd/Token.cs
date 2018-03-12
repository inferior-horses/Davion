using System;
using System.Collections.Generic;
using System.Text;

namespace FrontEnd
{
    public enum Tokens:uint
    {
        kErrorToken = 0,

        kTimesTocken = 1,
        kDivToken = 2,

        kPlusToken = 11,
        kMinusToken = 12,

        kEqlToken = 20,
        kNeqToken = 21,
        kIssToken = 22,
        kGeqToken = 23,
        kLeqToken = 24,
        kGtrToken = 25,

        kPeriodToken = 30,
        kCommaToken = 31,
        kOpenbracketToken = 32,
        kClosebracketToken = 34,
        kCloseparenToken = 35,

        kBecomesToken = 40,
        kThenToken = 41,
        kDoToken = 42,

        kOpenparenToken = 50,

        kNumber = 60,
        kIdent = 61,

        kSemiToken = 70,
        kEndToken = 80,
        kOdToken = 81,
        kFiToken = 82,

        kElseToken = 90,

        kLetToken = 100,
        kCallToken = 101,
        kIfToken = 102,
        kWhileToken = 103,
        kReturnToken = 104,

        kVarToken = 110,
        kArrayToken = 111,
        kFuncToken = 112,
        kProcToken = 113,

        kBeginToken = 150,

        kMainToken = 200,

        kNullToken = 254,
        kEndofToken = 255
    };

    public class TokenHelper { 
        public static Dictionary<string, Tokens> TOKENSET = new Dictionary<string, Tokens>()
        {
            {new string((char)0x00, 1), Tokens.kErrorToken},
            {@"*", Tokens.kTimesTocken},
            {@"/", Tokens.kDivToken},
            {@"+", Tokens.kPlusToken},
            {@"-", Tokens.kMinusToken},
            {@"==", Tokens.kEqlToken},
            {@"!=", Tokens.kNeqToken},
            {@"<", Tokens.kIssToken},
            {@">=", Tokens.kGeqToken},
            {@"<=", Tokens.kLeqToken},
            {@">", Tokens.kGtrToken},
            {@".", Tokens.kPeriodToken},
            {@",", Tokens.kCommaToken},
            {@"[", Tokens.kOpenbracketToken},
            {@"]", Tokens.kClosebracketToken},
            {@")", Tokens.kCloseparenToken},
            {@"<-", Tokens.kBecomesToken},
            {@"then", Tokens.kThenToken},
            {@"do", Tokens.kDoToken},
            {@"(", Tokens.kOpenparenToken},

            // {@"", Tokens.kNumber},
            // {@"", Tokens.kIdent},

            {@";", Tokens.kSemiToken},
            {@"}", Tokens.kEndToken},
            {@"od", Tokens.kOdToken},
            {@"fi", Tokens.kFiToken},
            {@"else", Tokens.kElseToken},
            {@"let", Tokens.kLetToken},
            {@"call", Tokens.kCallToken},
            {@"if", Tokens.kIfToken},
            {@"while", Tokens.kWhileToken},
            {@"return", Tokens.kReturnToken},
            {@"var", Tokens.kVarToken},
            {@"array", Tokens.kArrayToken},
            {@"function", Tokens.kFuncToken},
            {@"procedure", Tokens.kProcToken},
            {@"{", Tokens.kBeginToken},
            {@"main", Tokens.kMainToken},
            {new string((char)0xff, 1), Tokens.kEndofToken}
        };

        public static bool MatchKeyword(string token_str, out Tokens cur_token_ref){ // if is a keyword, modify cur_token_ref, return true. else only return false;
            // ALERT : may change cur_token_ref into the default value if no key was matched. Backup method: check with keyword one by one.
            if(TOKENSET.TryGetValue(token_str, out cur_token_ref)){
                return true;
            }
            else {
                return false;
            }
        }
    }
}

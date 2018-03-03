using System;
using System.Collections.Generic;
using System.Text;

namespace Davion.FrontEnd
{
    enum Tokens:uint
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

        kEofToken = 255
    };

    static const Dictionary<string, ushort> TOKENSET =
    {
        {(char)0x00, kErrorToken},
        {@"*", kTimesTocken},
        {@"/", kDivToken},
        {@"+", kPlusToken},
        {@"-", kMinusToken},
        {@"==", kEqlToken},
        {@"!=", kNeqToken},
        {@"<", kIssToken},
        {@">=", kGeqToken},
        {@"<=", kLeqToken},
        {@">", kGtrToken},
        {@".", kPeriodToken},
        {@",", kCommaToken},
        {@"[", kOpenbracketToken},
        {@"]", kClosebracketToken},
        {@")", kCloseparenToken},
        {@"<-", kBecomesToken},
        {@"then", kThenToken},
        {@"do", kDoToken},
        {@"(", kOpenparenToken},

        // {@"", kNumber},
        // {@"", kIdent},

        {@";", kSemiToken},
        {@"}", kEndToken},
        {@"od", kOdToken},
        {@"fi", kFiToken},
        {@"else", kElseToken},
        {@"let", kLetToken},
        {@"call", kCallToken},
        {@"if", kIfToken},
        {@"while", kWhileToken},
        {@"return", kReturnToken},
        {@"var", kVarToken},
        {@"array", kArrayToken},
        {@"function", kFuncToken},
        {@"procedure", kProcToken},
        {@"{", kBeginToken},
        {@"main", kMainToken},
        {(char)0xff, kEofToken}
    };
}

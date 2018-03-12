using System;
using System.Collections.Generic;
using System.Text;
using FrontEnd;

namespace Davion
{
    public class ScannerTest
    {
    	private Scanner scanner_;

    	public ScannerTest(string file_name){
    		scanner_ = new Scanner(file_name);
    	}

    	public void PrintToken(){
    		Tokens cur_sym;
            uint counter = 0;
    		while((cur_sym = scanner_.GetSym()) != Tokens.kEndofToken){
                uint line = scanner_.GetFileLine();
                if (cur_sym == Tokens.kErrorToken){
    				char cur_char = scanner_.DebugCurChar();
		    		Console.WriteLine("detected error in line {0} at char '{1}' ",
		    			line, cur_char);
    			}

                string cur_sym_str = "";
                if (cur_sym == Tokens.kNumber)
                {
                    cur_sym_str = "NUMBER : " + scanner_.Val;
                }
                else if (cur_sym == Tokens.kIdent) {
                    cur_sym_str = "IDENTIFIER : " + Scanner.identifier_table[scanner_.Id];
                }
                else { 
                    cur_sym_str = scanner_.Token2String(cur_sym);
                }
                Console.WriteLine("Token #{0} in line {1} is {2}", counter++, line, cur_sym_str);
    		}
    	}

    }
}

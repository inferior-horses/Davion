using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FrontEnd
{
    public class FileReader
    {
        public static char kEndSymbol = (char)0xff;
        public static char kEOLSymbol = (char)0xfd;

        private System.IO.StreamReader file_stream_;
		private int pos_in_line_;
		private string cur_line_;
		public uint NumberOfLine { get; private set; }

    	public FileReader(string file_name)
    	{
            try { 
                FileInfo file_in = new FileInfo(file_name);
                this.file_stream_ = file_in.OpenText();
                if (!file_in.Exists)
                {
                    Console.WriteLine("No such a file...");
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            // this.file_stream_ = new System.IO.StreamReader(@file_name);
            this.NumberOfLine = 0;
            this.pos_in_line_ = -1;
            this.cur_line_ = "";
            PrintFilePath(file_name);
        }

        public void PrintFilePath(string file_name)
        {
//            Console.WriteLine("Open File at {0}", Path.GetDirectory(@file_name));
        }

    	public char GetSym()
    	{
    		// if need next line
    		while(pos_in_line_ >= cur_line_.Length || cur_line_.Length == 0)
            {
    			if(!NextLine()){
    				return kEndSymbol;
    			}
    			else{
    				pos_in_line_ = 0;
                    return kEOLSymbol;
    			}
    		}

    		char r = cur_line_[pos_in_line_];
    		
    		pos_in_line_++;

    		return r;
    	}

    	private bool NextLine()
    	{
    		while((cur_line_ = file_stream_.ReadLine()) != null){
    			this.NumberOfLine++;

    			if(cur_line_.Trim() != null){
	    			pos_in_line_ = 0;
    				return true;
    			}
    		}
			return false;
    	}

        public bool SkipLine(){ // Jump to NextLine;
        	return NextLine();
        }

    	public void Error(string error_msg)
    	{
            Console.Error.WriteLine("-- FileReader ERROR: {0} -- ", 
                error_msg);
    	}

    	// GetNumberofLine();
    	// GetCharPosition();

    }
}

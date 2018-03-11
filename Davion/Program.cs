using System;
using System.IO;

namespace Davion
{
    public class Program
    {
        static void Main(string[] args)
        {
            string dirpath = Directory.GetCurrentDirectory();
            Console.WriteLine("Hello World!" + dirpath);
            ScannerTest test_scanner = new ScannerTest(@"..\..\CodeSample\test002.txt");

            test_scanner.PrintToken();
            Console.ReadKey();
        }
    }
}
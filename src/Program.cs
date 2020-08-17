using System;

using DataKeep.Tokens;
using DataKeep.ParserTypes;
using DataKeep.Syntax;

namespace DataKeep
{
    class Program
    {
        static void Main(string[] args)
        {

            DebugDK.StartStopwatch("main");
            DebugDK.SetLog(false);

            DebugDK.StartStopwatch("lexer");
            Lexer lexer = new Lexer(new FileHandler("/Users/QuinnyBoy/Documents/DataManager/DataKeep/data.dk"));
            lexer.LexAllLines();
            DebugDK.StopStopwatch("lexer");

            DebugDK.StartStopwatch("parser");
            Parser parser = new Parser(lexer);
            parser.ParseAllLines();
            parser.GiveStructInheritance();
            DebugDK.StopStopwatch("parser");
            parser.PrintAllData();

            DebugDK.StartStopwatch("syntaxparser");
            SyntaxParser syntaxParser = new SyntaxParser(new FileHandler("/Users/QuinnyBoy/Documents/DataManager/DataKeep/main.dks"));
            syntaxParser.ParseAllLines();
            DebugDK.StopStopwatch("syntaxparser");
            syntaxParser.PrintAllData();

            DebugDK.StartStopwatch("linker");
            Linker linker = new Linker(syntaxParser, parser);
            linker.Convert();
            linker.OutputToFile("output.csbutnotcs");
            DebugDK.StopStopwatch("linker");

            DebugDK.StopStopwatch("main");


        }
    }

  
}

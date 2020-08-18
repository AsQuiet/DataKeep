using System;


namespace DataKeep
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = args[0];
            string outputpath = args[1] + ".py";

            DebugDK.StartStopwatch("main");
            DebugDK.SetLog(false);

            DebugDK.StartStopwatch("lexer");
            Lexer lexer = new Lexer(new FileHandler(path));
            lexer.LexAllLines();
            DebugDK.StopStopwatch("lexer");

            DebugDK.StartStopwatch("parser");

            Parser parser = new Parser(lexer);
            parser.ParseAllLines();
            parser.GiveStructInheritance();
            DebugDK.StopStopwatch("parser");
            //parser.PrintAllData();

         

            DebugDK.StartStopwatch("exporter");
            Exporter exporter = Exporter.FromParser(parser, outputpath);
            exporter.Export();
            DebugDK.StopStopwatch("exporter");

            DebugDK.StopStopwatch("main");
        }
    }

  
}

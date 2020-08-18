using System;


namespace DataKeep
{
    class Program
    {
        static void Main(string[] args)
        {

            string macPath = "/Users/QuinnyBoy/Documents";
            //string winPath = "E:\\PROJECTS";               

            DebugDK.StartStopwatch("main");
            DebugDK.SetLog(true);

            DebugDK.StartStopwatch("lexer");
            Lexer lexer = new Lexer(new FileHandler(macPath + "/DataManager/DataKeep/data.dk"));
            lexer.LexAllLines();
            DebugDK.StopStopwatch("lexer");

            DebugDK.StartStopwatch("parser");

            Parser parser = new Parser(lexer);
            parser.ParseAllLines();
            parser.GiveStructInheritance();
            DebugDK.StopStopwatch("parser");
            parser.PrintAllData();

            DebugDK.StopStopwatch("main");

            Exporter exporter = Exporter.FromParser(parser, "output.py");
            
            exporter.Export();
            

        }
    }

  
}

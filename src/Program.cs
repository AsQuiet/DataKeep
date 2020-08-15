using System;
using DataKeep.Tokens;
namespace DataKeep
{
    class Program
    {
        static void Main(string[] args)
        {

            Lexer lexer = new Lexer(new FileHandler("data.dk"));
            lexer.LexAllLines();

            foreach(Token[] ts in lexer.fileTokens)
            {
                Console.WriteLine("------------");
                Token[] tss = Token.RemoveBeginWhiteSpace(ts);
                foreach(Token t in tss)
                {
                    Lexer.PrintToken(t);
                }
            }

            Parser parser = new Parser(lexer);

            parser.ParserAllLines();

            parser.PrintAllData();

        }
    }
}

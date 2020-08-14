using System;

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
                foreach(Token t in ts)
                {
                    Lexer.PrintToken(t);
                }
            }

        }
    }
}

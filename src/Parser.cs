using System;
using System.Collections;

using DataKeep.ParserTypes;
using DataKeep.Tokens;

namespace DataKeep
{

    class Parser
    {

        private Lexer lexer;
        private int currentLine = 0;

        private ArrayList pStructs = new ArrayList();
        private ArrayList pEnums = new ArrayList();

        private Token[] decoratorTokens;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;

        }

        private Token[] GetCurrentLine()
        {
            return lexer.fileTokens[currentLine];
        }

        public void ParseCurrentLine()
        {

            DetectStruct();
            DetectEnum();
            DetectField();
            DetectDecorator();
            DetectEnumEntry();
            Console.WriteLine("deteoion is done");
            currentLine += 1;
        }


        private void DetectStruct()
        {
            bool hasStruct = Token.IncludesType(GetCurrentLine(), LexerTypes.Struct);
            bool noSemiColon = !Token.IncludesType(GetCurrentLine(), LexerTypes.SemiColon);

            if (hasStruct && noSemiColon)
            {
                Console.WriteLine("Detected a struct");
            }

        }

        private void DetectEnum()
        {
            bool hasEnum = Token.IncludesType(GetCurrentLine(), LexerTypes.Enum);
            bool noSemiColon = !Token.IncludesType(GetCurrentLine(), LexerTypes.SemiColon);

            if (hasEnum && noSemiColon)
            {
                Console.WriteLine("Detected an enum");
            }

        }

        private void DetectField()
        {
            bool hasTypeDecl = Token.IncludesType(GetCurrentLine(), LexerTypes.TypeDecl);
            bool hasSemiColon = Token.IncludesType(GetCurrentLine(), LexerTypes.SemiColon);

            if (hasTypeDecl && hasSemiColon)
            {
                Console.WriteLine("muhahaha field detected");
            }
        }

        private void DetectDecorator()
        {
            bool hasDeco = Token.IncludesType(GetCurrentLine(), LexerTypes.Decorator);

            if (hasDeco)
            {
                Console.WriteLine("Detected a decorator.");
            }
        }

        private void DetectEnumEntry()
        {
            bool hasComma = Token.IncludesType(GetCurrentLine(), LexerTypes.Comma);
            LexerTypes[] includes = { LexerTypes.Comma, LexerTypes.Space, LexerTypes.Undefined };
            bool hasNothingElse = Token.TokensOnlyInclude(GetCurrentLine(), includes);

            if (hasNothingElse || hasComma)
            {
                Console.WriteLine("detect ad enum entry");
            }
        }


    }
      

}
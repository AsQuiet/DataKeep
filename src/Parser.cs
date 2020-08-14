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

        private bool inStruct = false;
        private bool inEnum = false;

        private PStruct activeStruct;
        private ArrayList structFieldBuffer;

        private string decoratorBuffer = "";

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
            DetectEnumEntry();
            DetectEndOfScope();

            if (decoratorBuffer != "")
                decoratorBuffer = "";

            DetectDecorator();

            Console.WriteLine(".. in struct :" + inStruct + " inEnum :" + inEnum + " current deco " + decoratorBuffer);
            currentLine += 1;
        }


        private void DetectStruct()
        {
            bool hasStruct = Token.IncludesType(GetCurrentLine(), LexerTypes.Struct);
            bool noSemiColon = !Token.IncludesType(GetCurrentLine(), LexerTypes.SemiColon);
            bool hasInheritance = Token.IncludesType(GetCurrentLine(), LexerTypes.Inheritance);

            if ((hasStruct && noSemiColon) || hasInheritance)
            {

                Console.WriteLine("Detected a struct");
                inStruct = true;

                string name = "";
                string inheritance = "";

                int start = Token.IndexOfType(GetCurrentLine(), LexerTypes.Struct) + 1;
                int stop;

                if (hasInheritance) {
                    stop = Token.IndexOfType(GetCurrentLine(), LexerTypes.Inheritance);
                    inheritance = Token.SmashTokens(Token.GetRange(GetCurrentLine(), stop + 1,GetCurrentLine().Length), "");
                } else
                    stop = GetCurrentLine().Length;

                name = Token.SmashTokens(Token.RemoveBeginWhiteSpace(Token.GetRange(GetCurrentLine(), start, stop)), "");
               
                activeStruct.name = name;
                activeStruct.inheritance = inheritance;
                activeStruct.deco = decoratorBuffer;
                structFieldBuffer = new ArrayList();

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

                int typeDeclIndex = Token.IndexOfType(GetCurrentLine(), LexerTypes.TypeDecl);
                string name = Token.SmashTokens(Token.GetRange(GetCurrentLine(), 0, typeDeclIndex), "");

                int semiColonIndex = Token.IndexOfType(GetCurrentLine(), LexerTypes.SemiColon);
                string type = Token.SmashTokens(Token.RemoveBeginWhiteSpace(Token.GetRange(GetCurrentLine(), typeDeclIndex + 1, semiColonIndex)), "");

                PField field;
                field.name = name;
                field.type = type;
                field.deco = decoratorBuffer;

                if (inStruct)
                    structFieldBuffer.Add(field);

            }
        }

        private void DetectDecorator()
        {
            bool hasDeco = Token.IncludesType(GetCurrentLine(), LexerTypes.Decorator);

            if (hasDeco)
            {
                string deco = Token.SmashTokens(Token.RemoveBeginWhiteSpace(Token.GetRange(GetCurrentLine(), 1, GetCurrentLine().Length)), "");
                decoratorBuffer = deco;
                Console.WriteLine("new deco : " + deco);
            }
        }

        private void DetectEnumEntry()
        {
            bool hasComma = Token.IncludesType(GetCurrentLine(), LexerTypes.Comma);
            LexerTypes[] includes = { LexerTypes.Comma, LexerTypes.Space, LexerTypes.Undefined };
            bool hasNothingElse = Token.TokensOnlyInclude(GetCurrentLine(), includes);

            if (hasNothingElse || hasComma)
            {
                Console.WriteLine("Detected an enum entry.");
            }
        }

        private void DetectEndOfScope()
        {
            bool hasCloseCurly = Token.IncludesType(GetCurrentLine(), LexerTypes.CloseCurly);

            if (hasCloseCurly)
            {
                Console.WriteLine("detected end of scope");

                if (inStruct)
                {
                    Console.WriteLine("ending struct scope");
                    activeStruct.pFields = (PField[])structFieldBuffer.ToArray(typeof(PField));

                    Console.WriteLine(PStruct.ToString(activeStruct));
                }

                inEnum = false;
                inStruct = false;

                

            }
        }


    }
      

}
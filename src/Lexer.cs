using System;
using System.Collections;
using DataKeep.Tokens;

namespace DataKeep
{
  
    enum LexerTypes
    {
        Struct,
        Enum,
        Inheritance,
        TypeDecl,
        Preprocess,
        Undefined,
        Decorator,
        Space,
        SemiColon,
        OpenParen,
        CloseParen,
        OpenCurly,
        CloseCurly,
        Abstract,
        Comma
    }
    

    class Lexer
    {

        public FileHandler fileHandler;

        public int currentLine = 0;     // lines of fielhandler
        public int lineIndex = 0;       // charachter indices

        private Hashtable keywordsTypes;

        public Token[][] fileTokens;

        public Lexer(FileHandler fh)
        {
            fileHandler = fh;
            fh.LoadFile();

            keywordsTypes = new Hashtable();
            AddKeywords();
        }

        public void AddKeywords()
        {
            keywordsTypes.Add("struct", LexerTypes.Struct);
            keywordsTypes.Add("abstract", LexerTypes.Abstract);
            keywordsTypes.Add("enum", LexerTypes.Enum);
            keywordsTypes.Add(":", LexerTypes.TypeDecl);
            keywordsTypes.Add("->", LexerTypes.Inheritance);
            keywordsTypes.Add("#", LexerTypes.Preprocess);
            keywordsTypes.Add(" ", LexerTypes.Space);
            keywordsTypes.Add(";", LexerTypes.SemiColon);
            keywordsTypes.Add("(", LexerTypes.OpenParen);
            keywordsTypes.Add(")", LexerTypes.CloseParen);
            keywordsTypes.Add("{", LexerTypes.OpenCurly);
            keywordsTypes.Add("}", LexerTypes.CloseCurly);
            keywordsTypes.Add("@", LexerTypes.Decorator);
            keywordsTypes.Add(",", LexerTypes.Comma);

        }

        public void LexAllLines()
        {
            ArrayList tokens = new ArrayList();

            while (true)
            {
                if (currentLine >= fileHandler.fileLines.Length)
                    break;

                tokens.Add(Token.RemoveBeginWhiteSpace(LexCurrentLine()));
                currentLine++;

            }

            fileTokens = (Token[][])tokens.ToArray(typeof(Token[]));
        }

        private Token[] LexCurrentLine()
        {
            ArrayList tokens = new ArrayList();

            while(true)
            {
                if (IsEof())
                    break;

                Token nToken = GetNextToken();
                tokens.Add(nToken);

                lineIndex += nToken.value.Length;
            }
            lineIndex = 0;

            return (Token[]) tokens.ToArray(typeof(Token));    
        }

        public Token GetNextToken()
        {
            Token nextToken;
            string nextSymbol = GetNextSymbol();

            if (nextSymbol == null)
            {
                nextToken.value = GetCurrentLine()[lineIndex].ToString();
                nextToken.type = LexerTypes.Undefined;
            } else
            {
                nextToken.value = nextSymbol;
                nextToken.type = (LexerTypes) keywordsTypes[nextSymbol];
            }

            return nextToken;
        }

        public string GetNextSymbol()
        {
            foreach(DictionaryEntry de in keywordsTypes)
            {
                if (StringFitsInLine(de.Key.ToString()))
                {
                    if (NextStringIs(de.Key.ToString()))
                    {
                        return de.Key.ToString();
                    }
                }
            }
            return null;
        }       

        private string GetCurrentLine()
        {
            return fileHandler.fileLines[currentLine];
        }

        private bool IsEof()
        {
            return lineIndex >= fileHandler.fileLines[currentLine].Length;
        }

        private bool StringFitsInLine(string rem)
        {
            return GetCurrentLine().Length - lineIndex >= rem.Length;
        }

        private bool NextStringIs(string rem)
        {
            bool fits = true;
            for (int i = 0; i< rem.Length; i++)
            {
                fits = rem[i] == GetCurrentLine()[lineIndex + i] && fits;
            }
            return fits;
        }


        public static void PrintToken(Token token)
        {
            Console.WriteLine("Token(" + token.value + "," + token.type + ")");
        }
    }

}
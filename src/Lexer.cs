using System;
using System.Collections;

using DataKeep.Tokens;

namespace DataKeep
{

    class Lexer
    {

        public FileHandler fileHandler;

        public int currentLine = 0;     // lines of filehandler
        public int lineIndex = 0;       // charachter indices

        private Hashtable keywordTypes;

        public Token[][] fileTokens;

        public Lexer(FileHandler fh)
        {
            fileHandler = fh;
            fh.LoadFile();

            keywordTypes = new Hashtable();
            AddKeywords();

        }

        public void AddKeywords()
        {

            keywordTypes.Add("struct", TokenTypes.Struct);
            keywordTypes.Add("abstract", TokenTypes.Abstract);
            keywordTypes.Add("enum", TokenTypes.Enum);
            keywordTypes.Add(":", TokenTypes.TypeDecl);
            keywordTypes.Add("->", TokenTypes.Inheritance);
            keywordTypes.Add("#", TokenTypes.Preprocess);
            keywordTypes.Add(" ", TokenTypes.Space);
            keywordTypes.Add(";", TokenTypes.SemiColon);
            keywordTypes.Add("(", TokenTypes.OpenParen);
            keywordTypes.Add(")", TokenTypes.CloseParen);
            keywordTypes.Add("{", TokenTypes.OpenCurly);
            keywordTypes.Add("}", TokenTypes.CloseCurly);
            keywordTypes.Add("@", TokenTypes.Decorator);
            keywordTypes.Add(",", TokenTypes.Comma);

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

            lineIndex = 0; // resest for next line

            return (Token[]) tokens.ToArray(typeof(Token));    
        }

        public Token GetNextToken()
        {
            Token nextToken;
            string nextSymbol = GetNextSymbol();

            if (nextSymbol == null)
            {
                nextToken.value = GetCurrentLine()[lineIndex].ToString();
                nextToken.type = TokenTypes.Undefined;
            } else
            {
                nextToken.value = nextSymbol;
                nextToken.type = (TokenTypes) keywordTypes[nextSymbol];
            }

            return nextToken;
        }

        public string GetNextSymbol()
        {
            foreach(DictionaryEntry de in keywordTypes)
            {
                string key = de.Key.ToString();

                if (StringFitsInLine(key))
                    if (NextStringIs(key))
                        return key;

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
                fits = rem[i] == GetCurrentLine()[lineIndex + i] && fits;
            
            return fits;
        }
    }

}
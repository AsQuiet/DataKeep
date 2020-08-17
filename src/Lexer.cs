using System;
using System.Collections;

using DataKeep.Tokens;

namespace DataKeep
{

    class Lexer
    {
        public FileHandler fileHandler;
        public Token[][] fileTokens;

        public int currentLine = 0;     // lines of filehandler
        public int lineIndex = 0;       // charachter indices

        private Hashtable keywordTypes;

        public Lexer(FileHandler fh)
        {
            DebugDK.SetPrefix("Lexer");
            DebugDK.Log("Creating lexer. \n[Lexer] Loading file.");

            fileHandler = fh;
            fh.LoadFile();

            DebugDK.Log("Creating Hashtable");
            InitHastable();
        }

        internal void InitHastable()
        {
            keywordTypes = new Hashtable()
            {
                {"struct" , TokenTypes.Struct },
                {"->", TokenTypes.Inheritance },
                {":", TokenTypes.TypeDecl },
                {"#", TokenTypes.Preprocess },
                {" ", TokenTypes.Space },
                {"(", TokenTypes.OpenParen },
                {")", TokenTypes.CloseParen },
                {"{", TokenTypes.OpenCurly },
                {"}", TokenTypes.CloseCurly },
                {"@", TokenTypes.Tag },
                {",", TokenTypes.Comma },
                {";", TokenTypes.SemiColon },
            };
        }

        public void LexAllLines()
        {
            ArrayList tokens = new ArrayList();

            DebugDK.Print("Reading and extracting tokens from file : " + fileHandler.path);
            DebugDK.Log("Lexing all lines.");

            while (true)
            {
                if (currentLine >= fileHandler.fileLines.Length)
                    break;

                tokens.Add(Token.RemoveBeginWhiteSpace(LexCurrentLine()));
                currentLine++;

                //Debug.Log("Lexing next line, current state is : ");
                //Debug.Log("     currentLine : " + currentLine + ", lineIndex : " + lineIndex);

            }

            fileTokens = (Token[][])tokens.ToArray(typeof(Token[]));
            DebugDK.Log("Lexer is done.");
        }

        private Token[] LexCurrentLine()
        {
            ArrayList tokens = new ArrayList();

            while(true)
            {
                if (IsEof())
                    break;

                //Debug.Log("Extracting all tokens from current line.");
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
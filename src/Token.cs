using System;
using DataKeep;
using System.Collections;
using System.Threading.Channels;

namespace DataKeep.Tokens
{

    struct Token
    {
        public string value;
        public LexerTypes type;

        public static bool IncludesType(Token[] tokens, LexerTypes type)
        {
            foreach(Token t in tokens)
            {
                if (t.type == type)
                    return true;
            }
            return false;
        }

        public static Token[] GetRange(Token[] tokens, int start, int stop)
        {
            ArrayList result = new ArrayList();

            for (int i = 0; i < tokens.Length; i++)
            {
                if (i >= start && i < stop)
                    result.Add(tokens[i]);
            }

            return (Token[])result.ToArray(typeof(Token)); 
        }

        public static string SmashTokens(Token[] tokens, string sep)
        {
            string result = "";
            foreach (Token s in tokens)
                result += s.value;
            return result;
        }

        public static Token[] RemoveBeginWhiteSpace(Token[] tokens)
        {
            ArrayList result = new ArrayList();

            bool add = false;
            foreach(Token t in tokens)
            {
                if (t.type != LexerTypes.Space)
                    add = true;
                if (add)
                    result.Add(t);
            }

            return (Token[])result.ToArray(typeof(Token));
        }

        public static bool TokensOnlyInclude(Token[] tokens, LexerTypes[] includes)
        {
            foreach(Token t in tokens)
            {
                bool isIncluded = false;

                foreach(LexerTypes lt in includes)
                {
                    isIncluded = isIncluded || lt == t.type;
                }

                if (!isIncluded)
                    return false;

            }
            return true;
        }

        public static int IndexOfType(Token[] tokens, LexerTypes lt)
        {
            for (int i = 0;i<tokens.Length; i++)
            {
                if (tokens[i].type == lt)
                    return i;
            }
            return -1;
        }

        public static Token[] StartWithChar(Token[] tokens)
        {
            ArrayList result = new ArrayList();
            bool hasFoundCharacter = false;
             
            foreach(Token t in tokens)
            {
                if (hasFoundCharacter)
                    result.Add(t);
                else
                {
                    /*bool isEmpty = true;
                    foreach (char c in t.value)
                        isEmpty = isEmpty && (c.Equals(" ") || c.Equals(""));*/
                    hasFoundCharacter = !Token.IsEmpty(t.value);
                }
                
            }

            return (Token[])result.ToArray(typeof(Token));
        }
            
        // same this as RemoveBeginWhiteSpace, but for strings...
        public static string RemoveBeginSpaces(string s)
        {
            string result = "";

            bool add = false;

            foreach(char c in s)
            {
                if (!c.Equals(' ') && !add)
                    add = true;
                if (add)
                    result += c;
            }

            return result;
        }

        public static bool IsEmpty(string s)
        {
            bool isEmpty = true;
            foreach (char c in s)
                isEmpty = isEmpty && (c.Equals(" ") || c.Equals(""));
            return isEmpty;
        }
    }

    /*


    ArrayList result = new ArrayList();

    bool add = false;
            foreach(Token t in tokens)
            {
                if (t.type != LexerTypes.Space)
                    add = true;
                if (add)
                    result.Add(t);*/
}

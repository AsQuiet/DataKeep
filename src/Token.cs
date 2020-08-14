using System;
using DataKeep;
using System.Collections;

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

    }


    
}
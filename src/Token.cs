using System;
using System.Collections;


namespace DataKeep.Tokens
{
    enum TokenTypes
    {
        Struct,
        Enum,
        Inheritance,
        TypeDecl,
        Preprocess,
        Undefined,
        Tag,
        Space,
        SemiColon,
        OpenParen,
        CloseParen,
        OpenCurly,
        CloseCurly,
        Abstract,
        Comma,
        Flag,
        Def,
    }

    struct Token
    {
        public string value;
        public TokenTypes type;

        public static string ToString(Token t)
        {
            return "Token(" + t.value + ", " + t.type + ")";
        }

        public static void PrintToken(Token t)
        {
            Console.WriteLine(Token.ToString(t));
        }

        public static bool IncludesType(Token[] tokens, TokenTypes type)
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
                if (t.type != TokenTypes.Space)
                    add = true;
                if (add)
                    result.Add(t);
            }

            return (Token[])result.ToArray(typeof(Token));
        }

        public static bool TokensOnlyInclude(Token[] tokens, TokenTypes[] includes)
        {
            foreach(Token t in tokens)
            {
                bool isIncluded = false;

                foreach(TokenTypes lt in includes)
                {
                    isIncluded = isIncluded || lt == t.type;
                }

                if (!isIncluded)
                    return false;

            }
            return true;
        }

        public static int IndexOfType(Token[] tokens, TokenTypes lt)
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

                if (!hasFoundCharacter)
                    hasFoundCharacter = !Token.IsEmpty(t.value);
                if (hasFoundCharacter)
                    result.Add(t);
                
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

        public static string[] ConvertToArray(string s)
        {
            ArrayList result = new ArrayList();
            string current = "";

            foreach (char c in s)
            {
                if (c.Equals(' '))
                    if (!(current == "" || current == " "))
                    {
                        result.Add(current);
                        current = "";
                    }

                if (!c.Equals(' '))
                    current += c;
           
            }

            if (current != "")
                result.Add(current);

            return (string[])result.ToArray(typeof(string));
        }

        public static bool IsEmpty(string s)
        {
            bool isEmpty = true;
            foreach (char c in s)
                isEmpty = isEmpty && (c.Equals(" ") || c.Equals(""));
            return isEmpty;
        }

        public static void PrintStringArr(ref string[] strings)
        {
            foreach (string s in strings)
                Console.WriteLine(s);
        }

        public static string StringArrToString(ref string[] strings, string sep)
        {
            string result = "";

            for (int i = 0; i < strings.Length; i++)
            {
                result += strings[i];
                if (i + 1 != strings.Length)
                    result += sep;
            }
            return result;
        }
    }


}

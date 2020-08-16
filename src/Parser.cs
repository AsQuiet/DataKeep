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

        public ArrayList structs = new ArrayList();

        private bool inStruct = false;

        private PStruct activeStruct;
        private ArrayList fieldBuffer;

        private string decoratorBuffer = "";

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
        }

        public void PrintAllData()
        {
            Console.WriteLine("\nWriting All The Data.");

            foreach (PStruct ps in structs)
                Console.WriteLine(PStruct.ToString(ps));
        
        }

        private Token[] GetCurrentLine()
        {
            return lexer.fileTokens[currentLine];
        }

        public void ParseAllLines()
        {
            Console.WriteLine("parsing all lines");
            while (true)
            {
                if (currentLine >= lexer.fileTokens.Length)
                    break;

                ParseCurrentLine();
            }
            Console.WriteLine("parsing is done");
        }

        public void ParseCurrentLine()
        {
            DetectStruct();
            DetectField();
            DetectEndOfScope();
            DetectDecorator();

            currentLine += 1;
        }


        private void DetectStruct()
        {
            bool hasStruct = Token.IncludesType(GetCurrentLine(), TokenTypes.Struct);
            bool noSemiColon = !Token.IncludesType(GetCurrentLine(), TokenTypes.SemiColon);
            bool hasInheritance = Token.IncludesType(GetCurrentLine(), TokenTypes.Inheritance);

            if ((hasStruct && noSemiColon) || hasInheritance)
            {

                Console.WriteLine("Detected a struct");
                inStruct = true;

                string inheritance = "";

                int start = Token.IndexOfType(GetCurrentLine(), TokenTypes.Struct) + 1;
                int stop;

                if (hasInheritance)
                {
                    stop = Token.IndexOfType(GetCurrentLine(), TokenTypes.Inheritance);
                    inheritance = Token.SmashTokens(Token.GetRange(GetCurrentLine(), stop + 1, GetCurrentLine().Length), "");
                }
                else
                    stop = GetCurrentLine().Length;

                string name = Token.SmashTokens(Token.GetRange(GetCurrentLine(), start, stop), "");

                activeStruct.name = name;
                activeStruct.inheritance = inheritance;
                activeStruct.tags = GetTags();
                fieldBuffer = new ArrayList();        // reset fields

            }

        }

        private void DetectField()
        {
            bool hasTypeDecl = Token.IncludesType(GetCurrentLine(), TokenTypes.TypeDecl);
            bool hasSemiColon = Token.IncludesType(GetCurrentLine(), TokenTypes.SemiColon);

            if (hasTypeDecl && hasSemiColon)
            {

                int typeDeclIndex = Token.IndexOfType(GetCurrentLine(), TokenTypes.TypeDecl);
                string name = Token.SmashTokens(Token.GetRange(GetCurrentLine(), 0, typeDeclIndex), "");

                int semiColonIndex = Token.IndexOfType(GetCurrentLine(), TokenTypes.SemiColon);
                string type = Token.SmashTokens(Token.GetRange(GetCurrentLine(), typeDeclIndex + 1, semiColonIndex), "");

                PField field;
                field.name = name;
                field.type = type;
                field.tags = GetTags();

                if (inStruct)
                    fieldBuffer.Add(field);

            }
        }

        private void DetectDecorator()
        {
            if (decoratorBuffer != "")
                decoratorBuffer = "";

            bool hasDeco = Token.IncludesType(GetCurrentLine(), TokenTypes.Tag);

            if (hasDeco)
            {
                decoratorBuffer = Token.SmashTokens(Token.GetRange(GetCurrentLine(), 1, GetCurrentLine().Length), "");
                Console.WriteLine("new deco : " + decoratorBuffer);
            }
        }

        private void DetectEndOfScope()
        {
            bool hasCloseCurly = Token.IncludesType(GetCurrentLine(), TokenTypes.CloseCurly);
    
            if (hasCloseCurly)
            {
                Console.WriteLine("detected end of scope");

                if (inStruct)
                {
                    Console.WriteLine("adding the struct");
                    activeStruct.fields = (PField[])fieldBuffer.ToArray(typeof(PField)); // adding the fields
                    structs.Add(activeStruct);
                }

                inStruct = false;
            }
        }

        public static string[] ExtractArguments(string s, bool addDefault)
        {
            ArrayList result = new ArrayList();

            string currentArg = "";
            bool inArg = false;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Equals(')') && inArg)
                    inArg = false;

                if (inArg && !s[i].Equals(','))
                    currentArg += s[i];

                if (inArg && s[i].Equals(','))
                {
                    result.Add(Token.RemoveBeginSpaces(currentArg));
                    currentArg = "";
                }

                if (s[i].Equals('(') && !inArg)
                    inArg = true;

            }
            result.Add(Token.RemoveBeginSpaces(currentArg));

            if (addDefault)
                result.Add("default");

            return (string[])result.ToArray(typeof(string));
        }

        private string[] GetTags()
        {
            string[] result = { "default" };

            if (decoratorBuffer == "")
                return result;

            result = ExtractArguments(decoratorBuffer, true);
            return result;
        }

        public void GiveStructInheritance()
        {
            ArrayList newStructs = new ArrayList();

            for (int i = 0; i < structs.Count; i++)
            {
                PStruct currentStruct = (PStruct)structs[i];

                if (!Token.IsEmpty(currentStruct.inheritance))
                {
                    Console.WriteLine("Looking for correct struct...");

                    // NOTE: change removebeginwhitespace function at the inheritance detection if problems with finding.
                    foreach (PStruct pStruct in structs)
                    {
                        Console.WriteLine("searching for match : " + pStruct.name + "; with " + currentStruct.inheritance);
                        if (pStruct.name.Contains(currentStruct.inheritance))
                        {
                            ArrayList newField = new ArrayList();

                            foreach (PField p in pStruct.fields)
                                newField.Add(p);
                            foreach (PField p in currentStruct.fields)
                                newField.Add(p);

                            currentStruct.fields = (PField[])newField.ToArray(typeof(PField));

                        }

                    }

                }

                newStructs.Add(currentStruct);

            }

            structs = newStructs;


        }



    }




}

using System;
using System.Collections;

using DataKeep.ParserTypes;
using DataKeep.Tokens;
using System.Diagnostics;

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
            DebugDK.SetPrefix("Parser");
        }

        public void PrintAllData()
        {
            if (DebugDK.log)
            {
                DebugDK.Log("\nWriting out all of the resulting parser data (PStructs) : ");

                foreach (PStruct ps in structs)
                    Debug.WriteLine(PStruct.ToString(ps));
            }
        }

        private Token[] GetCurrentLine()
        {
            return lexer.fileTokens[currentLine];
        }

        public void ParseAllLines()
        {
            DebugDK.Log("Starting parser");

            while (true)
            {
                if (currentLine >= lexer.fileTokens.Length)
                    break;

                ParseCurrentLine();

                //Debug.Log("Parsing next line, current state is : \n      " + " tagBuffer : " + decoratorBuffer + ", inStruct : " + inStruct + ", currentLine : " + currentLine);

            }

            DebugDK.Log("Parser is done.");
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
                DebugDK.Log("Detected a struct in the line : '" + Token.SmashTokens(GetCurrentLine(), "") + "'" );
                
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

                DebugDK.Log("Extracted from line, structname : " + name + ", inheritance : " + inheritance + ", tags : " + Token.StringArrToString(ref activeStruct.tags, "-"));

            }

        }

        private void DetectField()
        {
            bool hasTypeDecl = Token.IncludesType(GetCurrentLine(), TokenTypes.TypeDecl);
            bool hasSemiColon = Token.IncludesType(GetCurrentLine(), TokenTypes.SemiColon);

            if (hasTypeDecl && hasSemiColon)
            {

                DebugDK.Log("Detected a field in the line : '" + Token.SmashTokens(GetCurrentLine(), "") + "'");

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

                DebugDK.Log("Extracted from line, fieldname : " + name + ", fieldtype: " + type+ ", tags : " + Token.StringArrToString(ref field.tags, "-") + ", adding field to fieldBuffer : " + inStruct);
            }
        }

        private void DetectDecorator()
        {
            if (decoratorBuffer != "")
                decoratorBuffer = "";

            bool hasDeco = Token.IncludesType(GetCurrentLine(), TokenTypes.Tag);

            if (hasDeco)
            {
                DebugDK.Log("Detected a tag in the line : '" + Token.SmashTokens(GetCurrentLine(), "") + "'");

                decoratorBuffer = Token.SmashTokens(Token.GetRange(GetCurrentLine(), 1, GetCurrentLine().Length), "");

                DebugDK.Log("New tag found : '" + decoratorBuffer);
            }
        }

        private void DetectEndOfScope()
        {
            bool hasCloseCurly = Token.IncludesType(GetCurrentLine(), TokenTypes.CloseCurly);
    
            if (hasCloseCurly)
            {
                DebugDK.Log("Detected an end of scope in line : '" + Token.SmashTokens(GetCurrentLine(), "") + "'");
                DebugDK.Log("Adding a struct : " + inStruct);
                if (inStruct)
                {
                    DebugDK.Log("Adding the fieldBuffer to currentStruct.");
                    activeStruct.fields = (PField[])fieldBuffer.ToArray(typeof(PField)); // adding the fields
                    DebugDK.Log("Adding the currentStruct to structs.");
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

            DebugDK.Log("Giving each struct its inheritant fields.");

            DebugDK.Log("Looping to all the structs.");
            for (int i = 0; i < structs.Count; i++)
            {
                PStruct currentStruct = (PStruct)structs[i];

                DebugDK.Log("Checking if this struct has inheritance..., struct is : " + currentStruct.name);
                if (!Token.IsEmpty(currentStruct.inheritance))
                {
                    DebugDK.Log("Current struct has inheritance : " + currentStruct.inheritance);
                    DebugDK.Log("Looping through all the structs to find a match...");

                    // NOTE: change removebeginwhitespace function at the inheritance detection if problems with finding.
                    foreach (PStruct pStruct in structs)
                    {
                        DebugDK.Log("     Looking for match with struct " + pStruct.name);
                        if (pStruct.name.Contains(currentStruct.inheritance))
                        {
                            DebugDK.Log("     Match found. Adding fields.");
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


            DebugDK.Log("Struct inheritance is done.");
        }



    }




}

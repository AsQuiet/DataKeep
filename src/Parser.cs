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

        public Hashtable defs = new Hashtable();
        public Hashtable flags = new Hashtable();

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
                    Console.WriteLine(PStruct.ToString(ps));

                Console.WriteLine("Definitions : ");
                foreach (DictionaryEntry e in defs)
                    Console.WriteLine(e.Key + " : " + defs[e.Key]);
                Console.WriteLine("Flags : ");
                foreach (DictionaryEntry e in flags)
                    Console.WriteLine(e.Key + " : " + flags[e.Key]);

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
            DetectCommand();

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

                activeStruct.name = RemoveWhitespace(name);
                activeStruct.inheritance = RemoveWhitespace(inheritance);
                activeStruct.tags = GetTags();
                fieldBuffer = new ArrayList();        // reset fields

                DebugDK.Log("Extracted from line, structname : " + activeStruct.name + ", inheritance : " + activeStruct.inheritance + ", tags : ");

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
                field.name = RemoveWhitespace(name);
                field.type = GetFromHashtable(RemoveWhitespace(type), ref defs);
                field.tags = GetTags();

                if (inStruct)
                    fieldBuffer.Add(field);

                DebugDK.Log("Extracted from line, fieldname : " + field.name + ", fieldtype: " + field.type+ ", tags : " +", adding field to fieldBuffer : " + inStruct);
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

        private void DetectCommand()
        {
            bool hasFlag = Token.IncludesType(GetCurrentLine(), TokenTypes.Flag);
            string[] cmd = Token.ConvertToArray(Token.SmashTokens(GetCurrentLine(), ""));
            
            Token.PrintStringArr(ref cmd);
            if (hasFlag)
            {
                flags[cmd[1]] = cmd[2];
            }

            bool hasDef = Token.IncludesType(GetCurrentLine(), TokenTypes.Def);

            if (hasDef)
            {
                defs[cmd[1]] = '"' + cmd[2] + '"';
            }
        }


        public string[] ExtractArguments(string s, bool addDefault)
        {
            ArrayList result = new ArrayList();

            string currentArg = "";
            bool inArg = false;


            for (int i = 0; i < s.Length; i++)
            {
                char current = s[i];

                if (current.Equals(')') && inArg)
                    inArg = false;

                if (!current.Equals(',') && inArg)
                    currentArg += current;

                if ((current.Equals(',') || i == s.Length - 2) && inArg)
                {
                    if (!currentArg.Contains('$'))
                        currentArg = GetFromHashtable(RemoveWhitespace(currentArg), ref flags);
                    //else
                    //    currentArg = currentArg.Replace("$", "");
                    result.Add((currentArg));
                    currentArg = "";
                }

                if (current.Equals('(') && !inArg)
                    inArg = true;

            }
            

            if (addDefault)
                result.Add("default");

            return (string[])result.ToArray(typeof(string));
        }

        public PTag[] ExtractTags(string s, bool addDefault)
        {
            ArrayList result = new ArrayList();

            Console.WriteLine("extracting tags from " + s);

            string currentTagName = "";
            PTag currentTag;

            string argString = "";
            bool inArguments = false;

            string[] tagArgs = { };

            for (int i = 0; i < s.Length; i++)
            {
                char current = s[i];

                {   // argument checking

                    if (current.Equals('('))    // entering 
                    {
                        inArguments = true;
                    }

                    if (inArguments)            // adding
                    {
                        argString += current;
                    }

                    if (current.Equals(')'))    // extracting & reset
                    {
                        inArguments = false;
                        tagArgs = ExtractArguments(argString, false);

                        Console.WriteLine("found arguments : ");
                        Token.PrintStringArr(ref tagArgs);

                        argString = "";

                    }
                }

                if (!inArguments)
                {

                    if (!current.Equals(')') && !current.Equals(','))
                    {
                        currentTagName += current;
                    }

                    if ((current.Equals(',') || i == s.Length - 1))
                    {
                        Console.WriteLine("moving on to next tag, current name is " + currentTagName);

                        currentTag.name = RemoveWhitespace(currentTagName);
                        currentTag.arguments = tagArgs;

                        result.Add(currentTag);

                        {
                            currentTagName = "";
                            string[] temp = { };
                            tagArgs = temp;
                        }

                    }

                }

                

            }


            return (PTag[])result.ToArray(typeof(PTag));
        }

        private PTag[] GetTags()
        {
            
            //PTag defaultTag;
            //defaultTag.name = "default";
            //string[] temp = { };
            //defaultTag.arguments = temp;

            PTag[] defaultTs = { };

            return (decoratorBuffer == "") ? defaultTs : ExtractTags(decoratorBuffer, true);
        }

        public static string RemoveWhitespace(string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        public void GiveStructInheritance()
        {
            ArrayList newStructs = new ArrayList();

            DebugDK.Log("Giving each struct its inheritant fields.");

            DebugDK.Log("Looping to all the structs.");
            for (int i = 0; i < structs.Count; i++)
            {
                PStruct currentStruct = (PStruct)structs[i];

                DebugDK.Log("Checking if this struct has inheritance..., struct is : " + currentStruct.name + " |" + i);
                if (currentStruct.inheritance != "")
                {
                    DebugDK.Log("Current struct has inheritance : " + currentStruct.inheritance);
                    DebugDK.Log("Looping through all the structs to find a match...");

                    // NOTE: change removebeginwhitespace function at the inheritance detection if problems with finding.
                    foreach (PStruct pStruct in structs)
                    {
                        DebugDK.Log("     Looking for match with struct " + pStruct.name);
                        if (pStruct.name == currentStruct.inheritance)
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

        private string GetFromHashtable(string s, ref Hashtable h)
        {
            if (h.ContainsKey(s))
                return (string) h[s];
            return s;
        }


    }




}

using System;
using System.Collections;

using DataKeep.ParserTypes;
using DataKeep.Tokens;

namespace DataKeep
{

    struct CompileSettings
    {
        public string namespaceName { get; set; }
        public string[] usingNames;
        public string outputFileName;

        public static string ToString(CompileSettings cs)
        {
            string s =  "CompileSettings(" + cs.namespaceName + ", " + cs.outputFileName + ", using : [";
            foreach (string c in cs.usingNames)
                s += c + ", ";
            return s + "])";
        }

    }

    class Parser
    {

        private Lexer lexer;
        private int currentLine = 0;

        public ArrayList pStructs = new ArrayList();
        public ArrayList pEnums = new ArrayList();

        private bool inStruct = false;
        private bool inEnum = false;

        private PStruct activeStruct;
        private PEnum activeEnum;
        private ArrayList enumEntryBuffer;
        private ArrayList structFieldBuffer;

        private string decoratorBuffer = "";

        public CompileSettings compileSettings;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            SetCompileSettingsDefaults();

        }

        public void SetCompileSettingsDefaults()
        {
            compileSettings.namespaceName = "DataKeep.Output";
            compileSettings.outputFileName = "DataKeepOutput.cs";
            string[] temp = { "System" };
            compileSettings.usingNames = temp;
        }

        public void PrintAllData()
        {
            Console.WriteLine("\nWriting All The Data.");

            foreach (PStruct ps in pStructs)
                Console.WriteLine(PStruct.ToString(ps));

            foreach (PEnum pe in pEnums)
                Console.WriteLine(PEnum.ToString(pe));
        }

        public void PrintCompileSettings()
        {
            Console.WriteLine(CompileSettings.ToString(compileSettings));
        }

        private Token[] GetCurrentLine()
        {
            return lexer.fileTokens[currentLine];
        }

        public void ParserAllLines()
        {
            Console.WriteLine("parsing all lines");
            while (true)
            {
                if (currentLine >= lexer.fileTokens.Length)
                    break;

                if (DetectCommand())
                    currentLine++;
                else
                    ParseCurrentLine();

                //Console.WriteLine("currentlineparseDone[in struct : " + inStruct + "; inEnum : " + inEnum + "; current deco: " + decoratorBuffer + "]");
            }
            Console.WriteLine("parsing is done");
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
            currentLine += 1;
        }

        //
        //      DETECTION FUNCTIONS
        //
        private void DetectStruct()
        {
            bool hasStruct = Token.IncludesType(GetCurrentLine(), TokenTypes.Struct);
            bool noSemiColon = !Token.IncludesType(GetCurrentLine(), TokenTypes.SemiColon);
            bool hasInheritance = Token.IncludesType(GetCurrentLine(), TokenTypes.Inheritance);

            if ((hasStruct && noSemiColon) || hasInheritance)
            {

                Console.WriteLine("Detected a struct");
                inStruct = true;

                string name = "";
                string inheritance = "";

                int start = Token.IndexOfType(GetCurrentLine(), TokenTypes.Struct) + 1;
                int stop;

                if (hasInheritance) {
                    stop = Token.IndexOfType(GetCurrentLine(), TokenTypes.Inheritance);
                    inheritance = Token.SmashTokens(Token.RemoveBeginWhiteSpace(Token.GetRange(GetCurrentLine(), stop + 1, GetCurrentLine().Length)), "");
                } else
                    stop = GetCurrentLine().Length;

                name = Token.SmashTokens(Token.RemoveBeginWhiteSpace(Token.GetRange(GetCurrentLine(), start, stop)), "");
               
                activeStruct.name = name;
                activeStruct.inheritance = inheritance;
                activeStruct.decorators = GetDecorators();
                structFieldBuffer = new ArrayList();

            }

        }

        private void DetectEnum()
        {
            bool hasEnum = Token.IncludesType(GetCurrentLine(), TokenTypes.Enum);
            bool noSemiColon = !Token.IncludesType(GetCurrentLine(), TokenTypes.SemiColon);

            if (hasEnum && noSemiColon)
            {
                Console.WriteLine("Detected an enum");
                inEnum = true;

                int start = Token.IndexOfType(GetCurrentLine(), TokenTypes.Enum) + 1;
                int end = GetCurrentLine().Length;
                string name = Token.SmashTokens(Token.RemoveBeginWhiteSpace(Token.GetRange(GetCurrentLine(), start, end)), "");

                activeEnum.name = name;
                activeEnum.decorators = GetDecorators();
                enumEntryBuffer = new ArrayList();

            }

        }

        private void DetectField()
        {
            bool hasTypeDecl = Token.IncludesType(GetCurrentLine(), TokenTypes.TypeDecl);
            bool hasSemiColon = Token.IncludesType(GetCurrentLine(), TokenTypes.SemiColon);

            if (hasTypeDecl && hasSemiColon)
            {

                int typeDeclIndex = Token.IndexOfType(GetCurrentLine(), TokenTypes.TypeDecl);
                string name = Token.SmashTokens(Token.StartWithChar(Token.GetRange(GetCurrentLine(), 0, typeDeclIndex)), "");

                int semiColonIndex = Token.IndexOfType(GetCurrentLine(), TokenTypes.SemiColon);
                string type = Token.SmashTokens(Token.RemoveBeginWhiteSpace(Token.GetRange(GetCurrentLine(), typeDeclIndex + 1, semiColonIndex)), "");

                PField field;
                field.name = name;
                field.type = type;
                field.decorators = GetDecorators();

                if (inStruct)
                    structFieldBuffer.Add(field);

            }
        }

        private void DetectDecorator()
        {
            bool hasDeco = Token.IncludesType(GetCurrentLine(), TokenTypes.Decorator);

            if (hasDeco)
            {
                string deco = Token.SmashTokens(Token.RemoveBeginWhiteSpace(Token.GetRange(GetCurrentLine(), 1, GetCurrentLine().Length)), "");
                decoratorBuffer = deco;
                Console.WriteLine("new deco : " + deco);
            }
        }

        private void DetectEnumEntry()
        {
            bool hasComma = Token.IncludesType(GetCurrentLine(), TokenTypes.Comma);
            TokenTypes[] includes = { TokenTypes.Comma, TokenTypes.Space, TokenTypes.Undefined };
            bool hasNothingElse = Token.TokensOnlyInclude(GetCurrentLine(), includes);
            bool hasDeco = Token.IncludesType(GetCurrentLine(), TokenTypes.Decorator);

            if ((hasNothingElse || hasComma) && GetCurrentLine().Length != 0 && !hasDeco)
            {
                Console.WriteLine("Detected an enum entry.");
                string entry = "";

                int stop = GetCurrentLine().Length;
                if (hasComma)
                    stop = Token.IndexOfType(GetCurrentLine(), TokenTypes.Comma);

                entry = Token.SmashTokens(Token.StartWithChar(Token.GetRange(GetCurrentLine(), 0, stop)), "");
                enumEntryBuffer.Add(entry);
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
                    activeStruct.pFields = (PField[])structFieldBuffer.ToArray(typeof(PField)); // adding the fields
                    pStructs.Add(activeStruct);
                }

                if (inEnum)
                {
                    activeEnum.entries = (string[])enumEntryBuffer.ToArray(typeof(string));
                    pEnums.Add(activeEnum);
                }

                inEnum = false;
                inStruct = false;

            }
        }

        private bool DetectCommand()
        {
            bool hasCommandSymbol = Token.IncludesType(GetCurrentLine(), TokenTypes.Preprocess);

            if (hasCommandSymbol)
            {
                string command = "";

                int start = Token.IndexOfType(GetCurrentLine(), TokenTypes.Preprocess);
                int end = GetCurrentLine().Length;
                command = Token.SmashTokens(Token.RemoveBeginWhiteSpace(Token.GetRange(GetCurrentLine(), start, end)), "");
                HandleCommand(command);
            }

            return hasCommandSymbol;

        }

        //
        //      COMMAND FUNCTIONS
        //

        private void HandleCommand(string command)
        {
            string[] cmds = Token.ConvertToArray(command);
            switch (cmds[0])
            {
                case "#using":
                    UsingCommand(command);
                    break;
                case "#namespace":
                    NamespaceCommand(ref cmds);
                    break;
                default:
                    break;
            }

        }

        private void UsingCommand(string command)
        {
            compileSettings.usingNames = ExtractArguments(command);
        }

        private void NamespaceCommand(ref string[] command)
        {
            compileSettings.namespaceName = command[1];
        }

        public static string[] ExtractArguments(string s)
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
            return (string[])result.ToArray(typeof(string));
        }

        private string[] GetDecorators()
        {
            string[] result = { "no decorators" };
            if (decoratorBuffer == "")
                return result;
            return ExtractArguments(decoratorBuffer);
        }

        public void GiveStructInheritance()
        {
            ArrayList newStructs = new ArrayList();

            for (int i = 0; i < pStructs.Count; i++)
            {
                PStruct currentStruct = (PStruct) pStructs[i];

                if (!Token.IsEmpty(currentStruct.inheritance))
                {
                    Console.WriteLine("Looking for correct struct...");

                    // NOTE: change removebeginwhitespace function at the inheritance detection if problems with finding.
                    foreach(PStruct pStruct in pStructs)
                    {
                        Console.WriteLine("searching for match : " + pStruct.name + "; with " + currentStruct.inheritance);
                        if (pStruct.name.Contains(currentStruct.inheritance))
                        {
                            ArrayList newField = new ArrayList();

                            foreach (PField p in pStruct.pFields)
                                newField.Add(p);
                            foreach (PField p in currentStruct.pFields)
                                newField.Add(p);

                            //Console.WriteLine("New Fields :");
                            //for (int y = 0; y< newField.Count; y++)
                            //    Console.WriteLine(PField.ToString((PField)newField[y]));

                            currentStruct.pFields = (PField[])newField.ToArray(typeof(PField));

                        }
                        
                    }

                }

                newStructs.Add(currentStruct);

            }

            pStructs = newStructs;


        }



    }
      

}

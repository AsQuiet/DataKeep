using System;
using System.Collections;

using DataKeep.Tokens;

namespace DataKeep.Syntax
{
    class SyntaxParser
    {
        FileHandler fileHandler;
        int currentLine = 0;

        // buffers
        private string fieldBuffer = "";        // structs & enums
        private ArrayList blueprintLinesBuffer;
        private ArrayList fieldTemplateBuffer;  //structs
        private ArrayList entryBuffer;          //enums

        private StructTemplate structBuffer;
        private EnumTemplate enumBuffer;

        private bool inStruct = false;
        private bool inField = false;
        private bool inEnum = false;
        private bool inEntry = false;

        //final extracted data
        private ArrayList structTemplates = new ArrayList();
        private ArrayList enumTemplates = new ArrayList();

        public SyntaxParser(FileHandler fh)
        {
            fileHandler = fh;
            fh.LoadFile();

        }

        public string GetCurrentLine()
        {
            return fileHandler.fileLines[currentLine];
        }

        public void ParseAllLines()
        {

            while(true)
            {
                if (currentLine >= fileHandler.fileLines.Length)
                    break;

                AddBlueprintLine();

                if (inField)
                    EndFieldCommand(GetCurrentLine());
                if (inEntry)
                    AddEnumEntryCommand(GetCurrentLine());

                ParseCurrentLine();
                currentLine++;

                
            }
        }

        public void ParseCurrentLine()
        {
            string[] command = Token.ConvertToArray(GetCurrentLine());

            if (command.Length != 0)
            {
                switch (command[0])
                {

                    case "::defstruct":
                        DefStructCommand(command);
                        break;
                    case "::endstruct":
                        EndStructCommand();
                        break;
                    case "::$":
                        DefFieldCommand(GetCurrentLine());
                        break;
                    case "::defenum":
                        DefEnumCommand(command);
                        break;
                    case "::endenum":
                        EndEnumCommand();
                        break;
                    case "::&":
                        DefEntryCommand(GetCurrentLine());
                        break;
                    default:
                        break;

                }
            }

        }

        public void PrintAllData()
        {
            StructTemplate[] sts = (StructTemplate[])structTemplates.ToArray(typeof(StructTemplate));
            EnumTemplate[] ets = (EnumTemplate[])enumTemplates.ToArray(typeof(EnumTemplate));

            Console.WriteLine("Printing all the data..");

            foreach (StructTemplate st in sts)
                Console.WriteLine(StructTemplate.ToString(st));

            foreach (EnumTemplate et in ets)
                Console.WriteLine(EnumTemplate.ToString(et));
        }
        
        // `detection functions`

        // struct detection
        public void DefStructCommand(string[] command)
        {
            string[] tags = GetRange(ref command, 1, command.Length);

            structBuffer.allowedTags = GetAllowed(ref tags);
            structBuffer.deniedTags = GetDenied(ref tags);

            blueprintLinesBuffer = new ArrayList();
            fieldTemplateBuffer = new ArrayList();
            inStruct = true;
            Console.WriteLine("creating new struct buffer");
            
        }

        public void AddBlueprintLine()
        {
            if (inStruct || inEnum)
                blueprintLinesBuffer.Add(GetCurrentLine());
        }

        public void EndStructCommand()
        {
            structBuffer.blueprint = (string[])blueprintLinesBuffer.ToArray(typeof(string));
            structBuffer.fields = (FieldTemplate[])fieldTemplateBuffer.ToArray(typeof(FieldTemplate));

            structTemplates.Add(structBuffer);

            Console.WriteLine("ending enw struct " + structBuffer.fields.Length);
            inStruct = false;
        }


        // field detection
        public void DefFieldCommand(string line)
        {
            fieldBuffer = line;
            inField = true;
        }

        public void EndFieldCommand(string line)
        {
            FieldTemplate nField;

            // getting tags from buffer
            string[] arr = Token.ConvertToArray(fieldBuffer);
            string[] tags = GetRange(ref arr, 1, arr.Length);

            nField.allowedTags = GetAllowed(ref tags);
            nField.deniedTags = GetDenied(ref tags);

            // adding blueprint
            nField.blueprint = line;

            fieldTemplateBuffer.Add(nField);

            inField = false;
        }

        // enum detection
        public void DefEnumCommand(string[] command)
        {
            Console.WriteLine("adding a enum");

            {   // extracting data
                string[] tags = GetRange(ref command, 1, command.Length);
                enumBuffer.allowedTags = GetAllowed(ref tags);
                enumBuffer.deniedTags = GetDenied(ref tags);
            }

            {   // resseting lists etc
                blueprintLinesBuffer = new ArrayList();     
                entryBuffer = new ArrayList();
            }

            inEnum = true;

        }

        public void DefEntryCommand(string line)
        {
            fieldBuffer = line;
            inEnum = true;
            inStruct = false;
            inEntry = true;
            inField = false;
        }

        public void AddEnumEntryCommand(string line)
        {
            Console.WriteLine("adding an entry.");
            EntryTemplate nEntry;

            {
                string[] arr = Token.ConvertToArray(fieldBuffer);
                string[] tags = GetRange(ref arr, 1, arr.Length);
                nEntry.allowedTags = GetAllowed(ref tags);
                nEntry.deniedTags = GetDenied(ref tags);
            }

            nEntry.blueprint = line;

            entryBuffer.Add(nEntry);
            inEntry = false;
        }

        public void EndEnumCommand()
        {
            Console.WriteLine("ending the current enum");

            enumBuffer.blueprint = (string[]) blueprintLinesBuffer.ToArray(typeof(string));
            enumBuffer.entries = (EntryTemplate[]) entryBuffer.ToArray(typeof(EntryTemplate));

            enumTemplates.Add(enumBuffer);

            inEnum = false;
            inField = false;
        }

        // utility functions

        public string[] GetRange(ref string[] strings, int start, int stop)
        {
            ArrayList result = new ArrayList();
            for (int i = 0; i < strings.Length; i++)
            {
                if (i >= start && i < stop)
                    result.Add(strings[i]);
            }
            return (string[])result.ToArray(typeof(string));
        }

        public string[] GetAllowed(ref string[] tags)
        {
            ArrayList result = new ArrayList();
            foreach (string e in tags)
                if (!e.StartsWith("/"))
                    result.Add(e);
            return (string[])result.ToArray(typeof(string));
        }

        public string[] GetDenied(ref string[] tags)
        {
            ArrayList result = new ArrayList();
            foreach (string e in tags)
                if (e.StartsWith("/"))
                    result.Add(e);
            return (string[])result.ToArray(typeof(string));
        }

        public string SmashStrings(ref string[] tosmash)
        {
            string s = "";
            foreach (string e in tosmash)
                s += e;
            return s;
        }


    }


}
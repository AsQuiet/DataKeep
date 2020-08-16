using System;
using System.Collections;
using DataKeep;
using DataKeep.Tokens;

namespace DataKeep.Syntax
{
    class SyntaxParser
    {
        FileHandler fileHandler;
        int currentLine = 0;

        // buffers
        private string fieldBuffer = "";
        private ArrayList blueprintLinesBuffer;
        private StructTemplate structBuffer;
        private ArrayList fieldTemplateBuffer;

        private bool inStruct = false;
        private bool inField = false;

        //final extracted data
        private ArrayList structTemplates = new ArrayList();
        private ArrayList enumTemplates;

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

                ParseCurrentLine();
                AddStructBlueprintLine();
                currentLine++;
                Console.WriteLine("parse loop current line is " + currentLine + " and the max length is " + fileHandler.fileLines.Length);
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
                    default:
                        break;

                }
            }

        }

        public void PrintAllData()
        {
            StructTemplate[] sts = (StructTemplate[])structTemplates.ToArray(typeof(StructTemplate));
            Console.WriteLine("Printing all the data..");

            foreach (StructTemplate st in sts)
                Console.WriteLine(StructTemplate.ToString(st));
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

        public void AddStructBlueprintLine()
        {
            if (inStruct)
                blueprintLinesBuffer.Add(GetCurrentLine());
        }

        public void EndStructCommand()
        {
            structBuffer.blueprint = (string[])blueprintLinesBuffer.ToArray(typeof(string));
            structBuffer.fields = (StructFieldTemplate[])fieldTemplateBuffer.ToArray(typeof(StructFieldTemplate));

            structTemplates.Add(structBuffer);

            Console.WriteLine("ending enw struct");
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
            StructFieldTemplate nField;

            // getting tags from buffer
            string[] arr = Token.ConvertToArray(fieldBuffer);
            string[] tags = GetRange(ref arr, 1, arr.Length);

            nField.allowedTags = GetAllowed(ref tags);
            nField.deniedTags = GetDenied(ref tags);

            // adding blueprint
            nField.blueprint = line;

            fieldTemplateBuffer.Add(nField);
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
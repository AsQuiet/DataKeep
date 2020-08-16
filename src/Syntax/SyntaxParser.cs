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
        private ArrayList templateLinesBuffer;  // stores all the lines of the struct (strings)
        private ArrayList fieldTemplateBuffer;  // stores all the field structs of the struct (FieldTemplates)

        private StructTemplate structBuffer;
        private string fieldBuffer = "";

        private bool inStruct = false;
        private bool inField = false;

        //final extracted data
        public ArrayList structTemplates = new ArrayList();

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

                AddTemplateLine();

                if (inField)
                    EndFieldCommand(GetCurrentLine());

                ParseCurrentLine();
                currentLine++;
            }
        }

        internal void ParseCurrentLine()
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
            StructTemplate[] sts = (StructTemplate[]) structTemplates.ToArray(typeof(StructTemplate));

            Console.WriteLine("Printing all the data..");

            foreach (StructTemplate st in sts)
                Console.WriteLine(StructTemplate.ToString(st));
        }

        // struct detection
        public void DefStructCommand(string[] command)
        {
            string[] tags = GetRange(ref command, 1, command.Length);

            structBuffer.allowedTags = GetAllowed(ref tags);
            structBuffer.deniedTags = GetDenied(ref tags);

            templateLinesBuffer = new ArrayList();      // resseting
            fieldTemplateBuffer = new ArrayList();
            inStruct = true;
            
            Console.WriteLine("creating new struct buffer");
            
        }

        public void AddTemplateLine()
        {
            if (inStruct)
                templateLinesBuffer.Add(GetCurrentLine());
        }

        public void EndStructCommand()
        {
            structBuffer.template = (string[]) templateLinesBuffer.ToArray(typeof(string));
            structBuffer.fields = (FieldTemplate[]) fieldTemplateBuffer.ToArray(typeof(FieldTemplate));

            structTemplates.Add(structBuffer);
            inStruct = false;

            Console.WriteLine("ending enw struct " + structBuffer.fields.Length);
            
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
            nField.template = line;
            fieldTemplateBuffer.Add(nField);

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
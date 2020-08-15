using System;
using DataKeep;

using DataKeep.Tokens;

namespace DataKeep.Syntax
{

    class SyntaxParser
    {

        FileHandler fileHandler;
        public int currentLine = 0;

        public StructTemplate currentStruct;

        public SyntaxParser(FileHandler fh)
        {
            fileHandler = fh;
            fh.LoadFile();
        }


        public void ParseCurrentLine()
        {
            Console.WriteLine("parsing the line : " + GetCurrentLine());
            string[] comps = Token.ConvertToArray(GetCurrentLine());
      
            if (comps[0].Contains("::"))
                HandleCommand(ref comps);

        }

        public string GetCurrentLine()
        {
            return fileHandler.fileLines[currentLine];
        }

        public bool IsEof()
        {
            return currentLine >= fileHandler.fileLines.Length;
        }

        public void HandleCommand(ref string[] command)
        {
            switch (command[0])
            {
                case "::defstruct":
                    Console.WriteLine("defingi na s truct ");
                    break;
                default:
                    break;
            }
        }

    
    }

}
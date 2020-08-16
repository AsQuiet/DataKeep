using System;
using System.IO;


namespace DataKeep
{

    class FileHandler
    {
        public string path;
        public string[] fileLines;

        public FileHandler(string path)
        {
            this.path = path;
        }

        public void LoadFile()
        {
            fileLines = File.ReadAllLines(path);
        }

        public void PrintFile()
        {
            for (int i = 0; i < fileLines.Length; i++)
                Console.WriteLine(fileLines[i]);
        }


    }

}
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
            string save = Debug.prefix;
            Debug.SetPrefix("FileHandler");
            Debug.Print("Loading the file at path : " + path);
            Debug.SetPrefix(save);
            fileLines = File.ReadAllLines(path);
        }

        public void PrintFile()
        {
            for (int i = 0; i < fileLines.Length; i++)
                Console.WriteLine(fileLines[i]);
        }


    }

    class Debug
    {
        public static string prefix = "";

        private static bool msg = true;     // updates on what compiler is doing
        public static bool log = false;    // printing out raw data

        public static void Print(string msg_)
        {
            if (msg)
                Console.WriteLine("[" + prefix + "] " + msg_);
        }

        public static void Log(string msg_)
        {
            if (log)
                Console.WriteLine("[" + prefix + "] " + msg_);
        }

        public static void SetPrint(bool b)
        {
            msg = b;
        }

        public static void SetLog(bool b)
        {
            log = b;
        }

        public static void SetPrefix(string s)
        {
            prefix = s;
        }

    }

}
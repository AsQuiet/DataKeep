using System;
using System.IO;
using System.Diagnostics;
using System.Collections;

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
            //string save = Debug.prefix;
            //Debug.SetPrefix("FileHandler");
            //Debug.Print("Loading the file at path : " + path);
            //Debug.SetPrefix(save);
            fileLines = File.ReadAllLines(path);
        }

        public void PrintFile()
        {
            for (int i = 0; i < fileLines.Length; i++)
                Console.WriteLine(fileLines[i]);
        }


    }

    class DebugDK
    {
        public static string prefix = "";

        private static bool msg = true;     // updates on what compiler is doing
        public static bool log = false;    // printing out raw data
        public static bool timer = true;

        private static Hashtable timers = new Hashtable();

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

        public static void StartStopwatch(string id)
        {
            timers[id] = NanoTime();
        }

        public static void StopStopwatch(string id)
        {
            long current = NanoTime();
            long elapsed = current - ((long)timers[id]);
            long elapsedMS = elapsed / 1000000L;

            if (timer)
                Console.WriteLine("[Timer] '" + id + "' took " + elapsed + "ns. (" + elapsedMS + "ms).");

        }

        public static long NanoTime()
        {
            double timestamp = Stopwatch.GetTimestamp();
            double nanoseconds = 1_000_000_000.0 * timestamp / Stopwatch.Frequency;
            return (long)nanoseconds;
        }

    }

}
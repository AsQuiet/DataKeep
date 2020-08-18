using System;

using System.Collections;
using DataKeep.ParserTypes;

// exporting all of the found structs etc to a python file
namespace DataKeep
{

    struct Exporter
    {

        public Hashtable defs;
        public Hashtable flags;

        public PStruct[] structs;

        public string outputPath;

        public static Exporter FromParser(Parser p, string output)
        {
            Exporter n_Exporter;
            n_Exporter.defs = p.defs;
            n_Exporter.flags = p.flags;
            n_Exporter.structs = (PStruct[])p.structs.ToArray(typeof(PStruct));
            n_Exporter.outputPath = output;
            return n_Exporter;
        }

        public void Export()
        {
            Console.WriteLine("exporting...");
        }

        internal string ConvertHashtable(ref Hashtable hash)
        {
            string result = "";
            return result;
        }

    }


}
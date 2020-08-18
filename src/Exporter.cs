using System;
using System.IO;

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


            ArrayList lines = new ArrayList();

            lines.Add("from datakeep import DataKeepStruct, DataKeepField, DataKeepTag");

            lines.Add(ConvertHashtable(ref defs, "DEFS"));
            lines.Add(ConvertHashtable(ref flags, "FLAGS"));

            lines.Add("ALL_STRUCTS = []");
            foreach (PStruct struct_ in structs)
            {
                lines.Add("ALL_STRUCTS.append(" + ConvertPStruct(struct_) + ")");
            }

            string[] lines_ = (string[])lines.ToArray(typeof(string));

            File.WriteAllLines(outputPath, lines_);


        }

        internal string ConvertHashtable(ref Hashtable hash, string name)
        {
            string result = "";

            result += name + " = {\n";

            int i = 0;
            foreach (DictionaryEntry e in hash)
            {

                result += '"' + (string)e.Key + '"' + " : "+ (string)e.Value;
                if (i != hash.Count - 1)
                    result += ","; 
                result += "\n";
                i += 1;
            }

            result += "}\n";

            return result;
        }

        internal string ConvertPTag(ref PTag tag)
        {
            string result = "DataKeepTag(";

            result += '"' + tag.name + "\", [";

            int i = 0;
            foreach (string arg in tag.arguments)
            {

                if (arg.Contains("$"))
                    result += '"' + arg.Replace("$", "") + '"';
                else
                    result += arg;
                if (i != tag.arguments.Length - 1)
                    result += ", ";
                i++;
            }
            

            return result + "])";
        }

        internal string ConvertPField(ref PField field)
        {
            string result = "DataKeepField(";

            result += '"' + field.name + "\", ";

            if (field.type.Contains('"'))
                result += field.type + ", [";
            else
                result += '"' + field.type + "\", [";

            for (int i = 0; i< field.tags.Length; i++)
            {
                result += ConvertPTag(ref field.tags[i]);
                if (i != field.tags.Length - 1)
                    result += ","; 
            }

            return result + "])";
        }

        internal string ConvertPStruct(PStruct struct_)
        {
            string result = "DataKeepStruct(";

            result += '"' + struct_.name + "\", [";

            for (int i = 0; i < struct_.tags.Length; i++)
            {
                result += ConvertPTag(ref struct_.tags[i]);
                if (i != struct_.tags.Length - 1)
                    result += ",";
            }
            result += "], [";

            for (int i = 0; i < struct_.fields.Length; i++)
            {
                result += ConvertPField(ref struct_.fields[i]);
                if (i != struct_.fields.Length - 1)
                    result += ",";
            }

            return result + "])";
        }

    }


}
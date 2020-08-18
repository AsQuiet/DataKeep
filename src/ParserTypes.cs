using DataKeep.Tokens;

using System;
namespace DataKeep.ParserTypes
{

    struct PField
    {
        public string name;
        public string type;
        public PTag[] tags;

        public static string ToString(PField pf)
        {
            return "PField("+pf.name+", "+pf.type+", ["+ PTag.ArrToString(ref pf.tags) + "])";
        }

    }

    struct PStruct
    {
        public string name;
        public PField[] fields;
        public PTag[] tags;
        public string inheritance;

        public static string ToString(PStruct ps)
        {
            string s = "PStruct(" + ps.name + ", [" + PTag.ArrToString(ref ps.tags) + "], " + ps.inheritance + ", with fields: ";

            foreach (PField pf in ps.fields)
                s += "\n   " + PField.ToString(pf);

            return s + "\n)";
        }
    }

    struct PTag
    {

        public string name;
        public string[] arguments;

        public static string ToString(PTag tag)
        {
            string result = "PTag(";

            result += tag.name;
            result += ", [";

            foreach (string arg in tag.arguments)
                result += arg + ",";

            return result + "])";
        }

        public static string ArrToString(ref PTag[] tags)
        {
            string result = "";
            foreach (PTag tag in tags)
            {
                result += PTag.ToString(tag) + ",";
            }
            return result;
        }
    }




}
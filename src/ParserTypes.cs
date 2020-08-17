using DataKeep.Tokens;

using System;
namespace DataKeep.ParserTypes
{

    struct PField
    {
        public string name;
        public string type;
        public string[] tags;

        public static string ToString(PField pf)
        {
            return "PField("+pf.name+", "+pf.type+", ("+Token.StringArrToString(ref pf.tags, "-")+"))";
        }

    }

    struct PStruct
    {
        public string name;
        public PField[] fields;
        public string[] tags;
        public string inheritance;

        public static string ToString(PStruct ps)
        {
            string s = "PStruct(" + ps.name + ", (" + Token.StringArrToString(ref ps.tags, "-") + "), " + ps.inheritance + ", with fields: ";

            foreach (PField pf in ps.fields)
                s += "\n   " + PField.ToString(pf);

            return s + "\n)";
        }
    }




}
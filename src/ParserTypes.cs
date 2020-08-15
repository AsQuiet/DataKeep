using System;
using DataKeep;
using DataKeep.Tokens;
namespace DataKeep.ParserTypes
{

    struct PField
    {
        public string name;
        public string type;
        public string[] decorators;

        public static string ToString(PField pf)
        {
            return "PField("+pf.name+", "+pf.type+", ("+Token.StringArrToString(ref pf.decorators, "-")+"))";
        }

    }

    struct PStruct
    {
        public string name;
        public PField[] pFields;
        public string[] decorators;
        public string inheritance;

        public static string ToString(PStruct ps)
        {
            string s = "PStruct(" + ps.name + ", (" + Token.StringArrToString(ref ps.decorators, "-") + "), " + ps.inheritance + ", with fields: ";

            foreach (PField pf in ps.pFields)
                s += "\n   " + PField.ToString(pf);

            return s + "\n)";
        }
    }

    struct PEnum
    {
        public string name;
        public string[] decorators;
        public string[] entries;
           
        public static string ToString(PEnum pe)
        {
            string s = "PEnum(" + pe.name + ", (" + Token.StringArrToString(ref pe.decorators, "-") + "), with entries: ";
            foreach (string entry in pe.entries)
                s += "\n   " + entry;
            return s + "\n)";
        }

    }

}
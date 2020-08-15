using System;
using DataKeep;



namespace DataKeep.ParserTypes
{

    struct PField
    {
        public string name;
        public string type;
        public string deco;

        public static string ToString(PField pf)
        {
            return "PField("+pf.name+", "+pf.type+", "+pf.deco+")";
        }
    }

    struct PStruct
    {
        public string name;
        public PField[] pFields;
        public string deco;
        public string inheritance;

        public static string ToString(PStruct ps)
        {
            string s = "PStruct(" + ps.name + ", " + ps.deco + ", " + ps.inheritance + ", with fields: ";

            foreach (PField pf in ps.pFields)
                s += "\n   " + PField.ToString(pf);

            return s + "\n)";
        }
    }

    struct PEnum
    {
        public string name;
        public string deco;
        public string[] entries;
           
        public static string ToString(PEnum pe)
        {
            string s = "PEnum(" + pe.name + ", " + pe.deco + ", with entries: ";
            foreach (string entry in pe.entries)
                s += "\n   " + entry;
            return s + "\n)";
        }

    }

}
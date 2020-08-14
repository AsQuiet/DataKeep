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
            return "PField : " + pf.name + ", type : " + pf.type + ", deco : " + pf.deco; 
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
            string s = "PStruct : " + ps.name + ", inher. : " + ps.inheritance + ", deco : " + ps.deco + " with fields : ";

            foreach (PField pf in ps.pFields)
                s += "\n   " + PField.ToString(pf);

            return s;
        }
    }

    struct PEnum
    {
        public string name;
        public string deco;
        public string[] entries;
    }

}
using System;


namespace DataKeep.Syntax
{

    struct StructTemplate
    {
        public string[] blueprint;
        public string[] allowedTags;
        public string[] deniedTags;
        public StructFieldTemplate[] fields;

        public static string ToString(StructTemplate st)
        {
            string s = "(";

            s += "\nblueprint lines : ";
            foreach (string e in st.blueprint)
                s += "\n    " + e;

            s += "\nallowedtags : ";
            foreach (string e in st.allowedTags)
                s += "\n    " + e;

            s += "\ndeniedtags : ";
            foreach (string e in st.deniedTags)
                s += "\n    " + e;

            s += "\nstructfields : ";

            s += "\n)";

            return s;
        }
    }

    struct StructFieldTemplate
    {
        public string blueprint;
        public string[] allowedTags;
        public string[] deniedTags;
    }

}

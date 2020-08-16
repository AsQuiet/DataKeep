using System;

namespace DataKeep.Syntax
{

    struct StructTemplate
    {
        public string[] template;
        public string[] allowedTags;
        public string[] deniedTags;
        public FieldTemplate[] fields;

        public static string ToString(StructTemplate st)
        {
            string s = "(";

            s += "\nblueprint lines : ";
            foreach (string e in st.template)
                s += "\n    " + e;

            s += "\nallowedtags : ";
            foreach (string e in st.allowedTags)
                s += "\n    " + e;

            s += "\ndeniedtags : ";
            foreach (string e in st.deniedTags)
                s += "\n    " + e;

            s += "\nstructfields : \n";
            foreach (FieldTemplate sft in st.fields)
                s += FieldTemplate.ToString(sft);

            s += "\n)";

            return s;
        }
    }

    struct FieldTemplate
    {
        public string template;
        public string[] allowedTags;
        public string[] deniedTags;

        public static string ToString(FieldTemplate st)
        {
            string s = "(";
            s += "\n    blueprint : " + st.template;

            s += "\n    allowedtags : ";
            foreach (string e in st.allowedTags)
                s += "\n        " + e;

            s += "\n    deniedtags : ";
            foreach (string e in st.deniedTags)
                s += "\n        " + e;

            s += "\n)";
            return s;
        }
    }

}

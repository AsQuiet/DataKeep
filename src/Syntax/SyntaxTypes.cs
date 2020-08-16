using System;


namespace DataKeep.Syntax
{

    struct StructTemplate
    {
        public string[] blueprint;
        public string[] allowedTags;
        public string[] deniedTags;
        public FieldTemplate[] fields;

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

            s += "\nstructfields : \n";
            foreach (FieldTemplate sft in st.fields)
                s += FieldTemplate.ToString(sft);

            s += "\n)";

            return s;
        }
    }

    struct FieldTemplate
    {
        public string blueprint;
        public string[] allowedTags;
        public string[] deniedTags;

        public static string ToString(FieldTemplate st)
        {
            string s = "(";
            s += "\n    blueprint : " + st.blueprint;

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

    struct EnumTemplate
    {
        public string[] blueprint;
        public string[] allowedTags;
        public string[] deniedTags;
        public EntryTemplate[] entries;

        public static string ToString(EnumTemplate et)
        {
            string s = "(";

            s += "\nblueprint lines : ";
            foreach (string e in et.blueprint)
                s += "\n    " + e;

            s += "\nallowedtags : ";
            foreach (string e in et.allowedTags)
                s += "\n    " + e;

            s += "\ndeniedtags : ";
            foreach (string e in et.deniedTags)
                s += "\n    " + e;

            s += "\nstructfields : \n";
            foreach (EntryTemplate sft in et.entries)
                s += EntryTemplate.ToString(sft);

            s += "\n)";

            return s;
        }

    }

    struct EntryTemplate
    {
        public string blueprint;
        public string[] allowedTags;
        public string[] deniedTags;

        public static string ToString(EntryTemplate et)
        {
            string s = "(";
            s += "\n    blueprint : " + et.blueprint;

            s += "\n    allowedtags : ";
            foreach (string e in et.allowedTags)
                s += "\n        " + e;

            s += "\n    deniedtags : ";
            foreach (string e in et.deniedTags)
                s += "\n        " + e;

            s += "\n)";
            return s;
        }
    }

}

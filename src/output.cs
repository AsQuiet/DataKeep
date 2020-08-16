namespace DataKeep.Test
{

    struct FieldTemplate
    {
        public string template;
        public string[] allowedTags;
        public string[] deniedTags;
        public string secureTin;

        public static string ToString(FieldTemplate struct_)
        {
            string s = "(";

            s += "\n" + struct_.template;
            foreach (string e in struct_.allowedTags) { s += e; }
            foreach (string e in struct_.deniedTags) { s += e; }


            return s + ")";
        }
    }
    struct StructTemplate
    {
        public string template;
        public string[] allowedTags;
        public string[] deniedTagsReWorkedVersion2;
        public FieldTemplate[] fields;

        public static string ToString(StructTemplate struct_)
        {
            string s = "(";

            s += "\n" + struct_.template;
            foreach (string e in struct_.allowedTags) { s += e; }
            foreach (string e in struct_.deniedTagsReWorkedVersion2) { s += e; }
            foreach (FieldTemplate e in struct_.fields) { s += FieldTemplate.ToString(e); }

            return s + ")";
        }
    }

}
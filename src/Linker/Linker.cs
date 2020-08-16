using System;
using System.Collections;
using System.IO;
using DataKeep.ParserTypes;
using DataKeep.Syntax;

namespace DataKeep
{

    struct LinkerData
    {
        public PStruct[] structs;
        public PEnum[] enums;

        public StructTemplate[] structTemplates;
        public EnumTemplate[] enumTemplates;

        public string[] structCode;
        public string[] enumCode;

        public string finalCode;
    }

    class Linker
    {

        LinkerData core;

        SyntaxParser sParser;
        Parser parser;

        public Linker(SyntaxParser sp, Parser p)
        {
            sParser = sp;
            parser = p;

            core.structs = (PStruct[])p.pStructs.ToArray(typeof(PStruct));
            core.enums = (PEnum[])p.pEnums.ToArray(typeof(PEnum));
            core.structTemplates = (StructTemplate[])sp.structTemplates.ToArray(typeof(StructTemplate));
            core.enumTemplates = (EnumTemplate[])sp.enumTemplates.ToArray(typeof(EnumTemplate));

            
        }

        public void Convert()
        {
            Console.WriteLine("converting all of the structs");
            foreach(PStruct st in core.structs)
                ConvertStruct(st);
        }

        public void OutputToFile(string path)
        {
            File.WriteAllText(path, core.finalCode);
        }

        public void ConvertStruct(PStruct struct_)
        {
            StructTemplate[] templates = FindCorrectStructBlueprints(struct_, core.structTemplates);

            /*Console.WriteLine("result is :");
            foreach (StructTemplate s in templates)
                Console.WriteLine(StructTemplate.ToString(s));*/

            string code = "";
            string master = "";
            foreach(StructTemplate st in templates)
            {
                string templateCode = FillStructTemplate(struct_, st);

                if (templateCode.Contains("%tags%"))
                    master += templateCode;
                else
                    code += templateCode;
            }

            master = master.Replace("%tags%", code);

            core.finalCode += master + "\n";
        }

        public StructTemplate[] FindCorrectStructBlueprints(PStruct struct_, StructTemplate[] templates)
        {
            ArrayList result = new ArrayList();

            for (int i = 0; i  < templates.Length; i++)
            {
                StructTemplate currentT = templates[i];

                bool tagInAllowed = false;
                bool tagInDenied = false;

                foreach (string s in struct_.decorators)
                {
                    Console.WriteLine("checking the decorator : " + s);

                    foreach (string temp in currentT.allowedTags)
                        tagInAllowed = tagInAllowed || temp.Contains(s);

                    foreach (string temp in currentT.deniedTags)
                        tagInDenied = tagInDenied || temp.Contains("/" + s);

                    Console.WriteLine("allowed/denied(" + tagInAllowed + "," + tagInDenied + ")");

                }

                if (tagInAllowed && !tagInDenied)
                    result.Add(currentT);
               

            }

            return (StructTemplate[])result.ToArray(typeof(StructTemplate));

        }

        public StructTemplate FindMasterStructTemplate(StructTemplate[] templates)
        {
            StructTemplate result = templates[0];

            foreach(StructTemplate st in templates)
            {
                bool hasTag = false;
                foreach (string s in st.blueprint)
                    hasTag = hasTag || s.Contains("%tags%");
                if (hasTag)
                    result = st;
            }

            return result;
        }

        public string FillStructTemplate(PStruct struct_, StructTemplate template)
        {
            string result = "";
            string fieldCode = "";

            {
                int commandCount = 0;
                for (int i = 0; i < template.blueprint.Length; i++)
                {
                    string current = template.blueprint[i];
                    
                    current = current.Replace("%structname%", struct_.name);
                    Console.WriteLine("current blueprint is : " + current + " and commandcount is " + commandCount);

                    if (commandCount != 0)
                        commandCount--;

                    if (current.Contains("::$") || current.Contains("::endstruct"))
                        commandCount = 2;

                    if (current.Contains("::startfields"))
                    {
                        commandCount = 1;
                        result += "\n%fields";
                    }

                    if (current.Contains("::endfields"))
                    {
                        commandCount = 1;
                        result += "here%";
                    }
                        
                    if (commandCount == 0)
                        result += "\n" + current;
                }

                //Console.WriteLine("result i s : " + result);

            }
            
            {   // getting the field code
                for (int i = 0; i < struct_.pFields.Length; i++)
                {
                    FieldTemplate correct = FindCorrectFieldTemplate(struct_.pFields[i], template.fields);
                    string templateCode = correct.blueprint;

                    //Console.WriteLine("field of struct_ with data \n" + PField.ToString(struct_.pFields[i]) + "\n found match with \n" + FieldTemplate.ToString(correct));

                    templateCode = templateCode.Replace("%fieldname%", struct_.pFields[i].name);
                    templateCode = templateCode.Replace("%fieldtype%", struct_.pFields[i].type);
                    //templateCode = templateCode.Replace("%i%", i);

                    //Console.WriteLine("code is : \n" + templateCode);
                    fieldCode += templateCode;

                    if (i + 1 != struct_.pFields.Length)
                        fieldCode += "\n";
                }
            }

            return result.Replace("%fieldshere%", fieldCode);
        }

        public FieldTemplate FindCorrectFieldTemplate(PField field, FieldTemplate[] templates)
        {

            for (int i = 0; i < templates.Length; i++)
            {
                FieldTemplate current = templates[i];

                bool tagInAllowed = false;
                bool tagInDenied = false;

                foreach (string s in field.decorators)
                {

                    foreach (string temp in current.allowedTags)
                        tagInAllowed = tagInAllowed || temp.Contains(s);

                    foreach (string temp in current.deniedTags)
                        tagInDenied = tagInDenied || temp.Contains(s);
                }

                if (tagInAllowed && !tagInDenied)
                    return current;


            }
             

            return templates[0];
        }

        enum EntityType
        {
            Player,
            Zombie,
            Undead

        }

        struct Entity
        {
            public string entityID;
            public EntityType entityType;

        }

       


    }

}
/*
 * struct conversion : 
 * 
 * foreach struct :
 * 1 find corresponding blueprints
 * 2 find 'master' blueprint(has %tags% in blueprints)
 * 3 fill out master blueprint (structname)
 * 
 * 4 for each pfielde of struct:
 * 4.1 find approprita blueprint
 * 4.2 fill out blueprint
 * 4.3 add to struct code
 * 
 * 5 fill other remaining blueprints (step 3 - 4)
 * 6 add to core code
 * 
 */
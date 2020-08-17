﻿using System;
using System.Collections;
using System.IO;
using DataKeep.Tokens;
using DataKeep.ParserTypes;
using DataKeep.Syntax;

namespace DataKeep
{

    class Linker
    {
        public PStruct[] structs;
        public StructTemplate[] structTemplates;

        public string finalCode;

        public Linker(SyntaxParser sp, Parser p)
        {
            DebugDK.SetPrefix("Linker");
            DebugDK.Log("Getting structs and structTemplates from SyntaxParser and Parser");
            structs = (PStruct[]) p.structs.ToArray(typeof(PStruct));
            structTemplates = (StructTemplate[]) sp.structTemplates.ToArray(typeof(StructTemplate));
        }

        public void Convert()
        {
            DebugDK.Log("Converting every struct...");

            foreach (PStruct st in structs)
                ConvertStruct(st);
        }

        public void OutputToFile(string path)
        {
            DebugDK.Print("Outputting all data to file : " + path);
            File.WriteAllText(path, finalCode);
        }

        public void ConvertStruct(PStruct struct_)
        {
            DebugDK.Log("Converting the struct with name : " + struct_.name);
            StructTemplate[] templates = FindCorrectStructTemplates(struct_, structTemplates);
                 
            string code = "";
            string master = "";

            if (templates.Length == 0)
                return;

            DebugDK.Log("Filling every corresponding structtemplate.");
            foreach (StructTemplate st in templates)
            {
                string templateCode = FillStructTemplate(struct_, st);

                if (templateCode.Contains("%tags%"))
                    master += templateCode;
                else
                    code += templateCode;
            }

            master = master.Replace("%tags%", code);

            finalCode += master;
        }

        public StructTemplate[] FindCorrectStructTemplates(PStruct struct_, StructTemplate[] templates)
        {
            DebugDK.Log("Finding the correct structTemplates for struct with name " + struct_.name);
            ArrayList result = new ArrayList();

            for (int i = 0; i  < templates.Length; i++)
            {
                StructTemplate currentT = templates[i];

                bool tagInAllowed = false;
                bool tagInDenied = false;

                DebugDK.Log("Looking for match with current Template.");

                foreach (string s in struct_.tags)
                {
                    DebugDK.Log("Checking the tag '" + s + "' for a match.");

                    foreach (string temp in currentT.allowedTags)
                        tagInAllowed = tagInAllowed || temp == s;

                    foreach (string temp in currentT.deniedTags)
                        tagInDenied = tagInDenied || temp == "/" + s;

                    DebugDK.Log("Result of checking (allowed/denied) is (" +tagInAllowed + "/" + tagInDenied + ")");

                }

                if (tagInAllowed && !tagInDenied)
                    result.Add(currentT);
               
            }

            return (StructTemplate[]) result.ToArray(typeof(StructTemplate));

        }

        

        public string FillStructTemplate(PStruct struct_, StructTemplate template)
        {
            string result = "";
            string fieldCode = "";

            DebugDK.Log("Filling out struct with name : " + struct_.name);

            {
                int commandCount = 0;
                for (int i = 0; i < template.template.Length; i++)
                {
                    string current = template.template[i];
                    
                    current = current.Replace("%structname%", struct_.name);

                    DebugDK.Log("current line in template.templates (string[]) is : " + current);

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
                        if (!current.Contains("%tags%"))
                            result += "\n" + current;
                        else
                            result += current;
                }

                DebugDK.Log("Filled out template is : " + result);

            }
            
            {   // getting the field code
                DebugDK.Log("Filling out the fields for struct with name : " + struct_.name);
                for (int i = 0; i < struct_.fields.Length; i++)
                {

                    FieldTemplate? searchResult = FindCorrectFieldTemplate(struct_.fields[i], template.fields);

                    if (searchResult == null)
                        continue;

                    FieldTemplate correct = (FieldTemplate) searchResult;
                    string templateCode = correct.template;

                    //Debug.Log("Field of struct_ with data \n" + PField.ToString(struct_.fields[i]) + "\n found match with \n" + FieldTemplate.ToString(correct));

                    DebugDK.Log("Replacing variables in found template...");
                    templateCode = templateCode.Replace("%fieldname%", struct_.fields[i].name);
                    templateCode = templateCode.Replace("%fieldtype%", struct_.fields[i].type);


                    DebugDK.Log("Resulting code is : " + templateCode);
                    fieldCode += templateCode;
                    
                    fieldCode += "\n";
                }
            }

            return result.Replace("%fieldshere%", fieldCode);
        }

        public FieldTemplate? FindCorrectFieldTemplate(PField field, FieldTemplate[] templates)
        {
            DebugDK.Log("Finding the correct template for field with name : " + field.name);
            for (int i = 0; i < templates.Length; i++)
            {
                FieldTemplate current = templates[i];

                bool tagInAllowed = false;
                bool tagInDenied = false;

                foreach (string s in field.tags)
                {

                    foreach (string temp in current.allowedTags)
                        tagInAllowed = tagInAllowed || temp == s;

                    foreach (string temp in current.deniedTags)
                        tagInDenied = tagInDenied || temp == "/" + s;
                }

                 if (tagInAllowed && !tagInDenied)
                    return current;


            }
             

            return null;
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
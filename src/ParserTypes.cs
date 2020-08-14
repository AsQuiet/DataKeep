using System;
using DataKeep;



namespace DataKeep.ParserTypes
{

    struct PField
    {
        string name;
        string type;
        string[] decos;
    }

    struct PStruct
    {
        string name;
        PField[] pFields;
        string[] decos;
        string[] inheritance;
    }

    struct PEnum
    {
        string name;
        string[] deocs;
        string[] enums;
    }

}
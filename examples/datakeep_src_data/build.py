from datakeep import *
from output import *

def defaultStruct(struct, file):

    write(file, "struct " + struct.name + "\n{")
    
    for field in GetFields(struct):
        write(file, "public {} {};".format(field.type, field.name))

    
def structToString(struct, file):

    write(file, "public static string ToString({} struct_)".format(struct.name))
    write(file, "{\nstring result = \"\";")

    for field in GetFields(struct):

        if (HasTags(field, "Convert")):
            conversionCase = GetArgument(GetTag(field, 0), 0)
            conversionType = GetArgument(GetTag(field, 0), 1)

            if (conversionCase == "arr"):
                write(file, "foreach({} e in struct_.{}) ".format(conversionType, field.name) + "{")
                write(file, "result += {}.ToString(e);".format(conversionType))
                write(file, "}")
            elif (conversionCase == "default"):
                write(file, "result += {}.ToString(e);".format(conversionType))
        else:
            write(file, "result += struct_." + field.name + ";")

    write(file, "}")
    write(file, "}")
    
    

def main(struct, index, file):
    
    if (HasTags(struct, "NoBody")):
        return
    
    # default struct
    defaultStruct(struct, file)

    if (HasTags(struct, "ToString")):
        structToString(struct, file)

def beginFile(file):
    write(file, "using System;")
    write(file, "namespace Output \n{")

def endFile(file):
    write(file, "}")

WRITE_BEGIN(beginFile, "output.cs")
LOOP_ALL_STRUCTS(ALL_STRUCTS, main, "output.cs")
WRITE_END(endFile, "output.cs")
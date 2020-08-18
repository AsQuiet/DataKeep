# ##### CODE GENERATED BY DATAKEEP.CS
from datakeep import *

ALL_STRUCTS = [] 

nStruct = DataKeepStruct("player")
nStruct.inheritance = "entity"

field1 = DataKeepField("name", "string")
field2 = DataKeepField("entityId", "uint")
field3 = DataKeepField("age", "int")

tag1 = DataKeepTag("Print")
tag2 = DataKeepTag("NoPrint")
tag3 = DataKeepTag("DefaultValue")
tag3.arguments.append("0")

field1.tags.append(tag2)
field3.tags.append(tag3)
nStruct.tags.append(tag1)

nStruct.fields.append(field1)
nStruct.fields.append(field2)
nStruct.fields.append(field3)

ALL_STRUCTS.append(nStruct)

# ###### DATAKEEP DPI (api)

# def StructHasTags():
#     return None
# def StructNameIs():
#     return None
# def StructHasInheritance():
#     return None
# def TagHasArguments():
#     return None
# def GetTagArguments():
#     return None
# def StructHasFields():
#     return None
# def FieldHasTags():
#     return None
# def FieldIsType():
#     return None

    
def CreateDefaults(struct, index, file):

    if (HasTags(struct, "NoBody")):
        return
    
    write(file, "struct " + struct.name)
    write(file, "{")

    for field in struct.fields:

        if (HasTags(field, "DefaultValue")):
            
            argvalue = GetArguments(GetTag(field, "DefaultValue"))[0]

            write(file, "public {} {} = {};".format(field.type, field.name, argvalue))
        else:
            write(file, "public {} {};".format(field.type, field.name))

    if (HasTags(struct, "Print")):
        write(file, "public static void Print(" + struct.name + " struct_)")
        write(file, "{")

        for field in struct.fields:
            if (not HasTags(field, "NoPrint")):
                write(file, 'Console.WriteLine("\\n{}" + struct_.{});'.format(field.name, field.name))

        write(file, "}")

    write(file, "}")

def CreateNamespaces(file):
    write(file, "using System;")
    write(file, "namespace DataKeep.Output \n{")

def EndFile(file):
    write(file, "}")

WRITE_BEGIN(CreateNamespaces)
LOOP_ALL_STRUCTS(ALL_STRUCTS, CreateDefaults)
WRITE_END(EndFile)
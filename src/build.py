from datakeep import *
from output import ALL_STRUCTS, DEFS, FLAGS

def start(file):
    write(file, "using System;")
    write(file, "namespace DataKeep.Output \n{")
    write(file, "struct EntityCreation \n{")
    write(file, "public static void InitEntities () \n{")

def end(file):
    write(file, "}")
    write(file, "}")
    write(file, "}")
    

def create_entity(struct, file):

    entityName = GetArgument(GetTag(struct, 0), 0)


    write(file, "Entity {} = core.scene.CreateEntity(\"{}\");".format(entityName, entityName) )

    for field in GetFields(struct):

        if HasTags(field, "Components"):
            for arg in GetArguments(GetTag(field, 0)):
                write(file, "{}.AddNewComp({});".format(entityName, arg))


def main(struct, index, file):
    
    
    if (HasTags(struct, "Entity")):
        create_entity(struct, file)

path = "output.cs"
WRITE_BEGIN(start, path)
LOOP_ALL_STRUCTS(ALL_STRUCTS, main, path)
WRITE_END(end, path)


from datakeep import *
from output import ALL_STRUCTS, DEFS, FLAGS

def main(struct, index, file):
    
    if (HasTags(struct, "NoBody")):
        return
    
    #creating default struct    
    write(file, "typedef " + struct.name + " " + struct.name + ";")
    write(file, "struct " + struct.name + "\n{")

    for field in GetFields(struct):
        write(file, "{} {};".format(field.type, field.name))

    write(file, "};")

    #creating ToString function
    if (HasTags(struct, "Print")):

        write(file, "void Print{} ({} *struct_)".format(struct.name, struct.name))
        write(file, "{")

        for field in GetFields(struct):

            if (HasTags(field, "Tag")):
                value = GetArgument(GetTag(field, 0), 0) + " : "
            else:
                value = ""
            
            id_type = "d" # create some sort of dictionary to check field.type against (int -> d, string -> s, ....)

            write(file, "printf(\"{}%{}\", struct_->{});".format(value, id_type, field.name))
        write(file, "}")
        


LOOP_ALL_STRUCTS(ALL_STRUCTS, main, "colordata.c", append_file=False)


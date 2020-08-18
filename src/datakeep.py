import os, sys

class DataKeepObject(object):

    def __init__(self, name, tags):
        self.name = name
        self.tags = tags       
        pass

class DataKeepStruct(DataKeepObject):

    def __init__(self, name, tags, fields):
        super().__init__(name, tags)
        self.inheritance = ""
        self.fields = fields

    def __str__(self):
        s = "struct\n(\n"

        s += "name : " + self.name + "\n"
        s += "inheritance : " + self.inheritance + "\n"

        s += "fields : "
        for field in self.fields:
            s += "\n" + str(field)
        

        s += "tags : "
        for tag in self.tags:
            s += "\n" + str(tag)

        return s + "\n)"

class DataKeepField(DataKeepObject):

    def __init__(self, name, type_, tags):
        super().__init__(name, tags)
        self.type = type_
        
    def __str__(self):
        s = "field{\n"

        s += "name : " + self.name + "\n"
        s += "type : " + self.type + "\n"

        s += "tags : "
        for tag in self.tags:
            s += "\n" + str(tag)
        s += "\n}\n"
        
        return s

class DataKeepTag(DataKeepObject):

    def __init__(self, name, arguments):
        super().__init__(name, []) # tags should be ignored
        self.arguments = arguments
        
    def __str__(self):
        s = "tag("
        s += "\nname : " + self.name
        s += "\narguments : \n"
        for arg in self.arguments:
            s += str(arg) + ","
        return s + "\n)"

        
def LOOP_ALL_STRUCTS(structs, method, output_path="output.txt", append_file=True):
    f = open(output_path, "a" if append_file else "w")

    for x in range(len(structs)):
        method(structs[x], x, f)
    
    f.close()

def WRITE_BEGIN(method, output_path="output.txt"):
    f = open(output_path, "w")
    method(f)
    f.close()

def WRITE_END(method, output_path="output.txt"):
    f = open(output_path, "a")
    method(f)
    f.close()

def HasTags(structfield, *args):
        result = True

        for tag in args:
            
            found = False
            for mTag in structfield.tags:
                found = found or mTag.name == tag
            result = result and found 

        return result

def GetTag(structfield, tagName):
    for tag in structfield.tags:
        if (tag.name == tagName):
            return tag
    return None

def GetArguments(tag):
    if (tag == None):
        return None
    return tag.arguments

def write(file, text):
    file.write(text + "\n")

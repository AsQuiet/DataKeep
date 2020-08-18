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
        s = "Struct("

        s +=  self.name + ", tags : ["
        for tag in self.tags:
            s += str(tag) + ", "

        s += "] fields : "
        for field in self.fields:
            s += "\n    " + str(field)

        return s + "\n)"

class DataKeepField(DataKeepObject):

    def __init__(self, name, type_, tags):
        super().__init__(name, tags)
        self.type = type_
        
    def __str__(self):
        s = "Field("

        s += self.name + ", "
        s += self.type + ", "

        s += "tags : ["
        for tag in self.tags:
            s += str(tag)
        s += "])"
        
        return s

class DataKeepTag(DataKeepObject):

    def __init__(self, name, arguments):
        super().__init__(name, []) # tags should be ignored
        self.arguments = arguments
        
    def __str__(self):
        s = "Tag("
        s += self.name
        s += ", ["
        for arg in self.arguments:
            s += str(arg) + ","
        return s + "])"

        
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

def write(file, text):
    file.write(text + "\n")

def HasTags(obj, *args):
        result = True

        for tag in args:
            
            found = False
            for mTag in obj.tags:
                found = found or mTag.name == tag
            result = result and found 

        return result

def GetTags(obj):
    return obj.tags

def GetTag(obj, index):
    return obj.tags[index]

def GetFields(struct):
    return struct.fields

def GetField(struct, index):
    return struct.fields[index]

def GetArguments(tag):
    return tag.arguments

def GetArgument(tag, index):
    return tag.arguments[index]
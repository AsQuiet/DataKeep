import os
import datamanager.string as string
class DataManager():

    # user's dictionaries and structs that are created during runtime will be stored here
    global_data = {}
    temp_data = {}

    # contain the data struct of the user
    data_structs = []
    # used for quickly accesing the struct in the data_structs array
    struct_names = {}

    data_types = {
        "i" : "integer",
        "f" : "float",
        "b" : "boolean",
        "s" : "string",
        "*i" : "integer array",
        "*f" : "float array",
        "*b" : "boolean array",
        "*s" : "string array"
    }

    @staticmethod
    def load_in_structs(path):
        # checking
        if not os.path.exists(path):
            print("The given path does not exist.") 
            return 1  
        
        # reading 
        f = open(path, "r")
        lines = []
        for line in f:
            l = line.rstrip("\n")
            # dont save comments or empty lines
            if "//" in l: continue
            if string.is_empty(l): continue

            # turning the line into an array
            lines.append(string.list_separator(l, " "))
        
        f.close()
        DataManager.parse_lines(lines)
        return 0

    @staticmethod
    def parse_lines(lines):

        # used for detecting tokens
        new_struct_encountered = False
        new_struct = {}
        new_struct_name = "new struct"

        for line in lines:
            # checking if we should make a new struct
            if "struct" in line:
                new_struct_name = string.remove_characters(line[1], [" "])
                new_struct_encountered = True
                continue
            
            if "{" in line:continue
            if "}" in line:
                DataManager.struct_names[new_struct_name] = len(DataManager.data_structs) 
                DataManager.data_structs.append(new_struct.copy())
                new_struct = {}
                new_struct_encountered = False 
                continue
            
            if new_struct_encountered:
                key = string.remove_characters(line[0], [" "])
                value = string.remove_characters(DataManager.get_flag(":",line), " ")
                new_struct[key] = value

        # print(str(DataManager.data_structs))
        # print(str(DataManager.struct_names))
        
    @staticmethod
    def get_flag(flag, lines):
        try:
            return lines[lines.index(flag) + 1]
        except:
            return None
        
    # ------------------------------------------------------------------------------------------------------------------------
    #  USER FUNCTIONS
    # ------------------------------------------------------------------------------------------------------------------------
    @staticmethod
    def generate_python_variables(path="datamanager_variables.py", all_lower_case=True):

        variables = []

        # porting the struct_names
        for key in DataManager.struct_names.keys():
            key_s = str(key)
            if all_lower_case:variables.append([key_s.lower(), key_s])
            else:variables.append([key_s, key_s])
            
        # porting all of the field of every struct
        for struct in DataManager.data_structs:
            for key in struct.keys():
                key_s = str(key)
                if all_lower_case:variables.append([key_s.lower(), key_s])
                else:variables.append([key_s, key_s])

        # writing all of the variables to a file
        f = open(path, "w")
        for var in variables:
            f.write(var[0] + "='" + var[1] + "'\n")
        f.close()
        pass

    @staticmethod
    def create(name, struct_name):
        if name == None or struct_name == None:
            DataManager.error_NoStructFound()
            return
        
        # accesing the struct blueprint
        try:
            new_struct = (DataManager.data_structs[DataManager.struct_names[struct_name]]).copy()
            DataManager.global_data[name] = new_struct
        except:
            DataManager.error_NoStructFound()
    
    @staticmethod
    def set(name, field, value):
        try:
            DataManager.global_data[name][field] = value
        except:
            DataManager.error_NoStructFound()

    @staticmethod
    def get(name, field=None):
        try:
            if field == None:return DataManager.global_data[name]
            return DataManager.global_data[name][field]
        except:
            DataManager.error_NoStructFound()
            return None
    

    @staticmethod
    def error_NoStructFound():
        print("The given struct could not be found.")
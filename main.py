from datamanager import DataManager as dm
from datamanager_variables import *

# loading in structs
dm.load_in_structs("mydata.data")

# generating all of the variables we might need
dm.generate_python_variables("datamanager_variables.py")

# creating a struct (Profile)
dm.create("profile1", profile)

dm.set("profile1", name, "quinten")
dm.set("profile1", last, "lenaerts")
dm.set("profile1", age, 16)

print(dm.get("profile1"))
print(dm.get("profile1", age))




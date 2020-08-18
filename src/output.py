from datakeep import DataKeepStruct, DataKeepField, DataKeepTag
DEFS = {
"int" : "uint"
}

FLAGS = {
"true" : True,
"ISDEAD" : 0,
"ISALIVE" : 1,
"untrue" : False
}

ALL_STRUCTS = []
ALL_STRUCTS.append(DataKeepStruct("Base", [], [DataKeepField("baseID", "uint", [])]))
ALL_STRUCTS.append(DataKeepStruct("UpgradedBaseBoi", [DataKeepTag("Print", ["PrintUBB", 12, True])], [DataKeepField("baseID", "uint", []),DataKeepField("name", "string", [DataKeepTag("DefaultValue", [" muahahahh"])]),DataKeepField("age", "uint", [DataKeepTag("DefaultValue", [0, "some string", 12]),DataKeepTag("PrivateHJ_", [True, False])]),DataKeepField("state", "uint", [DataKeepTag("State", [0])])]))

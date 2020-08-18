from datakeep import DataKeepStruct, DataKeepField, DataKeepTag
DEFS = {
"ee" : "string;",
"s" : "string;"
}

FLAGS = {
"ZombieSpell" : 2,
"Transform" : 0,
"SpriteRenderer" : 1
}

ALL_STRUCTS = []
ALL_STRUCTS.append(DataKeepStruct("Base", [DataKeepTag("NoBody", [])], [DataKeepField("entityID", "string;", []),DataKeepField("entityC", "int", [])]))
ALL_STRUCTS.append(DataKeepStruct("Player", [DataKeepTag("Entity", ["Player"])], [DataKeepField("entityID", "string;", []),DataKeepField("entityC", "int", []),DataKeepField("compList", "string;", [DataKeepTag("Components", [0, 1])])]))
ALL_STRUCTS.append(DataKeepStruct("Enemy", [DataKeepTag("Entity", ["Enemy"])], [DataKeepField("entityID", "string;", []),DataKeepField("entityC", "int", []),DataKeepField("compList", "string;", [DataKeepTag("Components", [0, 1, 2])])]))

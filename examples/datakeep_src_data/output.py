from datakeep import DataKeepStruct, DataKeepField, DataKeepTag
DEFS = {
"s" : "string",
"s[]" : "string[]"
}

FLAGS = {
}

ALL_STRUCTS = []
ALL_STRUCTS.append(DataKeepStruct("Base", [DataKeepTag("NoBody", [])], [DataKeepField("name", "string", []),DataKeepField("tags", "PTag[]", [DataKeepTag("Convert", ["arr", " PTag"])])]))
ALL_STRUCTS.append(DataKeepStruct("PStruct", [DataKeepTag("ToString", []),DataKeepTag("Print", [])], [DataKeepField("name", "string", []),DataKeepField("tags", "PTag[]", [DataKeepTag("Convert", ["arr", " PTag"])]),DataKeepField("fields", "PField[]", [DataKeepTag("Convert", ["arr", " PField"])]),DataKeepField("inheritance", "string", [])]))
ALL_STRUCTS.append(DataKeepStruct("PField", [DataKeepTag("ToString", []),DataKeepTag("Print", [])], [DataKeepField("name", "string", []),DataKeepField("tags", "PTag[]", [DataKeepTag("Convert", ["arr", " PTag"])]),DataKeepField("type", "string", [])]))
ALL_STRUCTS.append(DataKeepStruct("PTag", [DataKeepTag("ToString", []),DataKeepTag("Print", [])], [DataKeepField("name", "string", []),DataKeepField("arguments", "string[]", [DataKeepTag("Convert", ["arr", " string"])])]))

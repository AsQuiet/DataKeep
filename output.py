from datakeep import DataKeepStruct, DataKeepField, DataKeepTag
DEFS = {
}

FLAGS = {
}

ALL_STRUCTS = []
ALL_STRUCTS.append(DataKeepStruct("ColorRGB", [DataKeepTag("Print", [])], [DataKeepField("r", "int", []),DataKeepField("g", "int", []),DataKeepField("b", "int", []),DataKeepField("a", "int", [DataKeepTag("Tag", ["alpha"])])]))

using System;
namespace Output 
{
struct PStruct
{
public string name;
public PTag[] tags;
public PField[] fields;
public string inheritance;
public static string ToString(PStruct struct_)
{
string result = "";
result += struct_.name;
foreach( PTag e in struct_.tags) {
result +=  PTag.ToString(e);
}
foreach( PField e in struct_.fields) {
result +=  PField.ToString(e);
}
result +=  string.ToString(e);
}
}
struct PField
{
public string name;
public PTag[] tags;
public string type;
public static string ToString(PField struct_)
{
string result = "";
result += struct_.name;
foreach( PTag e in struct_.tags) {
result +=  PTag.ToString(e);
}
result += struct_.type;
}
}
struct PTag
{
public string name;
public string[] arguments;
public static string ToString(PTag struct_)
{
string result = "";
result += struct_.name;
foreach( string e in struct_.arguments) {
result +=  string.ToString(e);
}
}
}
}

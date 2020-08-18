# DataKeep
DataKeep is a data-generation and handler tool written C# and Python.<br>
## What is DataKeep?
DataKeep reads it's own files, .dk files, and parses them into Python files, then, using the DataKeep Python Interface, the user is able to specify exactly what happens with all the data based on tags that all the data have. DataKeep is heavily inspired by <a href="https://github.com/ryanfleury/data_desk">Ryan Fluery's Data Desk</a> tool.

## Demo
<p>Let's say that you are working in a C project and you need to handle color data. You also want some functions to debug this data and whatnot. You could decide to create a DataKeep file like the one below : </p>

> data.dk
```
@Print
struct ColorRGB
{
    r : int;
    g : int;
    b : int;
    @Tag($alpha)
    a : int;
}

```
<p>We've defined a simple data structure of an rgb color with an alpha component. Now we want to be able to Print this struct, so we give the struct a Print tag. <br>The first three fields that will be printed are obvious in terms of what they mean but we want an explanation of what the fourth component is. So we give it a tag, called Tag, that takes an argument. ('$' sign indicates it's a string, so whitespace should not be removed)</p>
<p>Now we convert this datakeep file to a python file by running : </p>

```
DataKeep data.dk colordata 
```
<p>
DataKeep parses all this data into Python objects that you can then use in combination with a bunch of methods that datakeep.py provides, to specify how this data should be exported.
<br>Here is an example of a script that converts this data into a usable C file.</p>

> build.py
```python
from datakeep import *
from colordata import ALL_STRUCTS, DEFS, FLAGS

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
            
            id_type = "d" # check field.type (string -> s, int -> d, ...)

            write(file, "printf(\"{}%{}\", struct_->{});".format(value, id_type, field.name))
        write(file, "}")
        


LOOP_ALL_STRUCTS(ALL_STRUCTS, main, "colordata.c", append_file=False)
```
Running this python script will result in your file being created  : 
> colordata.c
```C
typedef ColorRGB ColorRGB;
struct ColorRGB
{
int r;
int g;
int b;
int a;
};
void PrintColorRGB (ColorRGB *struct_)
{
printf("%d", struct_->r);
printf("%d", struct_->g);
printf("%d", struct_->b);
printf("alpha : %d", struct_->a);
}
```

## Installation
There are two presets in the release folder for mac & windows. Simply download your release, add it to your system path and you can run DataKeep. 
<br>If the releases do not work for some reason, follow these steps to build it yourself.
<ul>
<li>Make sure you have .NET installed (C#, Visual Studio, ...), (can you run dotnet in terminal?)</li>
<li>Create a new project by entering, 'dotnet new console -o DataKeep'</li>
<li>Paste all of the source code into the newly created project folder.</li>
    <li>In your .csproj file add this property group </li></ul>

```html
<PropertyGroup>
    <RuntimeIdentifiers>win10-x64;ubuntu.16.10-x64;osx.10.12-x64</RuntimeIdentifiers>
</PropertyGroup>
```

<ul>
<li>Then go into your project and run:
<br> dotnet publish -c Release -r [RuntimeIdentifier]
</li>
</ul>
<p>This should build the entire thing for you.</p>

## DataKeep File Syntax


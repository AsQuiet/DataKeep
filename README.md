# DataKeep

## compiler - commands
Compiler command are defined by using the symbol '#' and will tell the compiler what to do. eg:
```c
#using (System, Program.OtherData, System.Collections)
#namespace Program.Data
...
#end namespace
```

<ul>

<li>using => will make imports/using commands for all packages.</li>
<li>namespace => in what namespace should these structs etc belong</li>
<li>end => ends other commands that have a scope (namespace, ...) </li>
<li>typedef [type] [name] => creates an alias for a type</li>

</ul>

## tags (or decorators)
```

@Print
struct Profile
{
    name : string; 
    last : string;
    age : int;
    @NoPrint
    id : ProfileID;
}

```
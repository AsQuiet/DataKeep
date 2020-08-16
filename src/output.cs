
using System;
namespace DataKeep.Output
{



enum EntityType
{
Player ,
Zombie ,
Undead 

}

struct MainEntity
{
public string entityID ;
public EntityType entityType ;

}

struct Player 
{
public string entityID ;
public EntityType entityType ;
public int health ;
public int damage ;
public double someFloatValue ;

public static string ToString(Player  struct_)
{	
string result = "(";
result += "\n" + struct_.entityID ;
result += "\n" + struct_.entityType ;
result += "\n" + struct_.health ;
result += "\n" + struct_.damage ;
result += "\n" + struct_.someFloatValue ;
return result + ")";
}
public static void Print(Player  struct_)
{
Console.WriteLine(ToString(struct_));
}
}



}

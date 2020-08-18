using System;
namespace DataKeep.Output
{
struct Vector3
{
public double x;
public double[] y.ToString;
public double[] z;

public static string ToString(Vector3 struct_)
{
string s = "(";
s += "\nx : " + struct_.x;
foreach(double e in struct_.y) {s += double.ToString(e);}
foreach(double e in struct_.z) {s += e;}

return s + ")";
}
public static void Print(Vector3 struct_)
{
Console.WriteLine(Vector3.ToString(struct_));

}
}
struct TransformComponent
{
public uint entityID;
public string entityName;
public Vector3 pos;
public Vector3 vel;
public Vector3 acc;

public static string ToString(TransformComponent struct_)
{
string s = "(";
s += "\nentityID : " + struct_.entityID;
s += "\nentityName : " + struct_.entityName;
s += "\npos : " + Vector3.ToString(struct_.pos);
s += "\nvel : " + Vector3.ToString(struct_.vel);
s += "\nacc : " + Vector3.ToString(struct_.acc);

return s + ")";
}
public static void Print(TransformComponent struct_)
{
Console.WriteLine(TransformComponent.ToString(struct_));

}
}
struct SpriteRendererComponent
{
public uint entityID;
public string entityName;
public string img;

public static string ToString(SpriteRendererComponent struct_)
{
string s = "(";
s += "\nentityID : " + struct_.entityID;
s += "\nentityName : " + struct_.entityName;
s += "\nimg : " + struct_.img;

return s + ")";
}
public static void Print(SpriteRendererComponent struct_)
{
Console.WriteLine(SpriteRendererComponent.ToString(struct_));

}
}
enum PlayerStates
{
Alive,
Dead,
UnDead,

}
}
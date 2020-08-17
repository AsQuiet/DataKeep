import os, sys

os.system("dotnet run");

lines = []
f = open("output.csbutnotcs", "r");
for line in f:
	lines.append(line);
f.close()

f = open("output.cs", "w");

f.write("using System;\n")
f.write("namespace DataKeep.Output\n{")

for line in lines:
	f.write(line)

f.write("\n}")
f.close()

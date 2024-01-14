#!/usr/bin/bash

dotnet new console -o $1
dotnet add $1 reference LibAoc
dotnet add $1 reference IntCode
dotnet sln add $1
cp template_intcode.cs "$1/Program.cs"

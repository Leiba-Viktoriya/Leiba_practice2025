#!/bin/bash
set -e

dotnet build PluginA/PluginA.csproj -c Debug
dotnet build PluginB/PluginB.csproj -c Debug
dotnet build PluginC/PluginC.csproj -c Debug

mkdir -p Plugins

cp PluginA/bin/Debug/net8.0/PluginA.dll Plugins/
cp PluginB/bin/Debug/net8.0/PluginB.dll Plugins/
cp PluginC/bin/Debug/net8.0/PluginC.dll Plugins/

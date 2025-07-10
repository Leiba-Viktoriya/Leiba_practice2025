using System;
using PluginContracts;

namespace Plugins;

[PluginLoad]
public class PluginA : ICommand
{
    public void Execute()
    {
        Console.WriteLine("PluginA выполнен.");
    }
}

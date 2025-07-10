using System;
using PluginContracts;

namespace Plugins;

[PluginLoad("PluginA")]
public class PluginB : ICommand
{
    public void Execute()
    {
        Console.WriteLine("PluginB выполнен после PluginA.");
    }
}

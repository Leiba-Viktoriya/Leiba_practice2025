using System;
using PluginContracts;

namespace Plugins;

[PluginLoad("PluginA", "PluginB")]
public class PluginC : ICommand
{
    public void Execute()
    {
        Console.WriteLine("PluginC выполнен после PluginA и PluginB.");
    }
}

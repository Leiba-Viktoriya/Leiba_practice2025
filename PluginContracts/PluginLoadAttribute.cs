using System;

namespace PluginContracts;

[AttributeUsage(AttributeTargets.Class)]
public class PluginLoadAttribute : Attribute
{
    public string[] DependsOn { get; }

    public PluginLoadAttribute(params string[] dependsOn)
    {
        DependsOn = dependsOn;
    }
}

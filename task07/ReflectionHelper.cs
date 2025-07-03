using System;
using System.Linq;
using System.Reflection;

namespace task07;

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        var displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        var versionAttr = type.GetCustomAttribute<VersionAttribute>();

        if (displayNameAttr != null)
            Console.WriteLine($"DisplayName: {displayNameAttr.DisplayName}");

        if (versionAttr != null)
            Console.WriteLine($"Version: {versionAttr.Major}.{versionAttr.Minor}");

        Console.WriteLine("Methods:");
        type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .ToList()
            .ForEach(m =>
            {
                var attr = m.GetCustomAttribute<DisplayNameAttribute>();
                if (attr != null)
                    Console.WriteLine($"  {m.Name}: {attr.DisplayName}");
            });

        Console.WriteLine("Properties:");
        type.GetProperties()
            .ToList()
            .ForEach(p =>
            {
                var attr = p.GetCustomAttribute<DisplayNameAttribute>();
                if (attr != null)
                    Console.WriteLine($"  {p.Name}: {attr.DisplayName}");
            });
    }
}

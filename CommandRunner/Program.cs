using CommandLib;
using System.Reflection;

string pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "FileSystemCommands.dll");
if (!File.Exists(pluginPath))
{
    Console.WriteLine($"Плагин не найден: {pluginPath}");
    return;
}

var assembly = Assembly.LoadFrom(pluginPath);
var commandTypes = assembly.GetTypes()
                           .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

foreach (var type in commandTypes)
{
    var constructors = type.GetConstructors();
    var constructor = constructors.FirstOrDefault();
    if (constructor == null) continue;

    var parameters = constructor.GetParameters();

    object? instance = type.Name switch
    {
        "DirectorySizeCommand" => Activator.CreateInstance(type, Path.GetTempPath()),
        "FindFilesCommand" => Activator.CreateInstance(type, Path.GetTempPath(), "*.txt"),
        _ => null
    };

    if (instance is ICommand cmd)
    {
        Console.WriteLine($"Выполняем: {type.Name}");
        cmd.Execute();
        Console.WriteLine();
    }
}

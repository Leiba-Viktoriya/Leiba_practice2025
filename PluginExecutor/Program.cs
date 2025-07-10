using PluginContracts;
using System.Reflection;

string pluginsPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
if (!Directory.Exists(pluginsPath))
{
    Console.WriteLine($"Папка с плагинами не найдена: {pluginsPath}");
    return;
}

var dllFiles = Directory.GetFiles(pluginsPath, "*.dll");
var loadedTypes = new List<Type>();

foreach (var dll in dllFiles)
{
    try
    {
        var assembly = Assembly.LoadFrom(dll);
        var pluginTypes = assembly.GetTypes()
            .Where(t => typeof(ICommand).IsAssignableFrom(t) &&
                        t.GetCustomAttribute<PluginLoadAttribute>() != null &&
                        t.GetConstructor(Type.EmptyTypes) != null);

        loadedTypes.AddRange(pluginTypes);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка загрузки {dll}: {ex.Message}");
    }
}

var graph = new Dictionary<Type, List<Type>>();
foreach (var type in loadedTypes)
{
    var attr = type.GetCustomAttribute<PluginLoadAttribute>();
    var depends = new List<Type>();

    foreach (var depName in attr.DependsOn)
    {
        var depType = loadedTypes.FirstOrDefault(t => t.Name == depName);
        if (depType != null) depends.Add(depType);
    }

    graph[type] = depends;
}

var sorted = new List<Type>();
var visited = new HashSet<Type>();

void Visit(Type node)
{
    if (visited.Contains(node)) return;

    foreach (var dep in graph[node])
        Visit(dep);

    visited.Add(node);
    sorted.Add(node);
}

foreach (var type in graph.Keys)
    Visit(type);

foreach (var type in sorted)
{
    if (Activator.CreateInstance(type) is ICommand command)
    {
        Console.WriteLine($"Выполняется: {type.Name}");
        command.Execute();
    }
}

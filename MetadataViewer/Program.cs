using System;
using System.Reflection;

if (args.Length == 0)
{
    Console.WriteLine("Укажите путь к DLL-файлу в аргументах командной строки.");
    return;
}

var dllPath = args[0];
if (!File.Exists(dllPath))
{
    Console.WriteLine($"Файл не найден: {dllPath}");
    return;
}

try
{
    var assembly = Assembly.LoadFrom(dllPath);
    Console.WriteLine($"Сборка: {Path.GetFileName(dllPath)}");

    foreach (var type in assembly.GetTypes())
    {
        Console.WriteLine($"\n=== Класс: {type.FullName} ===");

        var classAttrs = type.GetCustomAttributes(inherit: false);
        foreach (var attr in classAttrs)
        {
            Console.WriteLine($"Атрибут класса: {attr.GetType().Name}");
        }

        var constructors = type.GetConstructors();
        foreach (var ctor in constructors)
        {
            var parameters = ctor.GetParameters();
            var paramInfo = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
            Console.WriteLine($"Конструктор: {type.Name}({paramInfo})");
        }

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var method in methods)
        {
            Console.WriteLine($"\nМетод: {method.Name}");

            foreach (var attr in method.GetCustomAttributes(false))
            {
                Console.WriteLine($" └ Атрибут метода: {attr.GetType().Name}");
            }

            var parameters = method.GetParameters();
            foreach (var param in parameters)
            {
                Console.WriteLine($" └ Параметр: {param.ParameterType.Name} {param.Name}");
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка при загрузке сборки: {ex.Message}");
}

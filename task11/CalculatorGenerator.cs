using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace practice2025;

public static class CalculatorGenerator
{
    public static object CreateCalculatorInstance()
    {
        string className = "Calculator";
        string interfaceName = typeof(ICalculator).FullName!;
        string assemblyName = $"DynamicCalculator_{Guid.NewGuid()}";

        string code = $@"
using System;

public class {className} : {interfaceName}
{{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
}}";

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .ToList();

        var compilation = CSharpCompilation.Create(
            assemblyName,
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            var errors = string.Join(Environment.NewLine, result.Diagnostics.Select(d => d.ToString()));
            throw new Exception("Ошибка компиляции:\n" + errors);
        }

        ms.Seek(0, SeekOrigin.Begin);

        var context = new AssemblyLoadContext(assemblyName, isCollectible: true);
        var assembly = context.LoadFromStream(ms);
        var type = assembly.GetType(className);
        return Activator.CreateInstance(type!);
    }
}

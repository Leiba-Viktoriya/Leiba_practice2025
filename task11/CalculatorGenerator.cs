using System;
using System.Linq;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Runtime.Loader;

namespace practice2025;

public static class CalculatorGenerator
{
    public static object CreateCalculatorInstance()
    {
        string code = @"
using System;

public class Calculator : practice2025.ICalculator
{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
}";

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .ToList();

        var compilation = CSharpCompilation.Create(
            "DynamicCalculator",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            var diagnostics = string.Join("\n", result.Diagnostics.Select(d => d.ToString()));
            throw new Exception("Ошибка компиляции:\n" + diagnostics);
        }

        ms.Seek(0, SeekOrigin.Begin);
        var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
        var type = assembly.GetType("Calculator");
        return Activator.CreateInstance(type);
    }
}

using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace task05
{
    public class ClassAnalyzer
    {
        private readonly Type _type;

        public ClassAnalyzer(Type type)
        {
            _type = type;
        }

        public IEnumerable<string> GetPublicMethods() =>
            _type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                 .Select(m => m.Name);

        public IEnumerable<string> GetMethodParams(string methodName) =>
            _type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                 .Where(m => m.Name == methodName)
                 .SelectMany(m => m.GetParameters().Select(p => p.Name).Append(m.ReturnType.Name));

        public IEnumerable<string> GetAllFields() =>
            _type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                 .Select(f => f.Name);

        public IEnumerable<string> GetProperties() =>
            _type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                 .Select(p => p.Name);

        public bool HasAttribute<T>() where T : Attribute =>
            _type.GetCustomAttributes(typeof(T), inherit: true).Any();
    }
}

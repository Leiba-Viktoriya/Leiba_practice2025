using System;
using Xunit;
using task05;
using System.Linq;

namespace task05tests
{
    public class TestClass
    {
        public int PublicField;
        private string _privateField;
        public int Property { get; set; }

        public void Method() { }
        public string AnotherMethod(int a, string b) => "";
        public void NoParams() { }
        public int WithParams(int x, string y) => 0;
    }

    [Serializable]
    public class AttributedClass { }

    public class ClassAnalyzerTests
    {
        [Fact]
        public void GetPublicMethods_ReturnsCorrectMethods()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var methods = analyzer.GetPublicMethods().ToList();
            Assert.Contains("Method", methods);
            Assert.Contains("AnotherMethod", methods);
            Assert.Contains("NoParams", methods);
            Assert.Contains("WithParams", methods);
        }

        [Fact]
        public void GetAllFields_IncludesPrivateAndPublicInstanceFieldsOnly()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var fields = analyzer.GetAllFields().ToList();
            Assert.Contains("_privateField", fields);
            Assert.Contains("PublicField", fields);
            Assert.DoesNotContain("StaticField", fields);
        }

        [Fact]
        public void GetProperties_ReturnsPropertyName()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var props = analyzer.GetProperties().ToList();
            Assert.Single(props);
            Assert.Equal("Property", props[0]);
        }

        [Fact]
        public void HasAttribute_ReturnsTrueIfAttributePresent()
        {
            var analyzer = new ClassAnalyzer(typeof(AttributedClass));
            Assert.True(analyzer.HasAttribute<SerializableAttribute>());
        }

        [Fact]
        public void GetMethodParams_WithParamsMethod_ReturnsParamsAndReturnType()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var result = analyzer.GetMethodParams("WithParams").ToList();
            Assert.Contains("x", result);
            Assert.Contains("y", result);
            Assert.Contains("Int32", result);
        }

        [Fact]
        public void GetMethodParams_NoParamsMethod_ReturnsOnlyReturnType()
        {
            var analyzer = new ClassAnalyzer(typeof(TestClass));
            var result = analyzer.GetMethodParams("NoParams").ToList();
            Assert.Single(result);
            Assert.Equal("Void", result[0]);
        }
    }
}

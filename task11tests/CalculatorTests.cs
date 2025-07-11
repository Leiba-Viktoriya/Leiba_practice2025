using Xunit;
using practice2025;

namespace task11tests;

public class CalculatorTests
{
    [Fact]
    public void AddTest()
    {
        var calc = (ICalculator)CalculatorGenerator.CreateCalculatorInstance();
        Assert.Equal(7, calc.Add(3, 4));
    }

    [Fact]
    public void MinusTest()
    {
        var calc = (ICalculator)CalculatorGenerator.CreateCalculatorInstance();
        Assert.Equal(2, calc.Minus(5, 3));
    }

    [Fact]
    public void MulTest()
    {
        var calc = (ICalculator)CalculatorGenerator.CreateCalculatorInstance();
        Assert.Equal(20, calc.Mul(4, 5));
    }

    [Fact]
    public void DivTest()
    {
        var calc = (ICalculator)CalculatorGenerator.CreateCalculatorInstance();
        Assert.Equal(2, calc.Div(10, 5));
    }
}

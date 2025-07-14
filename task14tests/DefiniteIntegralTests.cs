using Xunit;
using practice2025.Task14;
using System;

namespace task14tests
{
    public class DefiniteIntegralTests
    {
        [Fact]
        public void Solve_Identity_OnSymmetricInterval_IsZero()
        {
            Func<double, double> X = x => x;
            double result = DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2);
            Assert.Equal(0, result, 1e-4);
        }

        [Fact]
        public void Solve_Sin_OnSymmetricInterval_IsZero()
        {
            Func<double, double> SIN = x => Math.Sin(x);
            double result = DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8);
            Assert.Equal(0, result, 1e-4);
        }

        [Fact]
        public void Solve_Identity_OnPositiveInterval_EqualsArea()
        {
            Func<double, double> X = x => x;
            double result = DefiniteIntegral.Solve(0, 5, X, 1e-6, 8);
            Assert.Equal(10, result, 1e-5);
        }
    }
}

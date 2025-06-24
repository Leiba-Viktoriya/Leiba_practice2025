using Xunit;
using TextAnalysisExtensions;

namespace StringAnalysisTests
{
    public class PalindromeTests
    {
        [Theory]
        [InlineData("А роза упала на лапу Азора", true)]
        [InlineData("Hello, world!", false)]
        [InlineData("", false)]
        [InlineData("Was it a car or a cat I saw?", true)]
        [InlineData("12321", true)]
        [InlineData("Madam in Eden, I'm Adam", true)]
        public void TestIsPalindrome(string input, bool expected)
        {
            Assert.Equal(expected, input.IsPalindrome());
        }

        [Fact]
        public void IsPalindrome_WithMixedCase_ReturnsTrue()
        {
            string input = "LeVEl";
            Assert.True(input.IsPalindrome());
        }

        [Fact]
        public void IsPalindrome_WithOnlyPunctuation_ReturnsFalse()
        {
            string input = "!?,.";
            Assert.False(input.IsPalindrome());
        }
    }
}

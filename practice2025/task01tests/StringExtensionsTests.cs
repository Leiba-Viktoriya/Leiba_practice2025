using Xunit;
using task01;

public class StringExtensionsTests
{
    [Fact]
    public void Palindrome_InRussian_ReturnsTrue()
    {
        var testInput = "А роза упала на лапу Азора";
        Assert.True(testInput.IsPalindrome());
    }

    [Fact]
    public void NonPalindrome_EnglishPhrase_ReturnsFalse()
    {
        var testInput = "Hello, world!";
        Assert.False(testInput.IsPalindrome());
    }

    [Fact]
    public void Palindrome_EmptyString_ReturnsFalse()
    {
        var testInput = string.Empty;
        Assert.False(testInput.IsPalindrome());
    }

    [Fact]
    public void Palindrome_WithSymbols_IsHandledCorrectly()
    {
        var testInput = "Was it a car or a cat I saw?";
        Assert.True(testInput.IsPalindrome());
    }
}

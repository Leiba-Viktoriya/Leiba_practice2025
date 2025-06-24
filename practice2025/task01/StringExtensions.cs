using System;
using System.Linq;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            var filtered = string.Concat(
                source
                    .ToLowerInvariant()
                    .Where(ch => !char.IsWhiteSpace(ch) && !char.IsPunctuation(ch))
            );

            var reversed = new string(filtered.Reverse().ToArray());
            return filtered.Equals(reversed);
        }
    }
}

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            var chars = new List<char>();
            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c))
                    chars.Add(char.ToLower(c));
            }

            for (int i = 0, j = chars.Count - 1; i < j; i++, j--)
            {
                if (chars[i] != chars[j])
                    return false;
            }

            return true;
        }
    }
}

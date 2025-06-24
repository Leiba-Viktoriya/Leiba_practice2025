namespace TextAnalysisExtensions
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            var cleanedChars = input
                .ToLower()
                .Where(c => !char.IsWhiteSpace(c) && !char.IsPunctuation(c))
                .ToArray();

            return IsMirroredSequence(cleanedChars);
        }

        private static bool IsMirroredSequence(char[] characters)
        {
            int left = 0;
            int right = characters.Length - 1;
            while (left < right)
            {
                if (characters[left++] != characters[right--])
                    return false;
            }
            return true;
        }
    }
}

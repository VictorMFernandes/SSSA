using System;

namespace SSSA.Helper.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveTexts(this string input, params string[] stringsToRemove)
        {
            var result = input;

            if (string.IsNullOrEmpty(input))
            {
                return result;
            }

            foreach (var stringToRemove in stringsToRemove)
            {
                if (string.IsNullOrEmpty(stringToRemove))
                {
                    continue;
                }

                var startIndex = result.IndexOf(stringToRemove, StringComparison.InvariantCulture);

                while (startIndex != -1)
                {
                    result = startIndex == -1 ? result : result.Remove(startIndex, stringToRemove.Length);
                    startIndex = result.IndexOf(stringToRemove, StringComparison.InvariantCulture);
                }
            }

            return result;
        }
    }
}

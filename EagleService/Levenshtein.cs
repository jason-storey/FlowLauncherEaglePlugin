using System;

namespace Flow.Launcher.Plugin.EagleCool
{
    public class Levenshtein
    {
        public static int Distance(string a, string b)
        {
            if (string.IsNullOrWhiteSpace(a))
                return string.IsNullOrWhiteSpace(b) ? 0 : b.Length;
            if (string.IsNullOrWhiteSpace(b))
                return string.IsNullOrWhiteSpace(a) ? 0 : a.Length;
            
            var aLength = a.Length;
            var bLength = b.Length;
            var combinations = new int[aLength + 1, bLength + 1];
            for (var i = 0; i <= aLength; combinations[i, 0] = i++) { }
            for (var j = 0; j <= bLength; combinations[0, j] = j++) { }
            
            for (var i = 1; i <= aLength; i++) for (var j = 1; j <= bLength; j++)
            {
                var cost = b[j - 1] == a[i - 1] ? 0 : 1;
                combinations[i, j] = Math.Min(
                    Math.Min(combinations[i - 1, j] + 1, combinations[i, j - 1] + 1),
                    combinations[i - 1, j - 1] + cost);
            }

            return combinations[aLength, bLength];
        }
    }
}
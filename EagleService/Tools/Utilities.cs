using System.Text.RegularExpressions;

namespace Eagle
{
    public class Utilities
    {
        static public string UpperCaseFirstChar(string text) {
            return Regex.Replace(text, "^[a-z]", m => m.Value.ToUpper());
        }
    }
}
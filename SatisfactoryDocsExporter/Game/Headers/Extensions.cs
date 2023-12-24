using System.Text.RegularExpressions;

namespace SatisfactoryDocsExporter.Game.Headers;

public static class Extensions
{
    public static string Replace(this string str, Regex regex, string newValue)
    {
        return regex.Replace(str, newValue);
    }
}
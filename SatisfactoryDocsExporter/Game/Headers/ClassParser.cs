using System.Text.RegularExpressions;
using Serilog;

namespace SatisfactoryDocsExporter.Game.Headers;

public partial class ClassParser
{
    private static readonly string[] IgnoredClasses =
    {
        "UFGAnimNotify_AutoAkEvent"
    };

    public List<ClassDefinition> ParseFile(string content)
    {
        return GetSimpleClassLineRegex()
            .Matches(content)
            .Select(classLineMatch => ParseLine(classLineMatch.Value))
            .OfType<ClassDefinition>()
            .ToList();
    }

    private ClassDefinition? ParseLine(string classLine)
    {
        if (IgnoredClasses.Any(ignoredClass => classLine.Contains(ignoredClass))) return null;

        classLine = classLine
            .Replace(GetCommentRegex(), "")
            .Replace(GetGenericsRegex(), "")
            .Replace("final", "")
            .Replace("public", "")
            .Replace("::", "__");

        var match = GetClassNameRegex().Match(classLine);

        if (!match.Success) Log.Warning("Failed to match class: {0}", classLine);

        return match.Success ? ExtractClassDefinition(match) : null;
    }

    private ClassDefinition ExtractClassDefinition(Match match)
    {
        var className = match.Groups[1].Value;

        var parents = Enumerable.Empty<Capture>();

        if (match.Groups[2].Success) parents = parents.Concat(match.Groups[2].Captures);

        if (match.Groups[3].Success) parents = parents.Concat(match.Groups[3].Captures);

        var parentClassNames = parents
            .Select(capture => capture.Value)
            .ToList();

        return new ClassDefinition(className, parentClassNames);
    }

    [GeneratedRegex(@"^class\s+FACTORYGAME_API\s+(\w+)(?:\s*:\s*(\w+)(?:\s*,\s*(\w+))*)?\s*$")]
    private static partial Regex GetClassNameRegex();

    [GeneratedRegex(@"^class\s+FACTORYGAME_API.*$", RegexOptions.Multiline)]
    private static partial Regex GetSimpleClassLineRegex();

    [GeneratedRegex(@"(?:/\*.*?\*/)|(?://.*)")]
    private static partial Regex GetCommentRegex();

    [GeneratedRegex(@"<\s*\w+\s*>")]
    private static partial Regex GetGenericsRegex();
}
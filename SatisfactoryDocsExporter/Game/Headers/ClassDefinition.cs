using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace SatisfactoryDocsExporter.Game.Headers;

public partial class ClassDefinition(string className, IEnumerable<string> parentClassNames)
{
    public string ClassName { get; } = NormalizeClassName(className);
    public ImmutableList<string> ParentClassNames { get; } = parentClassNames.Distinct().Select(NormalizeClassName).ToImmutableList();

    private static string NormalizeClassName(string className)
    {
        return GetClassPrefixRegex().Replace(className, "FG");
    }

    [GeneratedRegex(@"^[UA]FG")]
    private static partial Regex GetClassPrefixRegex();
}
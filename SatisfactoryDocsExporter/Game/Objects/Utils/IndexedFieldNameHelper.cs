using CUE4Parse.UE4.Assets.Objects;

namespace SatisfactoryDocsExporter.Game.Objects.Utils;

public class IndexedFieldNameHelper
{
    private readonly Dictionary<string, bool> _useIndex = new();

    public IndexedFieldNameHelper(IEnumerable<FPropertyTag> properties)
    {
        foreach (var property in properties) Register(property);
    }

    public string GetName(FPropertyTag property)
    {
        var namePlain = property.Name.Text;
        var nameIndexed = $"{namePlain}[{property.ArrayIndex}]";

        return UseIndex(property) ? nameIndexed : namePlain;
    }

    private void Register(FPropertyTag property)
    {
        if (property.ArrayIndex > 0) _useIndex.TryAdd(property.Name.Text, true);
    }

    private bool UseIndex(FPropertyTag property)
    {
        return _useIndex.GetValueOrDefault(property.Name.Text, false);
    }
}
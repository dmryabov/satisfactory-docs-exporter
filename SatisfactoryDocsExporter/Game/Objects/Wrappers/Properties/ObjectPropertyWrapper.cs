using CUE4Parse.UE4.Assets.Objects.Properties;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

public class ObjectPropertyWrapper : AbstractPropertyWrapper<ObjectProperty>
{
    private static readonly HashSet<string> ExportedTypes =
    [
        "ScriptStruct",
        "Class",
        "Texture2D",
        "BlueprintGeneratedClass"
    ];

    private readonly object? _value;

    public ObjectPropertyWrapper(ObjectProperty property) : base(property)
    {
        _value = InitValue();
    }

    public override bool IsExported()
    {
        return _value != null && IsExportedType();
    }

    public override object? GetValue()
    {
        return _value;
    }

    public string GetStringValue()
    {
        return Property.Value.Name;
    }

    private string GetValueType()
    {
        var obj = Property.Value.ResolvedObject;

        return obj?.Class?.Name.Text ?? obj?.GetType().Name ?? "Unknown";
    }

    private bool IsExportedType()
    {
        var type = GetValueType();

        return ExportedTypes.Contains(type) || type.StartsWith("BP_");
    }

    private object? InitValue()
    {
        var obj = Property.Value.ResolvedObject;

        if (obj == null) return null;

        return new Dictionary<string, string?>
        {
            { "Class", obj.GetFullName() },
            { "Name", obj.GetPathName(false) },
            { "Type", obj.Class?.Name.Text ?? obj.GetType().Name }
        };
    }
}
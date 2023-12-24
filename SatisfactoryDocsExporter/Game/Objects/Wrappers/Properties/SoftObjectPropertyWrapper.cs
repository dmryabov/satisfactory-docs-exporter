using CUE4Parse.UE4.Assets.Objects.Properties;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

public class SoftObjectPropertyWrapper : AbstractPropertyWrapper<SoftObjectProperty>
{
    private const string ScriptPrefix = "/Script/FactoryGame.";

    private static readonly HashSet<string> ExportedTypes =
    [
        "Texture2D",
        "BlueprintGeneratedClass"
    ];

    private readonly object? _value;

    public SoftObjectPropertyWrapper(SoftObjectProperty property) : base(property)
    {
        _value = InitValue();
    }

    public override bool IsExported()
    {
        return _value != null;
    }

    public override object? GetValue()
    {
        return _value;
    }

    private object? InitValue()
    {
        var asset = Property.Value.AssetPathName.Text;

        if (asset.StartsWith(ScriptPrefix))
        {
            var className = asset[ScriptPrefix.Length..];

            return new Dictionary<string, string>
            {
                { "Class", $"Class'{asset}'" },
                { "Name", className },
                { "Type", "Class" }
            };
        }

        if (!Property.Value.TryLoad(out var uobject)) return null;
        if (!ExportedTypes.Contains(uobject.ExportType)) return null;

        return new Dictionary<string, string>
        {
            { "Class", uobject.GetFullName() },
            { "Name", uobject.GetShortName() },
            { "Type", uobject.ExportType }
        };
    }
}
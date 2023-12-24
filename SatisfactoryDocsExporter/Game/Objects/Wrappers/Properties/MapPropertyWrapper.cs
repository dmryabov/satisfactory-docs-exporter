using CUE4Parse.UE4.Assets.Objects.Properties;
using Serilog;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

public class MapPropertyWrapper : AbstractPropertyWrapper<MapProperty>
{
    private readonly Dictionary<string, IPropertyWrapper> _dict;

    public MapPropertyWrapper(MapProperty property) : base(property)
    {
        _dict = InitValues();
    }

    public override bool IsExported()
    {
        return _dict.Count > 0;
    }

    public override object? GetValue()
    {
        return _dict;
    }

    private Dictionary<string, IPropertyWrapper> InitValues()
    {
        return Property.Value.Properties
            .Select(MakeKeyValuePair)
            .Where(pair => pair.Value.IsExported())
            .ToDictionary();
    }

    private static KeyValuePair<string, IPropertyWrapper> MakeKeyValuePair(KeyValuePair<FPropertyTagType, FPropertyTagType?> pair)
    {
        return new KeyValuePair<string, IPropertyWrapper>(StringifyKey(pair.Key), PropertyWrapperFactory.Wrap(pair.Value));
    }

    private static string StringifyKey(FPropertyTagType tag)
    {
        return tag switch
        {
            IntProperty p => new IntPropertyWrapper(p).GetIntValue().ToString(),
            NameProperty p => new NamePropertyWrapper(p).GetValue(),
            ObjectProperty p => new ObjectPropertyWrapper(p).GetStringValue(),
            _ => LogUnknownKeyType(tag)
        };
    }

    private static string LogUnknownKeyType(FPropertyTagType tag)
    {
        Log.Warning("Unknown map key type {0}", tag.GetType().ToString());

        return "UNKNOWN_KEY_TYPE";
    }
}
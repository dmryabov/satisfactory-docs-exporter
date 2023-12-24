using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Objects.Properties;
using SatisfactoryDocsExporter.Game.Objects.Utils;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

public class StructPropertyWrapper : AbstractPropertyWrapper<StructProperty>
{
    private readonly Dictionary<string, IPropertyWrapper>? _dict;

    public StructPropertyWrapper(StructProperty property) : base(property)
    {
        _dict = InitValues();
    }

    public override bool IsExported()
    {
        return _dict == null || _dict.Count > 0;
    }

    public override object? GetValue()
    {
        return _dict != null ? _dict : Property;
    }

    private Dictionary<string, IPropertyWrapper>? InitValues()
    {
        if (Property.Value.StructType is not IPropertyHolder value) return null;

        var nameHelper = new IndexedFieldNameHelper(value.Properties);

        return value.Properties
            .Select(p => MakeKeyValuePair(p, nameHelper))
            .Where(pair => pair.Value.IsExported())
            .ToDictionary();
    }

    private static KeyValuePair<string, IPropertyWrapper> MakeKeyValuePair(FPropertyTag property, IndexedFieldNameHelper nameHelper)
    {
        return new KeyValuePair<string, IPropertyWrapper>(nameHelper.GetName(property), PropertyWrapperFactory.Wrap(property.Tag));
    }
}
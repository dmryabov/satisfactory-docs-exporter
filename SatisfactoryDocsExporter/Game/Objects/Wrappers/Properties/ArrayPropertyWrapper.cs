using CUE4Parse.UE4.Assets.Objects.Properties;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

public class ArrayPropertyWrapper : AbstractPropertyWrapper<ArrayProperty>
{
    private readonly List<IPropertyWrapper> _values;

    public ArrayPropertyWrapper(ArrayProperty property) : base(property)
    {
        _values = InitValues();
    }

    public override bool IsExported()
    {
        return _values.Count > 0;
    }

    public override object? GetValue()
    {
        return _values;
    }

    private List<IPropertyWrapper> InitValues()
    {
        return Property.Value.Properties
            .Select(PropertyWrapperFactory.Wrap)
            .Where(p => p.IsExported())
            .ToList();
    }
}
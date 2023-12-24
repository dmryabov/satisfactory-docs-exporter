using CUE4Parse.UE4.Assets.Objects.Properties;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

public class IntPropertyWrapper(IntProperty property) : AbstractPropertyWrapper<IntProperty>(property)
{
    public override bool IsExported()
    {
        return true;
    }

    public override object? GetValue()
    {
        return GetIntValue();
    }

    public int GetIntValue()
    {
        return Property.Value;
    }
}
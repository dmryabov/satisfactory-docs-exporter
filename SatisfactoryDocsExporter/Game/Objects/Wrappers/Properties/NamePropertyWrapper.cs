using CUE4Parse.UE4.Assets.Objects.Properties;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

public class NamePropertyWrapper(NameProperty property) : AbstractPropertyWrapper<NameProperty>(property)
{
    public override bool IsExported()
    {
        return true;
    }

    public override string GetValue()
    {
        return Property.Value.Text;
    }
}
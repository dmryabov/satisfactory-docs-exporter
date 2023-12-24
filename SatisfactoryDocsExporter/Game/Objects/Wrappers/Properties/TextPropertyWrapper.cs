using CUE4Parse.UE4.Assets.Objects.Properties;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

public class TextPropertyWrapper(TextProperty property) : AbstractPropertyWrapper<TextProperty>(property)
{
    public override bool IsExported()
    {
        return true;
    }

    public override object? GetValue()
    {
        return Property.Value.Text;
    }
}
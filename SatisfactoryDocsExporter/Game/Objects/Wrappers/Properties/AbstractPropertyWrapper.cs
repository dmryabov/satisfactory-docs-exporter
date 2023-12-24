using CUE4Parse.UE4.Assets.Objects.Properties;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

public abstract class AbstractPropertyWrapper<T>(T property) : IPropertyWrapper
    where T : FPropertyTagType
{
    protected readonly T Property = property;

    public abstract bool IsExported();

    public abstract object? GetValue();
}
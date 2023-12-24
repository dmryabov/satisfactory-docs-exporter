using CUE4Parse.UE4.Assets.Objects;
using SatisfactoryDocsExporter.Game.Objects.Utils;
using SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers;

public class UObjectPropertyWrapper : IPropertyWrapper
{
    private static readonly HashSet<string> ExcludedNames =
    [
        "RootComponent",
        "UberGraphFrame",
        "mValidBuildables",
        "mHologramClass",
        "mCachedStackSize"
    ];

    private readonly IPropertyWrapper _property;

    public UObjectPropertyWrapper(FPropertyTag property, IndexedFieldNameHelper nameHelper)
    {
        _property = PropertyWrapperFactory.Wrap(property.Tag!);
        Name = nameHelper.GetName(property);
    }

    public string Name { get; }

    public bool IsExported()
    {
        return !ExcludedNames.Contains(Name) && _property.IsExported();
    }

    public object? GetValue()
    {
        return _property.GetValue();
    }
}
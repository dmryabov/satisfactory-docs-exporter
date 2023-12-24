using CUE4Parse.UE4.Assets.Objects.Properties;
using Serilog;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

public class DebugPropertyWrapper(FPropertyTagType? property) : IPropertyWrapper
{
    public bool IsExported()
    {
        return true;
    }

    public object? GetValue()
    {
        Log.Warning("Trying to wrap unknown property type {0}", property?.GetType().Name ?? "NULL");

        return property;
    }
}
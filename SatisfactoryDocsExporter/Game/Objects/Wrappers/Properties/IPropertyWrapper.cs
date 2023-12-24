using Newtonsoft.Json;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

[JsonConverter(typeof(PropertyConverter))]
public interface IPropertyWrapper
{
    public bool IsExported();

    public object? GetValue();
}
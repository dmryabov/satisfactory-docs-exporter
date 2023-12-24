using Newtonsoft.Json;
using SatisfactoryDocsExporter.Game.Objects.Wrappers;
using SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace SatisfactoryDocsExporter.Game.Objects;

public class JsonConverters : JsonConverter<UObjectWrapper>
{
    public override void WriteJson(JsonWriter writer, UObjectWrapper? value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        value?.WriteJson(writer, serializer);
        writer.WriteEndObject();
    }

    public override UObjectWrapper? ReadJson(JsonReader reader, Type objectType, UObjectWrapper? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public class PropertyConverter : JsonConverter<IPropertyWrapper>
{
    public override void WriteJson(JsonWriter writer, IPropertyWrapper? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value?.GetValue());
    }

    public override IPropertyWrapper? ReadJson(JsonReader reader, Type objectType, IPropertyWrapper? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
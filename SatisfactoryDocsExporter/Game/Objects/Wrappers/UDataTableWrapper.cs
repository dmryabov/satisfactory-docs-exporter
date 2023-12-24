using CUE4Parse.UE4.Assets.Exports.Engine;
using Newtonsoft.Json;
using SatisfactoryDocsExporter.Game.Objects.Wrappers.Properties;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers;

public class UDataTableWrapper : UObjectWrapper
{
    private readonly UDataTable _inner;

    public UDataTableWrapper(UDataTable inner) : base(inner, "DataTable", ["DataTable"])
    {
        _inner = inner;
    }

    protected override void WriteJsonProperties(JsonWriter writer, JsonSerializer serializer)
    {
        base.WriteJsonProperties(writer, serializer);

        writer.WritePropertyName("Rows");

        writer.WriteStartObject();
        foreach (var row in _inner.RowMap)
        {
            var value = row.Value.Properties
                .Select(tag => new KeyValuePair<string, IPropertyWrapper>(tag.Name.Text, PropertyWrapperFactory.Wrap(tag.Tag)))
                .ToDictionary();

            writer.WritePropertyName(row.Key.Text);
            serializer.Serialize(writer, value);
        }

        writer.WriteEndObject();
    }
}
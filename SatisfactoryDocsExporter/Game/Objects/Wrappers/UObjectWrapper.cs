using System.Collections.Immutable;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.Engine;
using Newtonsoft.Json;
using SatisfactoryDocsExporter.Game.Objects.Utils;

namespace SatisfactoryDocsExporter.Game.Objects.Wrappers;

[JsonConverter(typeof(JsonConverters))]
public class UObjectWrapper
{
    private readonly UObject _inner;
    private readonly Lazy<List<UObjectPropertyWrapper>> _properties;

    public UObjectWrapper(UObject inner, string nativeClass, IEnumerable<string> nativeClassChain)
    {
        _inner = inner;
        _properties = new Lazy<List<UObjectPropertyWrapper>>(GetProperties);

        Id = inner.GetFullName();
        NativeClass = nativeClass;
        NativeClassChain = nativeClassChain.ToImmutableList();
    }

    public string Id { get; }
    public string NativeClass { get; }
    public ImmutableList<string> NativeClassChain { get; }
    public List<UObjectPropertyWrapper> Properties => _properties.Value;

    protected internal void WriteJson(JsonWriter writer, JsonSerializer serializer)
    {
        WriteJsonHeader(writer, serializer);
        WriteJsonProperties(writer, serializer);
    }

    protected virtual void WriteJsonHeader(JsonWriter writer, JsonSerializer serializer)
    {
        writer.WritePropertyName("NativeClass");
        writer.WriteValue(NativeClass);

        writer.WritePropertyName("NativeClassChain");
        serializer.Serialize(writer, NativeClassChain);

        writer.WritePropertyName("Class");
        writer.WriteValue(_inner.GetFullName());

        writer.WritePropertyName("Name");
        writer.WriteValue(_inner.GetShortName());

        writer.WritePropertyName("Type");
        writer.WriteValue(_inner.ExportType);
    }

    protected virtual void WriteJsonProperties(JsonWriter writer, JsonSerializer serializer)
    {
        if (Properties.Count == 0) return;

        writer.WritePropertyName("Properties");

        writer.WriteStartObject();

        foreach (var property in Properties.Where(p => p.IsExported()))
        {
            writer.WritePropertyName(property.Name);
            serializer.Serialize(writer, property);
        }

        writer.WriteEndObject();
    }

    public override string ToString()
    {
        return $"UObjectWrapper {_inner}";
    }

    private List<UObjectPropertyWrapper> GetProperties()
    {
        var uobject = _inner switch
        {
            UBlueprintGeneratedClass blueprint => blueprint.ClassDefaultObject.Load()!,
            _ => _inner
        };

        var nameHelper = new IndexedFieldNameHelper(uobject.Properties);

        return uobject.Properties
            .Where(p => p.Tag != null)
            .Select(p => new UObjectPropertyWrapper(p, nameHelper))
            .Where(p => p.IsExported())
            .ToList();
    }
}
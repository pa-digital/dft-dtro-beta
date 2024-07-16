using System;
using DfT.DTRO.Models.SchemaTemplate;
using Newtonsoft.Json;

namespace DfT.DTRO.Converters;

public class SchemaVersionJsonConverter : JsonConverter<SchemaVersion>
{
    public override SchemaVersion ReadJson(JsonReader reader, Type objectType, SchemaVersion existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String)
        {
            throw new JsonException($"Can't convert value of type {reader.TokenType} to a SchemaVersion object.");
        }

        var stringValue = reader.Value as string;

        try
        {
            return new SchemaVersion(stringValue);
        }
        catch (Exception ex)
        {
            throw new JsonException($"Can't convert value '{stringValue}' to a SchemaVersion object.", ex);
        }
    }

    public override void WriteJson(JsonWriter writer, SchemaVersion value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}

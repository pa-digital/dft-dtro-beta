using System;
using System.Collections.Generic;
using System.Linq;
using DfT.DTRO.Models.DtroJson;
using Newtonsoft.Json;

namespace DfT.DTRO.Converters;

public class BoundingBoxJsonConverter : JsonConverter<BoundingBox>
{
    public override void WriteJson(JsonWriter writer, BoundingBox value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.WestLongitude);
        writer.WriteValue(value.SouthLatitude);
        writer.WriteValue(value.EastLongitude);
        writer.WriteValue(value.NorthLatitude);
        writer.WriteEndArray();
    }

    public override BoundingBox ReadJson(
        JsonReader reader,
        Type objectType,
        BoundingBox existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        try
        {
            List<double?> coordinates = new()
            {
                reader.ReadAsDouble(),
                reader.ReadAsDouble(),
                reader.ReadAsDouble(),
                reader.ReadAsDouble()
            };

            if (!coordinates.All(coordinate => coordinate.HasValue))
            {
                throw new JsonReaderException("Invalid bounding box definition");
            }

            BoundingBox boundingBox = new(
                coordinates[0].GetValueOrDefault(),
                coordinates[1].GetValueOrDefault(),
                coordinates[2].GetValueOrDefault(),
                coordinates[3].GetValueOrDefault());
            reader.Read();
            return boundingBox;
        }
        catch (InvalidOperationException e)
        {
            throw new JsonReaderException("Invalid bounding box definition", e);
        }
    }
}
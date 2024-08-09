using Newtonsoft.Json;

namespace DfT.DTRO.Converters;

/// <summary>
/// Converts tuple of 4 coordinates into <see cref="BoundingBox"/> object
/// </summary>
public class BoundingBoxJsonConverter : JsonConverter<BoundingBox>
{
    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, BoundingBox value, JsonSerializer serializer)
    {
        writer.WriteStartArray();
        writer.WriteValue(value.WestLongitude);
        writer.WriteValue(value.SouthLatitude);
        writer.WriteValue(value.EastLongitude);
        writer.WriteValue(value.NorthLatitude);
        writer.WriteEndArray();
    }

    /// <inheritdoc />
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
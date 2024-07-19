using Newtonsoft.Json;

namespace Dft.DTRO.Tests.CodeiumTests.Integration;

[ExcludeFromCodeCoverage]
public class BoundingBoxJsonConverterTests
{
    private readonly JsonSerializer _serializer;

    public BoundingBoxJsonConverterTests()
    {
        _serializer = new JsonSerializer();
        _serializer.Converters.Add(new BoundingBoxJsonConverter());
    }

    [Fact]
    public void WriteJson_ShouldSerializeBoundingBoxToJson()
    {
        var boundingBox = new BoundingBox(1.0, 2.0, 3.0, 4.0);
        var writer = new StringWriter();
        _serializer.Serialize(writer, boundingBox);

        Assert.Equal("[1.0,2.0,3.0,4.0]", writer.ToString());
    }

    [Fact]
    public void ReadJson_ShouldDeserializeJsonToBoundingBox()
    {
        var json = "[1.0,2.0,3.0,4.0]";
        var reader = new JsonTextReader(new StringReader(json));
        var boundingBox = _serializer.Deserialize<BoundingBox>(reader);

        Assert.Equal(1.0, boundingBox.WestLongitude);
        Assert.Equal(2.0, boundingBox.SouthLatitude);
        Assert.Equal(3.0, boundingBox.EastLongitude);
        Assert.Equal(4.0, boundingBox.NorthLatitude);
    }

    [Fact]
    public void ReadJson_ShouldThrowExceptionForInvalidJson()
    {
        var json = "[1.0,2.0,3.0]";
        var reader = new JsonTextReader(new StringReader(json));

        Assert.Throws<JsonReaderException>(() => _serializer.Deserialize<BoundingBox>(reader));
    }
}
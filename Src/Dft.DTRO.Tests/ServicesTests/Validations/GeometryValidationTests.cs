using Newtonsoft.Json.Linq;

namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class GeometryValidationTests
{
    private readonly IGeometryValidation _sut;

    public GeometryValidationTests()
    {
        LoggingExtension loggingExtension = new();
        IBoundingBoxService boundingBoxService = new BoundingBoxService(loggingExtension);

        _sut = new GeometryValidation(boundingBoxService, loggingExtension);
    }

    [Fact]
    public void ValidateAgainstCurrentSchemaVersionReturnsBoundingBox()
    {
        List<SemanticValidationError> errors = new();
        const string payload = "{\r\n    \"Polygon\": {\r\n        \"version\": 1,\r\n        \"polygon\": \"SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))\",\r\n        \"ExternalReference\": [\r\n            {\r\n                \"lastUpdateDate\": \"1981-02-08 11:30:43\",\r\n                \"UniqueStreetReferenceNumber\": {\r\n                    \"usrn\": 96854586\r\n                }\r\n            }\r\n        ]\r\n    }\r\n}";
        BoundingBox expected = new() { WestLongitude = 178750, SouthLatitude = 178750, EastLongitude = 529200, NorthLatitude = 529200 };

        var actual = _sut.ValidateGeometryAgainstCurrentSchemaVersion(JObject.Parse(payload), errors);

        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ValidateAgainstPreviousSchemaVersionsReturnsBoundingBox()
    {
        List<SemanticValidationError> errors = new();
        const string payload = " {\r\n     \"geometry\": {\r\n         \"version\": 1,\r\n         \"DirectedLinear\": {\r\n             \"directedLineString\": \"SRID=27700;LINESTRING(529050 178750, 529157 178805, 529250 178860)\"\r\n         },\r\n         \"ExternalReference\": [\r\n             {\r\n                 \"lastUpdateDate\": \"1981-02-08 11:30:43\",\r\n                 \"UniqueStreetReferenceNumber\": {\r\n                     \"usrn\": 96854586\r\n                 }\r\n             }\r\n         ]\r\n     }\r\n }";
        var schemaVersion = new SchemaVersion("3.2.0");
        BoundingBox expected = new() { WestLongitude = 178750, SouthLatitude = 178750, EastLongitude = 529250, NorthLatitude = 529250 };
        var actual = _sut.ValidateGeometryAgainstPreviousSchemaVersions(JObject.Parse(payload), schemaVersion, errors);

        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }
}
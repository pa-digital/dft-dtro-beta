using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dft.DTRO.Tests.ServicesTests.Validations;

[ExcludeFromCodeCoverage]
public class SemanticValidationServiceTests
{
    private readonly Mock<ISystemClock> _mockClock;
    private readonly Mock<IOldConditionValidationService> _mockConditionValidationService;
    private readonly Mock<IDtroDal> _mockDtroDal;
    private readonly IGeometryValidation _geometryValidation;
    private readonly LoggingExtension _loggingExtension;


    public SemanticValidationServiceTests()
    {
        _mockClock = new Mock<ISystemClock>();
        _mockDtroDal = new Mock<IDtroDal>();
        _mockConditionValidationService = new Mock<IOldConditionValidationService>();
        _loggingExtension = new LoggingExtension();
        IBoundingBoxService boundingBoxService = new BoundingBoxService(_loggingExtension);
        _geometryValidation = new GeometryValidation(boundingBoxService, _loggingExtension);
        _mockClock.Setup(it => it.UtcNow).Returns(new DateTime(2023, 5, 1));
    }

    [Theory]
    [InlineData("3.2.4")]
    [InlineData("3.3.0")]
    public async Task AllowsLastUpdateDateInThePast(string version)
    {
        DtroSubmit dtro = PrepareDtro(version != "3.3.0"
            ? @"{""geometry"": { ""version"": 1, ""Polygon"": { ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1927-09-26 00:00:00""}]}}"
            : @"{""Polygon"": { ""version"": 1, ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))"", ""ExternalReference"": [{ ""lastUpdateDate"": ""1927-09-26 00:00:00""}]}}", version);

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _geometryValidation, _loggingExtension);

        Tuple<BoundingBox, List<SemanticValidationError>>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result.Item2);
    }

    [Theory]
    [InlineData("3.2.4")]
    [InlineData("3.3.0")]
    public async Task AllowsCoordinatesWithinPolygon(string version)
    {
        DtroSubmit dtro = PrepareDtro(version != "3.3.0"
            ? @"{""geometry"": { ""version"": 1, ""Polygon"": { ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}"
            : @"{""Polygon"": { ""version"": 1, ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))"", ""ExternalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}", version);

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _geometryValidation, _loggingExtension);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 178750, SouthLatitude = 178750, EastLongitude = 529200, NorthLatitude = 529200 };
        Assert.Equal(expected, actual.Item1);
    }

    [Theory]
    [InlineData("3.2.4")]
    [InlineData("3.3.0")]
    public async Task AllowsCoordinatesWithinPointGeometry(string version)
    {
        DtroSubmit dtro = PrepareDtro(version != "3.3.0"
            ? @"{""geometry"": { ""version"": 1, ""PointGeometry"": { ""point"": ""SRID=27700;POINT(529157 178805)"", ""representation"": ""centreLinePoint""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}"
            : @"{""PointGeometry"": { ""version"": 1, ""point"": ""SRID=27700;POINT(529157 178805)"", ""representation"": ""centreLinePoint"", ""ExternalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}", version);

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _geometryValidation, _loggingExtension);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 178805, SouthLatitude = 178805, EastLongitude = 529157, NorthLatitude = 529157 };
        Assert.Equal(expected, actual.Item1);
    }

    [Theory]
    [InlineData("3.2.4")]
    [InlineData("3.3.0")]
    public async Task AllowsCoordinatesWithinLinearGeometry(string version)
    {
        DtroSubmit dtro = PrepareDtro(version != "3.3.0"
            ? @"{""geometry"": { ""version"": 1, ""LinearGeometry"": { ""direction"": ""bidirectional"", ""lateralPosition"": ""centreline"",""linestring"": ""SRID=27700;LINESTRING(529050 178750, 529157 178805, 529250 178860)""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}"
            : @"{""LinearGeometry"": { ""version"": 1, ""direction"": ""bidirectional"", ""lateralPosition"": ""centreline"",""linestring"": ""SRID=27700;LINESTRING(529050 178750, 529157 178805, 529250 178860)"", ""ExternalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}", version);

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _geometryValidation, _loggingExtension);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 178750, SouthLatitude = 178750, EastLongitude = 529250, NorthLatitude = 529250 };
        Assert.Equal(expected, actual.Item1);
    }

    [Theory]
    [InlineData("3.2.4")]
    [InlineData("3.3.0")]
    public async Task AllowsCoordinatesWithinDirectedLinearGeometry(string version)
    {
        DtroSubmit dtro = PrepareDtro(version != "3.3.0"
         ? @"{""geometry"": { ""version"": 1, ""DirectedLinear"": { ""directedLineString"": ""SRID=27700;LINESTRING(529050 178750, 529157 178805, 529250 178860)""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}"
         : @"{""DirectedLinear"": { ""version"": 1, ""directedLineString"": ""SRID=27700;LINESTRING(529050 178750, 529157 178805, 529250 178860)"", ""ExternalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}", version);

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _geometryValidation, _loggingExtension);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 178750, SouthLatitude = 178750, EastLongitude = 529250, NorthLatitude = 529250 };
        Assert.Equal(expected, actual.Item1);
    }

    [Theory]
    [InlineData("3.2.4")]
    [InlineData("3.3.0")]
    public async Task WhenVersionGeometryIsIntegerReturnsNoError(string version)
    {
        DtroSubmit dtro = PrepareDtro(version != "3.3.0"
            ? @"{""geometry"": { ""version"": 1, ""Polygon"": { ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}"
            : @"{""Polygon"": { ""version"": 1, ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))"", ""ExternalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}", version);


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _geometryValidation, _loggingExtension);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);
        Assert.Empty(actual.Item2);
    }

    [Theory]
    [InlineData("3.2.4")]
    [InlineData("3.3.0")]
    public async Task WhenVersionGeometryIsNotIntegerReturnsError(string version)
    {
        DtroSubmit dtro = PrepareDtro(version != "3.3.0"
            ? @"{""geometry"": { ""version"": ""1"", ""Polygon"": { ""polygon"": ""SRID=27700;POLYGON((30 10, 40 40, 20 40, 10 20, 30 10))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}"
            : @"{""Polygon"": { ""version"": ""1"", ""polygon"": ""SRID=27700;POLYGON((30 10, 40 40, 20 40, 10 20, 30 10))"", ""ExternalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}", version);


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _geometryValidation, _loggingExtension);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);
        Assert.NotEmpty(actual.Item2);
    }


    [Theory]
    [InlineData("3.2.4")]
    [InlineData("3.3.0")]
    public async Task WhenBritishNationalGridSpatialReferenceIsNotPresentReturnsError(string version)
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""ExternalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}", version);


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _geometryValidation, _loggingExtension);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);
        Assert.NotEmpty(actual.Item2);
    }

    [Theory]
    [InlineData("3.2.4")]
    [InlineData("3.3.0")]
    public async Task WhenBritishNationalGridSpatialReferenceIsIncorrectReturnsError(string version)
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""SRID=27770;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""ExternalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}", version);


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _geometryValidation, _loggingExtension);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);
        Assert.NotEmpty(actual.Item2);
    }

    [Fact]
    public async Task TypoInExternalReferenceReturnsError()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""externalR"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}", "3.3.0");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _geometryValidation, _loggingExtension);

        Tuple<BoundingBox, List<SemanticValidationError>>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result.Item2);
    }

    private static DtroSubmit PrepareDtro(string jsonData, string schemaVersion) =>
        new()
        {
            Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonData, new ExpandoObjectConverter()),
            SchemaVersion = schemaVersion
        };
}
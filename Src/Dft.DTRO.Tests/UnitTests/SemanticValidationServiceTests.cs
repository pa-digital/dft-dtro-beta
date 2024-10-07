using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class SemanticValidationServiceTests
{
    private readonly Mock<ISystemClock> _mockClock;
    private readonly Mock<IConditionValidationService> _mockConditionValidationService;
    private readonly Mock<IDtroDal> _mockDtroDal;
    private readonly IBoundingBoxService _boundingBoxService;

    public SemanticValidationServiceTests()
    {
        _mockClock = new Mock<ISystemClock>();
        _mockDtroDal = new Mock<IDtroDal>();
        _mockConditionValidationService = new Mock<IConditionValidationService>();
        _boundingBoxService = new BoundingBoxService();
        _mockClock.Setup(it => it.UtcNow).Returns(new DateTime(2023, 5, 1));
    }

    [Fact]
    public async Task AllowsLastUpdateDateInThePast()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1927-09-26 00:00:00""}]}}");


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result.Item2);
    }

    [Fact]
    public async Task DisallowsLastUpdateDateInTheFuture()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""externalReference"": [{ ""lastUpdateDate"": ""2227-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result.Item2);
    }

    [Fact]
    public async Task AllowsCoordinatesWithinPolygon()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 529100, SouthLatitude = 178750, EastLongitude = 529200, NorthLatitude = 178860 };
        Assert.Equal(expected, actual.Item1);
    }

    [Fact]
    public async Task AllowsCoordinatesWithinPointGeometry()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""PointGeometry"": {
            ""point"": ""SRID=27700;POINT(529157 178805)"", ""representation"": ""centreLinePoint""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 529157, SouthLatitude = 178805, EastLongitude = 529157, NorthLatitude = 178805 };
        Assert.Equal(expected, actual.Item1);
    }

    [Fact]
    public async Task AllowsCoordinatesWithinLinearGeometry()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""LinearGeometry"": {
            ""direction"": ""bidirectional"", ""lateralPosition"": ""centreline"",""linestring"": ""SRID=27700;LINESTRING(529050 178750, 529157 178805, 529250 178860)""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 529050, SouthLatitude = 178750, EastLongitude = 529250, NorthLatitude = 178860 };
        Assert.Equal(expected, actual.Item1);
    }

    [Fact]
    public async Task AllowsCoordinatesWithinDirectedLinearGeometry()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""DirectedLinear"": {
            ""directedLineString"": ""SRID=27700;LINESTRING(529050 178750, 529157 178805, 529250 178860)""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 529050, SouthLatitude = 178750, EastLongitude = 529250, NorthLatitude = 178860 };
        Assert.Equal(expected, actual.Item1);
    }

    [Fact]
    public async Task WhenVersionGeometryIsIntegerReturnsNoError()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);
        Assert.Empty(actual.Item2);
    }

    [Fact]
    public async Task WhenVersionGeometryIsNotIntegerReturnsError()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": ""1"", ""Polygon"": {
            ""polygon"": ""SRID=27700;POLYGON((30 10, 40 40, 20 40, 10 20, 30 10))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);
        Assert.NotEmpty(actual.Item2);
    }

    [Fact]
    public async Task DoNotAllowsCoordinatesWithNotAcceptedGeometry()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Square"": {
            ""Circle"": ""SRID=27700;Circle((30 10, 40 40, 20 40, 10 20, 30 10))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(actual.Item2);
    }

    [Fact]
    public async Task WhenBritishNationalGridSpatialReferenceIsNotPresentReturnsError()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);
        Assert.NotEmpty(actual.Item2);
    }

    [Fact]
    public async Task WhenBritishNationalGridSpatialReferenceIsIncorrectReturnsError()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""SRID=27770;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);
        Assert.NotEmpty(actual.Item2);
    }

    [Fact]
    public async Task TypoInExternalReferenceReturnsError()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""SRID=27700;POLYGON((529100 178750, 529200 178750, 529200 178860, 529100 178860, 529100 178750))""}, ""externalR"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result.Item2);
    }
    private static DtroSubmit PrepareDtro(string jsonData, string schemaVersion = "3.2.3")
    {
        return new DtroSubmit
        {
            Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonData, new ExpandoObjectConverter()),
            SchemaVersion = schemaVersion
        };
    }
}
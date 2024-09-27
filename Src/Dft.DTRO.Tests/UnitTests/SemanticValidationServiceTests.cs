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
            ""polygon"": ""((30 10, 40 40, 20 40, 10 20, 30 10))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1927-09-26 00:00:00""}]}}");


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result.Item2);
    }

    [Fact]
    public async Task DisallowsLastUpdateDateInTheFuture()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""((30 10, 40 40, 20 40, 10 20, 30 10))""}, ""externalReference"": [{ ""lastUpdateDate"": ""2227-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result.Item2);
    }

    [Fact]
    public async Task AllowsCoordinatesWithinPolygon()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""((30 10, 40 40, 20 40, 10 20, 30 10))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 10, SouthLatitude = 10, EastLongitude = 40, NorthLatitude = 40 };
        Assert.Equal(expected, actual.Item1);
    }

    [Fact]
    public async Task AllowsCoordinatesWithinPointGeometry()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""PointGeometry"": {
            ""point"": ""((10 40), (40 30), (20 20), (30 10))"", ""representation"": ""centreLinePoint""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 10, SouthLatitude = 10, EastLongitude = 40, NorthLatitude = 40 };
        Assert.Equal(expected, actual.Item1);
    }

    [Fact]
    public async Task AllowsCoordinatesWithinLinearGeometry()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""LinearGeometry"": {
            ""direction"": ""bidirectional"", ""lateralPosition"": ""centreline"",""linestring"": ""(30 10, 10 30, 40 40)""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 10, SouthLatitude = 10, EastLongitude = 40, NorthLatitude = 40 };
        Assert.Equal(expected, actual.Item1);
    }

    [Fact]
    public async Task AllowsCoordinatesWithinDirectedLinearGeometry()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""DirectedLinear"": {
            ""directedLineString"": ""(10 10, 20 20, 10 40)""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        BoundingBox expected = new() { WestLongitude = 10, SouthLatitude = 10, EastLongitude = 20, NorthLatitude = 40 };
        Assert.Equal(expected, actual.Item1);
    }

    [Fact]
    public async Task WhenVersionGeometryIsIntegerReturnsNoError()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""((30 10, 40 40, 20 40, 10 20, 30 10))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);
        Assert.Empty(actual.Item2);
    }

    [Fact]
    public async Task WhenVersionGeometryIsNotIntegerReturnsError()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": ""1"", ""Polygon"": {
            ""polygon"": ""((30 10, 40 40, 20 40, 10 20, 30 10))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");


        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);
        Assert.NotEmpty(actual.Item2);
    }

    [Fact]
    public async Task DoNotAllowsCoordinatesWithNotAcceptedGeometry()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Square"": {
            ""Circle"": ""((30 10, 40 40, 20 40, 10 20, 30 10))""}, ""externalReference"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object, _boundingBoxService);

        Tuple<BoundingBox, List<SemanticValidationError>>? actual = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(actual.Item2);
    }

    [Fact]
    public async Task TypoInExternalReferenceReturnsError()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""version"": 1, ""Polygon"": {
            ""polygon"": ""((30 10, 40 40, 20 40, 10 20, 30 10))""}, ""externalR"": [{ ""lastUpdateDate"": ""1987-09-26 00:00:00""}]}}");

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
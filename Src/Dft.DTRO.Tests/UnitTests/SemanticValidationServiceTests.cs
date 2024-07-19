using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class SemanticValidationServiceTests
{
    private readonly Mock<ISystemClock> _mockClock;
    private readonly Mock<IConditionValidationService> _mockConditionValidationService;
    private readonly Mock<IDtroDal> _mockDtroDal;

    public SemanticValidationServiceTests()
    {
        _mockClock = new Mock<ISystemClock>();
        _mockDtroDal = new Mock<IDtroDal>();
        _mockConditionValidationService = new Mock<IConditionValidationService>();

        _mockClock.Setup(it => it.UtcNow).Returns(new DateTime(2023, 5, 1));
    }

    [Fact]
    public async Task AllowsLastUpdateDateInThePast()
    {
        DtroSubmit dtro = PrepareDtro(@"{""lastUpdateDate"": ""2012-04-23T18:25:43.511Z""}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsLastUpdateDateInTheFuture()
    {
        DtroSubmit dtro = PrepareDtro(@"{""lastUpdateDate"": ""2027-04-23T18:25:43.511Z""}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsCoordinatesWithinBoundingBoxOsgb()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""crs"": ""osgb36Epsg27700"", ""coordinates"": {
            ""type"": ""Polygon"", ""coordinates"": [[[-10000, -10000],[0,0]]]}}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsCoordinatesWithinBoundingBoxWgs()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""crs"": ""wgs84Epsg4326"", ""coordinates"": {
            ""type"": ""Polygon"", ""coordinates"": [[[1, 55],[-3,60.3]]]}}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsCoordinatesOutsideOfBoundingBoxOsgb()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""crs"": ""wgs84Epsg4326"", ""coordinates"": {
            ""type"": ""Polygon"", ""coordinates"": [[[-103940, 55],[-3,2000000.44]]]}}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task DisallowsCoordinatesOutsideOfBoundingBoxWgs()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""crs"": ""wgs84Epsg4326"", ""coordinates"": {
            ""type"": ""Polygon"", ""coordinates"": [[[-8, 48],[3,60.9]]]}}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsDtroWithEmptyReferenceToAnotherDtro()
    {
        DtroSubmit dtro = PrepareDtro(@"{""crossRefTro"": []}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsDtroWithoutReferenceToAnotherDtro()
    {
        DtroSubmit dtro = PrepareDtro("{}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task DoesNotApplyValidationForDtroFromExcludedSchema()
    {
        _mockDtroDal.Setup(it => it.DtroExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        Guid id = new("5ca30f2d-1270-4b37-99ca-21c5afd79ccb");
        DtroSubmit dtro = PrepareDtro(
            $@"{{""source"": {{ ""crossRefTro"": [""{id.ToString()}""] }} }}",
            "3.1.1"
        );

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        _mockDtroDal.VerifyNoOtherCalls();
        Assert.Empty(result);
    }

    [Fact]
    public async Task AppliesValidationForDtroWithMatchingSchema()
    {
        _mockDtroDal.Setup(it => it.DtroExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        Guid id = new("5ca30f2d-1270-4b37-99ca-21c5afd79ccb");
        DtroSubmit dtro = PrepareDtro(
            $@"{{""source"": {{ ""crossRefTro"": [""{id.ToString()}""] }} }}",
            "3.1.2"
        );

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        _mockDtroDal.Verify(it => it.DtroExistsAsync(id));
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task AllowsReferencingExistingDtro()
    {
        _mockDtroDal.Setup(it => it.DtroExistsAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        Guid id = new("5ca30f2d-1270-4b37-99ca-21c5afd79ccb");
        DtroSubmit dtro = PrepareDtro($@"{{""source"": {{ ""crossRefTro"": [""{id.ToString()}""] }} }}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        _mockDtroDal.Verify(it => it.DtroExistsAsync(id));
        Assert.Empty(result);
    }

    [Fact]
    public async Task DisallowsReferencingNonExistingDtro()
    {
        _mockDtroDal.Setup(it => it.DtroExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        Guid id = new("5ca30f2d-1270-4b37-99ca-21c5afd79ccb");
        DtroSubmit dtro = PrepareDtro($@"{{""source"": {{ ""crossRefTro"": [""{id.ToString()}""] }} }}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.NotEmpty(result);
        _mockDtroDal.Verify(it => it.DtroExistsAsync(id));
    }

    [Fact]
    public async Task AllowsPoints()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""crs"": ""osgb36Epsg27700"", ""coordinates"": {
                  ""type"": ""Point"", ""coordinates"": [0,0]}}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    [Fact]
    public async Task AllowsLineStrings()
    {
        DtroSubmit dtro = PrepareDtro(@"{""geometry"": { ""crs"": ""osgb36Epsg27700"", ""coordinates"": {
                  ""type"": ""LineString"", ""coordinates"": [[0,0],[0,0],[0,0]]}}}");

        SemanticValidationService sut = new(_mockClock.Object, _mockDtroDal.Object,
            _mockConditionValidationService.Object);

        List<SemanticValidationError>? result = await sut.ValidateCreationRequest(dtro);

        Assert.Empty(result);
    }

    private static DtroSubmit PrepareDtro(string jsonData, string schemaVersion = "10.0.0")
    {
        return new DtroSubmit
        {
            Data = JsonConvert.DeserializeObject<ExpandoObject>(jsonData, new ExpandoObjectConverter()),
            SchemaVersion = schemaVersion
        };
    }
}
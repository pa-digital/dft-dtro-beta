namespace Dft.DTRO.Tests.UnitTests;

[ExcludeFromCodeCoverage]
public class MappingServiceTests
{
    private static readonly string[] ValidDtroHistoriesFor323 =
    {
        "../../../TestFiles/D-TROs/3.2.3/temporary TRO - new-Polygon.json",
        "../../../TestFiles/D-TROs/3.2.3/valid-noChange.json",
        "../../../TestFiles/D-TROs/3.2.3/temporary TRO - fullRevoke.json"
    };

    private readonly IDtroMappingService _sut;

    public MappingServiceTests()
    {
        Dictionary<string, string> dictionary = new() { { "Key1", "Value1" } };
        IConfigurationRoot? mockConfiguration = new ConfigurationBuilder().AddInMemoryCollection(dictionary).Build();
        Mock<IBoundingBoxService> mockBoundingBoxService = new();
        Mock<LoggingExtension> mockLoggingExtension = new();

        _sut = new DtroMappingService(
            mockConfiguration,
            mockBoundingBoxService.Object,
            mockLoggingExtension.Object);
    }

    [Theory]
    [InlineData("../../../TestFiles/D-TROs/3.3.0/JSON-3.3.0-example-Derbyshire 2024 DJ388 partial (somerset test).json", "3.3.0")]
    [InlineData("../../../TestFiles/D-TROs/3.3.0/JSON-3.3.0-example-TTRO-HeightRestrictionWithConditions.json", "3.3.0")]
    [InlineData("../../../TestFiles/D-TROs/3.3.0/JSON-3.3.0-example-TTRO-multiple-nested-condition-sets.json", "3.3.0")]
    [InlineData("../../../TestFiles/D-TROs/3.3.0/JSON-3.3.0-example-TTRO-RoadClosureWithDiversionRoute.json", "3.3.0")]
    [InlineData("../../../TestFiles/D-TROs/3.3.0/JSON-3.3.0-example-TTRO-SuspensionOneWay.json", "3.3.0")]
    [InlineData("../../../TestFiles/D-TROs/3.3.0/JSON-3.3.0-example-TTRO-TempOneWayWithConditions.json", "3.3.0")]
    [InlineData("../../../TestFiles/D-TROs/3.3.0/JSON-3.3.0-example-TTRO-WeightRestriction.json", "3.3.0")]
    public async Task InferIndexFieldsReturnsSampleFiles(string path, string schemaVersion)
    {
        var dtro = await Utils.CreateDtroObject(path, schemaVersion);
        _sut.InferIndexFields(ref dtro);

        Assert.NotNull(dtro.TrafficAuthorityCreatorId);
        Assert.NotNull(dtro.TrafficAuthorityOwnerId);
        Assert.NotNull(dtro.TroName);
    }

    [Fact]
    public void GetSource_Returns_DtroHistoryResponse()
    {
        List<DTROHistory> histories = Utils.CreateRequestDtroHistoryObject(ValidDtroHistoriesFor323);
        List<DtroHistorySourceResponse> actual = histories
            .Select(_sut.GetSource)
            .Where(response => response != null)
            .ToList();

        Assert.True(actual.Any());

        Assert.Equal(2, actual.Count);

        Assert.Equal(actual[0].Created, actual[1].Created);

        Assert.True(actual[1].LastUpdated > actual[0].LastUpdated);
    }

    [Fact]
    public void GetProvision_Returns_DtroHistoryProvision()
    {
        List<DTROHistory> histories = Utils.CreateRequestDtroHistoryObject(ValidDtroHistoriesFor323);
        List<DtroHistoryProvisionResponse> actual = histories
            .SelectMany(_sut.GetProvision)
            .Where(response => response != null)
            .ToList();

        Assert.True(actual.Any());

        Assert.Equal(2, actual.Count);

        Assert.Equal(actual[0].Created, actual[1].Created);

        Assert.True(actual[1].LastUpdated > actual[0].LastUpdated);
    }
}
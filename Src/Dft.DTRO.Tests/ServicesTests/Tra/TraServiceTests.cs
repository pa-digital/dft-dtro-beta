namespace Dft.DTRO.Tests.ServicesTests.Tra;

[ExcludeFromCodeCoverage]
public class TraServiceTests
{
    private readonly Mock<ITraDal> _mockTraDal = new();
    private readonly Mock<IDtroMappingService> _mockDtroMappingService = new();
    
    private readonly ITraService _sut;

    public TraServiceTests()
    {
        _sut = new TraService(_mockTraDal.Object, _mockDtroMappingService.Object);
    }

    [Theory]
    [InlineData("tra1", 1)]
    [InlineData("Tra2", 0)]
    [InlineData(null, 0)]
    public async Task GetTrasAsyncReturnsRecords(string? traName, int records)
    {
        var queryParameters = new GetAllTrasQueryParameters()
        {
            TraName = traName,
        };

        _mockTraDal
            .Setup(it => it.GetTrasAsync(queryParameters))
            .ReturnsAsync(() => MockTestObjects.GetTras(queryParameters));

        _mockDtroMappingService
            .Setup(it => it.MapToTraFindAllResponse(It.IsAny<TrafficRegulationAuthority>()))
            .Returns(MockTestObjects.TraFindAllResponse.First);

        var actual = await _sut.GetTrasAsync(queryParameters);

        Assert.Equal(records, actual.Count());
    }

    [Fact]
    public async Task GetDtrosAsyncReturnsNoRecords()
    {
        _mockTraDal
            .Setup(it => it.GetTrasAsync(new GetAllTrasQueryParameters()))
            .ReturnsAsync(() => new List<TrafficRegulationAuthority>());

        _mockDtroMappingService
            .Setup(it => it.MapToTraFindAllResponse(It.IsAny<TrafficRegulationAuthority>()))
            .Returns(MockTestObjects.TraFindAllResponse.First);

        await Assert.ThrowsAnyAsync<NotFoundException>(async () => await _sut.GetTrasAsync(new GetAllTrasQueryParameters()));

    }
}
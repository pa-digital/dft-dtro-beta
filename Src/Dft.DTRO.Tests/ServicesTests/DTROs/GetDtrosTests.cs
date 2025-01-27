namespace Dft.DTRO.Tests.ServicesTests.DTROs;

[ExcludeFromCodeCoverage]
public class GetDtrosTests
{
    private readonly Mock<IDtroUserDal> _mockDtroUserDal = new();
    private readonly Mock<IDtroDal> _mockDtroDal = new();
    private readonly Mock<IDtroHistoryDal> _mockDtroHistoryDal = new();
    private readonly Mock<ISchemaTemplateDal> _mockSchemaTemplateDal = new();
    private readonly Mock<IDtroMappingService> _mockDtroMappingService = new();
    private readonly Mock<IDtroGroupValidatorService> _mockDtroGroupValidatorService = new();


    private readonly IDtroService _sut;

    public GetDtrosTests()
    {
        _sut = new DtroService(
                _mockDtroUserDal.Object,
                _mockDtroDal.Object,
                _mockDtroHistoryDal.Object,
                _mockSchemaTemplateDal.Object,
                _mockDtroMappingService.Object,
                _mockDtroGroupValidatorService.Object
            );
    }

    [Theory]
    [InlineData(1000, "2025-01-21 16:21:26", "2025-01-23 11:02:41", 3)]
    [InlineData(3300, "2025-01-21 16:21:26", "2025-01-23 11:02:41", 2)]
    [InlineData(3300, "2025-01-21 16:21:26", "2025-01-22 16:21:26", 2)]
    [InlineData(5000, "2025-01-21 16:21:26", "2025-01-24 11:02:41", 4)]
    [InlineData(0, null, null, 12)]
    public async Task GetDtrosAsyncReturnsRecords(int? traCode, string? startDate, string? endDate, int records)
    {
        var queryParameters = new GetAllQueryParameters
        {
            TraCode = traCode,
            StartDate = startDate != null ? DateTime.Parse(startDate) : null,
            EndDate = endDate != null ? DateTime.Parse(endDate) : null
        };

        _mockDtroDal
            .Setup(it => it.GetDtrosAsync(queryParameters))
            .ReturnsAsync(() => MockTestObjects.GetDtros(queryParameters));

        _mockDtroMappingService
            .Setup(it => it.MapToDtroResponse(It.IsAny<DfT.DTRO.Models.DataBase.DTRO>()))
            .Returns(MockTestObjects.DtroResponses.First);

        var actual = await _sut.GetDtrosAsync(queryParameters);

        Assert.Equal(records, actual.Count());
    }

    [Fact]
    public async Task GetDtrosAsyncReturnsNoRecords()
    {
        _mockDtroDal
            .Setup(it => it.GetDtrosAsync(new GetAllQueryParameters()))
            .ReturnsAsync(() => new List<DfT.DTRO.Models.DataBase.DTRO>());

        _mockDtroMappingService
            .Setup(it => it.MapToDtroResponse(It.IsAny<DfT.DTRO.Models.DataBase.DTRO>()))
            .Returns(MockTestObjects.DtroResponses.First);

        await Assert.ThrowsAnyAsync<NotFoundException>(async () => await _sut.GetDtrosAsync(new GetAllQueryParameters()));

    }
}
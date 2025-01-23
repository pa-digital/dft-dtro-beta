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

    [Fact]
    public async Task GetDtrosAsyncReturnsRecords()
    {
        var queryParameters = new QueryParameters
        {
            TraCode = 1000,
            StartDate = new DateTime(),
            EndDate = new DateTime()
        };

        _mockDtroDal
            .Setup(it => it.GetDtrosAsync(queryParameters))
            .ReturnsAsync(() => MockTestObjects.Dtros);

        _mockDtroMappingService
            .Setup(it => it.MapToDtroResponse(It.IsAny<DfT.DTRO.Models.DataBase.DTRO>()))
            .Returns(MockTestObjects.DtroResponses.First);

        var actual = await _sut.GetDtrosAsync(queryParameters);

        Assert.NotEmpty(actual);
    }

    [Fact]
    public async Task GetDtrosAsyncReturnsNoRecords()
    {
        _mockDtroDal
            .Setup(it => it.GetDtrosAsync(new QueryParameters()))
            .ReturnsAsync(() => new List<DfT.DTRO.Models.DataBase.DTRO>());

        _mockDtroMappingService
            .Setup(it => it.MapToDtroResponse(It.IsAny<DfT.DTRO.Models.DataBase.DTRO>()))
            .Returns(MockTestObjects.DtroResponses.First);

        await Assert.ThrowsAnyAsync<NotFoundException>(async () => await _sut.GetDtrosAsync(new QueryParameters()));

    }
}
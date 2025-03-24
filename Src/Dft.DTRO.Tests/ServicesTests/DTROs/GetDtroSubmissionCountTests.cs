using System.Threading.Tasks;
using Moq;
using Xunit;

public class GetDtroSubmissionCountTests
{
    private readonly Mock<IDtroUserDal> _mockDtroUserDal = new();
    private readonly Mock<IDtroDal> _mockDtroDal = new();
    private readonly Mock<IDtroHistoryDal> _mockDtroHistoryDal = new();
    private readonly Mock<ISchemaTemplateDal> _mockSchemaTemplateDal = new();
    private readonly Mock<IDtroMappingService> _mockDtroMappingService = new();
    private readonly Mock<IDtroGroupValidatorService> _mockDtroGroupValidatorService = new();
    private readonly DtroService _dtroService;

    public GetDtroSubmissionCountTests()
    {
        _dtroService = new DtroService(
                _mockDtroUserDal.Object,
                _mockDtroDal.Object,
                _mockDtroHistoryDal.Object,
                _mockSchemaTemplateDal.Object,
                _mockDtroMappingService.Object,
                _mockDtroGroupValidatorService.Object
            );
    }

    [Fact]
    public async Task GetDtroSubmissionCountReturnsCorrectCount()
    {
        int expectedCount = 10;
        _mockDtroDal.Setup(d => d.GetDtroSubmissionCount()).ReturnsAsync(expectedCount);

        int result = await _dtroService.GetDtroSubmissionCount();
        Assert.Equal(expectedCount, result);
        _mockDtroDal.Verify(d => d.GetDtroSubmissionCount(), Times.Once);
    }
}

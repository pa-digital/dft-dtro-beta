namespace Dft.DTRO.Tests.ControllerTests;

public class TraControllerTests
{
    private readonly Mock<ITraService> _mockTraService = new();

    private readonly TraController _sut;

    public TraControllerTests()
    {
        ILogger<TraController> mockLogger = MockLogger.Setup<TraController>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _sut = new TraController(_mockTraService.Object, mockLogger, mockLoggingExtension.Object);

        Guid appId = Guid.NewGuid();
        Mock<HttpContext> mockContext = MockHttpContext.Setup();
    }

    [Fact]
    public async Task FindAllReturnsRecords()
    {
        var queryParameters = new GetAllTrasQueryParameters()
        {
            TraName = "name",
        };
        _mockTraService
            .Setup(it => it.GetTrasAsync(queryParameters))
            .ReturnsAsync(() => MockTestObjects.TraFindAllResponse);

        IActionResult? actual = await _sut.FindAll(queryParameters);

        Assert.NotNull(actual);

        Assert.Equal(200, ((ObjectResult)actual).StatusCode);
    }
}
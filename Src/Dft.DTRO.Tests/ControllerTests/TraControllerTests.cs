namespace Dft.DTRO.Tests.ControllerTests;

[ExcludeFromCodeCoverage]
public class TraControllerTests
{
    private readonly Mock<ITraService> _mockTraService = new();
    private readonly Mock<IRequestCorrelationProvider> _mockRequestCorrelationProvider = new();
    private readonly Mock<IAppIdMapperService> _mockXAppIdMapperService = new();

    private readonly TraController _sut;

    public TraControllerTests()
    {
        ILogger<TraController> mockLogger = MockLogger.Setup<TraController>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _sut = new TraController(_mockTraService.Object, mockLogger, mockLoggingExtension.Object);

        Guid xAppId = Guid.NewGuid();
        Mock<HttpContext> mockContext = MockHttpContext.Setup();

        _mockXAppIdMapperService
            .Setup(it => it.GetAppId(mockContext.Object))
            .ReturnsAsync(() => xAppId);

        _mockRequestCorrelationProvider
            .SetupGet(provider => provider.CorrelationId)
            .Returns(() => xAppId.ToString());
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
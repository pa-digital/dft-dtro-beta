using DfT.DTRO.Models.App;

namespace Dft.DTRO.Tests.ControllerTests;

[ExcludeFromCodeCoverage]
public class AppControllerTests
{
    private readonly Mock<IAppService> _mockAppService = new();
    private readonly Mock<IRequestCorrelationProvider> _mockRequestCorrelationProvider = new();
    private readonly Mock<IAppIdMapperService> _mockXAppIdMapperService = new();

    private readonly AppController _sut;

    public AppControllerTests()
    {
        ILogger<AppController> mockLogger = MockLogger.Setup<AppController>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _sut = new AppController(_mockAppService.Object, mockLogger, mockLoggingExtension.Object);

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
    public async Task CreateAppReturnsApp()
    {
        var appInput = new AppInput()
        {
            Name = "name",
            Username = "username"
        };
        _mockAppService
            .Setup(it => it.CreateApp(appInput))
            .ReturnsAsync(() => MockTestObjects.App);

        IActionResult? actual = await _sut.CreateApp(appInput);

        Assert.NotNull(actual);

        Assert.Equal(200, ((ObjectResult)actual).StatusCode);
    }
}
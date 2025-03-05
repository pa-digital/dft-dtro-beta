namespace Dft.DTRO.Tests.ControllerTests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService = new();
    private readonly Mock<IRequestCorrelationProvider> _mockRequestCorrelationProvider = new();
    private readonly Mock<IAppIdMapperService> _mockXAppIdMapperService = new();

    private readonly AuthController _sut;

    public AuthControllerTests()
    {
        ILogger<AuthController> mockLogger = MockLogger.Setup<AuthController>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _sut = new AuthController(_mockAuthService.Object, mockLogger, mockLoggingExtension.Object);

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
    public async Task GetTokenReturnsAuthToken()
    {
        var authTokenInput = new AuthTokenInput()
        {
            Username = "username",
            Password = "password"
        };
        _mockAuthService
            .Setup(it => it.GetToken(authTokenInput))
            .ReturnsAsync(() => MockTestObjects.AuthToken);

        IActionResult? actual = await _sut.GetToken(authTokenInput);

        Assert.NotNull(actual);

        Assert.Equal(200, ((ObjectResult)actual).StatusCode);
    }
}
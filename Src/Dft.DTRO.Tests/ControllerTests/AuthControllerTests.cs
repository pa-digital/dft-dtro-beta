namespace Dft.DTRO.Tests.ControllerTests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService = new();

    private readonly AuthController _sut;

    public AuthControllerTests()
    {
        ILogger<AuthController> mockLogger = MockLogger.Setup<AuthController>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _sut = new AuthController(_mockAuthService.Object, mockLogger, mockLoggingExtension.Object);

        Guid appId = Guid.NewGuid();
        Mock<HttpContext> mockContext = MockHttpContext.Setup();
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
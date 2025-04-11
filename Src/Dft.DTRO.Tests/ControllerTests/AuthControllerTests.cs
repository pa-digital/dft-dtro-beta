using DfT.DTRO.Models.Auth;

namespace Dft.DTRO.Tests.ControllerTests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService = new();
    private readonly Mock<IEmailService> _mockEmailService = new();
    private readonly AuthController _sut;

    public AuthControllerTests()
    {
        ILogger<AuthController> mockLogger = MockLogger.Setup<AuthController>();
        _mockEmailService = new Mock<IEmailService>();

        _sut = new AuthController(_mockAuthService.Object, _mockEmailService.Object,mockLogger);

        Guid appId = Guid.NewGuid();
        Mock<HttpContext> mockContext = MockHttpContext.Setup();
    }

    [Fact]
    public async Task GetTokenReturnsOkStatusCode()
    {
        var authTokenInput = new AuthTokenInput { Username = "username", Password = "password" };
        _mockAuthService
            .Setup(it => it.GetToken(authTokenInput))
            .ReturnsAsync(() => MockTestObjects.AuthToken);

        IActionResult actual = await _sut.GetToken(authTokenInput);

        Assert.NotNull(actual);
        Assert.Equal(200, ((ObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task GetTokenThrowsException()
    {
        var authTokenInput = new AuthTokenInput { Username = "username", Password = "password" };
        _mockAuthService
            .Setup(authService => authService.GetToken(authTokenInput))
            .Throws<Exception>();

        IActionResult actual = await _sut.GetToken(authTokenInput);
        Assert.NotNull(actual);
        Assert.Equal(500, ((ObjectResult)actual).StatusCode);
    }
}
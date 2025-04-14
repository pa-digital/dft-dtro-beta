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

    [Theory]
    [InlineData("jon.doe@email.com","Test App")]
    public void RefreshSecretsReturnsOk(string email,string appName)
    {
        var emailNotificationResponse = new EmailNotificationResponse() { id = Guid.NewGuid().ToString() };
        var apigeeApplication = new ApigeeDeveloperApp() { Name = appName };
        _mockEmailService
            .Setup(it => it.SendEmail(email, apigeeApplication))
            .Returns(() => emailNotificationResponse);

        var actual = _sut.RefreshSecrets(email, apigeeApplication);
        Assert.NotNull(actual);
        Assert.Equal(200, ((ObjectResult)actual).StatusCode);
    }

    [Theory]
    [InlineData("Test App","jon.doe@email.com")]
    public void RefreshSecretsThrowsEmailException(string email, string appName)
    {
        var emailNotificationResponse = new EmailNotificationResponse();
        var apigeeApplication = new ApigeeDeveloperApp() { Name = appName };
        _mockEmailService
            .Setup(it => it.SendEmail(email, apigeeApplication))
            .Throws<EmailSendException>();

        var actual = _sut.RefreshSecrets(email, apigeeApplication);
        Assert.Equal(500, ((ObjectResult)actual).StatusCode);
    }

    [Theory]
    [InlineData("Test App","jon.doe@email.com")]
    public void RefreshSecretsThrowsException(string email, string appName)
    {
        var apigeeApplication = new ApigeeDeveloperApp() { Name = appName };
        _mockEmailService
            .Setup(it => it.SendEmail(email, apigeeApplication))
            .Throws<Exception>();

        var actual = _sut.RefreshSecrets(email, apigeeApplication);
        Assert.Equal(500, ((ObjectResult)actual).StatusCode);
    }
}
using DfT.DTRO.Consts;
using DfT.DTRO.Utilities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Dft.DTRO.Tests.ControllerTests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService = new();
    private readonly Mock<IUserDal> _mockUserDal = new();
    private readonly Mock<ITwoFactorAuthService> _mockTwoFactorAuthService = new();
    private readonly Mock<AuthHelper> _mockAuthHelper = new();

    private readonly AuthController _sut;

    public AuthControllerTests()
    {
        ILogger<AuthController> mockLogger = MockLogger.Setup<AuthController>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _sut = new AuthController(_mockAuthService.Object, _mockUserDal.Object, _mockTwoFactorAuthService.Object, _mockAuthHelper.Object, mockLogger, mockLoggingExtension.Object);

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

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        IActionResult? actual = await _sut.GetToken(authTokenInput);

        Assert.NotNull(actual);
        var okResult = Assert.IsType<OkResult>(actual);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task VerifyTokenValidTokenReturnsOkWithIsAdmin()
    {
        string email = "user@test.com";

        var mockCookies = new Mock<IRequestCookieCollection>();
        long futureTime = DateTimeOffset.Now.AddMinutes(10).ToUnixTimeSeconds();
        mockCookies.Setup(c => c["access_token"]).Returns($"validtoken:{futureTime}");

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers[RequestHeaderNames.Email] = email;
        httpContext.Request.Cookies = mockCookies.Object;

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _mockUserDal.Setup(x => x.GetUserFromEmail(email))
            .ReturnsAsync(new User { IsCentralServiceOperator = true });

        var result = await _sut.VerifyToken();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonConvert.SerializeObject(okResult.Value);
        var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        Assert.True(bool.Parse(response["isAdmin"].ToString()));
    }

     [Fact]
    public async Task VerifyTokenExpiredTokenReturnsUnauthorized()
    {
        string email = "user@test.com";

        long pastTime = DateTimeOffset.Now.AddMinutes(-10).ToUnixTimeSeconds();
        var mockCookies = new Mock<IRequestCookieCollection>();
        mockCookies.Setup(c => c["access_token"]).Returns($"validtoken:{pastTime}");
        
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers[RequestHeaderNames.Email] = email;
        httpContext.Request.Cookies = mockCookies.Object;

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _mockUserDal.Setup(x => x.GetUserFromEmail(email))
            .ReturnsAsync(new User { IsCentralServiceOperator = false });

        var result = await _sut.VerifyToken();
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task VerifyTokenMissingAccessTokenReturnsUnauthorized()
    {
        string email = "user@test.com";
        var mockCookies = new Mock<IRequestCookieCollection>();
        mockCookies.Setup(c => c["access_token"]).Returns("");
        
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers[RequestHeaderNames.Email] = email;
        httpContext.Request.Cookies = mockCookies.Object;

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _mockUserDal.Setup(x => x.GetUserFromEmail(email))
            .ReturnsAsync(new User { IsCentralServiceOperator = false });

        var result = await _sut.VerifyToken();
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task VerifyTokenMalformedAccessTokenThrowsFormatException()
    {
        string email = "user@test.com";
        var mockCookies = new Mock<IRequestCookieCollection>();
        mockCookies.Setup(c => c["access_token"]).Returns("invalidformat");
        
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers[RequestHeaderNames.Email] = email;
        httpContext.Request.Cookies = mockCookies.Object;

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _mockUserDal.Setup(x => x.GetUserFromEmail(email))
            .ReturnsAsync(new User { IsCentralServiceOperator = true });

        await Assert.ThrowsAsync<IndexOutOfRangeException>(() => _sut.VerifyToken());
    }

    [Fact]
    public async Task VerifyTokenUserFetchThrowsThrowsException()
    {
        string email = "user@test.com";
        var mockCookies = new Mock<IRequestCookieCollection>();
        mockCookies.Setup(c => c["access_token"]).Returns("invalidformat");
        
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers[RequestHeaderNames.Email] = email;
        httpContext.Request.Cookies = mockCookies.Object;

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _mockUserDal.Setup(x => x.GetUserFromEmail(email))
            .ThrowsAsync(new Exception("DB error"));

        var ex = await Assert.ThrowsAsync<Exception>(() => _sut.VerifyToken());
        Assert.Equal("DB error", ex.Message);
    }
}
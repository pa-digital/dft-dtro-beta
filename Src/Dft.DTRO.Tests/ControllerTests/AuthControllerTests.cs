using System.Security.Claims;
using DfT.DTRO.Consts;
using DfT.DTRO.Models.TwoFactorAuth;
using DfT.DTRO.Utilities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Dft.DTRO.Tests.ControllerTests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService = new();
    private readonly Mock<IUserDal> _mockUserDal = new();
    private readonly Mock<ITwoFactorAuthService> _mockTwoFactorAuthService = new();
    private readonly Mock<IAuthHelper> _mockAuthHelper = new();

    private readonly AuthController _sut;

    public AuthControllerTests()
    {
        ILogger<AuthController> mockLogger = MockLogger.Setup<AuthController>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _sut = new AuthController(_mockAuthService.Object, _mockUserDal.Object, _mockTwoFactorAuthService.Object, _mockAuthHelper.Object, mockLogger, mockLoggingExtension.Object);

        Guid appId = Guid.NewGuid();
        Mock<HttpContext> mockContext = MockHttpContext.Setup();
    }

    private AuthController SetupControllerWithUser(string email)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, email)
        }, "mock"));

        var context = MockHttpContext.Setup();
        context.SetupGet(x => x.User).Returns(user);

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = context.Object
        };

        return _sut;
    }

    [Fact]
    public async Task VerifyTokenReturnsIsAdminTrue()
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, "user@test.com") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        _mockUserDal.Setup(x => x.GetUserFromEmail("user@test.com")).ReturnsAsync(new User { IsCentralServiceOperator = true });

        var result = await _sut.VerifyToken();
        var okResult = Assert.IsType<OkObjectResult>(result);

        var json = JsonConvert.SerializeObject(okResult.Value);
        var value = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);

        Assert.True(value["isAdmin"]);
    }

    [Fact]
    public async Task AuthenticateUserReturnsUnauthorizedIfInvalid()
    {
        var input = new AuthTokenInput { Username = "user", Password = "wrong" };

        _mockAuthService.Setup(x => x.AuthenticateUser("user", "wrong")).ReturnsAsync(false);

        var result = await _sut.AuthenticateUser(input);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task AuthenticateUserReturnsTokenIfValid()
    {
        var input = new AuthTokenInput { Username = "user", Password = "pass" };

        _mockAuthService.Setup(x => x.AuthenticateUser("user", "pass")).ReturnsAsync(true);
        _mockTwoFactorAuthService.Setup(x => x.GenerateTwoFactorAuthCode("user")).ReturnsAsync(new TwoFactorAuthentication { Token = Guid.NewGuid() });

        var result = await _sut.AuthenticateUser(input);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var json = JsonConvert.SerializeObject(okResult.Value);
        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        Assert.True(dict.ContainsKey("token"));
        Assert.False(string.IsNullOrWhiteSpace(dict["token"]));
    }

    [Fact]
    public async Task ValidateTwoFactorAuthCodeReturnsBadRequestIfInvalid()
    {
        _mockTwoFactorAuthService.Setup(x => x.VerifyTwoFactorAuthCode(It.IsAny<Guid>().ToString(), It.IsAny<string>())).ReturnsAsync((TwoFactorAuthentication)null);

        var result = await _sut.ValidateTwoFactorAuthCode(new TwoFactorAuthInput { Code = "123", Token = Guid.NewGuid().ToString() });

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Invalid code", badRequest.Value.ToString());
    }

    [Fact]
    public async Task ValidateTwoFactorAuthCodeReturnsOkIfValid()
    {
        var user = new User { Email = "user@test.com" };
        var tfa = new TwoFactorAuthentication { Id = Guid.NewGuid(), Token = Guid.NewGuid(), User = user };

        _mockTwoFactorAuthService
            .Setup(x => x.VerifyTwoFactorAuthCode(tfa.Token.ToString(), "123"))
            .ReturnsAsync(tfa);

        _mockTwoFactorAuthService
            .Setup(x => x.DeleteTwoFactorAuthCodeById(tfa.Id))
            .Returns(Task.CompletedTask);

        var token = "mocked.jwt.token";
        var expiry = DateTime.UtcNow.AddMinutes(30);

        _mockAuthHelper
            .Setup(x => x.GenerateJwtToken(user.Email))
            .Returns(token);

        _mockAuthHelper
            .Setup(x => x.GetJwtExpiration(token))
            .Returns(expiry);

        var responseCookies = new Mock<IResponseCookies>();
        var response = new Mock<HttpResponse>();
        response.SetupGet(r => r.Cookies).Returns(responseCookies.Object);

        var httpContext = new Mock<HttpContext>();
        httpContext.SetupGet(x => x.Response).Returns(response.Object);

        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext.Object
        };

        var input = new TwoFactorAuthInput
        {
            Token = tfa.Token.ToString(),
            Code = "123"
        };

        var result = await _sut.ValidateTwoFactorAuthCode(input);

        Assert.IsType<OkResult>(result);
        responseCookies.Verify(c => c.Append(
            "jwtToken",
            token,
            It.Is<CookieOptions>(opt =>
                opt.Expires == expiry &&
                opt.HttpOnly &&
                opt.Secure &&
                opt.SameSite == SameSiteMode.None
            )
        ), Times.Once);
    }
}
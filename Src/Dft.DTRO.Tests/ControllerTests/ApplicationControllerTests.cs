using Dft.DTRO.Tests.Fixtures;

public class ApplicationControllerTests : IClassFixture<ApplicationControllerTestFixture>
{
    private readonly Mock<IApplicationService> _mockApplicationService;
    private readonly ApplicationController _controller;
    private readonly ApplicationControllerTestFixture _fixture;

    public ApplicationControllerTests(ApplicationControllerTestFixture fixture)
    {
        ILogger<ApplicationController> mockLogger = MockLogger.Setup<ApplicationController>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _mockApplicationService = new Mock<IApplicationService>();
        _controller = new ApplicationController(_mockApplicationService.Object, mockLogger, mockLoggingExtension.Object);
        _fixture = fixture;
        _controller.ControllerContext = _fixture.ControllerContext;
    }

    [Fact]
    public async Task ValidateApplicationNameValidNameReturnsOk()
    {
        var parameters = new ApplicationNameQueryParameters { Name = "ValidAppName" };
        _mockApplicationService.Setup(service => service.ValidateApplicationName("ValidAppName")).ReturnsAsync(true);
        var result = await _controller.ValidateApplicationName(parameters);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task ValidateApplicationNameInvalidNameReturnsBadRequest()
    {
        var parameters = new ApplicationNameQueryParameters { Name = "" };
        var result = await _controller.ValidateApplicationName(parameters);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task ValidateApplicationNameNoQueryParametersReturnsBadRequest()
    {
        var result = await _controller.ValidateApplicationName(null);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task GetApplicationDetailsValidRequestReturnsOk()
    {
        var userId = _controller.ControllerContext.HttpContext.Items["UserId"] as string;
        Guid appGuid = Guid.NewGuid();
        string appId = appGuid.ToString();

        var request = new ApplicationDetailsRequest { appId = appId };
        ApplicationDetailsDto mockAppDetails = new ApplicationDetailsDto { Name = "App1", AppId = appGuid, Purpose = "Test" };
        _mockApplicationService.Setup(service => service.GetApplicationDetails(appId)).ReturnsAsync(mockAppDetails);
        _mockApplicationService.Setup(service => service.ValidateAppBelongsToUser(userId, appId)).ReturnsAsync(true);

        var result = await _controller.GetApplicationDetails(request);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetApplicationDetailsNotUsersAppReturnsForbidden()
    {
        var userId = _controller.ControllerContext.HttpContext.Items["UserId"] as string;
        Guid appGuid = Guid.NewGuid();
        string appId = appGuid.ToString();

        var request = new ApplicationDetailsRequest { appId = appId };
        ApplicationDetailsDto mockAppDetails = new ApplicationDetailsDto { Name = "App1", AppId = appGuid, Purpose = "Test" };
        _mockApplicationService.Setup(service => service.GetApplicationDetails(appId)).ReturnsAsync(mockAppDetails);
        _mockApplicationService.Setup(service => service.ValidateAppBelongsToUser(userId, appId)).ReturnsAsync(false);

        var result = await _controller.GetApplicationDetails(request);
        var forbiddenRequestResult = Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task GetApplicationDetailsInvalidAppIdReturnsBadRequest()
    {

        var userId = _controller.ControllerContext.HttpContext.Items["UserId"] as string;

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(ctx => ctx.Items["UserId"]).Returns(userId);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };

        Guid appGuid = Guid.NewGuid();
        string appId = appGuid.ToString();

        var request = new ApplicationDetailsRequest { appId = appId };
        ApplicationDetailsDto mockAppDetails = new ApplicationDetailsDto { Name = "App1", AppId = appGuid, Purpose = "Test" };
        _mockApplicationService.Setup(service => service.GetApplicationDetails(appId)).ReturnsAsync((ApplicationDetailsDto)null);
        _mockApplicationService.Setup(service => service.ValidateAppBelongsToUser(userId, appId)).ReturnsAsync(true);

        var result = await _controller.GetApplicationDetails(request);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task GetApplicationsValidUserReturnsOk()
    {
        var applicationListDtos = new List<ApplicationListDto>
        {
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 1", Type = "Test", Tra = "Test" },
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 2", Type = "Test", Tra = "Test" },
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 3", Type = "Test", Tra = "Test" }
        };
        _mockApplicationService.Setup(service => service.GetApplicationList(It.IsAny<string>())).ReturnsAsync(applicationListDtos);
        var result = await _controller.GetApplications();
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task ActivateApplicationNullRequestReturnsBadRequest()
    {
        var result = await _controller.ActivateApplication(null);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task ActivateApplicationEmptyAppIdReturnsBadRequest()
    {
        var request = new ApplicationDetailsRequest { appId = "" };
        var result = await _controller.ActivateApplication(request);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task ActivateApplicationAppDoesNotBelongToUserReturnsForbid()
    {
        var request = new ApplicationDetailsRequest { appId = "valid-app-id" };
        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser("test-user-id", "valid-app-id"))
            .ReturnsAsync(false);

        var result = await _controller.ActivateApplication(request);
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task ActivateApplicationSuccessfulActivationReturnsOk()
    {
        var request = new ApplicationDetailsRequest { appId = "valid-app-id" };
        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser("user@test.com", "valid-app-id"))
            .ReturnsAsync(true);
        _mockApplicationService
            .Setup(s => s.ActivateApplicationById("valid-app-id"))
            .ReturnsAsync(true);

        var result = await _controller.ActivateApplication(request);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task ActivateApplicationInvalidOperationExceptionReturnsInternalServerError()
    {
        var request = new ApplicationDetailsRequest { appId = "valid-app-id" };
        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser("user@test.com", "valid-app-id"))
            .ReturnsAsync(true);
        _mockApplicationService
            .Setup(s => s.ActivateApplicationById("valid-app-id"))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        var result = await _controller.ActivateApplication(request);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task ActivateApplicationUnexpectedExceptionReturnsInternalServerError()
    {
        var userId = _controller.ControllerContext.HttpContext.Items["UserId"] as string;

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(ctx => ctx.Items["UserId"]).Returns(userId);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };

        var request = new ApplicationDetailsRequest { appId = "valid-app-id" };
        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser("user@test.com", "valid-app-id"))
            .ReturnsAsync(true);
        _mockApplicationService
            .Setup(s => s.ActivateApplicationById("valid-app-id"))
            .ThrowsAsync(new Exception("Something went wrong"));

        var result = await _controller.ActivateApplication(request);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
}
using Dft.DTRO.Tests.Fixtures;

public class ApplicationControllerTests : IClassFixture<ApplicationControllerTestFixture>
{
    private readonly Mock<IApplicationService> _mockApplicationService;
    private readonly ApplicationController _controller;
    private readonly ApplicationControllerTestFixture _fixture;
    private readonly string _xEmail;

    public ApplicationControllerTests(ApplicationControllerTestFixture fixture)
    {
        ILogger<ApplicationController> mockLogger = MockLogger.Setup<ApplicationController>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _mockApplicationService = new Mock<IApplicationService>();
        _controller = new ApplicationController(_mockApplicationService.Object, mockLogger, mockLoggingExtension.Object);
        _fixture = fixture;
        _controller.ControllerContext = _fixture.ControllerContext;
        _xEmail = "user@test.com";
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
    public async Task FindApplicationByIdValidRequestReturnsOk()
    {
        Guid appId = Guid.NewGuid();

        ApplicationDetailsDto mockAppDetails = new ApplicationDetailsDto { Name = "App1", AppId = appId, Purpose = "Test" };
        _mockApplicationService.Setup(service => service.GetApplicationDetails(appId)).ReturnsAsync(mockAppDetails);
        _mockApplicationService.Setup(service => service.ValidateAppBelongsToUser(_xEmail, appId)).ReturnsAsync(true);

        var result = await _controller.FindApplicationById(_xEmail, appId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task FindApplicationByIdNotUsersAppReturnsForbidden()
    {
        Guid appId = Guid.NewGuid();
        
        ApplicationDetailsDto mockAppDetails = new ApplicationDetailsDto { Name = "App1", AppId = appId, Purpose = "Test" };
        _mockApplicationService.Setup(service => service.GetApplicationDetails(appId)).ReturnsAsync(mockAppDetails);
        _mockApplicationService.Setup(service => service.ValidateAppBelongsToUser(_xEmail, appId)).ReturnsAsync(false);

        var result = await _controller.FindApplicationById(_xEmail, appId);
        var forbiddenRequestResult = Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task FindApplicationByIdInvalidAppIdReturnsBadRequest()
    {
        Guid appId = Guid.NewGuid();

        ApplicationDetailsDto mockAppDetails = new ApplicationDetailsDto { Name = "App1", AppId = appId, Purpose = "Test" };
        _mockApplicationService.Setup(service => service.GetApplicationDetails(appId)).ReturnsAsync((ApplicationDetailsDto)null);
        _mockApplicationService.Setup(service => service.ValidateAppBelongsToUser(_xEmail, appId)).ReturnsAsync(true);

        var result = await _controller.FindApplicationById(_xEmail, appId);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task FindApplicationsValidUserReturnsOk()
    {
        var applicationListDtos = new List<ApplicationListDto>
        {
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 1", Type = "Test", Tra = "Test" },
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 2", Type = "Test", Tra = "Test" },
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 3", Type = "Test", Tra = "Test" }
        };
        _mockApplicationService.Setup(service => service.GetApplicationList(It.IsAny<string>())).ReturnsAsync(applicationListDtos);
        var result = await _controller.FindApplications(_xEmail);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task ActivateApplicationAppDoesNotBelongToUserReturnsForbid()
    {
        Guid appId = Guid.NewGuid();

        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser(_xEmail, appId))
            .ReturnsAsync(false);

        var result = await _controller.ActivateApplication(_xEmail, appId);
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task ActivateApplicationSuccessfulActivationReturnsOk()
    {
        Guid appId = Guid.NewGuid();

        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser(_xEmail, appId))
            .ReturnsAsync(true);
        _mockApplicationService
            .Setup(s => s.ActivateApplicationById(appId))
            .ReturnsAsync(true);

        var result = await _controller.ActivateApplication(_xEmail, appId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task ActivateApplicationInvalidOperationExceptionReturnsInternalServerError()
    {
        Guid appId = Guid.NewGuid();

        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser(_xEmail, appId))
            .ReturnsAsync(true);
        _mockApplicationService
            .Setup(s => s.ActivateApplicationById(appId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        var result = await _controller.ActivateApplication(_xEmail, appId);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task ActivateApplicationUnexpectedExceptionReturnsInternalServerError()
    {
        Guid appId = Guid.NewGuid();

        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser(_xEmail, appId))
            .ReturnsAsync(true);
        _mockApplicationService
            .Setup(s => s.ActivateApplicationById(appId))
            .ThrowsAsync(new Exception("Something went wrong"));

        var result = await _controller.ActivateApplication(_xEmail, appId);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
}
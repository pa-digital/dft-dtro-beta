namespace Dft.DTRO.Tests.ControllerTests;

public class ApplicationControllerTests
{
    private readonly Mock<IApplicationService> _mockApplicationService;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly ApplicationController _controller;
    private readonly string _email;
    private readonly ApplicationResponse _applicationResponse;
    private readonly EmailNotificationResponse _emailNotificationResponse;

    public ApplicationControllerTests()
    {
        ILogger<ApplicationController> mockLogger = MockLogger.Setup<ApplicationController>();

        _mockApplicationService = new Mock<IApplicationService>();
        _mockEmailService = new Mock<IEmailService>();
        _controller = new ApplicationController(_mockApplicationService.Object, mockLogger, _mockEmailService.Object);
        _email = "user@test.com";
        _applicationResponse = new ApplicationResponse { Name = "Test" };
        _emailNotificationResponse = new EmailNotificationResponse { id = Guid.NewGuid().ToString() };
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

        var expectedResponse = new ApplicationResponse
        {
            AppId = appId,
            Name = "Test",
            Purpose = "Test",
            SwaCode = 123,
            CreatedAt = 0, 
            LastModifiedAt = 0, 
            Status = "approved", 
            DeveloperId = "dev-123",
            Credentials = new List<AppCredential>
            {
                new AppCredential
                {
                    ConsumerKey = "key-123",
                    ConsumerSecret = "secret-456",
                    ExpiresAt = -1,
                    IssuedAt = 0,
                    Status = "approved"
                }
            }
        };
        
        _mockApplicationService.Setup(service => service.GetApplication(_email, appId)).ReturnsAsync(expectedResponse);
        _mockApplicationService.Setup(service => service.ValidateAppBelongsToUser(_email, appId)).ReturnsAsync(true);

        var result = await _controller.FindApplicationById(_email, appId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task FindApplicationByIdNotUsersAppReturnsForbidden()
    {
        Guid appId = Guid.NewGuid();
        
        var expectedResponse = new ApplicationResponse
        {
            AppId = appId,
            Name = "Test",
            Purpose = "Test",
            SwaCode = 123,
            CreatedAt = 0, 
            LastModifiedAt = 0, 
            Status = "approved", 
            DeveloperId = "dev-123",
            Credentials = new List<AppCredential>
            {
                new AppCredential
                {
                    ConsumerKey = "key-123",
                    ConsumerSecret = "secret-456",
                    ExpiresAt = -1,
                    IssuedAt = 0,
                    Status = "approved"
                }
            }
        };
        _mockApplicationService.Setup(service => service.GetApplication(_email, appId)).ReturnsAsync(expectedResponse);
        _mockApplicationService.Setup(service => service.ValidateAppBelongsToUser(_email, appId)).ReturnsAsync(false);

        var result = await _controller.FindApplicationById(_email, appId);
        var forbiddenRequestResult = Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task FindApplicationsValidUserReturnsOk()
    {
        var paginatedRequest = new PaginatedRequest { Page = 1, PageSize = 10 };
        var applicationListDtos = new List<ApplicationListDto>
        {
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 1", Type = "Test", Tra = "Test" },
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 2", Type = "Test", Tra = "Test" },
            new ApplicationListDto { Id = Guid.NewGuid(), Name = "App 3", Type = "Test", Tra = "Test" }
        };

        var paginatedResponse = new PaginatedResponse<ApplicationListDto>(
            applicationListDtos.AsReadOnly(), paginatedRequest.Page, applicationListDtos.Count);

        _mockApplicationService
            .Setup(service => service.GetApplications(It.IsAny<string>(), It.IsAny<PaginatedRequest>()))
            .ReturnsAsync(paginatedResponse);

        var result = await _controller.FindApplications(_email, paginatedRequest);

        var actionResult = Assert.IsType<ActionResult<PaginatedResponse<ApplicationListDto>>>(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        Assert.Equal(200, okObjectResult.StatusCode);

        var responseValue = Assert.IsType<PaginatedResponse<ApplicationListDto>>(okObjectResult.Value);
        Assert.Equal(paginatedResponse.TotalCount, responseValue.TotalCount);
        Assert.Equal(paginatedResponse.Results.Count(), responseValue.Results.Count());
    }

    [Fact]
    public async Task ActivateApplicationAppDoesNotBelongToUserReturnsForbid()
    {
        Guid appId = Guid.NewGuid();

        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser(_email, appId))
            .ReturnsAsync(false);

        var result = await _controller.ActivateApplication(_email, appId);
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task ActivateApplicationSuccessfulActivationReturnsOk()
    {
        Guid appId = Guid.NewGuid();

        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser(_email, appId))
            .ReturnsAsync(true);
        _mockApplicationService
            .Setup(s => s.ActivateApplicationById(_email, appId))
            .ReturnsAsync(true);
        _mockApplicationService
            .Setup(s => s.GetApplication(_email, appId))
            .ReturnsAsync(_applicationResponse);
        _mockEmailService
            .Setup(s => s.SendEmail(_applicationResponse.Name, _email, ApplicationStatusType.Active.Status))
            .Returns(_emailNotificationResponse);

        var result = await _controller.ActivateApplication(_email, appId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task ActivateApplicationInvalidOperationExceptionReturnsInternalServerError()
    {
        Guid appId = Guid.NewGuid();

        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser(_email, appId))
            .ReturnsAsync(true);
        _mockApplicationService
            .Setup(s => s.ActivateApplicationById(_email, appId))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        var result = await _controller.ActivateApplication(_email, appId);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task ActivateApplicationUnexpectedExceptionReturnsInternalServerError()
    {
        Guid appId = Guid.NewGuid();

        _mockApplicationService
            .Setup(s => s.ValidateAppBelongsToUser(_email, appId))
            .ReturnsAsync(true);
        _mockApplicationService
            .Setup(s => s.ActivateApplicationById(_email, appId))
            .ThrowsAsync(new Exception("Something went wrong"));

        var result = await _controller.ActivateApplication(_email, appId);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
    
    [Fact]
    public async Task GetInactiveApplicationsValidUserReturnsOk()
    {
        var request = new PaginatedRequest { Page = 1, PageSize = 1 };

        var applications = new List<ApplicationInactiveListDto>
        {
            new() { TraName = "TraName", Type = "Type", UserEmail = "UserEmail", Username = "Username" }
        };

        _mockApplicationService
            .Setup(it => it.GetInactiveApplications(request))
            .ReturnsAsync(() => new PaginatedResponse<ApplicationInactiveListDto>(applications, 1, applications.Count));

        var actual = await _controller.FindInactiveApplications(request);
        var okResult = Assert.IsType<OkObjectResult>(actual.Result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetPendingApplicationsThrowsArgumentNullException()
    {
        var request = new PaginatedRequest { Page = 1, PageSize = 1 };

        _mockApplicationService
            .Setup(service => service.GetInactiveApplications(request))
            .ThrowsAsync(new ArgumentNullException());

        var actual = await _controller.FindInactiveApplications(request);
        var requestResult = Assert.IsType<BadRequestObjectResult>(actual.Result);
        Assert.Equal(400, requestResult.StatusCode);
    }

    [Fact]
    public async Task  GetPendingApplicationsThrowsInvalidOperationException()
    {
        var request = new PaginatedRequest { Page = 1, PageSize = 1 };

        _mockApplicationService
            .Setup(service => service.GetInactiveApplications(request))
            .ThrowsAsync(new InvalidOperationException());

        var actual = await _controller.FindInactiveApplications(request);
        var requestResult = Assert.IsType<ObjectResult>(actual.Result);
        Assert.Equal(500, requestResult.StatusCode);
    }

    [Fact]
    public async Task GetPendingApplicationsThrowsException()
    {
        var request = new PaginatedRequest { Page = 1, PageSize = 1 };

        _mockApplicationService
            .Setup(service => service.GetInactiveApplications(request))
            .ThrowsAsync(new Exception());

        var actual = await _controller.FindInactiveApplications(request);
        var requestResult = Assert.IsType<ObjectResult>(actual.Result);
        Assert.Equal(500, requestResult.StatusCode);
    }
}
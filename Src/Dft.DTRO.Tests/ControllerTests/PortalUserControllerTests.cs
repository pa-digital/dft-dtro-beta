using DfT.DTRO.Models.PortalUser;

public class PortalUsersControllerTests
{
    private readonly Mock<IPortalUserService> _mockPortalUserService;
    private readonly Mock<ILogger<PortalUsersController>> _mockLogger;
    private readonly Mock<LoggingExtension> _mockLoggingExtension;
    private readonly PortalUsersController _controller;

    public PortalUsersControllerTests()
    {
        _mockPortalUserService = new Mock<IPortalUserService>();
        _mockLogger = new Mock<ILogger<PortalUsersController>>();
        _mockLoggingExtension = new Mock<LoggingExtension>();

        _controller = new PortalUsersController(
            _mockPortalUserService.Object,
            _mockLogger.Object,
            _mockLoggingExtension.Object
        );

        // Set up fake HttpContext to bypass TokenValidation middleware
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(ctx => ctx.Items["UserId"]).Returns("user@test.com");
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };
    }

    [Fact]
    public async Task CanPublish_ReturnsOkResult_WithExpectedResponse()
    {
        var expectedResponse = new PortalUserResponse { canPublish = true };
        _mockPortalUserService
            .Setup(service => service.CanUserPublish("user@test.com"))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.CanPublish();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualResponse = Assert.IsType<PortalUserResponse>(okResult.Value);
        Assert.True(actualResponse.canPublish);
    }

    [Fact]
    public async Task CanPublish_Returns500_WhenExceptionIsThrown()
    {
        _mockPortalUserService
            .Setup(service => service.CanUserPublish("user@test.com"))
            .ThrowsAsync(new Exception("Database failure"));
        
        var result = await _controller.CanPublish();
        
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}

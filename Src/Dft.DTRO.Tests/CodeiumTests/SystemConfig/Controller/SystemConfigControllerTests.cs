namespace Dft.DTRO.Tests.CodeiumTests.SystemConfig.Controller;

public class SystemConfigControllerTests
{
    private readonly Mock<IAppIdMapperService> _mockXappIdMapperService;
    private readonly Mock<ISystemConfigService> _mockSystemConfigService;
    private readonly Mock<ILogger<SystemConfigController>> _mockLogger;
    private readonly Mock<LoggingExtension.Builder> _mockLoggingBuilder;
    private readonly Mock<LoggingExtension> _mockLoggingExtension;
    private readonly SystemConfigController _controller;

    public SystemConfigControllerTests()
    {
        _mockSystemConfigService = new Mock<ISystemConfigService>();
        _mockLogger = new Mock<ILogger<SystemConfigController>>();
        _mockLoggingBuilder = new Mock<LoggingExtension.Builder>();
        _mockLoggingExtension = new Mock<LoggingExtension>();

        _mockXappIdMapperService = new Mock<IAppIdMapperService>();
        _mockXappIdMapperService.Setup(service => service.GetAppId(It.IsAny<HttpContext>())).ReturnsAsync(Guid.NewGuid());


        _controller = new SystemConfigController(
            _mockSystemConfigService.Object,
            _mockXappIdMapperService.Object,
            _mockLogger.Object,
            _mockLoggingExtension.Object);
    }

    [Fact]
    public async Task GetSystemName_ReturnsOkResult_WithSystemName()
    {
        // Arrange
        var systemName = "TestSystem";
        _mockSystemConfigService.Setup(service => service.GetSystemConfigAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new SystemConfigResponse { IsTest = true, SystemName = "TestSystem" });

        // Act
        var result = await _controller.GetSystemConfig(Guid.NewGuid());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.True(((SystemConfigResponse)okResult.Value).IsTest);
        Assert.Equal(systemName, ((SystemConfigResponse)okResult.Value).SystemName);
    }

    [Fact]
    public async Task GetSystemName_ReturnsInternalServerError_OnException()
    {
        // Arrange
        _mockSystemConfigService.Setup(service => service.GetSystemConfigAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Service failure"));

        // Act
        var result = await _controller.GetSystemConfig(Guid.NewGuid());

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        var apiErrorResponse = Assert.IsType<ApiErrorResponse>(statusCodeResult.Value);
        Assert.Equal("Internal Server Error", apiErrorResponse.Message);
    }
}
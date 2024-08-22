
using DfT.DTRO.Models.SystemConfig;

public class SystemConfigControllerTests
{
    private readonly Mock<ISystemConfigService> _mockSystemConfigService;
    private readonly Mock<ILogger<SystemConfigController>> _mockLogger;
    private readonly SystemConfigController _controller;

    public SystemConfigControllerTests()
    {
        _mockSystemConfigService = new Mock<ISystemConfigService>();
        _mockLogger = new Mock<ILogger<SystemConfigController>>();
        _controller = new SystemConfigController(_mockSystemConfigService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetSystemName_ReturnsOkResult_WithSystemName()
    {
        // Arrange
        var systemName = "TestSystem";
        _mockSystemConfigService.Setup(service => service.GetSystemConfigAsync())
            .ReturnsAsync(new DfT.DTRO.Models.SystemConfig.SystemConfigResponse { IsTest = true, SystemName = "TestSystem" });

        // Act
        var result = await _controller.GetSystemConfig();

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
        _mockSystemConfigService.Setup(service => service.GetSystemConfigAsync())
            .ThrowsAsync(new Exception("Service failure"));

        // Act
        var result = await _controller.GetSystemConfig();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        var apiErrorResponse = Assert.IsType<ApiErrorResponse>(statusCodeResult.Value);
        Assert.Equal("Internal Server Error", apiErrorResponse.Message);
    }
}

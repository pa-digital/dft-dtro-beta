﻿namespace Dft.DTRO.Tests.CodeiumTests.SystemConfig.Controller;

public class SystemConfigControllerTests
{
    private readonly Mock<IXappIdMapperService> _mockXappIdMapperService;
    private readonly Mock<ISystemConfigService> _mockSystemConfigService;
    private readonly Mock<ILogger<SystemConfigController>> _mockLogger;
    private readonly SystemConfigController _controller;

    public SystemConfigControllerTests()
    {
        _mockSystemConfigService = new Mock<ISystemConfigService>();
        _mockLogger = new Mock<ILogger<SystemConfigController>>();

        _mockXappIdMapperService = new Mock<IXappIdMapperService>();
        _mockXappIdMapperService.Setup(service => service.GetXappId(It.IsAny<HttpContext>())).ReturnsAsync(Guid.NewGuid());


        _controller = new SystemConfigController(_mockSystemConfigService.Object, _mockXappIdMapperService.Object, _mockLogger.Object);
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
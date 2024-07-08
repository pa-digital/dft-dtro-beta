using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using DfT.DTRO.Controllers;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.Metrics;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Dft.DTRO.Tests.CodeiumTests.Rules.Controller;
[ExcludeFromCodeCoverage]
public class MetricsControllerTests
{
    private Mock<IMetricsService> _mockMetricsService;
    private Mock<ILogger<MetricsController>> _mockLogger;
    private MetricsController _controller;
    private MetricRequest _metricRequest = new MetricRequest() {DateFrom = new DateTime(2024, 1, 2), DateTo = new DateTime(2024, 1, 12)};
    public MetricsControllerTests()
    {
        _mockMetricsService = new Mock<IMetricsService>();
        _mockLogger = new Mock<ILogger<MetricsController>>();

        _controller = new MetricsController(
         _mockMetricsService.Object,
         _mockLogger.Object);
    }
    // Remove the GetRuleById_ReturnsOk_WhenRuleExists test or add the corresponding method to MetricsController
    [Fact]
    public void HealthApi_ReturnsOkWithTrue()
    {
        // Arrange
        var expected = true;
        // Act
        var result = _controller.HealthApi();
        // Assert
        var okResult = result.Result as ObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult?.Value);
    }


    [Fact]
    public void HealthTraId_ReturnsStatusCode200_WhenTraIdIsProvided()
    {
        // Arrange
        int traId = 123;

        // Act
        var result = _controller.HealthTraId(traId);

        // Assert
        var okResult = result.Result as ObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(traId, okResult.Value);
    }

    [Fact]
    public void HealthTraId_ReturnsStatusCode404_WhenTraIdIsNotProvided()
    {
        // Arrange
        int? traId = null;

        // Act
        var result = _controller.HealthTraId(traId);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
        var apiErrorResponse = notFoundResult.Value as ApiErrorResponse;
        Assert.Equal("Not found", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task HealthDatabase_ReturnsStatusCode200_WhenDatabaseIsAvailable()
    {
        // Arrange
        _mockMetricsService.Setup(service => service.CheckDataBase()).ReturnsAsync(true);

        // Act
        var result = await _controller.HealthDatabase();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public async Task HealthDatabase_ReturnsStatusCode404_WhenDatabaseIsNotAvailable()
    {
        // Arrange
        _mockMetricsService.Setup(service => service.CheckDataBase()).ReturnsAsync(false);

        // Act
        var result = await _controller.HealthDatabase();

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
        var apiErrorResponse = notFoundResult.Value as ApiErrorResponse;
        Assert.Equal("Not found", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task HealthDatabase_ReturnsStatusCode500_OnException()
    {
        // Arrange
        _mockMetricsService.Setup(service => service.CheckDataBase()).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.HealthDatabase();

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
        var apiErrorResponse = objectResult.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task GetMetricsForTra_ReturnsStatusCode200_WithValidInput()
    {
        // Arrange
     
        _metricRequest.TraId = 123;
        var metricSummary = new MetricSummary
        {
            SystemFailureCount = 1,
            SubmissionFailureCount = 2,
            SubmissionCount = 3,
            DeletionCount = 4,
            SearchCount = 5,
            EventCount = 6
        };

        _mockMetricsService.Setup(service => service.GetMetrics(_metricRequest)).ReturnsAsync(metricSummary);

        // Act
        var result = await _controller.GetMetricsForTra(_metricRequest);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(metricSummary, okResult.Value);
    }

    [Fact]
    public async Task GetMetricsForTra_ReturnsStatusCode500_OnException()
    {
        // Arrange
        _metricRequest.TraId = 123;

        _mockMetricsService.Setup(service => service.GetMetrics(_metricRequest)).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetMetricsForTra(_metricRequest);

        // Assert
        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
        var apiErrorResponse = objectResult.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
    }
}




using System.Diagnostics.CodeAnalysis;
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
    private MetricRequest _metricRequest = new MetricRequest() { DateFrom = new DateTime(2024, 1, 2), DateTo = new DateTime(2024, 1, 12) };
    public MetricsControllerTests()
    {
        _mockMetricsService = new Mock<IMetricsService>();
        _mockLogger = new Mock<ILogger<MetricsController>>();

        _controller = new MetricsController(
         _mockMetricsService.Object,
         _mockLogger.Object);
    }
    [Fact]
    public void HealthApi_ReturnsOkWithTrue()
    {
        var expected = true;
        var result = _controller.HealthApi();
        var okResult = result.Result as ObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult?.Value);
    }


    [Fact]
    public void HealthTraId_ReturnsStatusCode200_WhenTraIdIsProvided()
    {
        int traId = 123;

        var result = _controller.HealthTraId(traId);

        var okResult = result.Result as ObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(traId, okResult.Value);
    }

    [Fact]
    public void HealthTraId_ReturnsStatusCode404_WhenTraIdIsNotProvided()
    {
        int? traId = null;

        var result = _controller.HealthTraId(traId);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
        var apiErrorResponse = notFoundResult.Value as ApiErrorResponse;
        Assert.Equal("Not found", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task HealthDatabase_ReturnsStatusCode200_WhenDatabaseIsAvailable()
    {
        _mockMetricsService.Setup(service => service.CheckDataBase()).ReturnsAsync(true);

        var result = await _controller.HealthDatabase();

        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public async Task HealthDatabase_ReturnsStatusCode404_WhenDatabaseIsNotAvailable()
    {
        _mockMetricsService.Setup(service => service.CheckDataBase()).ReturnsAsync(false);

        var result = await _controller.HealthDatabase();

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
        var apiErrorResponse = notFoundResult.Value as ApiErrorResponse;
        Assert.Equal("Not found", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task HealthDatabase_ReturnsStatusCode500_OnException()
    {
        _mockMetricsService.Setup(service => service.CheckDataBase()).ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.HealthDatabase();

        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
        var apiErrorResponse = objectResult.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task GetMetricsForTra_ReturnsStatusCode200_WithValidInput()
    {
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

        var result = await _controller.GetMetricsForTra(_metricRequest);

        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(metricSummary, okResult.Value);
    }

    [Fact]
    public async Task GetMetricsForTra_ReturnsStatusCode500_OnException()
    {
        _metricRequest.TraId = 123;

        _mockMetricsService.Setup(service => service.GetMetrics(_metricRequest)).ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.GetMetricsForTra(_metricRequest);

        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
        var apiErrorResponse = objectResult.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
    }
}




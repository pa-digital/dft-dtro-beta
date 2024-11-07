namespace Dft.DTRO.Tests.CodeiumTests.Metrics.Controller;

[ExcludeFromCodeCoverage]
public class MetricsControllerTests
{
    private readonly MetricsController _controller;

    private readonly MetricRequest _metricRequest =
        new() { DateFrom = new DateTime(2024, 1, 2), DateTo = new DateTime(2024, 1, 12) };

    private readonly Mock<IMetricsService> _mockMetricsService;
    private readonly Mock<LoggingExtension.Builder> _mockLoggingBuilder;
    private readonly Mock<LoggingExtension> _mockLoggingExtension;

    public MetricsControllerTests()
    {
        _mockMetricsService = new Mock<IMetricsService>();
        Mock<ILogger<MetricsController>> mockLogger = new();
        _mockLoggingBuilder = new Mock<LoggingExtension.Builder>();
        _mockLoggingExtension = new Mock<LoggingExtension>();

        _controller = new MetricsController(
            _mockMetricsService.Object,
            mockLogger.Object,
            _mockLoggingExtension.Object);
    }

    [Fact]
    public void HealthApi_ReturnsOkWithTrue()
    {
        const bool expected = true;
        ActionResult<bool>? result = _controller.HealthApi();
        ObjectResult? okResult = result.Result as ObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task HealthDatabase_ReturnsStatusCode200_WhenDatabaseIsAvailable()
    {
        _mockMetricsService.Setup(service => service.CheckDataBase()).ReturnsAsync(true);

        ActionResult<bool>? result = await _controller.HealthDatabase();

        OkObjectResult? okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.True((bool)(okResult.Value ?? false));
    }

    [Fact]
    public async Task HealthDatabase_ReturnsStatusCode404_WhenDatabaseIsNotAvailable()
    {
        _mockMetricsService.Setup(service => service.CheckDataBase()).ReturnsAsync(false);

        ActionResult<bool>? result = await _controller.HealthDatabase();

        NotFoundObjectResult? notFoundResult = result.Result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
        ApiErrorResponse? apiErrorResponse = notFoundResult.Value as ApiErrorResponse;
        Assert.Equal("Not found", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task HealthDatabase_ReturnsStatusCode500_OnException()
    {
        _mockMetricsService.Setup(service => service.CheckDataBase()).ThrowsAsync(new Exception("Test exception"));

        ActionResult<bool>? result = await _controller.HealthDatabase();

        ObjectResult? objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
        ApiErrorResponse? apiErrorResponse = objectResult.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task GetMetricsForTra_ReturnsStatusCode200_WithValidInput()
    {
        MetricSummary metricSummary = new()
        {
            SystemFailureCount = 1,
            SubmissionFailureCount = 2,
            SubmissionCount = 3,
            DeletionCount = 4,
            SearchCount = 5,
            EventCount = 6
        };

        _mockMetricsService.Setup(service => service.GetMetrics(_metricRequest)).ReturnsAsync(metricSummary);

        ActionResult<MetricSummary>? result = await _controller.GetMetricsForDtroUser(_metricRequest);

        OkObjectResult? okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(metricSummary, okResult.Value);
    }

    [Fact]
    public async Task GetMetricsForTra_ReturnsStatusCode500_OnException()
    {
        _mockMetricsService.Setup(service => service.GetMetrics(_metricRequest))
            .ThrowsAsync(new Exception("Test exception"));

        ActionResult<MetricSummary>? result = await _controller.GetMetricsForDtroUser(_metricRequest);

        ObjectResult? objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
        ApiErrorResponse? apiErrorResponse = objectResult.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
    }
}
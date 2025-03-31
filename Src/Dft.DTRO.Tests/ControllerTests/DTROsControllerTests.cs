namespace Dft.DTRO.Tests.ControllerTests;

public class DTROsControllerTests
{
    private readonly Mock<IDtroService> _mockDtroService = new();
    private readonly Mock<IMetricsService> _mockMetricsService = new();

    private readonly DTROsController _sut;
    private readonly Guid _appId;

    public DTROsControllerTests()
    {
        ILogger<DTROsController> mockLogger = MockLogger.Setup<DTROsController>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _sut = new DTROsController(
            _mockDtroService.Object,
            _mockMetricsService.Object,
            mockLogger,
            mockLoggingExtension.Object);

        _appId = Guid.NewGuid();
        Mock<HttpContext> mockContext = MockHttpContext.Setup();

        _mockMetricsService
            .Setup(it => it.IncrementMetric(It.IsAny<MetricType>(), It.IsAny<Guid>()))
            .ReturnsAsync(() => true);
    }

    [Fact]
    public async Task CreateFromFileReturnsCreated()
    {
        _mockDtroService
            .Setup(it => it.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<Guid>()))
            .ReturnsAsync(() => MockTestObjects.GuidResponse);

        IActionResult? actual = await _sut.CreateFromFile(_appId, MockTestObjects.TestFile);

        Assert.NotNull(actual);

        Assert.Equal(201, ((ObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task CreateFromFileThrowDtroValidationException()
    {
        _mockDtroService
            .Setup(it => it.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<Guid>()))
            .Throws<DtroValidationException>();

        IActionResult? actual = await _sut.CreateFromFile(_appId, MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(400, ((BadRequestObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task CreateFromFileThrowsNotFoundException()
    {
        _mockDtroService
            .Setup(it => it.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<Guid>()))
            .Throws<NotFoundException>();

        IActionResult? actual = await _sut.CreateFromFile(_appId, MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(404, ((NotFoundObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task CreateFromFileThrowsInvalidOperationException()
    {
        _mockDtroService
            .Setup(it => it.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<Guid>()))
            .Throws<InvalidOperationException>();

        IActionResult? actual = await _sut.CreateFromFile(_appId, MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(400, ((BadRequestObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task CreateFromFileThrowsException()
    {
        _mockDtroService
            .Setup(it => it.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<Guid>()))
            .Throws<Exception>();

        IActionResult? actual = await _sut.CreateFromFile(_appId, MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(500, ((ObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task UpdateFromFileReturnsCreated()
    {
        _mockDtroService
            .Setup(it =>
                it.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(),
                    It.IsAny<Guid>()))
            .ReturnsAsync(() => MockTestObjects.GuidResponse);

        IActionResult? actual = await _sut.UpdateFromFile(_appId, Guid.NewGuid(), MockTestObjects.TestFile);

        Assert.NotNull(actual);

        Assert.Equal(200, ((ObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task UpdateFromFileThrowDtroValidationException()
    {
        _mockDtroService
            .Setup(it =>
                it.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(),
                    It.IsAny<Guid>()))
            .Throws<DtroValidationException>();

        IActionResult? actual = await _sut.UpdateFromFile(_appId, Guid.NewGuid(), MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(400, ((BadRequestObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task UpdateFromFileThrowsNotFoundException()
    {
        _mockDtroService
            .Setup(it =>
                it.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(),
                    It.IsAny<Guid>()))
            .Throws<NotFoundException>();

        IActionResult? actual = await _sut.UpdateFromFile(_appId, Guid.NewGuid(), MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(404, ((NotFoundObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task UpdateFromFileThrowsInvalidOperationException()
    {
        _mockDtroService
            .Setup(it =>
                it.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(),
                    It.IsAny<Guid>()))
            .Throws<InvalidOperationException>();

        IActionResult? actual = await _sut.UpdateFromFile(_appId, Guid.NewGuid(), MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(400, ((BadRequestObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task UpdateFromFileThrowsException()
    {
        _mockDtroService
            .Setup(it =>
                it.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(),
                    It.IsAny<Guid>()))
            .Throws<Exception>();

        IActionResult? actual = await _sut.UpdateFromFile(_appId, Guid.NewGuid(), MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(500, ((ObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task GetAllReturnsRecords()
    {
        var queryParameters = new GetAllQueryParameters
        {
            TraCode = 1000,
            StartDate = new DateTime(2025, 1, 21, 16, 21, 26),
            EndDate = new DateTime(2025, 1, 23, 11, 2, 41)
        };
        _mockDtroService
            .Setup(it => it.GetDtrosAsync(queryParameters))
            .ReturnsAsync(() => MockTestObjects.DtroResponses);

        IActionResult? actual = await _sut.GetAll(queryParameters);

        Assert.NotNull(actual);

        Assert.Equal(200, ((ObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task GetDTROSubmissionCountReturnsOkWithCorrectCount()
    {
        int expectedCount = 5;
        _mockDtroService.Setup(s => s.GetDtroSubmissionCount()).ReturnsAsync(expectedCount);

        var result = await _sut.GetDTROSubmissionCount();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseData = Assert.IsType<DtroCountResponse>(okResult.Value);
        Assert.Equal(expectedCount, responseData.Count);
    }

    [Fact]
    public async Task GetDTROSubmissionCountReturns500OnException()
    {
        _mockDtroService.Setup(s => s.GetDtroSubmissionCount()).ThrowsAsync(new Exception("Database error"));

        var result = await _sut.GetDTROSubmissionCount();
        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusResult.StatusCode);

        var errorResponse = Assert.IsType<ApiErrorResponse>(statusResult.Value);
        Assert.Equal("Internal Server Error", errorResponse.Message);
    }
}
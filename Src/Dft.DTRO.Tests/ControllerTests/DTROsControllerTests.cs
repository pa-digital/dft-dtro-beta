namespace Dft.DTRO.Tests.ControllerTests;

[ExcludeFromCodeCoverage]
public class DTROsControllerTests
{
    private readonly Mock<IDtroService> _mockDtroService = new();
    private readonly Mock<IMetricsService> _mockMetricsService = new();
    private readonly Mock<IRequestCorrelationProvider> _mockRequestCorrelationProvider = new();
    private readonly Mock<IAppIdMapperService> _mockXAppIdMapperService = new();

    private readonly DTROsController _sut;
    private readonly Guid _xAppId;

    public DTROsControllerTests()
    {
        ILogger<DTROsController> mockLogger = MockLogger.Setup<DTROsController>();
        var mockLoggingExtension = new Mock<LoggingExtension>();

        _sut = new DTROsController(
            _mockDtroService.Object,
            _mockMetricsService.Object,
            _mockRequestCorrelationProvider.Object,
            _mockXAppIdMapperService.Object,
            mockLogger,
            mockLoggingExtension.Object);

        _xAppId = Guid.NewGuid();
        Mock<HttpContext> mockContext = MockHttpContext.Setup();

        _mockXAppIdMapperService
            .Setup(it => it.GetAppId(mockContext.Object))
            .ReturnsAsync(() => _xAppId);

        _mockMetricsService
            .Setup(it => it.IncrementMetric(It.IsAny<MetricType>(), It.IsAny<Guid>()))
            .ReturnsAsync(() => true);

        _mockRequestCorrelationProvider
            .SetupGet(provider => provider.CorrelationId)
            .Returns(() => _xAppId.ToString());
    }

    [Fact]
    public async Task CreateFromFileReturnsCreated()
    {
        _mockDtroService
            .Setup(it => it.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync(() => MockTestObjects.GuidResponse);

        IActionResult? actual = await _sut.CreateFromFile(_xAppId, MockTestObjects.TestFile);

        Assert.NotNull(actual);

        Assert.Equal(201, ((ObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task CreateFromFileThrowDtroValidationException()
    {
        _mockDtroService
            .Setup(it => it.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), It.IsAny<Guid>()))
            .Throws<DtroValidationException>();

        IActionResult? actual = await _sut.CreateFromFile(_xAppId, MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(400, ((BadRequestObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task CreateFromFileThrowsNotFoundException()
    {
        _mockDtroService
            .Setup(it => it.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), It.IsAny<Guid>()))
            .Throws<NotFoundException>();

        IActionResult? actual = await _sut.CreateFromFile(_xAppId, MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(404, ((NotFoundObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task CreateFromFileThrowsInvalidOperationException()
    {
        _mockDtroService
            .Setup(it => it.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), It.IsAny<Guid>()))
            .Throws<InvalidOperationException>();

        IActionResult? actual = await _sut.CreateFromFile(_xAppId, MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(400, ((BadRequestObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task CreateFromFileThrowsException()
    {
        _mockDtroService
            .Setup(it => it.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), It.IsAny<Guid>()))
            .Throws<Exception>();

        IActionResult? actual = await _sut.CreateFromFile(_xAppId, MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(500, ((ObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task UpdateFromFileReturnsCreated()
    {
        _mockDtroService
            .Setup(it =>
                it.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(),
                    It.IsAny<Guid>()))
            .ReturnsAsync(() => MockTestObjects.GuidResponse);

        IActionResult? actual = await _sut.UpdateFromFile(_xAppId, Guid.NewGuid(), MockTestObjects.TestFile);

        Assert.NotNull(actual);

        Assert.Equal(200, ((ObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task UpdateFromFileThrowDtroValidationException()
    {
        _mockDtroService
            .Setup(it =>
                it.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(),
                    It.IsAny<Guid>()))
            .Throws<DtroValidationException>();

        IActionResult? actual = await _sut.UpdateFromFile(_xAppId, Guid.NewGuid(), MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(400, ((BadRequestObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task UpdateFromFileThrowsNotFoundException()
    {
        _mockDtroService
            .Setup(it =>
                it.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(),
                    It.IsAny<Guid>()))
            .Throws<NotFoundException>();

        IActionResult? actual = await _sut.UpdateFromFile(_xAppId, Guid.NewGuid(), MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(404, ((NotFoundObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task UpdateFromFileThrowsInvalidOperationException()
    {
        _mockDtroService
            .Setup(it =>
                it.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(),
                    It.IsAny<Guid>()))
            .Throws<InvalidOperationException>();

        IActionResult? actual = await _sut.UpdateFromFile(_xAppId, Guid.NewGuid(), MockTestObjects.TestFile);

        Assert.NotNull(actual);
        Assert.Equal(400, ((BadRequestObjectResult)actual).StatusCode);
    }

    [Fact]
    public async Task UpdateFromFileThrowsException()
    {
        _mockDtroService
            .Setup(it =>
                it.TryUpdateDtroAsJsonAsync(It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(),
                    It.IsAny<Guid>()))
            .Throws<Exception>();

        IActionResult? actual = await _sut.UpdateFromFile(_xAppId, Guid.NewGuid(), MockTestObjects.TestFile);

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
}
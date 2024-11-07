namespace Dft.DTRO.Tests.CodeiumTests.Rules.Controller;

[ExcludeFromCodeCoverage]
public class RulesControllerTests
{
    private readonly RulesController _controller;
    private readonly Mock<IRuleTemplateService> _mockRuleTemplateService;
    private readonly Mock<LoggingExtension.Builder> _mockLoggingBuilder;
    private readonly Mock<LoggingExtension> _mockLoggingExtension;

    public RulesControllerTests()
    {
        _mockRuleTemplateService = new Mock<IRuleTemplateService>();
        Mock<IRequestCorrelationProvider> mockCorrelationProvider = new();
        Mock<ILogger<RulesController>> mockLogger = new();
        _mockLoggingBuilder = new Mock<LoggingExtension.Builder>();
        _mockLoggingExtension = new Mock<LoggingExtension>();

        _controller = new RulesController(
            _mockRuleTemplateService.Object,
            mockCorrelationProvider.Object,
            mockLogger.Object,
            _mockLoggingExtension.Object);
    }


    [Fact]
    public async Task GetRulesVersions_ReturnsOk_WithVersions()
    {
        List<RuleTemplateOverview> expectedVersions = new()
        {
            new RuleTemplateOverview { SchemaVersion = new SchemaVersion("1.0.0") },
            new RuleTemplateOverview { SchemaVersion = new SchemaVersion("2.0.0") }
        };


        _mockRuleTemplateService.Setup(mock => mock.GetRuleTemplatesVersionsAsync())
            .ReturnsAsync(expectedVersions);

        IActionResult? result = await _controller.GetVersions();
        OkObjectResult? okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expectedVersions, okResult.Value);
    }

    [Fact]
    public async Task GetRulesVersions_ReturnsInternalServerError_OnException()
    {
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplatesVersionsAsync())
            .ThrowsAsync(new Exception("Test exception"));
        IActionResult? result = await _controller.GetVersions();
        ObjectResult? objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult.StatusCode);
        ApiErrorResponse? apiErrorResponse = objectResult.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
    }


    [Fact]
    public async Task GetRules_ReturnsOkResult_WithTemplates()
    {
        RuleTemplateResponse ruleTemplateResponse = new() { Template = "", SchemaVersion = new SchemaVersion("1.0.0") };

        List<RuleTemplateResponse> expected = new() { ruleTemplateResponse };

        _mockRuleTemplateService.Setup(s => s.GetRuleTemplatesAsync()).ReturnsAsync(expected);
        IActionResult? result = await _controller.Get();
        OkObjectResult? okResult = result as OkObjectResult;
        Assert.Equal(expected, okResult?.Value);
    }

    [Fact]
    public async Task GetRules_ReturnsInternalServerError_OnException()
    {
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplatesAsync()).ThrowsAsync(new Exception());
        IActionResult? result = await _controller.Get();
        ObjectResult? objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult?.StatusCode);
        ApiErrorResponse? apiErrorResponse = objectResult?.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task GetRule_ReturnsOk_WithValidVersion()
    {
        SchemaVersion version = new("1.0.0");
        RuleTemplateResponse expectedRule = new();
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateAsync(version)).ReturnsAsync(expectedRule);
        IActionResult? result = await _controller.GetByVersion(version.ToString());
        Assert.IsType<OkObjectResult>(result);
        OkObjectResult? okResult = result as OkObjectResult;
        Assert.Equal(expectedRule, okResult?.Value);
    }

    [Fact]
    public async Task GetRule_ReturnsNotFound_WhenNotFoundExceptionIsThrown()
    {
        SchemaVersion version = new("1.0.0");
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateAsync(version)).ThrowsAsync(new NotFoundException());
        IActionResult? result = await _controller.GetByVersion(version.ToString());
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetRule_ReturnsBadRequest_WhenInvalidOperationExceptionIsThrown()
    {
        SchemaVersion version = new("1.0.0");
        string exceptionMessage = "Invalid operation";
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateAsync(version))
            .ThrowsAsync(new InvalidOperationException(exceptionMessage));
        IActionResult? result = await _controller.GetByVersion(version.ToString());
        Assert.IsType<BadRequestObjectResult>(result);
        BadRequestObjectResult? badRequestResult = result as BadRequestObjectResult;
        ApiErrorResponse? errorResponse = badRequestResult?.Value as ApiErrorResponse;
        Assert.Equal("Bad Request", errorResponse?.Message);
    }

    [Fact]
    public async Task GetRule_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        SchemaVersion version = new("1.0.0");
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateAsync(version)).ThrowsAsync(new Exception());
        IActionResult? result = await _controller.GetByVersion(version.ToString());
        Assert.IsType<ObjectResult>(result);
        ObjectResult? objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult?.StatusCode);
        ApiErrorResponse? errorResponse = objectResult?.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", errorResponse?.Message);
    }
}
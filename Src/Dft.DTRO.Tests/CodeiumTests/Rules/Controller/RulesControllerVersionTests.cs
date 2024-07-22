namespace Dft.DTRO.Tests.CodeiumTests.Rules.Controller;

[ExcludeFromCodeCoverage]
public class RulesControllerVersionTests
{
    private readonly RulesController _controller;
    private readonly Mock<IRuleTemplateService> _mockRuleTemplateService;

    public RulesControllerVersionTests()
    {
        _mockRuleTemplateService = new Mock<IRuleTemplateService>();
        Mock<IRequestCorrelationProvider> mockCorrelationProvider = new();
        Mock<ILogger<RulesController>> mockLogger = new();
        _controller = new RulesController(
            _mockRuleTemplateService.Object,
            mockCorrelationProvider.Object,
            mockLogger.Object);
    }


    [Fact]
    public async Task GetRulesVersions_ReturnsOk_WithVersions()
    {
        List<RuleTemplateOverview> expectedVersions = new List<RuleTemplateOverview>
        {
            new() { SchemaVersion = new SchemaVersion("1.0.0") },
            new() { SchemaVersion = new SchemaVersion("2.0.0") }
        };


        _mockRuleTemplateService.Setup(mock => mock.GetRuleTemplatesVersionsAsync())
            .ReturnsAsync(expectedVersions);

        IActionResult? result = await _controller.GetVersions();
        OkObjectResult? okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult?.StatusCode);
        Assert.Equal(expectedVersions, okResult?.Value);
    }

    [Fact]
    public async Task GetRulesVersions_ReturnsInternalServerError_OnException()
    {
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplatesVersionsAsync())
            .ThrowsAsync(new Exception("Test exception"));
        IActionResult? result = await _controller.GetVersions();
        ObjectResult? objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult?.StatusCode);
        ApiErrorResponse? apiErrorResponse = objectResult?.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
    }
}
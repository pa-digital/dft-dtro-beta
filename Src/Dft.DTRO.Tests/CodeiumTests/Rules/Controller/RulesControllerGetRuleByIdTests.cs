namespace Dft.DTRO.Tests.CodeiumTests.Rules.Controller;

[ExcludeFromCodeCoverage]
public class RulesControllerGetRuleByIdTests
{
    private readonly RulesController _controller;
    private readonly Mock<IRequestCorrelationProvider> _mockCorrelationProvider;
    private readonly Mock<ILogger<RulesController>> _mockLogger;
    private readonly Mock<IRuleTemplateService> _mockRuleTemplateService;
    private readonly Mock<LoggingExtension.Builder> _mockLoggingBuilder;
    private readonly Mock<LoggingExtension> _mockLoggingExtension;

    public RulesControllerGetRuleByIdTests()
    {
        _mockRuleTemplateService = new Mock<IRuleTemplateService>();
        _mockCorrelationProvider = new Mock<IRequestCorrelationProvider>();
        _mockLogger = new Mock<ILogger<RulesController>>();
        _mockLoggingBuilder = new Mock<LoggingExtension.Builder>();
        _mockLoggingExtension = new Mock<LoggingExtension>();

        _controller = new RulesController(
            _mockRuleTemplateService.Object,
            _mockCorrelationProvider.Object,
            _mockLogger.Object,
            _mockLoggingExtension.Object);
    }


    [Fact]
    public async Task GetRuleById_ReturnsOk_WhenRuleExists()
    {
        RuleTemplateResponse expected = new()
        {
            Template = "",
            SchemaVersion = new SchemaVersion("1.0.0")
        };

        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expected);
        IActionResult? result = await _controller.GetById(new Guid());
        Assert.IsType<OkObjectResult>(result);
        OkObjectResult? okResult = result as OkObjectResult;
        Assert.Equal(expected, okResult?.Value);
    }

    [Fact]
    public async Task GetRuleById_ReturnsNotFound_WhenRuleDoesNotExist()
    {
        Guid ruleId = Guid.NewGuid();
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateByIdAsync(ruleId)).ThrowsAsync(new NotFoundException());
        IActionResult? result = await _controller.GetById(ruleId);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetRuleById_ReturnsBadRequest_WhenInvalidOperationExceptionOccurs()
    {
        Guid ruleId = Guid.NewGuid();
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateByIdAsync(ruleId))
            .ThrowsAsync(new InvalidOperationException("Invalid operation"));
        IActionResult? result = await _controller.GetById(ruleId);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetRuleById_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        Guid ruleId = Guid.NewGuid();
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateByIdAsync(ruleId))
            .ThrowsAsync(new Exception("Unexpected error"));
        IActionResult? result = await _controller.GetById(ruleId);
        Assert.IsType<ObjectResult>(result);
        ObjectResult? objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult?.StatusCode);
    }
}
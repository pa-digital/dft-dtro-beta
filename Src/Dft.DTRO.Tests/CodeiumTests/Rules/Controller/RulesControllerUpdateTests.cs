namespace Dft.DTRO.Tests.CodeiumTests.Rules.Controller;

[ExcludeFromCodeCoverage]
public class RulesControllerUpdateTests
{
    private readonly RulesController _controller;
    private readonly Mock<IRequestCorrelationProvider> _mockCorrelationProvider;
    private readonly Mock<IRuleTemplateService> _mockRuleTemplateService;
    private readonly Mock<LoggingExtension.Builder> _mockLoggingBuilder;
    private readonly Mock<LoggingExtension> _mockLoggingExtension;

    public RulesControllerUpdateTests()
    {
        _mockRuleTemplateService = new Mock<IRuleTemplateService>();
        _mockCorrelationProvider = new Mock<IRequestCorrelationProvider>();
        Mock<ILogger<RulesController>> mockLogger = new();
        _mockLoggingBuilder = new Mock<LoggingExtension.Builder>();
        _mockLoggingExtension = new Mock<LoggingExtension>();

        _controller = new RulesController(
            _mockRuleTemplateService.Object,
            _mockCorrelationProvider.Object,
            mockLogger.Object,
            _mockLoggingExtension.Object);
    }

    [Fact]
    public async Task UpdateRule_WithNullFile_ReturnsBadRequest()
    {
        const string version = "1.0";
        IFormFile? file = null;
        IActionResult? result = await _controller.UpdateFromFile(version, file);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateRule_WithEmptyFile_ReturnsBadRequest()
    {
        const string version = "1.0";
        Mock<IFormFile> file = new();
        file.Setup(f => f.Length).Returns(0);
        IActionResult? result = await _controller.UpdateFromFile(version, file.Object);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateRule_WithValidFile_ReturnsCreatedAtAction()
    {
        const string version = "1.0";
        Mock<IFormFile> file = new();
        const string fileContent = "valid content";
        MemoryStream memoryStream = new(Encoding.UTF8.GetBytes(fileContent));
        file.Setup(f => f.Length).Returns(memoryStream.Length);
        file.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((stream, _) => memoryStream.CopyTo(stream));
        GuidResponse response = new() { Id = Guid.NewGuid() };
        _mockRuleTemplateService.Setup(s => s.UpdateRuleTemplateAsJsonAsync(version, fileContent, It.IsAny<string>()))
            .ReturnsAsync(response);
        _mockCorrelationProvider.Setup(c => c.CorrelationId).Returns("correlation-id");
        IActionResult? result = await _controller.UpdateFromFile(version, file.Object);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpdateRule_WithInvalidOperationException_ReturnsBadRequest()
    {
        const string version = "1.0";
        Mock<IFormFile> file = new();
        const string fileContent = "invalid content";
        MemoryStream memoryStream = new(Encoding.UTF8.GetBytes(fileContent));
        file.Setup(f => f.Length).Returns(memoryStream.Length);
        file.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((stream, _) => memoryStream.CopyTo(stream));
        _mockRuleTemplateService.Setup(s => s.UpdateRuleTemplateAsJsonAsync(version, fileContent, It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Invalid operation"));
        IActionResult? result = await _controller.UpdateFromFile(version, file.Object);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateRule_WithException_ReturnsInternalServerError()
    {
        const string version = "1.0";
        Mock<IFormFile> file = new();
        const string fileContent = "content causing exception";
        MemoryStream memoryStream = new(Encoding.UTF8.GetBytes(fileContent));
        file.Setup(f => f.Length).Returns(memoryStream.Length);
        file.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((stream, _) => memoryStream.CopyTo(stream));
        _mockRuleTemplateService.Setup(s => s.UpdateRuleTemplateAsJsonAsync(version, fileContent, It.IsAny<string>()))
            .ThrowsAsync(new Exception("General exception"));
        IActionResult? result = await _controller.UpdateFromFile(version, file.Object);
        Assert.IsType<ObjectResult>(result);
        ObjectResult? objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult?.StatusCode);
    }
}
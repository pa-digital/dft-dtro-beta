using System.Diagnostics.CodeAnalysis;
using DfT.DTRO.Controllers;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.RuleTemplate;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.RequestCorrelation;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dft.DTRO.Tests.CodeiumTests.Rules.Controller;

[ExcludeFromCodeCoverage]
public class RulesControllerTests
{
    private Mock<IRuleTemplateService> _mockRuleTemplateService;
    private Mock<IRequestCorrelationProvider> _mockCorrelationProvider;
    private Mock<ILogger<RulesController>> _mockLogger;
    private RulesController _controller;

    public RulesControllerTests()
    {
        _mockRuleTemplateService = new Mock<IRuleTemplateService>();
        _mockCorrelationProvider = new Mock<IRequestCorrelationProvider>();
        _mockLogger = new Mock<ILogger<RulesController>>();
        _controller = new RulesController(
            _mockRuleTemplateService.Object,
            _mockCorrelationProvider.Object,
            _mockLogger.Object);
    }


    [Fact]
    public async Task GetRulesVersions_ReturnsOk_WithVersions()
    {
        // Arrange
        var expectedVersions = new List<RuleTemplateOverview>
        {
            new RuleTemplateOverview() { SchemaVersion = new SchemaVersion("1.0.0") },
            new RuleTemplateOverview() { SchemaVersion = new SchemaVersion("2.0.0") }
        };


        _mockRuleTemplateService.Setup(mock => mock.GetRuleTemplatesVersionsAsync())
            .ReturnsAsync(expectedVersions);

        // Act
        var result = await _controller.GetVersions();
        // Assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult?.StatusCode);
        Assert.Equal(expectedVersions, okResult?.Value);
    }
    [Fact]
    public async Task GetRulesVersions_ReturnsInternalServerError_OnException()
    {
        // Arrange
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplatesVersionsAsync())
            .ThrowsAsync(new Exception("Test exception"));
        // Act
        var result = await _controller.GetVersions();
        // Assert

        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(500, objectResult?.StatusCode);
        var apiErrorResponse = objectResult?.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
    }


    [Fact]
    public async Task GetRules_ReturnsOkResult_WithTemplates()
    {
        // Arrange
        var ruleTemplateResponse = new RuleTemplateResponse();
        ruleTemplateResponse.Template = "";
        ruleTemplateResponse.SchemaVersion = new SchemaVersion("1.0.0");

        var expected = new List<RuleTemplateResponse>();
        expected.Add(ruleTemplateResponse);

        _mockRuleTemplateService.Setup(s => s.GetRuleTemplatesAsync()).ReturnsAsync(expected);
        // Act
        var result = await _controller.Get();
        // Assert
        var okResult = result as OkObjectResult;
        Assert.Equal(expected, okResult?.Value);
    }

    [Fact]
    public async Task GetRules_ReturnsInternalServerError_OnException()
    {
        // Arrange
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplatesAsync()).ThrowsAsync(new Exception());
        // Act
        var result = await _controller.Get();
        // Assert
        var objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult?.StatusCode);
        var apiErrorResponse = objectResult?.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
    }

    [Fact]
    public async Task GetRule_ReturnsOk_WithValidVersion()
    {
        // Arrange
        var version = new SchemaVersion("1.0.0");
        var expectedRule = new RuleTemplateResponse();
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateAsync(version)).ReturnsAsync(expectedRule);
        // Act
        var result = await _controller.GetByVersion(version.ToString());
        // Assert

        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal(expectedRule, okResult?.Value);
    }
    [Fact]
    public async Task GetRule_ReturnsNotFound_WhenNotFoundExceptionIsThrown()
    {
        // Arrange
        var version = new SchemaVersion("1.0.0");
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateAsync(version)).ThrowsAsync(new NotFoundException());
        // Act
        var result = await _controller.GetByVersion(version.ToString());
        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
    [Fact]
    public async Task GetRule_ReturnsBadRequest_WhenInvalidOperationExceptionIsThrown()
    {
        // Arrange
        var version = new SchemaVersion("1.0.0");
        var exceptionMessage = "Invalid operation";
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateAsync(version)).ThrowsAsync(new InvalidOperationException(exceptionMessage));
        // Act
        var result = await _controller.GetByVersion(version.ToString());
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        var errorResponse = badRequestResult?.Value as ApiErrorResponse;
        Assert.Equal("Bad Request", errorResponse?.Message);
    }

    [Fact]
    public async Task GetRule_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var version = new SchemaVersion("1.0.0");
        _mockRuleTemplateService.Setup(s => s.GetRuleTemplateAsync(version)).ThrowsAsync(new Exception());
        // Act
        var result = await _controller.GetByVersion(version.ToString());
        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult?.StatusCode);
        var errorResponse = objectResult?.Value as ApiErrorResponse;
        Assert.Equal("Internal Server Error", errorResponse?.Message);
    }
}
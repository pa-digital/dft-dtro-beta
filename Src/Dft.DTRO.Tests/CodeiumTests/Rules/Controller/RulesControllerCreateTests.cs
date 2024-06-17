using System.Text;
using DfT.DTRO.Controllers;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.RequestCorrelation;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dft.DTRO.Tests.CodeiumTests.Rules.Controller;

public class RulesControllerCreateTests
{
    private readonly Mock<IRuleTemplateService> _mockRuleTemplateService;
    private readonly Mock<IRequestCorrelationProvider> _mockCorrelationProvider;
    private readonly Mock<ILogger<RulesController>> _mockLogger;
    private readonly RulesController _controller;

    public RulesControllerCreateTests()
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
    public async Task CreateRule_WithNullFile_ReturnsBadRequest()
    {
        // Arrange
        string version = "1.0";
        IFormFile? file = null;
        // Act
        var result = await _controller.CreateFromFile(version, file);
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    [Fact]
    public async Task CreateRule_WithEmptyFile_ReturnsBadRequest()
    {
        // Arrange
        string version = "1.0";
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(0);
        // Act
        var result = await _controller.CreateFromFile(version, file.Object);
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    [Fact]
    public async Task CreateRule_WithValidFile_ReturnsCreatedAtAction()
    {
        // Arrange
        string version = "1.0";
        var file = new Mock<IFormFile>();
        var fileContent = "valid content";
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        file.Setup(f => f.Length).Returns(memoryStream.Length);
        file.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((stream, token) => memoryStream.CopyTo(stream));
        var response = new GuidResponse { Id = Guid.NewGuid() };
        _mockRuleTemplateService.Setup(s => s.SaveRuleTemplateAsJsonAsync(version, fileContent, It.IsAny<string>()))
            .ReturnsAsync(response);
        _mockCorrelationProvider.Setup(c => c.CorrelationId).Returns("correlation-id");
        // Act
        var result = await _controller.CreateFromFile(version, file.Object);
        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }
    [Fact]
    public async Task CreateRule_WithInvalidOperationException_ReturnsBadRequest()
    {
        // Arrange
        string version = "1.0";
        var file = new Mock<IFormFile>();
        var fileContent = "invalid content";
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        file.Setup(f => f.Length).Returns(memoryStream.Length);
        file.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((stream, token) => memoryStream.CopyTo(stream));
        _mockRuleTemplateService.Setup(s => s.SaveRuleTemplateAsJsonAsync(version, fileContent, It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Invalid operation"));
        // Act
        var result = await _controller.CreateFromFile(version, file.Object);
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    [Fact]
    public async Task CreateRule_WithException_ReturnsInternalServerError()
    {
        // Arrange
        string version = "1.0";
        var file = new Mock<IFormFile>();
        var fileContent = "content causing exception";
        var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
        file.Setup(f => f.Length).Returns(memoryStream.Length);
        file.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((stream, token) => memoryStream.CopyTo(stream));
        _mockRuleTemplateService.Setup(s => s.SaveRuleTemplateAsJsonAsync(version, fileContent, It.IsAny<string>()))
            .ThrowsAsync(new Exception("General exception"));
        // Act
        var result = await _controller.CreateFromFile(version, file.Object);
        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult?.StatusCode);
    }
}
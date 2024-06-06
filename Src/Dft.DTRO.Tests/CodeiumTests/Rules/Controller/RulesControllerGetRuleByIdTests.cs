using DfT.DTRO.Controllers;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.RuleTemplate;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.RequestCorrelation;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dft.DTRO.Tests.CodeiumTests.Rules.Controller
{
    public class RulesControllerGetRuleByIdTests
    {
        private Mock<IRuleTemplateService> _mockRuleTemplateService;
        private Mock<IRequestCorrelationProvider> _mockCorrelationProvider;
        private Mock<ILogger<RulesController>> _mockLogger;
        private RulesController _controller;

        public RulesControllerGetRuleByIdTests()
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
        public async Task GetRuleById_ReturnsOk_WhenRuleExists()
        {
            // Arrange
            var expected = new RuleTemplateResponse();
            expected.Template = "";
            expected.SchemaVersion = new SchemaVersion("1.0.0");

            _mockRuleTemplateService.Setup(s => s.GetRuleTemplateByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expected);
            // Act
            var result = await _controller.GetById(new Guid());
            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(expected, okResult?.Value);
        }
        [Fact]
        public async Task GetRuleById_ReturnsNotFound_WhenRuleDoesNotExist()
        {
            // Arrange
            var ruleId = Guid.NewGuid();
            _mockRuleTemplateService.Setup(s => s.GetRuleTemplateByIdAsync(ruleId)).ThrowsAsync(new NotFoundException());
            // Act
            var result = await _controller.GetById(ruleId);
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        [Fact]
        public async Task GetRuleById_ReturnsBadRequest_WhenInvalidOperationExceptionOccurs()
        {
            // Arrange
            var ruleId = Guid.NewGuid();
            _mockRuleTemplateService.Setup(s => s.GetRuleTemplateByIdAsync(ruleId)).ThrowsAsync(new InvalidOperationException("Invalid operation"));
            // Act
            var result = await _controller.GetById(ruleId);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetRuleById_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var ruleId = Guid.NewGuid();
            _mockRuleTemplateService.Setup(s => s.GetRuleTemplateByIdAsync(ruleId)).ThrowsAsync(new Exception("Unexpected error"));
            // Act
            var result = await _controller.GetById(ruleId);
            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult?.StatusCode);
        }
    }
}
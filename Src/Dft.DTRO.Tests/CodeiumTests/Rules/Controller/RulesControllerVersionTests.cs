using DfT.DTRO.Controllers;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.RuleTemplate;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.RequestCorrelation;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;

namespace Dft.DTRO.Tests.CodeiumTests.Rules.Controller
{
    public class RulesControllerVersionTests
    {
        private Mock<IRuleTemplateService> _mockRuleTemplateService;
        private Mock<IRequestCorrelationProvider> _mockCorrelationProvider;
        private Mock<ILogger<RulesController>> _mockLogger;
        private RulesController _controller;

        public RulesControllerVersionTests()
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
            var result = await _controller.GetRulesVersions();
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
            var result = await _controller.GetRulesVersions();
            // Assert

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult?.StatusCode);
            var apiErrorResponse = objectResult?.Value as ApiErrorResponse;
            Assert.Equal("Internal Server Error", apiErrorResponse?.Message);
        }
    }
}
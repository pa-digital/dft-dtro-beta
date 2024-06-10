using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Services.Mapping;

namespace Dft.DTRO.Tests.CodeiumTests.Rules.Mapping;

public class RulesTemplateMappingServiceTests
{
    private RuleTemplateMappingService _service;
    public RulesTemplateMappingServiceTests()
    {
        _service = new RuleTemplateMappingService();
    }

    [Fact]
    public void MapToRuleTemplateResponse_SingleRuleTemplate_ReturnsCorrectResponse()
    {
        // Arrange
        var ruleTemplate = new RuleTemplate
        {
            SchemaVersion = new SchemaVersion("1.0.0"),
            Template = "TemplateContent"
        };
        // Act
        var response = _service.MapToRuleTemplateResponse(ruleTemplate);
        // Assert
        Assert.Equal(ruleTemplate.SchemaVersion, response.SchemaVersion);
        Assert.Equal(ruleTemplate.Template, response.Template);
    }
    [Fact]
    public void MapToRuleTemplateResponse_EmptyList_ReturnsEmptyList()
    {
        // Arrange
        var ruleTemplates = new List<RuleTemplate>();
        // Act
        var responses = _service.MapToRuleTemplateResponse(ruleTemplates);
        // Assert
        Assert.NotNull(responses);
        Assert.Empty(responses);
    }

    [Fact]
    public void MapToRuleTemplateResponse_NonEmptyList_ReturnsListOfResponses()
    {
        // Arrange
        var ruleTemplates = new List<RuleTemplate>
        {
            new RuleTemplate { SchemaVersion = new SchemaVersion("1.0.0"), Template = "TemplateContent1" },
            new RuleTemplate { SchemaVersion = new SchemaVersion("2.0.0"), Template = "TemplateContent2" }
        };
        // Act
        var responses = _service.MapToRuleTemplateResponse(ruleTemplates);
        // Assert
        Assert.NotNull(responses);
        Assert.Equal(ruleTemplates.Count, responses.Count);
        for (int i = 0; i < ruleTemplates.Count; i++)
        {
            Assert.Equal(ruleTemplates[i].SchemaVersion, responses[i].SchemaVersion);
            Assert.Equal(ruleTemplates[i].Template, responses[i].Template);
        }
    }
}
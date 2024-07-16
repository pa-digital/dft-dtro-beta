using System.Diagnostics.CodeAnalysis;
using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Services.Mapping;

namespace Dft.DTRO.Tests.CodeiumTests.Rules.Mapping;

[ExcludeFromCodeCoverage]
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
        var ruleTemplate = new RuleTemplate
        {
            SchemaVersion = new SchemaVersion("1.0.0"),
            Template = "TemplateContent"
        };
        var response = _service.MapToRuleTemplateResponse(ruleTemplate);
        Assert.Equal(ruleTemplate.SchemaVersion, response.SchemaVersion);
        Assert.Equal(ruleTemplate.Template, response.Template);
    }
    [Fact]
    public void MapToRuleTemplateResponse_EmptyList_ReturnsEmptyList()
    {
        var ruleTemplates = new List<RuleTemplate>();
        var responses = _service.MapToRuleTemplateResponse(ruleTemplates);
        Assert.NotNull(responses);
        Assert.Empty(responses);
    }

    [Fact]
    public void MapToRuleTemplateResponse_NonEmptyList_ReturnsListOfResponses()
    {
        var ruleTemplates = new List<RuleTemplate>
        {
            new RuleTemplate { SchemaVersion = new SchemaVersion("1.0.0"), Template = "TemplateContent1" },
            new RuleTemplate { SchemaVersion = new SchemaVersion("2.0.0"), Template = "TemplateContent2" }
        };
        var responses = _service.MapToRuleTemplateResponse(ruleTemplates);
        Assert.NotNull(responses);
        Assert.Equal(ruleTemplates.Count, responses.Count);
        for (int i = 0; i < ruleTemplates.Count; i++)
        {
            Assert.Equal(ruleTemplates[i].SchemaVersion, responses[i].SchemaVersion);
            Assert.Equal(ruleTemplates[i].Template, responses[i].Template);
        }
    }
}
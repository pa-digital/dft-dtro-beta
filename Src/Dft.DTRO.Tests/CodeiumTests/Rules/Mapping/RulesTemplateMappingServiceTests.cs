namespace Dft.DTRO.Tests.CodeiumTests.Rules.Mapping;

[ExcludeFromCodeCoverage]
public class RulesTemplateMappingServiceTests
{
    private readonly RuleTemplateMappingService _service = new();

    [Fact]
    public void MapToRuleTemplateResponse_SingleRuleTemplate_ReturnsCorrectResponse()
    {
        RuleTemplate ruleTemplate = new() { SchemaVersion = new SchemaVersion("1.0.0"), Template = "TemplateContent" };
        RuleTemplateResponse? response = _service.MapToRuleTemplateResponse(ruleTemplate);
        Assert.Equal(ruleTemplate.SchemaVersion, response.SchemaVersion);
        Assert.Equal(ruleTemplate.Template, response.Template);
    }

    [Fact]
    public void MapToRuleTemplateResponse_EmptyList_ReturnsEmptyList()
    {
        List<RuleTemplate> ruleTemplates = new();
        List<RuleTemplateResponse>? responses = _service.MapToRuleTemplateResponse(ruleTemplates);
        Assert.NotNull(responses);
        Assert.Empty(responses);
    }

    [Fact]
    public void MapToRuleTemplateResponse_NonEmptyList_ReturnsListOfResponses()
    {
        List<RuleTemplate> ruleTemplates = new()
        {
            new RuleTemplate() { SchemaVersion = new SchemaVersion("1.0.0"), Template = "TemplateContent1" },
            new RuleTemplate() { SchemaVersion = new SchemaVersion("2.0.0"), Template = "TemplateContent2" }
        };
        List<RuleTemplateResponse>? responses = _service.MapToRuleTemplateResponse(ruleTemplates);
        Assert.NotNull(responses);
        Assert.Equal(ruleTemplates.Count, responses.Count);
        for (int i = 0; i < ruleTemplates.Count; i++)
        {
            Assert.Equal(ruleTemplates[i].SchemaVersion, responses[i].SchemaVersion);
            Assert.Equal(ruleTemplates[i].Template, responses[i].Template);
        }
    }
}
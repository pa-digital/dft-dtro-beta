namespace Dft.DTRO.Tests.CodeiumTests.Rules.Service;

[ExcludeFromCodeCoverage]
public class RuleTemplateServiceTests
{
    private readonly Mock<IDtroDal> _mockDtroDal;
    private readonly Mock<IRuleTemplateDal> _mockRuleTemplateDal;
    private readonly Mock<IRuleTemplateMappingService> _mockRuleTemplateMappingService;
    private readonly RuleTemplateService _ruleTemplateService;

    public RuleTemplateServiceTests()
    {
        _mockRuleTemplateDal = new Mock<IRuleTemplateDal>();
        _mockDtroDal = new Mock<IDtroDal>();
        _mockRuleTemplateMappingService = new Mock<IRuleTemplateMappingService>();
        _ruleTemplateService = new RuleTemplateService(_mockRuleTemplateDal.Object, _mockDtroDal.Object,
            _mockRuleTemplateMappingService.Object);
    }

    [Fact]
    public async Task RuleTemplateExistsAsync_ReturnsTrue()
    {
        SchemaVersion schemaVersion = new("1.0.0");
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(true);
        bool result = await _ruleTemplateService.RuleTemplateExistsAsync(schemaVersion);
        Assert.True(result);
    }

    [Fact]
    public async Task RuleTemplateExistsAsync_ReturnsFalse()
    {
        SchemaVersion schemaVersion = new("1.0.0");
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(false);
        bool result = await _ruleTemplateService.RuleTemplateExistsAsync(schemaVersion);
        Assert.False(result);
    }

    [Fact]
    public async Task GetRuleTemplateByIdAsync_Found()
    {
        Guid id = Guid.NewGuid();
        RuleTemplate ruleTemplate = new();
        RuleTemplateResponse ruleTemplateResponse = new();
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsByIdAsync(id)).ReturnsAsync(true);
        _mockRuleTemplateDal.Setup(x => x.GetRuleTemplateByIdAsync(id)).ReturnsAsync(ruleTemplate);
        _mockRuleTemplateMappingService.Setup(x => x.MapToRuleTemplateResponse(ruleTemplate))
            .Returns(ruleTemplateResponse);
        RuleTemplateResponse? result = await _ruleTemplateService.GetRuleTemplateByIdAsync(id);
        Assert.Equal(ruleTemplateResponse, result);
    }

    [Fact]
    public async Task GetRuleTemplateAsync_Found()
    {
        SchemaVersion schemaVersion = new("1.0.0");
        RuleTemplate ruleTemplate = new();
        RuleTemplateResponse ruleTemplateResponse = new();
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(true);
        _mockRuleTemplateDal.Setup(x => x.GetRuleTemplateAsync(schemaVersion)).ReturnsAsync(ruleTemplate);
        _mockRuleTemplateMappingService.Setup(x => x.MapToRuleTemplateResponse(ruleTemplate))
            .Returns(ruleTemplateResponse);
        RuleTemplateResponse? result = await _ruleTemplateService.GetRuleTemplateAsync(schemaVersion);
        Assert.Equal(ruleTemplateResponse, result);
    }

    [Fact]
    public async Task GetRuleTemplatesAsync_ReturnsList()
    {
        List<RuleTemplate> ruleTemplates = new();
        List<RuleTemplateResponse> ruleTemplateResponses = new();
        _mockRuleTemplateDal.Setup(x => x.GetRuleTemplatesAsync()).ReturnsAsync(ruleTemplates);
        _mockRuleTemplateMappingService.Setup(x => x.MapToRuleTemplateResponse(ruleTemplates))
            .Returns(ruleTemplateResponses);
        List<RuleTemplateResponse>? result = await _ruleTemplateService.GetRuleTemplatesAsync();
        Assert.Equal(ruleTemplateResponses, result);
    }

    [Fact]
    public async Task GetRuleTemplatesVersionsAsync_ReturnsList()
    {
        List<RuleTemplateOverview> ruleTemplateOverviews = new();
        _mockRuleTemplateDal.Setup(x => x.GetRuleTemplatesVersionsAsync()).ReturnsAsync(ruleTemplateOverviews);
        List<RuleTemplateOverview>? result = await _ruleTemplateService.GetRuleTemplatesVersionsAsync();
        Assert.Equal(ruleTemplateOverviews, result);
    }

    [Fact]
    public async Task SaveRuleTemplateAsJsonAsync_Success()
    {
        const string version = "1.0.0";
        SchemaVersion schemaVersion = new("1.0.0");
        const string rule = "{}";
        const string correlationId = "test-correlation-id";
        GuidResponse guidResponse = new();
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(false);
        _mockRuleTemplateDal.Setup(x => x.SaveRuleTemplateAsJsonAsync(version, rule, correlationId))
            .ReturnsAsync(guidResponse);
        GuidResponse? result = await _ruleTemplateService.SaveRuleTemplateAsJsonAsync(version, rule, correlationId);
        Assert.Equal(guidResponse, result);
    }

    [Fact]
    public async Task UpdateRuleTemplateAsJsonAsync_Success()
    {
        const string version = "1.0.0";
        const string rule = "{}";
        const string correlationId = "test-correlation-id";
        GuidResponse guidResponse = new();
        SchemaVersion schemaVersion = new("1.0.0");

        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(true);
        _mockDtroDal.Setup(x => x.DtroCountForSchemaAsync(schemaVersion)).ReturnsAsync(0);
        _mockRuleTemplateDal.Setup(x => x.UpdateRuleTemplateAsJsonAsync(version, rule, correlationId))
            .ReturnsAsync(guidResponse);
        GuidResponse? result = await _ruleTemplateService.UpdateRuleTemplateAsJsonAsync(version, rule, correlationId);
        Assert.Equal(guidResponse, result);
    }

    [Fact]
    public async Task UpdateRuleTemplateAsJsonAsync_NotFound()
    {
        const string version = "1.0.0";
        SchemaVersion schemaVersion = new("1.0.0");
        const string rule = "{}";
        const string correlationId = "test-correlation-id";
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(false);
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _ruleTemplateService.UpdateRuleTemplateAsJsonAsync(version, rule, correlationId));
    }

    [Fact]
    public async Task GetRuleTemplateAsync_NotFound()
    {
        SchemaVersion schemaVersion = new("1.0.0");
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(false);
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _ruleTemplateService.GetRuleTemplateAsync(schemaVersion));
    }

    [Fact]
    public async Task SaveRuleTemplateAsJsonAsync_AlreadyExists()
    {
        const string version = "1.0.0";
        const string rule = "{}";
        const string correlationId = "test-correlation-id";
        SchemaVersion schemaVersion = new("1.0.0");
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(true);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _ruleTemplateService.SaveRuleTemplateAsJsonAsync(version, rule, correlationId));
    }

    [Fact]
    public async Task GetRuleTemplateByIdAsync_NotFound()
    {
        Guid id = Guid.NewGuid();
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsByIdAsync(id)).ReturnsAsync(false);
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _ruleTemplateService.GetRuleTemplateByIdAsync(id));
    }
}
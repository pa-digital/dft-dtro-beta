using DfT.DTRO.Models.DataBase;
using DfT.DTRO.Models.Errors;
using DfT.DTRO.Models.RuleTemplate;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.Services;
using DfT.DTRO.Services.Mapping;

namespace Dft.DTRO.Tests.CodeiumTests.Rules.Service;

public class RuleTemplateServiceTests
{
    private Mock<IRuleTemplateDal> _mockRuleTemplateDal;
    private Mock<IDtroDal> _mockDtroDal;
    private Mock<IRuleTemplateMappingService> _mockRuleTemplateMappingService;
    private RuleTemplateService _ruleTemplateService;

    public RuleTemplateServiceTests()
    {
        _mockRuleTemplateDal = new Mock<IRuleTemplateDal>();
        _mockDtroDal = new Mock<IDtroDal>();
        _mockRuleTemplateMappingService = new Mock<IRuleTemplateMappingService>();
        _ruleTemplateService = new RuleTemplateService(_mockRuleTemplateDal.Object, _mockDtroDal.Object, _mockRuleTemplateMappingService.Object);
    }

    [Fact]
    public async Task RuleTemplateExistsAsync_ReturnsTrue()
    {
        var schemaVersion = new SchemaVersion("1.0.0");
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(true);
        var result = await _ruleTemplateService.RuleTemplateExistsAsync(schemaVersion);
        Assert.True(result);
    }

    [Fact]
    public async Task RuleTemplateExistsAsync_ReturnsFalse()
    {
        var schemaVersion = new SchemaVersion("1.0.0");
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(false);
        var result = await _ruleTemplateService.RuleTemplateExistsAsync(schemaVersion);
        Assert.False(result);
    }

    [Fact]
    public async Task GetRuleTemplateByIdAsync_Found()
    {
        var id = Guid.NewGuid();
        var ruleTemplate = new RuleTemplate();
        var ruleTemplateResponse = new RuleTemplateResponse();
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsByIdAsync(id)).ReturnsAsync(true);
        _mockRuleTemplateDal.Setup(x => x.GetRuleTemplateByIdAsync(id)).ReturnsAsync(ruleTemplate);
        _mockRuleTemplateMappingService.Setup(x => x.MapToRuleTemplateResponse(ruleTemplate)).Returns(ruleTemplateResponse);
        var result = await _ruleTemplateService.GetRuleTemplateByIdAsync(id);
        Assert.Equal(ruleTemplateResponse, result);
    }

    [Fact]
    public async Task GetRuleTemplateAsync_Found()
    {
        var schemaVersion = new SchemaVersion("1.0.0");
        var ruleTemplate = new RuleTemplate();
        var ruleTemplateResponse = new RuleTemplateResponse();
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(true);
        _mockRuleTemplateDal.Setup(x => x.GetRuleTemplateAsync(schemaVersion)).ReturnsAsync(ruleTemplate);
        _mockRuleTemplateMappingService.Setup(x => x.MapToRuleTemplateResponse(ruleTemplate)).Returns(ruleTemplateResponse);
        var result = await _ruleTemplateService.GetRuleTemplateAsync(schemaVersion);
        Assert.Equal(ruleTemplateResponse, result);
    }

    [Fact]
    public async Task GetRuleTemplatesAsync_ReturnsList()
    {
        var ruleTemplates = new List<RuleTemplate>();
        var ruleTemplateResponses = new List<RuleTemplateResponse>();
        _mockRuleTemplateDal.Setup(x => x.GetRuleTemplatesAsync()).ReturnsAsync(ruleTemplates);
        _mockRuleTemplateMappingService.Setup(x => x.MapToRuleTemplateResponse(ruleTemplates)).Returns(ruleTemplateResponses);
        var result = await _ruleTemplateService.GetRuleTemplatesAsync();
        Assert.Equal(ruleTemplateResponses, result);
    }

    [Fact]
    public async Task GetRuleTemplatesVersionsAsync_ReturnsList()
    {
        var ruleTemplateOverviews = new List<RuleTemplateOverview>();
        _mockRuleTemplateDal.Setup(x => x.GetRuleTemplatesVersionsAsync()).ReturnsAsync(ruleTemplateOverviews);
        var result = await _ruleTemplateService.GetRuleTemplatesVersionsAsync();
        Assert.Equal(ruleTemplateOverviews, result);
    }

    [Fact]
    public async Task SaveRuleTemplateAsJsonAsync_Success()
    {
        var version = "1.0.0";
        var schemaVersion = new SchemaVersion("1.0.0");
        var rule = "{}";
        var correlationId = "test-correlation-id";
        var guidResponse = new GuidResponse();
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(false);
        _mockRuleTemplateDal.Setup(x => x.SaveRuleTemplateAsJsonAsync(version, rule, correlationId)).ReturnsAsync(guidResponse);
        var result = await _ruleTemplateService.SaveRuleTemplateAsJsonAsync(version, rule, correlationId);
        Assert.Equal(guidResponse, result);
    }

    [Fact]
    public async Task UpdateRuleTemplateAsJsonAsync_Success()
    {
        var version = "1.0.0";
        var rule = "{}";
        var correlationId = "test-correlation-id";
        var guidResponse = new GuidResponse();
        var schemaVersion = new SchemaVersion("1.0.0");

        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(true);
        _mockDtroDal.Setup(x => x.DtroCountForSchemaAsync(schemaVersion)).ReturnsAsync(0);
        _mockRuleTemplateDal.Setup(x => x.UpdateRuleTemplateAsJsonAsync(version, rule, correlationId)).ReturnsAsync(guidResponse);
        var result = await _ruleTemplateService.UpdateRuleTemplateAsJsonAsync(version, rule, correlationId);
        Assert.Equal(guidResponse, result);
    }

    [Fact]
    public async Task UpdateRuleTemplateAsJsonAsync_NotFound()
    {
        var version = "1.0.0";
        var schemaVersion = new SchemaVersion("1.0.0");
        var rule = "{}";
        var correlationId = "test-correlation-id";
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(false);
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _ruleTemplateService.UpdateRuleTemplateAsJsonAsync(version, rule, correlationId));
    }

    [Fact]
    public async Task UpdateRuleTemplateAsJsonAsync_InUseByExistingDtros()
    {
        var version = "1.0.0";
        var schemaVersion = new SchemaVersion("1.0.0");
        var rule = "{}";
        var correlationId = "test-correlation-id";
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(true);
        _mockDtroDal.Setup(x => x.DtroCountForSchemaAsync(schemaVersion)).ReturnsAsync(1);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _ruleTemplateService.UpdateRuleTemplateAsJsonAsync(version, rule, correlationId));
    }

    [Fact]
    public async Task GetRuleTemplateAsync_NotFound()
    {
        var schemaVersion = new SchemaVersion("1.0.0");
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(false);
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _ruleTemplateService.GetRuleTemplateAsync(schemaVersion));
    }

    [Fact]
    public async Task SaveRuleTemplateAsJsonAsync_AlreadyExists()
    {
        var version = "1.0.0";
        var rule = "{}";
        var correlationId = "test-correlation-id";
        var schemaVersion = new SchemaVersion("1.0.0");
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsAsync(schemaVersion)).ReturnsAsync(true);
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _ruleTemplateService.SaveRuleTemplateAsJsonAsync(version, rule, correlationId));
    }

    [Fact]
    public async Task GetRuleTemplateByIdAsync_NotFound()
    {
        var id = Guid.NewGuid();
        _mockRuleTemplateDal.Setup(x => x.RuleTemplateExistsByIdAsync(id)).ReturnsAsync(false);
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _ruleTemplateService.GetRuleTemplateByIdAsync(id));
    }
}
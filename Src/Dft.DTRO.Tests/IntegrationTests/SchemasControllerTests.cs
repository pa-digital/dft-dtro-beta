using System.Dynamic;
using System.Net;
using DfT.DTRO;
using DfT.DTRO.Models.SchemaTemplate;
using DfT.DTRO.Models.SharedResponse;
using DfT.DTRO.Services;
using DfT.DTRO.Services.Mapping;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dft.DTRO.Tests.IntegrationTests;

public class SchemasControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    private const string ValidSchemaJsonPath = "./SchemaJsonExamples/example-3.1.1.json";

    private const string InvalidSchemaJsonPath = "./SchemaJsonExamples/invalid-schema.json";

    private readonly Mock<ISchemaTemplateService> _mockSchemaTemplateService;
    private readonly SchemaTemplateMappingService _schemaTemplateMappingService;

    public SchemasControllerTests(WebApplicationFactory<Program> factory)
    {
        _mockSchemaTemplateService = new Mock<ISchemaTemplateService>(MockBehavior.Strict);
        _schemaTemplateMappingService = new SchemaTemplateMappingService();
        _factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(_mockSchemaTemplateService.Object);
        }));
    }

    [Fact]
    public async Task GetSchemasVersions_ReturnsVersions()
    {

        var schema_template_overview = new List<SchemaTemplateOverview>
        {
            new SchemaTemplateOverview
            {
                SchemaVersion = "3.1.1",
                IsActive = false
            },
            new SchemaTemplateOverview
            {
                SchemaVersion = "3.1.2",
                IsActive = true
            },
            new SchemaTemplateOverview
            {
                SchemaVersion = "3.2.0",
                IsActive = true
            }
        };

        _mockSchemaTemplateService.Setup(mock => mock.GetSchemaTemplatesVersionsAsync())
            .ReturnsAsync(schema_template_overview);

        HttpClient client = _factory.CreateClient();
        HttpResponseMessage response = await client.GetAsync("/v1/schemas/versions");

        response.EnsureSuccessStatusCode();
        dynamic? data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        var schemas = (data as JArray)?.ToObject<SchemaTemplateOverview[]>();
        Assert.NotNull(schemas);
        Assert.NotEmpty(schemas!);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }

    [Fact]
    public async Task GetSchemasVersions_ReturnsEmpty()
    {

        _mockSchemaTemplateService.Setup(mock => mock.GetSchemaTemplatesVersionsAsync())
            .Returns(Task.FromResult(new List<SchemaTemplateOverview>()));

        HttpClient client = _factory.CreateClient();
        HttpResponseMessage response = await client.GetAsync("/v1/schemas/versions");

        response.EnsureSuccessStatusCode();
        dynamic? data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        var schemas = (data as JArray)?.ToObject<SchemaTemplateOverview[]>();
        Assert.Empty(schemas);
    }

    [Fact]
    public async Task GetSchemaTemplates_ReturnsSchemas()
    {
        var schemaTemplates = new List<SchemaTemplateResponse>
        {
            new SchemaTemplateResponse
            {
                SchemaVersion = new SchemaVersion("3.1.1"),
                Template = new ExpandoObject(),
                IsActive = false
            },
            new SchemaTemplateResponse
            {
                SchemaVersion = new SchemaVersion("3.1.2"),
                Template = new ExpandoObject(),
                IsActive = true
            },
            new SchemaTemplateResponse
            {
                SchemaVersion = new SchemaVersion("3.2.0"),
                Template = new ExpandoObject(),
                IsActive = true
            }
        };

        _mockSchemaTemplateService.Setup(mock => mock.GetSchemaTemplatesAsync())
            .Returns(Task.FromResult(schemaTemplates));

        HttpClient client = _factory.CreateClient();
        HttpResponseMessage response = await client.GetAsync("/v1/schemas");

        response.EnsureSuccessStatusCode();
        dynamic? data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        var schemas = (data as JArray)?.ToObject<SchemaTemplateResponse[]>();
        Assert.NotNull(schemas);
        Assert.NotEmpty(schemas!);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetSchemas_ReturnsNoSchemas()
    {
        _mockSchemaTemplateService.Setup(mock => mock.GetSchemaTemplatesAsync())
            .ReturnsAsync(new List<SchemaTemplateResponse>());

        HttpClient client = _factory.CreateClient();
        HttpResponseMessage response = await client.GetAsync("/v1/schemas");

        response.EnsureSuccessStatusCode();
        dynamic? data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        var schemas = (data as JArray)?.ToObject<SchemaTemplateResponse[]>();
        Assert.Empty(schemas);
    }

    [Theory]
    [InlineData("0.0.0")]
    public async Task Get_SchemaVersion_SchemaDoesNotExist(string schemaVersion)
    {

        _mockSchemaTemplateService.Setup(mock => mock.GetSchemaTemplateAsync(It.IsAny<SchemaVersion>()))
            .Returns(Task.FromResult<SchemaTemplateResponse?>(null));

        HttpClient client = _factory.CreateClient();
        HttpResponseMessage response = await client.GetAsync($"/v1/schemas/{schemaVersion}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

    }

    [Theory]
    [InlineData("3.1.2")]
    public async Task Patch_ActivateSchema_ValidVersion_ReturnsOk(string schemaVersion)
    {
        _mockSchemaTemplateService.Setup(mock => mock.SchemaTemplateExistsAsync(It.IsAny<SchemaVersion>()))
            .Returns(Task.FromResult(true));

        _mockSchemaTemplateService.Setup(mock => mock.ActivateSchemaTemplateAsync(It.IsAny<SchemaVersion>()))
            .Returns(Task.FromResult(new GuidResponse()));

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PatchAsync($"/v1/schemas/activate/{schemaVersion}", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("3.1.2")]
    public async Task Patch_DeActivateSchema_ValidVersion_ReturnsOk(string schemaVersion)
    {
        _mockSchemaTemplateService.Setup(mock => mock.SchemaTemplateExistsAsync(It.IsAny<SchemaVersion>()))
            .Returns(Task.FromResult(true));

        _mockSchemaTemplateService.Setup(mock => mock.DeActivateSchemaTemplateAsync(It.IsAny<SchemaVersion>()))
            .Returns(Task.FromResult(new GuidResponse()));

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PatchAsync($"/v1/schemas/deactivate/{schemaVersion}", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

}
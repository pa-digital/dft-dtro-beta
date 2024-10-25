namespace Dft.DTRO.Tests.ControllerTests;

[ExcludeFromCodeCoverage]
//TODO: The entire class needs refactoring
public class SchemasControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    //private readonly WebApplicationFactory<Program> _factory;

    //private readonly Mock<ISchemaTemplateService> _mockSchemaTemplateService;
    //private readonly Mock<IDtroUserDal> _mockDtroUserDal;
    //private readonly int? _taForTest = 1585;
    //private readonly Guid _xAppIdGuidForTest = Guid.NewGuid();
    //public SchemasControllerTests(WebApplicationFactory<Program> factory)
    //{
    //    _mockSchemaTemplateService = new Mock<ISchemaTemplateService>(MockBehavior.Strict);
    //    _mockDtroUserDal = new Mock<IDtroUserDal>(MockBehavior.Strict);

    //    _mockDtroUserDal.Setup(m => m.GetDtroUserByTraIdAsync(It.IsAny<int>()))
    //       .ReturnsAsync(new DtroUser() { Id = new Guid(), TraId = (int)_taForTest, UserGroup = (int)UserGroup.Tra, xAppId = _xAppIdGuidForTest, Name = "test" });
    //    _mockDtroUserDal.Setup(m => m.GetDtroUserOnAppIdAsync(It.IsAny<Guid>()))
    //        .ReturnsAsync(new DtroUser() { Id = new Guid(), TraId = (int)_taForTest, UserGroup = (int)UserGroup.Tra, xAppId = _xAppIdGuidForTest, Name = "test" });


    //    _mockDtroUserDal.Setup(m => m.AnyAdminUserExistsAsync())
    //        .ReturnsAsync(false);


    //    _factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
    //    {
    //        services.AddSingleton(_mockSchemaTemplateService.Object);
    //        services.AddSingleton(_mockDtroUserDal.Object);
    //    }));
    //}

    //[Fact]
    //public async Task GetSchemasVersions_ReturnsVersions()
    //{

    //    List<SchemaTemplateOverview> schemaTemplateOverviews = new()
    //    {
    //        new SchemaTemplateOverview
    //        {
    //            SchemaVersion = "3.1.1",
    //            IsActive = false
    //        },
    //        new SchemaTemplateOverview
    //        {
    //            SchemaVersion = "3.1.2",
    //            IsActive = true
    //        },
    //        new SchemaTemplateOverview
    //        {
    //            SchemaVersion = "3.2.0",
    //            IsActive = true
    //        }
    //    };

    //    _mockSchemaTemplateService.Setup(mock => mock.GetSchemaTemplatesVersionsAsync())
    //        .ReturnsAsync(schemaTemplateOverviews);

    //    HttpClient client = _factory.CreateClient();
    //    client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());
    //    client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

    //    HttpResponseMessage response = await client.GetAsync("/schemas/versions");

    //    response.EnsureSuccessStatusCode();
    //    dynamic? data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
    //    SchemaTemplateOverview[]? schemas = (data as JArray)?.ToObject<SchemaTemplateOverview[]>();
    //    Assert.NotNull(schemas);
    //    Assert.NotEmpty(schemas);
    //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    //}

    //[Fact]
    //public async Task GetSchemasVersions_ReturnsEmpty()
    //{

    //    _mockSchemaTemplateService.Setup(mock => mock.GetSchemaTemplatesVersionsAsync())
    //        .Returns(Task.FromResult(new List<SchemaTemplateOverview>()));

    //    HttpClient client = _factory.CreateClient();
    //    client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());
    //    client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

    //    HttpResponseMessage response = await client.GetAsync("/schemas/versions");

    //    response.EnsureSuccessStatusCode();
    //    dynamic? data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
    //    SchemaTemplateOverview[]? schemas = (data as JArray)?.ToObject<SchemaTemplateOverview[]>();
    //    Assert.Empty(schemas);
    //}

    //[Fact]
    //public async Task GetSchemaTemplates_ReturnsSchemas()
    //{
    //    List<SchemaTemplateResponse> schemaTemplates = new()
    //    {
    //        new SchemaTemplateResponse
    //        {
    //            SchemaVersion = new SchemaVersion("3.1.1"),
    //            Template = new ExpandoObject(),
    //            IsActive = false
    //        },
    //        new SchemaTemplateResponse
    //        {
    //            SchemaVersion = new SchemaVersion("3.1.2"),
    //            Template = new ExpandoObject(),
    //            IsActive = true
    //        },
    //        new SchemaTemplateResponse
    //        {
    //            SchemaVersion = new SchemaVersion("3.2.0"),
    //            Template = new ExpandoObject(),
    //            IsActive = true
    //        }
    //    };

    //    _mockSchemaTemplateService.Setup(mock => mock.GetSchemaTemplatesAsync())
    //        .Returns(Task.FromResult(schemaTemplates));

    //    HttpClient client = _factory.CreateClient();
    //    client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());
    //    client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

    //    HttpResponseMessage response = await client.GetAsync("/schemas");

    //    response.EnsureSuccessStatusCode();
    //    dynamic? data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
    //    SchemaTemplateResponse[]? schemas = (data as JArray)?.ToObject<SchemaTemplateResponse[]>();
    //    Assert.NotNull(schemas);
    //    Assert.NotEmpty(schemas);
    //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //}

    //[Fact]
    //public async Task GetSchemas_ReturnsNoSchemas()
    //{
    //    _mockSchemaTemplateService.Setup(mock => mock.GetSchemaTemplatesAsync())
    //        .ReturnsAsync(new List<SchemaTemplateResponse>());

    //    HttpClient client = _factory.CreateClient();
    //    client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());
    //    client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

    //    HttpResponseMessage response = await client.GetAsync("/schemas");

    //    response.EnsureSuccessStatusCode();
    //    dynamic? data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
    //    SchemaTemplateResponse[]? schemas = (data as JArray)?.ToObject<SchemaTemplateResponse[]>();
    //    Assert.Empty(schemas);
    //}

    //[Theory]
    //[InlineData("0.0.0")]
    //public async Task Get_SchemaVersion_SchemaDoesNotExist(string schemaVersion)
    //{

    //    _mockSchemaTemplateService.Setup(mock => mock.GetSchemaTemplateAsync(It.IsAny<SchemaVersion>()))
    //        .Returns(Task.FromResult<SchemaTemplateResponse?>(null));

    //    HttpClient client = _factory.CreateClient();
    //    client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());
    //    client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

    //    HttpResponseMessage response = await client.GetAsync($"/schemas/{schemaVersion}");

    //    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

    //}

    //[Theory]
    //[InlineData("3.1.2")]
    //public async Task Patch_ActivateSchema_ValidVersion_ReturnsOk(string schemaVersion)
    //{
    //    _mockSchemaTemplateService.Setup(mock => mock.SchemaTemplateExistsAsync(It.IsAny<SchemaVersion>()))
    //        .Returns(Task.FromResult(true));

    //    _mockSchemaTemplateService.Setup(mock => mock.ActivateSchemaTemplateAsync(It.IsAny<SchemaVersion>()))
    //        .Returns(Task.FromResult(new GuidResponse()));

    //    HttpClient client = _factory.CreateClient();
    //    client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());
    //    client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

    //    HttpResponseMessage response = await client.PatchAsync($"/schemas/activate/{schemaVersion}", null);

    //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //}

    //[Theory]
    //[InlineData("3.1.2")]
    //public async Task Patch_DeActivateSchema_ValidVersion_ReturnsOk(string schemaVersion)
    //{
    //    _mockSchemaTemplateService.Setup(mock => mock.SchemaTemplateExistsAsync(It.IsAny<SchemaVersion>()))
    //        .Returns(Task.FromResult(true));

    //    _mockSchemaTemplateService.Setup(mock => mock.DeActivateSchemaTemplateAsync(It.IsAny<SchemaVersion>()))
    //        .Returns(Task.FromResult(new GuidResponse()));

    //    HttpClient client = _factory.CreateClient();
    //    client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());
    //    client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

    //    HttpResponseMessage response = await client.PatchAsync($"/schemas/deactivate/{schemaVersion}", null);

    //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //}

}
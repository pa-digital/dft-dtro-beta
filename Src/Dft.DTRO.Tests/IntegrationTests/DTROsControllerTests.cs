using Newtonsoft.Json;
namespace Dft.DTRO.Tests.IntegrationTests;

[ExcludeFromCodeCoverage]
// Custom Middleware for testing
public class TestFeatureGateMiddleware
{
    private readonly RequestDelegate _next;

    public TestFeatureGateMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
    }
}


[ExcludeFromCodeCoverage]
public class DTROsControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private const string NewDirectedLinear = "../../../TestFiles/D-TROs/3.2.3/temporary TRO - new-DirectedLinear.json";
    private const string NewLinearGeometry = "../../../TestFiles/D-TROs/3.2.3/temporary TRO - new-LinearGeometry.json";
    private const string NewPointGeometry = "../../../TestFiles/D-TROs/3.2.3/temporary TRO - new-PointGeometry.json";
    private const string NewPolygon = "../../../TestFiles/D-TROs/3.2.3/temporary TRO - new-Polygon.json";

    private static readonly string[] ValidDtroHistories =
    {
        "../../../TestFiles/D-TROs/3.2.3/temporary TRO - new-Polygon.json",
        "../../../TestFiles/D-TROs/3.2.3/temporary TRO - fullAmendment.json",
        "../../../TestFiles/D-TROs/3.2.3/temporary TRO - fullRevoke.json"
    };

    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IDtroService> _mockDtroService;
    private readonly DtroMappingService _dtroMappingService;
    private readonly Mock<IDtroUserDal> _mockDtroUserDal;
    private readonly Mock<IXappIdMapperService> _xappIdMapperServiceMock;
    private readonly int _return_taForTest = 1585;
    private readonly Guid _xAppIdGuidForTest = Guid.NewGuid();

    public DTROsControllerTests(WebApplicationFactory<Program> factory)
    {
        ConfigurationBuilder configurationBuilder = new();
        IConfigurationRoot? configuration = configurationBuilder.Build();
        _xappIdMapperServiceMock = new Mock<IXappIdMapperService>();
        _dtroMappingService = new DtroMappingService(configuration, new BoundingBoxService());
        _mockDtroService = new Mock<IDtroService>(MockBehavior.Strict);
        _mockDtroUserDal = new Mock<IDtroUserDal>(MockBehavior.Strict);

        _xappIdMapperServiceMock.Setup(x => x.GetXappId(It.IsAny<HttpContext>())).ReturnsAsync(_xAppIdGuidForTest);

        _mockDtroUserDal.Setup(m => m.GetDtroUserByTraIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new DtroUser() { Id = new Guid(), TraId = _return_taForTest, UserGroup = (int)UserGroup.Tra, xAppId = _xAppIdGuidForTest, Name = "test" });

        _mockDtroUserDal.Setup(m => m.AnyAdminUserExistsAsync())
         .ReturnsAsync(false);

        _mockDtroUserDal.Setup(m => m.GetDtroUserOnAppIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new DtroUser() { Id = new Guid(), TraId = (int)_return_taForTest, UserGroup = (int)UserGroup.Tra, xAppId = _xAppIdGuidForTest, Name = "test" });

        _factory = factory
           .WithWebHostBuilder(builder => builder
               .ConfigureTestServices(services =>
               {
                   services.AddSingleton(_mockDtroService.Object);
                   services.AddSingleton(_mockDtroUserDal.Object);
               }));
    }

    [Theory]
    [InlineData(NewDirectedLinear, "3.3.3")]
    [InlineData(NewLinearGeometry, "3.2.3")]
    [InlineData(NewPointGeometry, "3.3.3")]
    [InlineData(NewPolygon, "3.2.3")]
    public async Task Post_DtroIsValid_CreatesDtroAndReturnsDtroId(string dtroPath, string version)
    {
        _mockDtroService.Setup(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), _xAppIdGuidForTest))
            .ReturnsAsync(new GuidResponse { Id = Guid.NewGuid() });

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        StringContent payload = await Utils.CreateDtroJsonPayload(dtroPath, version);

        HttpResponseMessage response = await client.PostAsync("/dtros/createFromBody", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        GuidResponse? data = JsonConvert.DeserializeObject<GuidResponse>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(data);
        Assert.IsType<Guid>(data.Id);

        _mockDtroService.Verify(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), _xAppIdGuidForTest), Times.Once);
    }

    [Fact]
    public async Task Post_SchemaDoesNotExist_ReturnsNotFoundError()
    {
        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        _mockDtroService.Setup(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), _xAppIdGuidForTest))
            .Throws((new NotFoundException()));

        StringContent payload = await Utils.CreateDtroJsonPayload(NewPolygon, "0.0.0");
        HttpResponseMessage response = await client.PostAsync("/dtros", payload);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_DtroIsInvalid_ReturnsValidationError()
    {
        _mockDtroService.Setup(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), _xAppIdGuidForTest))
            .Throws((new DtroValidationException()));

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        StringContent payload = await Utils.CreateDtroJsonPayload(NewLinearGeometry, "3.1.1", false);
        HttpResponseMessage response = await client.PostAsync("/dtros/createFromBody", payload);

        string readAsStringAsync = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("An error occurred: Value cannot be null. (Parameter 'source')", readAsStringAsync);
    }

    [Fact]
    public async Task Put_DtroIsValid_UpdatesDtroAndReturnsDtroId()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _xAppIdGuidForTest)).ReturnsAsync(new GuidResponse());

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        StringContent payload = await Utils.CreateDtroJsonPayload(NewPolygon, "3.2.3");
        HttpResponseMessage response = await client.PutAsync($"/dtros/updateFromBody/{dtroId}", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        string stringAsync = await response.Content.ReadAsStringAsync();
        GuidResponse? data = JsonConvert.DeserializeObject<GuidResponse>(stringAsync);
        Assert.NotNull(data);
        Assert.IsType<Guid>(data.Id);
        _mockDtroService.Verify(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _xAppIdGuidForTest), Times.Once);
    }

    [Fact]
    public async Task Put_DtroDoesNotExist_ReturnsNotFoundError()
    {
        Guid notExistingDtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _xAppIdGuidForTest)).Throws(new NotFoundException());


        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        StringContent payload = await Utils.CreateDtroJsonPayload(NewDirectedLinear, "3.1.1");
        HttpResponseMessage response = await client.PutAsync($"/dtros/updateFromBody/{notExistingDtroId}", payload);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Put_DtroIsInvalid_ReturnsBAdRequest()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _xAppIdGuidForTest)).Throws(new DtroValidationException());

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        StringContent payload = await Utils.CreateDtroJsonPayload(NewLinearGeometry, "3.1.1", false);
        HttpResponseMessage response = await client.PutAsync($"/dtros/updateFromBody/{dtroId}", payload);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task Get_DtroExists_ReturnsDtro()
    {
        var sampleDtro = await Utils.CreateDtroObject(NewLinearGeometry);
        var sampleDtroResponse = _dtroMappingService.MapToDtroResponse(sampleDtro);

        _mockDtroService.Setup(mock => mock.GetDtroByIdAsync
            (It.Is(sampleDtro.Id, EqualityComparer<Guid>.Default))).Returns(Task.FromResult(sampleDtroResponse));

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        HttpResponseMessage response = await client.GetAsync($"/dtros/{sampleDtro.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_DtroDoesNotExist_ReturnsNotFoundError()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.GetDtroByIdAsync(It.Is(dtroId, EqualityComparer<Guid>.Default)))
            .Throws(new NotFoundException());

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        HttpResponseMessage response = await client.GetAsync($"/dtros/{dtroId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_SourceHistory_ReturnsListOfHistoricSources()
    {
        var sourceResponses = Utils.CreateResponseDtroHistoryObject(ValidDtroHistories);


        _mockDtroService.Setup(it => it.GetDtroSourceHistoryAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult(sourceResponses));

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        HttpResponseMessage response = await client.GetAsync($"/dtros/sourceHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_SourceHistory_ReturnsNotFoundError()
    {
        _mockDtroService.Setup(it => it.GetDtroSourceHistoryAsync(It.IsAny<Guid>()))
            .Throws(new NotFoundException());

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        HttpResponseMessage response = await client.GetAsync("/dtros/sourceHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_SourceHistory_ReturnsBadRequestError()
    {
        _mockDtroService.Setup(it => it.GetDtroSourceHistoryAsync(It.IsAny<Guid>()))
            .Throws(new Exception());

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        HttpResponseMessage response = await client.GetAsync("/dtros/sourceHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task Get_ProvisionHistory_ReturnsListOfHistoricProvisions()
    {
        List<DtroHistoryProvisionResponse> provisionResponses = Utils.CreateResponseDtroProvisionHistory(ValidDtroHistories);

        _mockDtroService.Setup(it => it.GetDtroProvisionHistoryAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult(provisionResponses));

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        HttpResponseMessage response = await client.GetAsync("/dtros/provisionHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_ProvisionHistory_ReturnsNotFoundError()
    {
        _mockDtroService.Setup(it => it.GetDtroProvisionHistoryAsync(It.IsAny<Guid>()))
            .Throws(new NotFoundException());

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        HttpResponseMessage response = await client.GetAsync("/dtros/provisionHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(Skip = "Trying to connect to the database")]
    public async Task Get_ProvisionHistory_ReturnsBadRequestError()
    {
        _mockDtroService.Setup(it => it.GetDtroProvisionHistoryAsync(It.IsAny<Guid>()))
            .Throws(new Exception());

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        HttpResponseMessage response = await client.GetAsync("/dtros/provisionHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}
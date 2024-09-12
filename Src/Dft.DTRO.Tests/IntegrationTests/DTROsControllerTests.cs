using DfT.DTRO.Migrations;
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
    private const string ValidDtroJsonPath = "../../../../../examples/D-TROs/3.1.1/proper-data.json";
    private const string ValidComplexDtroJsonPath = "../../../../../examples/D-TROs/3.1.2/3.1.2-valid-complex-dtro.json";
    private const string ValidComplexDtroJsonPathV320 = "../../../../../examples/D-TROs/3.2.0/valid-new-x.json";
    private const string InvalidDtroJsonPath = "../../../../../examples/D-TROs/3.1.1/provision-empty.json";
    private static readonly string[] ValidDtroHistories =
    {
        "../../../../../examples/D-TROs/3.2.0/valid-new-x.json",
        "../../../../../examples/D-TROs/3.2.0/valid-fullAmendment.json",
        "../../../../../examples/D-TROs/3.2.0/valid-fullRevoke.json"
    };

    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IDtroService> _mockDtroService;
    private readonly DtroMappingService _dtroMappingService;
    private readonly Mock<IDtroUserDal> _mockDtroUserDal;
    private readonly int _return_taForTest = 1585;
    private readonly Guid _xAppIdGuidForTest = Guid.NewGuid();

    public DTROsControllerTests(WebApplicationFactory<Program> factory)
    {
        ConfigurationBuilder configurationBuilder = new();
        IConfigurationRoot? configuration = configurationBuilder.Build();

        _dtroMappingService = new DtroMappingService(configuration, new Proj4SpatialProjectionService());
        _mockDtroService = new Mock<IDtroService>(MockBehavior.Strict);
        _mockDtroUserDal = new Mock<IDtroUserDal>(MockBehavior.Strict);

        _mockDtroUserDal.Setup(m => m.GetDtroUserByTraIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new DtroUser() { Id = new Guid(), TraId = _return_taForTest, UserGroup = (int) UserGroup.Tra , xAppId = _xAppIdGuidForTest, Name = "test" });
       
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
    [InlineData(ValidComplexDtroJsonPath, "3.1.2")]
    [InlineData(ValidComplexDtroJsonPathV320, "3.2.0")]
    public async Task Post_DtroIsValid_CreatesDtroAndReturnsDtroId(string dtroPath, string version)
    {
        _mockDtroService.Setup(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(),_xAppIdGuidForTest))
            .ReturnsAsync(new GuidResponse { Id = Guid.NewGuid() });

        HttpClient client = _factory.CreateClient();
        
        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        var payload = await Utils.CreateDtroJsonPayload(dtroPath, version);

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

        StringContent payload = await Utils.CreateDtroJsonPayload(ValidDtroJsonPath, "0.0.0");
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

        StringContent payload = await Utils.CreateDtroJsonPayload(InvalidDtroJsonPath, "3.1.1", false);
        HttpResponseMessage response = await client.PostAsync("/dtros/createFromBody", payload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        DtroValidationExceptionResponse? data = JsonConvert.DeserializeObject<DtroValidationExceptionResponse>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(data);
    }

    [Fact]
    public async Task Put_DtroIsValid_UpdatesDtroAndReturnsDtroId()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _xAppIdGuidForTest)).ReturnsAsync(new GuidResponse());

        HttpClient client = _factory.CreateClient();
        
        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        StringContent payload = await Utils.CreateDtroJsonPayload(ValidDtroJsonPath, "3.1.1");
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

        StringContent payload = await Utils.CreateDtroJsonPayload(ValidDtroJsonPath, "3.1.1");
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

        StringContent payload = await Utils.CreateDtroJsonPayload(InvalidDtroJsonPath, "3.1.1", false);
        HttpResponseMessage response = await client.PutAsync($"/dtros/updateFromBody/{dtroId}", payload);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_DtroExists_ReturnsDtro()
    {
        var sampleDtro = await Utils.CreateDtroObject(ValidDtroJsonPath);
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

    [Fact]
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
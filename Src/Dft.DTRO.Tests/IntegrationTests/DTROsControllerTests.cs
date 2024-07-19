using Newtonsoft.Json;

namespace Dft.DTRO.Tests.IntegrationTests;

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
    private readonly int? _taForTest = 1585;

    public DTROsControllerTests(WebApplicationFactory<Program> factory)
    {
        ConfigurationBuilder configurationBuilder = new();
        IConfigurationRoot? configuration = configurationBuilder.Build();

        _dtroMappingService = new DtroMappingService(configuration, new Proj4SpatialProjectionService());
        _mockDtroService = new Mock<IDtroService>(MockBehavior.Strict);

        _factory = factory
            .WithWebHostBuilder(builder => builder
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(_mockDtroService.Object);
                }));
    }

    [Theory]
    [InlineData(ValidComplexDtroJsonPath, "3.1.2")]
    [InlineData(ValidComplexDtroJsonPathV320, "3.2.0")]
    public async Task Post_DtroIsValid_CreatesDtroAndReturnsDtroId(string dtroPath, string version)
    {
        _mockDtroService.Setup(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest))
            .ReturnsAsync(new GuidResponse { Id = Guid.NewGuid() });

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());

        var payload = await Utils.CreateDtroJsonPayload(dtroPath, version);

        HttpResponseMessage response = await client.PostAsync("/v1/dtros/createFromBody", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        GuidResponse? data = JsonConvert.DeserializeObject<GuidResponse>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(data);
        Assert.IsType<Guid>(data.Id);

        _mockDtroService.Verify(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest), Times.Once);
    }

    [Fact]
    public async Task Post_SchemaDoesNotExist_ReturnsNotFoundError()
    {
        HttpClient client = _factory.CreateClient();

        _mockDtroService.Setup(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest))
            .Throws((new NotFoundException()));

        StringContent payload = await Utils.CreateDtroJsonPayload(ValidDtroJsonPath, "0.0.0");
        HttpResponseMessage response = await client.PostAsync("/v1/dtros", payload);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_DtroIsInvalid_ReturnsValidationError()
    {
        _mockDtroService.Setup(mock => mock.SaveDtroAsJsonAsync(It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest))
            .Throws((new DtroValidationException()));
        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());

        StringContent payload = await Utils.CreateDtroJsonPayload(InvalidDtroJsonPath, "3.1.1", false);
        HttpResponseMessage response = await client.PostAsync("/v1/dtros/createFromBody", payload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ApiErrorResponse? data = JsonConvert.DeserializeObject<ApiErrorResponse>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(data);
        Assert.Equal("Dtro Validation Failure", data.Message);
    }

    [Fact]
    public async Task Put_DtroIsValid_UpdatesDtroAndReturnsDtroId()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest)).ReturnsAsync(new GuidResponse());
        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());

        StringContent payload = await Utils.CreateDtroJsonPayload(ValidDtroJsonPath, "3.1.1");
        HttpResponseMessage response = await client.PutAsync($"/v1/dtros/updateFromBody/{dtroId}", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        string stringAsync = await response.Content.ReadAsStringAsync();
        GuidResponse? data = JsonConvert.DeserializeObject<GuidResponse>(stringAsync);
        Assert.NotNull(data);
        Assert.IsType<Guid>(data.Id);
        _mockDtroService.Verify(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest), Times.Once);
    }

    [Fact]
    public async Task Put_DtroDoesNotExist_ReturnsNotFoundError()
    {
        Guid notExistingDtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest)).Throws(new NotFoundException());


        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());

        StringContent payload = await Utils.CreateDtroJsonPayload(ValidDtroJsonPath, "3.1.1");
        HttpResponseMessage response = await client.PutAsync($"/v1/dtros/updateFromBody/{notExistingDtroId}", payload);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Put_DtroIsInvalid_ReturnsBAdRequest()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.TryUpdateDtroAsJsonAsync
            (It.IsAny<Guid>(), It.IsAny<DtroSubmit>(), It.IsAny<string>(), _taForTest)).Throws(new DtroValidationException());

        HttpClient client = _factory.CreateClient();

        client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());

        StringContent payload = await Utils.CreateDtroJsonPayload(InvalidDtroJsonPath, "3.1.1", false);
        HttpResponseMessage response = await client.PutAsync($"/v1/dtros/updateFromBody/{dtroId}", payload);
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

        client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());

        HttpResponseMessage response = await client.GetAsync($"/v1/dtros/{sampleDtro.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_DtroDoesNotExist_ReturnsNotFoundError()
    {
        Guid dtroId = Guid.NewGuid();
        _mockDtroService.Setup(mock => mock.GetDtroByIdAsync(It.Is(dtroId, EqualityComparer<Guid>.Default)))
            .Throws(new NotFoundException());

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync($"/v1/dtros/{dtroId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_SourceHistory_ReturnsListOfHistoricSources()
    {
        List<DtroHistorySourceResponse> sourceResponses = Utils.CreateResponseDtroHistoryObject(ValidDtroHistories);


        _mockDtroService.Setup(it => it.GetDtroSourceHistoryAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult(sourceResponses));

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync($"/v1/dtros/sourceHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_SourceHistory_ReturnsNotFoundError()
    {
        _mockDtroService.Setup(it => it.GetDtroSourceHistoryAsync(It.IsAny<Guid>()))
            .Throws(new NotFoundException());

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/v1/dtros/sourceHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_SourceHistory_ReturnsBadRequestError()
    {
        _mockDtroService.Setup(it => it.GetDtroSourceHistoryAsync(It.IsAny<Guid>()))
            .Throws(new Exception());

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/v1/dtros/sourceHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task Get_ProvisionHistory_ReturnsListOfHistoricProvisions()
    {
        List<DtroHistoryProvisionResponse> provisionResponses = Utils.CreateResponseDtroProvisionHistory(ValidDtroHistories);

        _mockDtroService.Setup(it => it.GetDtroProvisionHistoryAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult(provisionResponses));

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/v1/dtros/provisionHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_ProvisionHistory_ReturnsNotFoundError()
    {
        _mockDtroService.Setup(it => it.GetDtroProvisionHistoryAsync(It.IsAny<Guid>()))
            .Throws(new NotFoundException());

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/v1/dtros/provisionHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_ProvisionHistory_ReturnsBadRequestError()
    {
        _mockDtroService.Setup(it => it.GetDtroProvisionHistoryAsync(It.IsAny<Guid>()))
            .Throws(new Exception());

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/v1/dtros/provisionHistory/C3B3BB0C-E3A6-47EF-83ED-4C48E56F9DD4");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}
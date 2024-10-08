using Newtonsoft.Json;

namespace Dft.DTRO.Tests.IntegrationTests;

[ExcludeFromCodeCoverage]
public class SearchControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private const string SampleDtroJsonPath = "../../../TestFiles/D-TROs/3.2.3/valid-noChange.json";

    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IDtroService> _mockStorageService;
    private readonly Guid _xAppId = Guid.NewGuid();
    public SearchControllerTests(WebApplicationFactory<Program> factory)
    {
        _mockStorageService = new Mock<IDtroService>(MockBehavior.Strict);
        Mock<IMetricsService> metricsMock = new();
        metricsMock.Setup(x => x.IncrementMetric(It.IsAny<MetricType>(), It.IsAny<Guid>())).ReturnsAsync(true);

        _factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(_mockStorageService.Object);
        }));
    }

    [Fact(Skip = "Test needs to be simplified")]
    public async Task Post_Search_NoDtroIsMatchingTheCriteria_ReturnsEmptyResult()
    {
        _mockStorageService.Setup(mock => mock.FindDtrosAsync(It.IsAny<DtroSearch>()))
            .Returns(Task.FromResult(
                new PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>(Array.Empty<DfT.DTRO.Models.DataBase.DTRO>(), 0)));
        HttpClient client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("x-app-id", _xAppId.ToString());

        DtroSearch search =
            new() { Queries = new[] { new SearchQuery { TraCreator = 1585 } }, Page = 1, PageSize = 10 };
        string payload = JsonConvert.SerializeObject(search);

        HttpResponseMessage response =
            await client.PostAsync("/search", new StringContent(payload, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        PaginatedResponse<DtroSearchResult>? data = JsonConvert.DeserializeObject<PaginatedResponse<DtroSearchResult>>(
            await response.Content.ReadAsStringAsync()
        );
        Assert.NotNull(data);
        Assert.Equal(1, data.Page);
        Assert.Equal(0, data.PageSize);
        Assert.Equal(0, data.TotalCount);
        Assert.Equal(0, data.Results.Count);
        Assert.Empty(data.Results);
    }

    [Fact(Skip = "Test needs to be simplified")]
    public async Task Post_Search_DtroMatchingTheCriteriaExists_ReturnsMatchingDtros()
    {
        DfT.DTRO.Models.DataBase.DTRO sampleDtro = await Utils.CreateDtroObject(SampleDtroJsonPath);

        _mockStorageService.Setup(mock => mock.FindDtrosAsync(It.IsAny<DtroSearch>()))
            .Returns(Task.FromResult(new PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>(new[] { sampleDtro }.ToList(), 1)));
        HttpClient client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("x-app-id", _xAppId.ToString());

        DtroSearch search =
            new() { Queries = new[] { new SearchQuery { TraCreator = 1585 } }, Page = 1, PageSize = 10 };
        string payload = JsonConvert.SerializeObject(search);

        HttpResponseMessage response =
            await client.PostAsync("/search", new StringContent(payload, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        PaginatedResponse<DtroSearchResult>? data = JsonConvert.DeserializeObject<PaginatedResponse<DtroSearchResult>>(
            await response.Content.ReadAsStringAsync()
        );
        Assert.NotNull(data);
        Assert.Equal(1, data.Page);
        Assert.Equal(1, data.PageSize);
        Assert.Equal(1, data.TotalCount);
        Assert.Equal(1, data.Results.Count);
        Assert.Equal(1585, data.Results.First().TrafficAuthorityOwnerId);
    }
}
using Newtonsoft.Json;

namespace Dft.DTRO.Tests.IntegrationTests;

[ExcludeFromCodeCoverage]
public class EventsControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private const string SampleDtroJsonPath = "../../TestFiles/D-TROs/3.1.1/proper-data.json";

    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IDtroService> _mockStorageService;
    private readonly Guid _xAppIdGuidForTest = Guid.NewGuid();

    public EventsControllerTests(WebApplicationFactory<Program> factory)
    {
        _mockStorageService = new Mock<IDtroService>(MockBehavior.Strict);

        Mock<IDtroUserDal> mockSwaCodeDal = new(MockBehavior.Strict);

        mockSwaCodeDal.Setup(m => m.GetDtroUserByTraIdAsync(It.IsAny<int>()))
           .ReturnsAsync(new DtroUser { Id = new Guid(), UserGroup = (int)UserGroup.Tra, xAppId = _xAppIdGuidForTest, Name = "test" });

        Mock<IMetricsService> metricsMock = new();
        metricsMock.Setup(x => x.IncrementMetric(It.IsAny<MetricType>(), It.IsAny<Guid>())).ReturnsAsync(true);

        _factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(_mockStorageService.Object);
            services.AddSingleton(metricsMock.Object);
        }));
    }

    [Fact(Skip = "Method too complicated")]
    public async Task Post_Events_NoDtroIsMatchingTheCriteria_ReturnsEmptyResult()
    {
        _mockStorageService.Setup(mock => mock.FindDtrosAsync(It.IsAny<DtroEventSearch>()))
            .ReturnsAsync(new DfT.DTRO.Models.DataBase.DTRO[] { }.ToList());
        HttpClient client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        DtroEventSearch searchCriteria = new() { Since = DateTime.Today, Page = 1, PageSize = 10 };
        string payload = JsonConvert.SerializeObject(searchCriteria);

        StringContent json = new(payload, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("/events", json);

        DtroEventSearchResult? data = JsonConvert.DeserializeObject<DtroEventSearchResult>(
            await response.Content.ReadAsStringAsync()
        );
        Assert.NotNull(data);
        Assert.Equal(1, data.Page);
        Assert.Equal(0, data.PageSize);
        Assert.Equal(0, data.TotalCount);
        Assert.Empty(data.Events);
        Assert.Empty(data.Events);
    }

    [Fact(Skip = "Method too complicated")]
    public async Task Post_Search_DtroMatchingTheCriteriaExists_ReturnsMatchingDtros()
    {
        DfT.DTRO.Models.DataBase.DTRO sampleDtro = await Utils.CreateDtroObject(SampleDtroJsonPath);

        _mockStorageService.Setup(mock => mock.FindDtrosAsync(It.IsAny<DtroEventSearch>()))
            .ReturnsAsync(new[] { sampleDtro }.ToList());
        HttpClient client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("x-app-id", _xAppIdGuidForTest.ToString());

        DtroEventSearch searchCriteria = new() { Since = DateTime.Today, Page = 1, PageSize = 10 };
        string payload = JsonConvert.SerializeObject(searchCriteria);

        StringContent json = new(payload, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync("/events", json);

        response.EnsureSuccessStatusCode();
        DtroEventSearchResult? data = JsonConvert.DeserializeObject<DtroEventSearchResult>(
            await response.Content.ReadAsStringAsync()
        );
        Assert.NotNull(data);
        Assert.Equal(1, data.Page);
        Assert.Equal(1, data.PageSize);
        Assert.Equal(1, data.TotalCount);
        Assert.Single(data.Events);
        Assert.Equal(1585, data.Events.First().TrafficAuthorityCreatorId);
    }
}
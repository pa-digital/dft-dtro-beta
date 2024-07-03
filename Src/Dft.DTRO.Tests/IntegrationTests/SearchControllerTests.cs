using DfT.DTRO;
using DfT.DTRO.Models.Filtering;
using DfT.DTRO.Models.Pagination;
using DfT.DTRO.Models.Search;
using DfT.DTRO.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Dft.DTRO.Tests.IntegrationTests;

[ExcludeFromCodeCoverage]
public class SearchControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private const string SampleDtroJsonPath = "./DtroJsonDataExamples/v3.1.1/proper-data.json";

    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IDtroService> _mockStorageService;
    private readonly Mock<IMetricsService> _metricsMock;
    private readonly int? _taForTest = 1585;
    public SearchControllerTests(WebApplicationFactory<Program> factory)
    {
        _mockStorageService = new Mock<IDtroService>(MockBehavior.Strict);
        _metricsMock = new Mock<IMetricsService>();
        _metricsMock.Setup(x => x.IncrementMetric(It.IsAny<MetricType>(), It.IsAny<int>())).ReturnsAsync(true);

        _factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(_mockStorageService.Object);
        }));
    }

    [Fact]
    public async Task Post_Search_NoDtroIsMatchingTheCriteria_ReturnsEmptyResult()
    {
        _mockStorageService.Setup(mock => mock.FindDtrosAsync(It.IsAny<DtroSearch>()))
            .Returns(Task.FromResult(
                new PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>(Array.Empty<DfT.DTRO.Models.DataBase.DTRO>(), 0)));
        HttpClient client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());

        DtroSearch search =
            new() { Queries = new[] { new SearchQuery { TraCreator = 1585 } }, Page = 1, PageSize = 10 };
        string payload = JsonConvert.SerializeObject(search);

        HttpResponseMessage response =
            await client.PostAsync("/v1/search", new StringContent(payload, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        PaginatedResponse<DtroSearchResult>? data = JsonConvert.DeserializeObject<PaginatedResponse<DtroSearchResult>>(
            await response.Content.ReadAsStringAsync()
        );
        Assert.NotNull(data);
        Assert.Equal(1, data!.Page);
        Assert.Equal(0, data.PageSize);
        Assert.Equal(0, data.TotalCount);
        Assert.Equal(0, data.Results.Count);
        Assert.Empty(data.Results);
    }

    [Fact]
    public async Task Post_Search_DtroMatchingTheCriteriaExists_ReturnsMatchingDtros()
    {
        DfT.DTRO.Models.DataBase.DTRO sampleDtro = await CreateDtroObject(SampleDtroJsonPath);

        _mockStorageService.Setup(mock => mock.FindDtrosAsync(It.IsAny<DtroSearch>()))
            .Returns(Task.FromResult(new PaginatedResult<DfT.DTRO.Models.DataBase.DTRO>(new[] { sampleDtro }.ToList(), 1)));
        HttpClient client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("ta", _taForTest.ToString());

        DtroSearch search =
            new() { Queries = new[] { new SearchQuery { TraCreator = 1585 } }, Page = 1, PageSize = 10 };
        string payload = JsonConvert.SerializeObject(search);

        HttpResponseMessage response =
            await client.PostAsync("/v1/search", new StringContent(payload, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        PaginatedResponse<DtroSearchResult>? data = JsonConvert.DeserializeObject<PaginatedResponse<DtroSearchResult>>(
            await response.Content.ReadAsStringAsync()
        );
        Assert.NotNull(data);
        Assert.Equal(1, data!.Page);
        Assert.Equal(1, data.PageSize);
        Assert.Equal(1, data.TotalCount);
        Assert.Equal(1, data.Results.Count);
        Assert.Equal(1585, data.Results.First().TrafficAuthorityOwnerId);
    }
}
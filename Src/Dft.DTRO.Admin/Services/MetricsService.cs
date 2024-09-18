namespace Dft.DTRO.Admin.Services;
public class MetricsService : IMetricsService
{
    private readonly HttpClient _client;
    private readonly IXappIdService _xappIdService;
    private readonly IErrHandlingService _errHandlingService;
    public MetricsService(IHttpClientFactory clientFactory, IXappIdService xappIdService, IErrHandlingService errHandlingService)
    {
        _client = clientFactory.CreateClient("ExternalApi");
        _xappIdService = xappIdService;
        _errHandlingService = errHandlingService;
    }

    public async Task<bool> HealthApi()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ConfigHelper.Version + "/healthApi");
            await _xappIdService.AddXAppIdHeader(request);

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var result = bool.Parse(content);

            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> HealthDatabase()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ConfigHelper.Version + "/healthDatabase");
            await _xappIdService.AddXAppIdHeader(request);

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var result = bool.Parse(content);

            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<MetricSummary> MetricsForDtroUser(MetricRequest metricRequest)
    {
        var jsonContent = JsonSerializer.Serialize(metricRequest);
        var param = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, ConfigHelper.Version + "/metricsForDtroUser")
        {
            Content = param
        };

        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var metricSummary = JsonSerializer.Deserialize<MetricSummary>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (metricSummary == null)
        {
            metricSummary = new MetricSummary();
        }
        return metricSummary;
    }

    public async Task<List<FullMetricSummary>> FullMetricsForDtroUser(MetricRequest metricRequest)
    {
        var jsonContent = JsonSerializer.Serialize(metricRequest);
        var param = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, ConfigHelper.Version + "/fullMetricsForDtroUser")
        {
            Content = param
        };

        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var metricSummary = JsonSerializer.Deserialize<List<FullMetricSummary>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (metricSummary == null)
        {
            return new List<FullMetricSummary>();
        }
        return metricSummary;
    }
}
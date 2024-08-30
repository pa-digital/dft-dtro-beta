using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Dft.DTRO.Admin.Services;
public class MetricsService : IMetricsService
{
    private readonly HttpClient _client;

    public MetricsService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("ExternalApi");
    }

    public async Task<bool> HealthApi()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/healthApi");
            Helper.AddXAppIdHeader(ref request);
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
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
            var request = new HttpRequestMessage(HttpMethod.Get, "/healthDatabase");
            Helper.AddXAppIdHeader(ref request);
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
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
        var request = new HttpRequestMessage(HttpMethod.Post, "/metricsForDtroUser")
        {
            Content = param
        };

        Helper.AddXAppIdHeader(ref request);

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

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
        var request = new HttpRequestMessage(HttpMethod.Post, "/fullMetricsForDtroUser")
        {
            Content = param
        };

        Helper.AddXAppIdHeader(ref request);

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var metricSummary = JsonSerializer.Deserialize<List<FullMetricSummary>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (metricSummary == null)
        {
            return new List<FullMetricSummary>();
        }
        return metricSummary;
    }
}
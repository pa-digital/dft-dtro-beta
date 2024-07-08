using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            var request = new HttpRequestMessage(HttpMethod.Get, "/v1/healthApi");
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
            var request = new HttpRequestMessage(HttpMethod.Get, "/v1/healthDatabase");
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

    public async Task<bool> TraIdMatch()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/v1/healthTraId");
            Helper.AddHeaders(ref request);
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = int.Parse(content);

            if (result == Helper.TraId())
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }


    public async Task<MetricSummary> MetricsForTra(MetricRequest metricRequest)
    {
        try
        {

            var jsonContent = JsonSerializer.Serialize(metricRequest);
            var param = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            // Create the request message
            var request = new HttpRequestMessage(HttpMethod.Post, "/v1/metricsForTra")
            {
                Content = param
            };

            Helper.AddHeaders(ref request);

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
        catch (HttpRequestException ex)
        {

            throw ex;
        }
    }


}
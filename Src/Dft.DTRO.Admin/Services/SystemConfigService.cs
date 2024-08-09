namespace Dft.DTRO.Admin.Services;
public class SystemConfigService : ISystemConfigService
{
    private readonly HttpClient _client;

    public SystemConfigService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("ExternalApi");
    }

    public async Task<string> GetSystemName()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/systemName");
        Helper.AddHeaders(ref request);

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
      
        if (jsonResponse == null)
        {
            return "Unknown";
        }
        return jsonResponse;
    }
}
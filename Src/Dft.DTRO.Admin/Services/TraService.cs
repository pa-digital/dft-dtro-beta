using System.Text.Json;

namespace Dft.DTRO.Admin.Services;
public class TraService : ITraService
{
    private readonly HttpClient _client;

    public TraService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("ExternalApi");
    }

    public async Task<List<SwaCodeResponse>> GetSwaCodes()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/v1/swaCodes");
        Helper.AddHeaders(ref request);

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var swaCodeList = JsonSerializer.Deserialize<List<SwaCodeResponse>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (swaCodeList == null)
        {
            swaCodeList = new List<SwaCodeResponse>();
        }
        return swaCodeList;
    }
}
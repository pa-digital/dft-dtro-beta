using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<List<SwaCodeResponse>> SearchSwaCodes(string partialName)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/v1/SearchSwaCodes/{partialName}");
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

    public async Task ActivateTraAsync(int traId)
    {
        var response = await _client.PatchAsync($"/v1/swaCodes/activate/{traId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateTraAsync(int traId)
    {
        var response = await _client.PatchAsync($"/v1/swaCodes/deactivate/{traId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateSchemaAsync(SwaCodeRequest swaCodeRequest)
    {

        var response = await _client.PutAsJsonAsync($"/v1/swaCodes/updateFromBody/", swaCodeRequest);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateSchemaAsync(SwaCodeRequest swaCodeRequest)
    {

        var response = await _client.PostAsJsonAsync($"/v1/swaCodes/createFromBody/", swaCodeRequest);
        response.EnsureSuccessStatusCode();
    }
}
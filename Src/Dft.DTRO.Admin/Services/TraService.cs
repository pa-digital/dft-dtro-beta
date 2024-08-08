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

    public async Task<List<SwaCode>> GetSwaCodes()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/swaCodes");
        Helper.AddHeaders(ref request);

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var swaCodeList = JsonSerializer.Deserialize<List<SwaCode>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (swaCodeList == null)
        {
            swaCodeList = new List<SwaCode>();
        }
        return swaCodeList;
    }

    public async Task<SwaCode> GetSwaCode(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/swaCodes/{id}");
        Helper.AddHeaders(ref request);

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var swaCode = JsonSerializer.Deserialize<SwaCode>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (swaCode == null)
        {
            swaCode = new SwaCode();
        }
        return swaCode;
    }

    public async Task<List<SwaCode>> SearchSwaCodes(string partialName)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/SearchSwaCodes/{partialName}");
        Helper.AddHeaders(ref request);

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var swaCodeList = JsonSerializer.Deserialize<List<SwaCode>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (swaCodeList == null)
        {
            swaCodeList = new List<SwaCode>();
        }
        return swaCodeList;
    }

    public async Task ActivateTraAsync(int traId)
    {
        var response = await _client.PatchAsync($"/swaCodes/activate/{traId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateTraAsync(int traId)
    {
        var response = await _client.PatchAsync($"/swaCodes/deactivate/{traId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateTraAsync(SwaCode swaCodeRequest)
    {

        var response = await _client.PutAsJsonAsync($"/swaCodes/updateFromBody/", swaCodeRequest);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateTraAsync(SwaCode swaCodeRequest)
    {

        var response = await _client.PostAsJsonAsync($"/swaCodes/createFromBody/", swaCodeRequest);
        response.EnsureSuccessStatusCode();
    }
}
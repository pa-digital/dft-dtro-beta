using System;
using System.Diagnostics;
using System.Reflection.Metadata;
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
        var request = new HttpRequestMessage(HttpMethod.Get, $"/swaCodes/search/{partialName}");
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
        //var response = await _client.PatchAsync($"/swaCodes/activate/{traId}", null);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/swaCodes/activate/{traId}");
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateTraAsync(int traId)
    {
        //var response = await _client.PatchAsync($"/swaCodes/deactivate/{traId}", null);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/swaCodes/deactivate/{traId}");
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateTraAsync(SwaCode swaCodeRequest)
    {

        //var response = await _client.PutAsJsonAsync($"/swaCodes/updateFromBody/", swaCodeRequest);
        var content = JsonContent.Create(swaCodeRequest);
        var request = new HttpRequestMessage(HttpMethod.Put, $"/swaCodes/updateFromBody/")
        {
            Content = content
        };
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateTraAsync(SwaCode swaCodeRequest)
    {

        //var response = await _client.PostAsJsonAsync($"/swaCodes/createFromBody/", swaCodeRequest);
        var content = JsonContent.Create(swaCodeRequest);
        var request = new HttpRequestMessage(HttpMethod.Post, $"/swaCodes/createFromBody/")
        {
            Content = content
        };
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
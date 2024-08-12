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
        var request = new HttpRequestMessage(HttpMethod.Get, "/v1/swaCodes");
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
        var request = new HttpRequestMessage(HttpMethod.Get, $"/v1/swaCodes/{id}");
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
        var request = new HttpRequestMessage(HttpMethod.Get, $"/v1/swaCodes/search/{partialName}");
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
        //var response = await _client.PatchAsync($"/v1/swaCodes/activate/{traId}", null);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/v1/swaCodes/activate/{traId}");
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateTraAsync(int traId)
    {
        //var response = await _client.PatchAsync($"/v1/swaCodes/deactivate/{traId}", null);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/v1/swaCodes/deactivate/{traId}");
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateTraAsync(SwaCode swaCodeRequest)
    {

        //var response = await _client.PutAsJsonAsync($"/v1/swaCodes/updateFromBody/", swaCodeRequest);
        var content = JsonContent.Create(swaCodeRequest);
        var request = new HttpRequestMessage(HttpMethod.Put, $"/v1/swaCodes/updateFromBody/")
        {
            Content = content
        };
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateTraAsync(SwaCode swaCodeRequest)
    {

        //var response = await _client.PostAsJsonAsync($"/v1/swaCodes/createFromBody/", swaCodeRequest);
        var content = JsonContent.Create(swaCodeRequest);
        var request = new HttpRequestMessage(HttpMethod.Post, $"/v1/swaCodes/createFromBody/")
        {
            Content = content
        };
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
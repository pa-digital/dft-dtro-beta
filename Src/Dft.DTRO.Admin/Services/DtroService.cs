using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Dft.DTRO.Admin.Services;
public class DtroService : IDtroService
{
    private readonly HttpClient _client;

    public DtroService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("ExternalApi");
    }


    public async Task<List<DtroHistoryProvisionResponse>> DtroProvisionHistory(Guid id)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/v1/dtros/provisionHistory/{id}");
        var response = await _client.SendAsync(httpRequestMessage);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var history = JsonSerializer.Deserialize<List<DtroHistoryProvisionResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return history;
    }


    public async Task<List<DtroHistorySourceResponse>> DtroSourceHistory(Guid id)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/v1/dtros/sourceHistory/{id}");
        var response = await _client.SendAsync(httpRequestMessage);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var history = JsonSerializer.Deserialize<List<DtroHistorySourceResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return history;
    }

    public async Task<PaginatedResponse<DtroSearchResult>> SearchDtros()
    {
        var search = new DtroSearch() { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() } };

        var jsonContent = JsonSerializer.Serialize(search);
        var param = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "/v1/search")
        {
            Content = param
        };

        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponse<DtroSearchResult>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return paginatedResponse;
    }

    public async Task CreateDtroAsync(IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "/v1/dtros/createFromFile")
        {
            Content = content
        };

        Helper.AddHeaders(ref request);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

    }

    public async Task UpdateDtroAsync(Guid id, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };

        var request = new HttpRequestMessage(HttpMethod.Put, $"/v1/dtros/updateFromFile/{id}")
        {
            Content = content
        };

        Helper.AddHeaders(ref request);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IActionResult> ReassignDtroAsync(Guid id, int toTraId)
    {

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/v1/dtros/Ownership/{id}/{toTraId}");
        Helper.AddHeaders(ref httpRequestMessage);

        var response = await _client.SendAsync(httpRequestMessage);
        if (response.IsSuccessStatusCode)
        {
            return new JsonResult(new { message = "Successfully reassigned the DTRO." });
        }

        return new JsonResult(new { message = "Failed to reassign the DTRO." }) { StatusCode = (int)response.StatusCode };
    }
}
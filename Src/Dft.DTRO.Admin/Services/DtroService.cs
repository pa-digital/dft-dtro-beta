using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dft.DTRO.Admin.Services;
public class DtroService
{
    private readonly HttpClient _client;

    public DtroService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("ExternalApi");
    }

    public async Task<PaginatedResponse<DtroSearchResult>> SearchDtros()
    {
        var search = new DtroSearch() { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() } };

        var response = await _client.PostAsJsonAsync("/v1/search", search);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PaginatedResponse<DtroSearchResult>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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

        AddHeaders(ref request);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

    }

    public async Task UpdateDtroAsync(Guid id, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };

        var request = new HttpRequestMessage(HttpMethod.Put, "/v1/dtros/updateFromFile")
        {
            Content = content
        };

        AddHeaders(ref request);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IActionResult> ReassignDtroAsync(Guid id, int toTraId)
    {

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/v1/dtros/Ownership/{id}/{toTraId}");
        AddHeaders(ref httpRequestMessage);

        var response = await _client.SendAsync(httpRequestMessage);
        if (response.IsSuccessStatusCode)
        {
            return new JsonResult(new { message = "Successfully reassigned the DTRO." });
        }

        return new JsonResult(new { message = "Failed to reassign the DTRO." }) { StatusCode = (int)response.StatusCode };
    }

    private void AddHeaders(ref HttpRequestMessage httpRequestMessage)
    {
        int ta = 1585;
        httpRequestMessage.Headers.Add("ta", ta.ToString());
    }
}
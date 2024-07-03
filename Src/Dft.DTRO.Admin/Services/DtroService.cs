using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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


    //[HttpGet]
    //[Route("/v1/dtros/provisionHistory/{dtroId:guid}")]
    //public async Task<ActionResult<List<DtroHistoryProvisionResponse>>> GetProvisionHistory(Guid dtroId)

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

        // Serialize the search object to JSON
        var jsonContent = JsonSerializer.Serialize(search);
        var param = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        // Create the request message
        var request = new HttpRequestMessage(HttpMethod.Post, "/v1/search")
        {
            Content = param
        };

        AddHeaders(ref request);
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

        var request = new HttpRequestMessage(HttpMethod.Put, $"/v1/dtros/updateFromFile/{id}")
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
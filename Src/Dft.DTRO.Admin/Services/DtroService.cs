using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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
        int ta = 1585;
        request.Headers.Add("ta", ta.ToString());

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

    }

    public async Task UpdateDtroAsync(Guid id, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        var response = await _client.PutAsync($"/v1/dtros/updateFromFile", content);
        response.EnsureSuccessStatusCode();
    }

}
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
        var search = new DtroSearch() { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { new() { VehicleType = "taxi" } } };
          
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
        var response = await _client.PostAsync($"/v1/createDtroFromFile", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateDtroAsync(int id, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        var response = await _client.PostAsync($"/v1/updateDtroFromFile/{id}", content);
        response.EnsureSuccessStatusCode();
    }

}
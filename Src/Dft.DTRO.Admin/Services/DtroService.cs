namespace Dft.DTRO.Admin.Services;
public class DtroService : IDtroService
{
    private readonly HttpClient _client;
    private readonly IXappIdService _xappIdService;
    public DtroService(IHttpClientFactory clientFactory, IXappIdService xappIdService)
    {
        _client = clientFactory.CreateClient("ExternalApi");
        _xappIdService = xappIdService;
    }

    public async Task<List<DtroHistoryProvisionResponse>> DtroProvisionHistory(Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/dtros/provisionHistory/{id}");
        _xappIdService.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var history = JsonSerializer.Deserialize<List<DtroHistoryProvisionResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return history;
    }

    public async Task<List<DtroHistorySourceResponse>> DtroSourceHistory(Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/dtros/sourceHistory/{id}");
        _xappIdService.AddXAppIdHeader(ref request);

        var response = await _client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var history = JsonSerializer.Deserialize<List<DtroHistorySourceResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return history;
    }

    public async Task<PaginatedResponse<DtroSearchResult>> SearchDtros(int? traId)
    {
        if (traId == 0)
        {
            traId = null;
        }
        var searchQuery = new SearchQuery { CurrentTraOwner = traId };
        var search = new DtroSearch() { Page = 1, PageSize = 10, Queries = new List<SearchQuery> { searchQuery } };

        var jsonContent = JsonSerializer.Serialize(search);
        var param = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, "/search")
        {
            Content = param
        };

        _xappIdService.AddXAppIdHeader(ref request);
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

        var request = new HttpRequestMessage(HttpMethod.Post, "/dtros/createFromFile")
        {
            Content = content
        };

        _xappIdService.AddXAppIdHeader(ref request);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

    }

    public async Task UpdateDtroAsync(Guid id, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };

        var request = new HttpRequestMessage(HttpMethod.Put, $"/dtros/updateFromFile/{id}")
        {
            Content = content
        };

        _xappIdService.AddXAppIdHeader(ref request);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IActionResult> ReassignDtroAsync(Guid id, Guid toDtroUserId)
    {

        var request = new HttpRequestMessage(HttpMethod.Post, $"/dtros/Ownership/{id}/{toDtroUserId}");
        _xappIdService.AddXAppIdHeader(ref request);

        var response = await _client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return new JsonResult(new { message = "Successfully reassigned the DTRO." });
        }

        return new JsonResult(new { message = "Failed to reassign the DTRO." }) { StatusCode = (int)response.StatusCode };
    }
}
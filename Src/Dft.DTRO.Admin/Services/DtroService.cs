
namespace Dft.DTRO.Admin.Services;
public class DtroService : IDtroService
{
    private readonly HttpClient _client;
    private readonly IXappIdService _xappIdService;
    private readonly IErrHandlingService _errHandlingService;
    public DtroService(IHttpClientFactory clientFactory, IXappIdService xappIdService, IErrHandlingService errHandlingService)
    {
        _client = clientFactory.CreateClient("ExternalApi");
        _xappIdService = xappIdService;
        _errHandlingService = errHandlingService;
    }

    public async Task<List<DtroHistoryProvisionResponse>> DtroProvisionHistory(Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ConfigHelper.Version + $"/dtros/provisionHistory/{id}");
        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);

        var content = await response.Content.ReadAsStringAsync();
        var history = JsonSerializer.Deserialize<List<DtroHistoryProvisionResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return history;
    }

    public async Task<List<DtroHistorySourceResponse>> DtroSourceHistory(Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, ConfigHelper.Version + $"/dtros/sourceHistory/{id}");
        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);

        var content = await response.Content.ReadAsStringAsync();
        var history = JsonSerializer.Deserialize<List<DtroHistorySourceResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return history;
    }

    public async Task<PaginatedResponse<DtroSearchResult>> SearchDtros(int? traId, int pageNumber)
    {
        if (traId == 0)
        {
            traId = null;
        }

        var searchQuery = new SearchQuery { CurrentTraOwner = traId };
        var search = new DtroSearch() { Page = pageNumber, PageSize = 50, Queries = new List<SearchQuery> { searchQuery } };

        var jsonContent = JsonSerializer.Serialize(search);
        var param = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, ConfigHelper.Version + "/search")
        {
            Content = param
        };

        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);

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

        var request = new HttpRequestMessage(HttpMethod.Post, ConfigHelper.Version + "/dtros/createFromFile")
        {
            Content = content
        };

        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }

    public async Task UpdateDtroAsync(Guid id, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };

        var request = new HttpRequestMessage(HttpMethod.Put, ConfigHelper.Version + $"/dtros/updateFromFile/{id}")
        {
            Content = content
        };

        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }

    public async Task<IActionResult> ReassignDtroAsync(Guid id, Guid toDtroUserId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, ConfigHelper.Version + $"/dtros/ownership/{id}/{toDtroUserId}");
        await _xappIdService.AddXAppIdHeader(request);

        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);

        return new JsonResult(new { message = "Failed to reassign the DTRO." }) { StatusCode = (int)response.StatusCode };
    }
}
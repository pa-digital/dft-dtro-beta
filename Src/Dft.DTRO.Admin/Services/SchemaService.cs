namespace Dft.DTRO.Admin.Services;
public class SchemaService : ISchemaService
{
    private readonly HttpClient _client;
    private readonly IXappIdService _xappIdService;
    public SchemaService(IHttpClientFactory clientFactory, IXappIdService xappIdService)
    {
        _client = clientFactory.CreateClient("ExternalApi");
        _xappIdService = xappIdService;
    }

    public async Task<List<SchemaTemplateOverview>> GetSchemaVersionsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/schemas/versions");
        _xappIdService.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<SchemaTemplateOverview>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task ActivateSchemaAsync(string version)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/schemas/activate/{version}");
        _xappIdService.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateSchemaAsync(string version)
    {
        //var response = await _client.PatchAsync($"/schemas/deactivate/{version}", null);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/schemas/deactivate/{version}");
        _xappIdService.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateSchemaAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        // var response = await _client.PutAsync($"/schemas/updateFromFile/{version}", content);
        var request = new HttpRequestMessage(HttpMethod.Put, $"/schemas/updateFromFile/{version}")
        {
            Content = content
        };
        _xappIdService.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateSchemaAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        // var response = await _client.PostAsync($"/schemas/createFromFile/{version}", content);
        var request = new HttpRequestMessage(HttpMethod.Post, $"/schemas/createFromFile/{version}")
        {
            Content = content
        };
        _xappIdService.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
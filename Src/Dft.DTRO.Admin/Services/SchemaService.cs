namespace Dft.DTRO.Admin.Services;
public class SchemaService : ISchemaService
{
    private readonly HttpClient _client;

    public SchemaService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("ExternalApi");
    }

    public async Task<List<SchemaTemplateOverview>> GetSchemaVersionsAsync()
    {
        var response = await _client.GetAsync("/schemas/versions");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<SchemaTemplateOverview>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task ActivateSchemaAsync(string version)
    {
        var response = await _client.PatchAsync($"/schemas/activate/{version}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateSchemaAsync(string version)
    {
        var response = await _client.PatchAsync($"/schemas/deactivate/{version}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateSchemaAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        var response = await _client.PutAsync($"/schemas/updateFromFile/{version}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateSchemaAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        var response = await _client.PostAsync($"/schemas/createFromFile/{version}", content);
        response.EnsureSuccessStatusCode();
    }
}
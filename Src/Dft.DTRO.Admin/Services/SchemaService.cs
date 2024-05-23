using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dft.DTRO.Admin.Services;
public class SchemaService 
{
    private readonly HttpClient _client;

    public SchemaService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("ExternalApi");
    }

    public async Task<List<SchemaTemplateOverview>> GetSchemaVersionsAsync()
    {
        var response = await _client.GetAsync("/v1/schemasversions");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<SchemaTemplateOverview>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task ActivateSchemaAsync(string version)
    {
        var response = await _client.PatchAsync($"/v1/activate/{version}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateSchemaAsync(string version)
    {
        var response = await _client.PatchAsync($"/v1/deactivate/{version}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateSchemaAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        var response = await _client.PostAsync($"/v1/updateSchemaFromFile/{version}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateSchemaAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        var response = await _client.PostAsync($"/v1/createSchemaFromFile/{version}", content);
        response.EnsureSuccessStatusCode();
    }
}
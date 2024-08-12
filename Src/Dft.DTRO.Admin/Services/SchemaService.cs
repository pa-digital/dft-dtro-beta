using System;
using System.Text.Json;

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
        //var response = await _client.GetAsync("/v1/schemas/versions");
        var request = new HttpRequestMessage(HttpMethod.Get, "/v1/schemas/versions");
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<SchemaTemplateOverview>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task ActivateSchemaAsync(string version)
    {
        //var response = await _client.PatchAsync($"/v1/schemas/activate/{version}", null);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/v1/schemas/activate/{version}");
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeactivateSchemaAsync(string version)
    {
        //var response = await _client.PatchAsync($"/v1/schemas/deactivate/{version}", null);
        var request = new HttpRequestMessage(HttpMethod.Patch, $"/v1/schemas/deactivate/{version}");
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateSchemaAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        // var response = await _client.PutAsync($"/v1/schemas/updateFromFile/{version}", content);
        var request = new HttpRequestMessage(HttpMethod.Put, $"/v1/schemas/updateFromFile/{version}")
        {
            Content = content
        };
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateSchemaAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        // var response = await _client.PostAsync($"/v1/schemas/createFromFile/{version}", content);
        var request = new HttpRequestMessage(HttpMethod.Post, $"/v1/schemas/createFromFile/{version}")
        {
            Content = content
        };
        Helper.AddHeaders(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
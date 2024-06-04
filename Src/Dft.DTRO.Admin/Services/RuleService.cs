using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dft.DTRO.Admin.Services;
public class RuleService 
{
    private readonly HttpClient _client;

    public RuleService(IHttpClientFactory clientFactory)
    {
        _client = clientFactory.CreateClient("ExternalApi");
    }

    public async Task UpdateRuleAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        var response = await _client.PostAsync($"/v1/updateRuleFromFile/{version}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateRuleAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        var response = await _client.PostAsync($"/v1/createRuleFromFile/{version}", content);
        response.EnsureSuccessStatusCode();
    }
}
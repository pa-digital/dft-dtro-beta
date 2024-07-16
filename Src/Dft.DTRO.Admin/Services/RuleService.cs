namespace Dft.DTRO.Admin.Services;
public class RuleService : IRuleService
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
        var response = await _client.PutAsync($"/v1/rules/updateFromFile/{version}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateRuleAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        var response = await _client.PostAsync($"/v1/rules/createFromFile/{version}", content);
        response.EnsureSuccessStatusCode();
    }
}
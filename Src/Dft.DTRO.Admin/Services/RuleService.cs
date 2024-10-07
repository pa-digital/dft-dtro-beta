namespace Dft.DTRO.Admin.Services;
public class RuleService : IRuleService
{
    private readonly HttpClient _client;
    private readonly IXappIdService _xappIdService;
    private readonly IErrHandlingService _errHandlingService;

    public RuleService(IHttpClientFactory clientFactory, IXappIdService xappIdService, IErrHandlingService errHandlingService)
    {
        _client = clientFactory.CreateClient("ExternalApi");
        _xappIdService = xappIdService;
        _errHandlingService = errHandlingService;
    }

    public async Task UpdateRuleAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };

        //var response = await _client.PutAsync($"/rules/updateFromFile/{version}", content);
        var request = new HttpRequestMessage(HttpMethod.Put, ConfigHelper.Version + $"/rules/updateFromFile/{version}")
        {
            Content = content
        };
        await _xappIdService.AddXAppIdHeader(request);
        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }

    public async Task CreateRuleAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        var request = new HttpRequestMessage(HttpMethod.Post, ConfigHelper.Version + $"/rules/createFromFile/{version}")
        {
            Content = content
        };
        await _xappIdService.AddXAppIdHeader(request);
        var response = await _client.SendAsync(request);
        await _errHandlingService.RedirectIfErrors(response);
    }
}
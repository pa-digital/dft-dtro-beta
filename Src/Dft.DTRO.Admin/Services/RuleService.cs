﻿namespace Dft.DTRO.Admin.Services;
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
        
        //var response = await _client.PutAsync($"/rules/updateFromFile/{version}", content);
        var request = new HttpRequestMessage(HttpMethod.Put, $"/rules/updateFromFile/{version}")
        {
            Content = content
        };
        Helper.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateRuleAsync(string version, IFormFile file)
    {
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream()), "file", file.FileName }
        };
        //var response = await _client.PostAsync($"/rules/createFromFile/{version}", content);
        var request = new HttpRequestMessage(HttpMethod.Post, $"/rules/createFromFile/{version}")
        {
            Content = content
        };
        Helper.AddXAppIdHeader(ref request);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
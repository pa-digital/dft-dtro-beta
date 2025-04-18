﻿using Newtonsoft.Json;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace DfT.DTRO.Apis.Clients;

public class ApigeeClient : IApigeeClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly SecretManagerClient _secretManagerClient;

    public ApigeeClient(IConfiguration configuration, HttpClient httpClient, SecretManagerClient secretManagerClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _secretManagerClient = secretManagerClient;
    }

    public async Task<HttpResponseMessage> CreateApp(string developerEmail, ApigeeDeveloperAppInput developerAppInput)
    {
        var requestUrl = $"developers/{developerEmail}/apps";
        return await SendRequest(HttpMethod.Post, requestUrl, developerAppInput);
    }
    
    public async Task<HttpResponseMessage> GetApp(string developerEmail, string name)
    {
        var requestUrl = $"developers/{developerEmail}/apps/{name}";
        return await SendRequest(HttpMethod.Get, requestUrl, null);
    }
    
    public async Task<HttpResponseMessage> DeleteApp(string developerEmail, string name)
    {
        var requestUrl = $"developers/{developerEmail}/apps/{name}";
        return await SendRequest(HttpMethod.Delete, requestUrl, null);
    }
    
    public async Task<HttpResponseMessage> UpdateAppStatus(string developerEmail, string name, string action)
    {
        var requestUrl = $"developers/{developerEmail}/apps/{name}?action={action}";
        return await SendOctetStreamRequest(HttpMethod.Post, requestUrl);
    }
    
    public async Task<HttpResponseMessage> DeleteDeveloper(string developerEmail)
    {
        var requestUrl = $"developers/{developerEmail}";
        return await SendRequest(HttpMethod.Delete, requestUrl, null);
    }

    private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string requestUrl, object requestMessageContent)
    {
        HttpRequestMessage requestMessage = await GetRequestMessage(method, requestUrl);
        if (requestMessageContent is not null)
        {
            var content = JsonConvert.SerializeObject(requestMessageContent);
            requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
        }
        return await _httpClient.SendAsync(requestMessage);
    }
    
    private async Task<HttpResponseMessage> SendOctetStreamRequest(HttpMethod method, string requestUrl)
    {
        HttpRequestMessage requestMessage = await GetRequestMessage(method, requestUrl);
        requestMessage.Content = new ByteArrayContent(new byte[0]);
        requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        return await _httpClient.SendAsync(requestMessage);
    }

    private async Task<HttpRequestMessage> GetRequestMessage(HttpMethod method, string requestUrl)
    {
        var apiUrl = _configuration.GetValue<string>("ApiSettings:ApigeeApiUrl");
        var requestUri = $"{apiUrl}{requestUrl}";
        var requestMessage = new HttpRequestMessage(method, requestUri);
        string secret = _secretManagerClient.GetSecret(ApiConsts.SaExecutionPrivateKeySecretName);
        GoogleCredential credential = GoogleCredential.FromJson(secret).CreateScoped(ApiConsts.GoogleApisAuthScope);
        string accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return requestMessage;
    }
}
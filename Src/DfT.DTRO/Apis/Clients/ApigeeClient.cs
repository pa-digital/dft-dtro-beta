using System.Net.Http;
using System.Net.Http.Headers;
using DfT.DTRO.Apis.Consts;
using DfT.DTRO.Models.Apigee;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

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
        return await SendRequest(HttpMethod.Get, requestUrl, "");
    }

    private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string requestUrl, object requestMessageContent)
    {
        string secret = _secretManagerClient.GetSecret(ApiConsts.SaExecutionPrivateKeySecretName);
        GoogleCredential credential = GoogleCredential.FromJson(secret).CreateScoped(ApiConsts.GoogleApisAuthScope);
        var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
        var apiUrl = _configuration.GetValue<string>("ApiSettings:ApigeeApiUrl");
        var requestUri = $"{apiUrl}{requestUrl}";
        var requestMessage = new HttpRequestMessage(method, requestUri);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        if (method != HttpMethod.Get && method != HttpMethod.Delete)
        {
            var content = JsonConvert.SerializeObject(requestMessageContent);
            requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
        }
        return await _httpClient.SendAsync(requestMessage);
    }

}
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
        var requestUri = $"developers/{developerEmail}/apps";
        return await SendRequest(HttpMethod.Post, requestUri, developerAppInput);
    }

    private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string requestUri, object requestMessageContent)
    {
        string secret = _secretManagerClient.GetSecret(ApiConsts.SaExecutionPrivateKeySecretName);
        GoogleCredential credential = GoogleCredential.FromJson(secret).CreateScoped(ApiConsts.GoogleApisAuthScope);
        var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
        var apiUrl = _configuration.GetValue<string>("ApiSettings:ApigeeApiUrl");
        var requestMessage = new HttpRequestMessage(method, $"{apiUrl}{requestUri}");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var content = JsonConvert.SerializeObject(requestMessageContent);
        requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
        return await _httpClient.SendAsync(requestMessage);
    }

}
using System.Net.Http;
using System.Net.Http.Headers;
using DfT.DTRO.Models.Auth;

namespace DfT.DTRO.Apis.Clients;

public class AuthClient : IAuthClient
{
    
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public AuthClient(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }
    
    public async Task<HttpResponseMessage> GetToken(AuthTokenInput authTokenInput)
    {
        var apiUrl = _configuration.GetValue<string>("ApiSettings:ApiUrl");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}oauth-generator");
        var username = authTokenInput.Username;
        var password = authTokenInput.Password;
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", 
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
        requestMessage.Content = new FormUrlEncodedContent([new KeyValuePair<string, string>("grant_type", "client_credentials")]);
        return await _httpClient.SendAsync(requestMessage);
    }
}
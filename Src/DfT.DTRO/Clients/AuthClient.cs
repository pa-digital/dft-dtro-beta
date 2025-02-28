using System.Net.Http;
using System.Net.Http.Headers;
using DfT.DTRO.Models.Auth;
using Newtonsoft.Json;

namespace DfT.DTRO.Services;

public class AuthClient : IAuthClient
{
    
    private readonly IConfiguration _configuration;

    public AuthClient(IConfiguration configuration) =>
        _configuration = configuration;
    
    public async Task<AuthToken> GetToken(AuthTokenInput authTokenInput)
    {
        HttpClient client = new();
        var apiUrl = _configuration.GetValue<string>("ApiSettings:ApiUrl");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}oauth-generator");
        var username = authTokenInput.Username;
        var password = authTokenInput.Password;
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", 
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
        requestMessage.Content = new FormUrlEncodedContent([new KeyValuePair<string, string>("grant_type", "client_credentials")]);;
        HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);
        var responseMessageContent = await responseMessage.Content.ReadAsStringAsync();
        if (responseMessage.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<AuthToken>(responseMessageContent);
        }
        throw new Exception(responseMessage.ToString());
    }
}
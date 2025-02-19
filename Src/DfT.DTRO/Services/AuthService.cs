using System.Net.Http;
using System.Net.Http.Headers;
using DfT.DTRO.Models.Auth;
using Newtonsoft.Json;

namespace DfT.DTRO.Services;

public class AuthService : IAuthService
{
    
    private static readonly string ClientId = "Q2a3tGiRAWOQ3yNRV1hzlPf3aASr0ANryVJAG3rdRxlaPp1J";
    private static readonly string ClientSecret = "Et5KArPAQPO1kE7PaZAAecgJ224Iyf1YiUTDgbLz7jyKjP81mOadW620fg7nJ4tT";
    
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration) =>
        _configuration = configuration;
    
    public async Task<AuthToken> GetToken(AuthTokenInput authTokenInput)
    {
        HttpClient client = new();
        var apiUrl = _configuration.GetValue<string>("ApiSettings:ApiUrl");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}oauth-generator");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", 
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}")));
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
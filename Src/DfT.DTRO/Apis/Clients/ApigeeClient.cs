using System.Net.Http;
using System.Net.Http.Headers;
using DfT.DTRO.Models.App;
using Newtonsoft.Json;

namespace DfT.DTRO.Apis.Clients;

public class ApigeeClient : IApigeeClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ApigeeClient(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }
    
    public async Task<HttpResponseMessage> CreateApp(AppInput appInput, string accessToken)
    {
        var apiUrl = _configuration.GetValue<string>("ApiSettings:ApigeeApiUrl");
        var developerEmail = appInput.Username;
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}developers/{developerEmail}/apps");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var content = JsonConvert.SerializeObject(appInput);
        requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
        return await _httpClient.SendAsync(requestMessage);
    }
}
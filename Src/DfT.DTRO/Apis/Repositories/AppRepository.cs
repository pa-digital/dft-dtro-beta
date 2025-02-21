using DfT.DTRO.Apis.Clients;
using DfT.DTRO.Models.App;
using Newtonsoft.Json;

namespace DfT.DTRO.Apis.Repositories;

public class AppRepository : IAppRepository
{
    
    private readonly IApigeeClient _apigeeClient;

    public AppRepository(IApigeeClient apigeeClient) =>
        _apigeeClient = apigeeClient;
    
    public async Task<App> CreateApp(AppInput appInput, string accessToken)
    {
        var responseMessage = await _apigeeClient.CreateApp(appInput, accessToken);
        var responseMessageContent = await responseMessage.Content.ReadAsStringAsync();
        return responseMessage.IsSuccessStatusCode ? JsonConvert.DeserializeObject<App>(responseMessageContent)
            : throw new Exception(responseMessage.ToString());
    }
}
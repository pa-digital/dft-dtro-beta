using DfT.DTRO.Apis.Clients;
using DfT.DTRO.Models.Apigee;
using DfT.DTRO.Models.App;
using Newtonsoft.Json;

namespace DfT.DTRO.Apis.Repositories;

public class ApigeeApigeeAppRepository : IApigeeAppRepository
{
    
    private readonly IApigeeClient _apigeeClient;

    public ApigeeApigeeAppRepository(IApigeeClient apigeeClient) =>
        _apigeeClient = apigeeClient;
    
    public async Task<ApigeeDeveloperApp> CreateApp(string developerEmail, ApigeeDeveloperAppInput developerAppInput)
    {
        var responseMessage = await _apigeeClient.CreateApp(developerEmail, developerAppInput);
        var responseMessageContent = await responseMessage.Content.ReadAsStringAsync();
        return responseMessage.IsSuccessStatusCode ? JsonConvert.DeserializeObject<ApigeeDeveloperApp>(responseMessageContent)
            : throw new Exception(responseMessageContent);
    }
}
using Newtonsoft.Json;

namespace DfT.DTRO.Apis.Repositories;

[ExcludeFromCodeCoverage]
public class ApigeeAppRepository : IApigeeAppRepository
{

    private readonly IApigeeClient _apigeeClient;

    public ApigeeAppRepository(IApigeeClient apigeeClient) =>
        _apigeeClient = apigeeClient;

    public async Task<ApigeeDeveloperApp> CreateApp(string developerEmail, ApigeeDeveloperAppInput developerAppInput)
    {
        var responseMessage = await _apigeeClient.CreateApp(developerEmail, developerAppInput);
        var responseMessageContent = await responseMessage.Content.ReadAsStringAsync();
        return responseMessage.IsSuccessStatusCode ? JsonConvert.DeserializeObject<ApigeeDeveloperApp>(responseMessageContent)
            : throw new Exception(responseMessageContent);
    }
    
    public async Task<ApigeeDeveloperApp> GetApp(string developerEmail, string name)
    {
        var responseMessage = await _apigeeClient.GetApp(developerEmail, name);
        var responseMessageContent = await responseMessage.Content.ReadAsStringAsync();
        return responseMessage.IsSuccessStatusCode ? JsonConvert.DeserializeObject<ApigeeDeveloperApp>(responseMessageContent)
            : throw new Exception(responseMessageContent);
    }
    
    public async Task<ApigeeDeveloperApp> DeleteApp(string developerEmail, string name)
    {
        var responseMessage = await _apigeeClient.DeleteApp(developerEmail, name);
        var responseMessageContent = await responseMessage.Content.ReadAsStringAsync();
        return responseMessage.IsSuccessStatusCode ? JsonConvert.DeserializeObject<ApigeeDeveloperApp>(responseMessageContent)
            : throw new Exception(responseMessageContent);
    }
    
    public async Task<ApigeeDeveloperApp> UpdateAppStatus(string developerEmail, string name, string action)
    {
        var responseMessage = await _apigeeClient.UpdateAppStatus(developerEmail, name, action);
        var responseMessageContent = await responseMessage.Content.ReadAsStringAsync();
        return responseMessage.IsSuccessStatusCode ? JsonConvert.DeserializeObject<ApigeeDeveloperApp>(responseMessageContent)
            : throw new Exception(responseMessageContent);
    }
}
using Newtonsoft.Json;

namespace DfT.DTRO.Apis.Repositories;

[ExcludeFromCodeCoverage]
public class ApigeeDeveloperRepository : IApigeeDeveloperRepository
{

    private readonly IApigeeClient _apigeeClient;

    public ApigeeDeveloperRepository(IApigeeClient apigeeClient) =>
        _apigeeClient = apigeeClient;
    
    public async Task<ApigeeDeveloper> DeleteDeveloper(string developerEmail)
    {
        var responseMessage = await _apigeeClient.DeleteDeveloper(developerEmail);
        var responseMessageContent = await responseMessage.Content.ReadAsStringAsync();
        return responseMessage.IsSuccessStatusCode ? JsonConvert.DeserializeObject<ApigeeDeveloper>(responseMessageContent)
            : throw new Exception(responseMessageContent);
    }
}
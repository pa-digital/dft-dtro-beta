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
}
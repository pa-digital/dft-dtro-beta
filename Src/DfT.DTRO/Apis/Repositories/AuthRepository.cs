using DfT.DTRO.Apis.Clients;
using DfT.DTRO.Models.Auth;
using Newtonsoft.Json;

namespace DfT.DTRO.Apis.Repositories;

public class AuthRepository : IAuthRepository
{
    
    private readonly IAuthClient _authClient;

    public AuthRepository(IAuthClient authClient) =>
        _authClient = authClient;
    
    public async Task<AuthToken> GetToken(AuthTokenInput authTokenInput)
    {
        var responseMessage = await _authClient.GetToken(authTokenInput);
        var responseMessageContent = await responseMessage.Content.ReadAsStringAsync();
        return responseMessage.IsSuccessStatusCode ? JsonConvert.DeserializeObject<AuthToken>(responseMessageContent)
            : throw new Exception(responseMessage.ToString());
    }
}
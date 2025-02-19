using DfT.DTRO.Models.Auth;

namespace DfT.DTRO.Services;

public class AuthService : IAuthService
{
    
    private readonly IAuthClient _authClient;

    public AuthService(IAuthClient authClient) =>
        _authClient = authClient;
    
    public async Task<AuthToken> GetToken(AuthTokenInput authTokenInput)
    {
       return await _authClient.GetToken(authTokenInput);
    }
}
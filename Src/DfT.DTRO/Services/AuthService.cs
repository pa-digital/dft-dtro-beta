using DfT.DTRO.Apis.Repositories;
using DfT.DTRO.Models.Auth;

namespace DfT.DTRO.Services;

public class AuthService : IAuthService
{
    
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository) =>
        _authRepository = authRepository;
    
    public async Task<AuthToken> GetToken(AuthTokenInput authTokenInput)
    {
       return await _authRepository.GetToken(authTokenInput);
    }
}
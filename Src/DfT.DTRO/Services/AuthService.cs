namespace DfT.DTRO.Services;

/// <inheritdoc cref="IAuthService"/>
public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="authRepository">Repository passed</param>
    public AuthService(IAuthRepository authRepository) => 
        _authRepository = authRepository;

    /// <inheritdoc cref="IAuthService"/>
    public async Task<AuthToken> GetToken(AuthTokenInput authTokenInput) => 
        await _authRepository.GetToken(authTokenInput);
}
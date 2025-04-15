using DfT.DTRO.Models.Auth;

namespace DfT.DTRO.Services;

public class AuthService : IAuthService
{

    private readonly IAuthRepository _authRepository;
    private readonly IUserDal _userDal;
    private readonly IAuthDal _authDal;

    public AuthService(IAuthRepository authRepository, IUserDal userDal, IAuthDal authDal)
    {
        _authRepository = authRepository;
        _userDal = userDal;
        _authDal = authDal;
    }

    public async Task<AuthToken> GetToken(AuthTokenInput authTokenInput)
    {
        return await _authRepository.GetToken(authTokenInput);
    }

    public async Task<bool> AuthenticateUser(string username, string password)
    {
        User user = await _userDal.GetUserFromEmail(username);
        Auth userAuth = await _authDal.GetAuthUser(user);
        return BCrypt.Net.BCrypt.Verify(password, userAuth.Password);
    }
}
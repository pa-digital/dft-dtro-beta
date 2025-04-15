using DfT.DTRO.Models.TwoFactorAuth;

namespace DfT.DTRO.Services;

public class TwoFactorAuthService : ITwoFactorAuthService
{

    private readonly ITwoFactorAuthDal _twoFactorAuthDal;
    private readonly IUserDal _userDal;

    public TwoFactorAuthService(ITwoFactorAuthDal twoFactorAuthDal, IUserDal userDal)
    {
        _twoFactorAuthDal = twoFactorAuthDal;
        _userDal = userDal;
    }

    public async Task<TwoFactorAuthentication> GenerateTwoFactorAuthCode(string username)
    {
        User user = await _userDal.GetUserFromEmail(username);
        TwoFactorAuthentication tfa = await _twoFactorAuthDal.SaveTwoFactorAuthCode(user);
        return tfa;
    }


    public async Task<TwoFactorAuthentication> VerifyTwoFactorAuthCode(string token, string code)
    {
        Guid tokenGuid = Guid.Parse(token);
        TwoFactorAuthentication tfa = await _twoFactorAuthDal.GetCodeByToken(tokenGuid);
        if (tfa.Code == code && tfa.ExpiresAt > DateTime.UtcNow)
        {
            return tfa;
        }

        return null;
    }

    public async Task DeleteTwoFactorAuthCodeById(Guid id)
    {
        await _twoFactorAuthDal.DeleteTwoFactorAuthCodeById(id);
    }

}
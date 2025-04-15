using DfT.DTRO.Models.TwoFactorAuth;

namespace DfT.DTRO.Services;

public interface ITwoFactorAuthService
{
    Task<TwoFactorAuthentication> GenerateTwoFactorAuthCode(string username);

    Task<TwoFactorAuthentication> VerifyTwoFactorAuthCode(string token, string code);

    Task DeleteTwoFactorAuthCodeById(Guid id);

}
namespace DfT.DTRO.DAL;

public interface ITwoFactorAuthDal
{
    Task<TwoFactorAuthentication> GetCodeByToken(Guid token);
    Task<TwoFactorAuthentication> SaveTwoFactorAuthCode(User user);
    Task DeleteTwoFactorAuthCodeById(Guid id);
}
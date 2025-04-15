namespace DfT.DTRO.Utilities
{
    public interface IAuthHelper
    {
        string GenerateJwtToken(string username);
        DateTime GetJwtExpiration(string token);
        string GenerateTwoFactorCode();
    }
}

namespace DfT.DTRO.DAL;

public class TwoFactorAuthDal(DtroContext context, IAuthHelper authHelper) : ITwoFactorAuthDal
{
    private readonly DtroContext _context = context;
    private readonly IAuthHelper _authHelper = authHelper;

    public async Task DeleteTwoFactorAuthCodeById(Guid id)
    {
        TwoFactorAuthentication tfa = await _context.TwoFactorAuthentication.FindAsync(id);

        if (tfa != null)
        {
            _context.TwoFactorAuthentication.Remove(tfa);
            await _context.SaveChangesAsync();
        }
    }


    public async Task<TwoFactorAuthentication> GetCodeByToken(Guid token)
    {
        return await _context.TwoFactorAuthentication
            .Include(u => u.User)
            .SingleOrDefaultAsync(t => t.Token == token);
    }


    public async Task<TwoFactorAuthentication> SaveTwoFactorAuthCode(User user)
    {
        Guid token = Guid.NewGuid();
        string code = _authHelper.GenerateTwoFactorCode();
        var record = new TwoFactorAuthentication
        {
            UserId = user.Id,
            Token = token,
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
        };

        await _context.TwoFactorAuthentication.AddAsync(record);
        await _context.SaveChangesAsync();

        return record;
    }
}
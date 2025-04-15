namespace DfT.DTRO.DAL;

public class AuthDal(DtroContext context) : IAuthDal
{
    private readonly DtroContext _context = context;


    public async Task<Auth> GetAuthUser(User user)
    {
        return await _context.Auth.SingleOrDefaultAsync(a => a.User == user);
    }
}
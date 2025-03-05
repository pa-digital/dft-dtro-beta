namespace DfT.DTRO.DAL;

public class UserDal(DtroContext context) : IUserDal
{
    private readonly DtroContext _context = context;

    public async Task DeleteUser(Guid userId)
    {
        User user = await _context.Users.FindAsync(userId);
        DtroUser dtroUser = await _context.DtroUsers.FindAsync(userId);
        if (user != null && dtroUser != null)
        {
            _context.Users.Remove(user);
            _context.DtroUsers.Remove(dtroUser);
            await _context.SaveChangesAsync();
        }
    }
}
namespace DfT.DTRO.DAL;

public class UserDal(DtroContext context) : IUserDal
{
    private readonly DtroContext _context = context;

    public async Task<PaginatedResult<UserListDto>> GetUsers(PaginatedRequest paginatedRequest)
    {
        IQueryable<UserListDto> query = _context.Users
            .Select(u => new UserListDto()
            {
                Name = $"{u.Forename} {u.Surname}",
                Email = u.Email,
                Created = u.Created.ToString()
            });
        IQueryable<UserListDto> paginatedQuery = query
            .Skip((paginatedRequest.Page - 1) * paginatedRequest.PageSize)
            .Take(paginatedRequest.PageSize);
        return new PaginatedResult<UserListDto>(paginatedQuery.ToList(), paginatedQuery.Count());
    }

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

    public async Task<User> GetUserFromEmail(string email)
    {
        User user = await _context.Users
            .Where(u => u.Email == email)
            .SingleOrDefaultAsync();
        
        return user ?? throw new Exception("No matching user found");
    }

    public async Task<bool> HasUserRequestedProductionAccess(Guid userId)
    {
        User user = await _context.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return user.ProductionAccessRequested;
    }

    public async Task RequestProductionAccess(Guid userId)
    {
        User user = await _context.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();
        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.ProductionAccessRequested = true;
        await _context.SaveChangesAsync();
    }
}
namespace DfT.DTRO.DAL;

public class UserDal(DtroContext context) : IUserDal
{
    public PaginatedResult<User> GetUsers(Guid userId)
    {
        var isAdmin = context.Users.Select(it => it.Id == userId).FirstOrDefault();
        if (!isAdmin)
            return new PaginatedResult<User>([], 0);

        var query = context
            .Users
            .Include(it => it.UserStatus)
            .Where(it => it.IsCentralServiceOperator);

        var paginatedQuery = query
            .OrderBy(it => it.Id)
            .Skip(1);

        return new PaginatedResult<User>([], 0);
    }
}
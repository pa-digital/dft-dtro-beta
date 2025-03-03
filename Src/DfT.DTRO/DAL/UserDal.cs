namespace DfT.DTRO.DAL;

/// <inheritdoc cref="IUserDal"/>
public class UserDal(DtroContext context) : IUserDal
{
    /// <inheritdoc cref="IUserDal"/>
    public PaginatedResult<User> GetUsers(UserRequest request)
    {
        var isAdmin = context
            .Users
            .Where(user => user.IsCentralServiceOperator)
            .Select(user => string.Equals(user.Email, request.UserId, StringComparison.InvariantCultureIgnoreCase))
            .FirstOrDefault();

        if (!isAdmin)
        {
            throw new InvalidOperationException("Users cannot be retrieved by users that are not administrators.");
        }

        var query = context.Users.Include(user => user.UserStatus);

        var paginatedQuery = query
            .OrderBy(user => user.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        return new PaginatedResult<User>(paginatedQuery.ToList(), paginatedQuery.Count());
    }
}